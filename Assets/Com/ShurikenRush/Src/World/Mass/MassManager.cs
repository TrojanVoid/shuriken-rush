using System.Collections.Generic;
using Com.ShurikenRush.System.DIContainer;
using UnityEngine;
using Com.ShurikenRush.World.Entity.Shuriken;

namespace Com.ShurikenRush.World.Mass
{
    public class MassManager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Transform _containerRoot;           // Player/ShurikenContainer
        [SerializeField] private Transform _playerRoot;              // Player root (parent of container)
        [SerializeField] private ShurikenController _prefab;

        [Header("Counts")]
        [SerializeField] private int _initialCount = 20;
        [SerializeField] private int _maxCount = 500;

        [Header("Slotting")]
        [SerializeField] private float _slotSpacing = 0.25f;         // diameter spacing
        [SerializeField] private float _settleTime = 0.08f;          // SmoothDamp time
        [SerializeField] private float _maxLerpSpeed = 999f;         // unlimited

        [Header("Centroid")]
        [SerializeField] private float _centroidSmoothTime = 0.06f;

        [Header("Cosmetics")]
        [SerializeField] private float _spinDegPerSec = 720f;

        private readonly List<ShurikenController> _active = new();
        private readonly Stack<ShurikenController> _pool = new();
        private Vector3[] _slots;                // local positions for indices
        private Vector3[] _vel;                  // per-shuriken SmoothDamp velocity
        private float _desiredLocalX;
        private float _centroidLocalX;
        private float _centroidVelX;

        public int CurrentCount => _active.Count;

        private void Awake()
        {
            
            if (!_containerRoot) Debug.LogError("[MassManager] ContainerRoot not set.");
            if (!_playerRoot && _containerRoot) _playerRoot = _containerRoot.parent;
            if (!_prefab) Debug.LogError("[MassManager] Prefab not set.");
            GlobalContextProvider.MassManager = this;
            Bootstrap(_initialCount);
            PrecomputeSlots(_maxCount, _slotSpacing);
            _vel = new Vector3[_maxCount];

            Debug.Log("[ ENTITY : MASS_MANAGER ] Initialized.");
        }

        private void OnDestroy()
        {
            if(GlobalContextProvider.MassManager == this) GlobalContextProvider.MassManager = null;
        }

        // Call from PlayerController.Update
        public void Tick(float dt, bool canMoveHorizontal, bool canMoveVertical)
        {
            // Move centroid (container local X) toward desired
         
            _centroidLocalX = Mathf.SmoothDamp(_centroidLocalX, _desiredLocalX, ref _centroidVelX, _centroidSmoothTime, Mathf.Infinity, dt);
            var lp = _containerRoot.localPosition;
            lp.x = _centroidLocalX;
            _containerRoot.localPosition = lp;                
            
        

            // Shuriken positioning
            int n = _active.Count;
            float rotStep = _spinDegPerSec * dt;
            for (int i = 0; i < n; i++)
            {
                var s = _active[i];
                Vector3 cur = s.CachedTransform.localPosition;
                Vector3 tar = _slots[i]; // container-local slot target
                Vector3 v = _vel[i];
                Vector3 next = Vector3.SmoothDamp(cur, tar, ref v, _settleTime, _maxLerpSpeed, dt);
                _vel[i] = v;
                s.SetLocalPosition(next);
                s.RotateY(rotStep);
            }
        }

        public void SetTargetWorldX(float worldX)
        {
            if (_playerRoot == null) return;
            Vector3 world = new Vector3(worldX, _playerRoot.position.y, _playerRoot.position.z);
            Vector3 local = _playerRoot.InverseTransformPoint(world);
            _desiredLocalX = local.x;
        }

        public Vector3 GetCentroidWorld()
        {
            return _containerRoot.position;
        }

        public void Bootstrap(int initialCount)
        {
            initialCount = Mathf.Clamp(initialCount, 0, _maxCount);
            EnsurePool(initialCount);
            SetCount(initialCount);
            SnapCentroidLocalX(0f);
        }

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
                    s.CachedTransform.SetParent(_containerRoot, false);
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
                var inst = Instantiate(_prefab, _containerRoot);
                inst.Deactivate();
                _pool.Push(inst);
            }
        }

        private ShurikenController GetFromPool()
        {
            if (_pool.Count > 0) return _pool.Pop();
            var inst = Instantiate(_prefab, _containerRoot);
            inst.Deactivate();
            return inst;
        }

        private void SnapCentroidLocalX(float x)
        {
            _desiredLocalX = x;
            _centroidLocalX = x;
            _centroidVelX = 0f;
            var lp = _containerRoot.localPosition;
            lp.x = x;
            _containerRoot.localPosition = lp;
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
    }
}
