using System.Collections;
using System.Collections.Generic;
using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Core.Observability;
using Com.AsterForge.ShurikenRush.World.Entity.Player;
using Com.AsterForge.ShurikenRush.World.Entity.Shuriken;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Manager.Shuriken
{
    public class ShurikenManager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Transform _massShurikensParent;           // Player/ShurikenContainer
        [SerializeField] private Transform _thrownShurikensParent;
        [SerializeField] private Transform _playerRoot;              // Player root (parent of container)
        [SerializeField] private ShurikenController _shurikenPrefab;

        [Header("Counts")]
        [SerializeField, Range(1, 100)] private int _initialCount = 20;
        [SerializeField, Range(50, 1000)] private int _maxCount = 500;

        [Header("Mass Shuriken Centroid System")]
        [SerializeField] private float _slotSpacing = 0.25f;         // diameter spacing
        [SerializeField] private float _settleTime = 0.08f;          // SmoothDamp time
        [SerializeField] private float _maxLerpSpeed = 999f;         // unlimited
        [SerializeField] private float _centroidSmoothTime = 0.06f;
        
        [Header("Throwing System")]
        [SerializeField, Range(0.01f, 1f)] private float _throwInterval = 0.075f;
        [SerializeField, Range(1f, 50f)] private float _throwVelocityMult = 6.5f;
        
        [Header("Cosmetics")]
        [SerializeField] private float _singleShurikenSpinPerSec = 720f;

        [SerializeField] private float _massSpinPerSec = 2f;

        // Class Instance Life Cycle fields
        private readonly List<ShurikenController> _active = new();
        private readonly Stack<ShurikenController> _pool = new();
        
        // Object Mass Centroid Transforming fields
        private Vector3[] _slots;                // local positions for indices
        private Vector3[] _vel;                  // per-shuriken SmoothDamp velocity
        private float _desiredLocalX;
        private float _centroidLocalX;
        private float _centroidVelX;
        private float _massRotationAngle;
        public int CurrentCount => _active.Count;

        // Object Throw system fields
        private bool _canShoot;
        private bool _isThrowing;
        private float _throwTimer;
        private Transform _throwOrigin;

        #region Unity Mono-Behaviour Object Life Cycle
        private void Awake()
        {
            if (!_massShurikensParent) Debug.LogError("[ MASS MANAGER ] Mass Shurikens Parent Game Object is not set.");
            if(!_thrownShurikensParent) Debug.LogError("[ MASS MANAGER ] Thrown Shurikens Parent Game Object is not set");
            if (!_playerRoot && _massShurikensParent) _playerRoot = _massShurikensParent.parent;
            if (!_shurikenPrefab) Debug.LogError("[MassManager] Shuriken Prefab is not set.");
            GameContext.ShurikenManager = this;
            Bootstrap(_initialCount);
            PrecomputeSlots(_maxCount, _slotSpacing);
            _vel = new Vector3[_maxCount];

            Debug.Log("[ ENTITY : MASS_MANAGER ] Initialized.");
        }

        private void OnEnable()
        {
            SignalBus.Subscribe<PlayerHitSignal>(OnPlayerHitSignal);
        }

        private void OnDisable()
        {
            SignalBus.Unsubscribe<PlayerHitSignal>(OnPlayerHitSignal);
        }

        private void OnDestroy()
        {
            if(GameContext.ShurikenManager == this) GameContext.ShurikenManager = null;
        }

        // Call from PlayerController.Update
        public void Tick(float dt, bool canMoveHorizontal, bool canMoveVertical)
        {
            // 1. Move centroid (container local X) toward desired
            _centroidLocalX = Mathf.SmoothDamp(_centroidLocalX, _desiredLocalX, ref _centroidVelX, _centroidSmoothTime, Mathf.Infinity, dt);
            var lp = _massShurikensParent.localPosition;
            lp.x = _centroidLocalX;
            _massShurikensParent.localPosition = lp;

            int n = _active.Count;
            if (n == 0) return;

            _massRotationAngle += _massSpinPerSec * dt;
            
            
            for (int i = 0; i < n; i++)
            {
                // re-calculate slot positions based on mass rotation angle
                var shuriken = _active[i];
                Vector3 localSlot = _slots[i];

                Quaternion rot = Quaternion.Euler(0f, _massRotationAngle, 0f);
                Vector3 rotatedSlot = rot * localSlot;
                Vector3 targetWorld = _massShurikensParent.TransformPoint(rotatedSlot);

                // smooth damp each slot towards the new rotated World Point
                Vector3 vel = _vel[i];
                Vector3 nextWorld = Vector3.SmoothDamp(shuriken.Transform.position, targetWorld, ref vel, _settleTime, _maxLerpSpeed, dt);
                _vel[i] = vel;

                // move object towards new world point
                shuriken.Transform.position = nextWorld;
                // rotate each object around its local y-axis
                shuriken.RotateY(_singleShurikenSpinPerSec * dt);
            }

            // 4. Shuriken Throwing
            if (_isThrowing && _active.Count > 0)
            {
                _throwTimer -= dt;
                if (_throwTimer <= 0f)
                {
                    _throwTimer = _throwInterval;
                    ThrowOne();
                }
            }
        }
        
        public void Bootstrap(int initialCount)
        {
            initialCount = Mathf.Clamp(initialCount, 0, _maxCount);
            EnsurePool(initialCount);
            SetCount(initialCount);
            SnapCentroidLocalX(0f);
        }
        #endregion

        #region Transform Functions
        public void SetTargetWorldX(float worldX)
        {
            if (_playerRoot == null) return;
            Vector3 world = new Vector3(worldX, _playerRoot.position.y, _playerRoot.position.z);
            Vector3 local = _playerRoot.InverseTransformPoint(world);
            _desiredLocalX = local.x;
        }

        public Vector3 GetCentroidWorld()
        {
            return _massShurikensParent.position;
        }
        #endregion
        
        #region Object Mass Centroid System
        public void SetCount(int targetCount)
        {
            targetCount = Mathf.Clamp(targetCount, 0, _maxCount);
            if (targetCount == _active.Count) return;

            if (targetCount > _active.Count)
            {
                int add = targetCount - _active.Count;
                for (int i = 0; i < add; i++)
                {
                    var s = GetFromPool();
                    s.Transform.SetParent(_massShurikensParent, false);
                    s.SetLocalPosition(Vector3.zero);
                    s.Activate();
                    _active.Add(s);
                }
            }
            else
            {
                int remove = _active.Count - targetCount;
                for (int i = 0; i < remove; i++)
                {
                    int idx = _active.Count - 1;
                    var s = _active[idx];
                    _active.RemoveAt(idx);
                    s.Deactivate();
                    _pool.Push(s);
                }
            }
        }

        private void EnsurePool(int min)
        {
            while ((_pool.Count + _active.Count) < min)
            {
                var inst = Instantiate(_shurikenPrefab, _massShurikensParent);
                inst.Deactivate();
                _pool.Push(inst);
            }
        }

        private ShurikenController GetFromPool()
        {
            if (_pool.Count > 0) return _pool.Pop();
            var inst = Instantiate(_shurikenPrefab, _massShurikensParent);
            inst.Deactivate();
            return inst;
        }

        private void SnapCentroidLocalX(float x)
        {
            _desiredLocalX = x;
            _centroidLocalX = x;
            _centroidVelX = 0f;
            var lp = _massShurikensParent.localPosition;
            lp.x = x;
            _massShurikensParent.localPosition = lp;
        }

        private void PrecomputeSlots(int maxCount, float spacing)
        {
            _slots = new Vector3[maxCount];
            // Ring 0
            int idx = 0;
            if (idx < maxCount) _slots[idx++] = Vector3.zero;

            float r = spacing;
            int ring = 1;
            while (idx < maxCount)
            {
                int cap = 6 * ring;
                float step = 360f / cap;
                for (int i = 0; i < cap && idx < maxCount; i++)
                {
                    float deg = step * i;
                    float rad = deg * Mathf.Deg2Rad;
                    _slots[idx++] = new Vector3(Mathf.Cos(rad) * r, 0f, Mathf.Sin(rad) * r);
                }
                ring++;
                r += spacing; // radial increment
            }
        }
        #endregion

        #region Throwing System 
        public void SetCanThrow(bool canShoot)
        {
            _canShoot = canShoot;
        }
        
        public void BeginThrowing()
        {
            _isThrowing = true;
            _throwTimer = 0f;
        }

        public void StopThrowing()
        {
            _isThrowing = false;
        }
        
        private void ThrowOne()
        {
            if (_active.Count == 0)
            {
                _throwTimer = _throwInterval;
                _isThrowing = false;
                return;
            }
            if (GameContext.PlayerController == null) return;

            int last = _active.Count - 1;
            var shuriken = _active[last];
            _active.RemoveAt(last);

            shuriken.Transform.SetParent(_thrownShurikensParent);

            var rb = shuriken.Rb;
            if (!rb) return;

            rb.isKinematic = false;
            rb.useGravity = false;

            Vector3 forward = GameContext.PlayerController.transform.forward;
            forward.y = 0f; // keep horizontal plane only
            forward.x = 0f;
            forward.Normalize();

            rb.linearVelocity = forward * _throwVelocityMult;

            StartCoroutine(ReturnAfterHit(shuriken, 10f));
        }
        
        private IEnumerator ReturnAfterHit(ShurikenController shuriken, float delay)
        {
            yield return new WaitForSeconds(delay);
            shuriken.Deactivate();
            _pool.Push(shuriken);
        }

        #endregion
        
        #region Signal Handlers

        private void OnPlayerHitSignal(PlayerHitSignal signal)
        {
            SetCount(_active.Count - signal.Damage);
        }
        #endregion
    }
}
