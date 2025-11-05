using System.Collections.Generic;
using System.Linq;
using Com.AsterForge.ShurikenRush.Manager.Projectile;
using Com.AsterForge.ShurikenRush.System.Animator;
using Com.AsterForge.ShurikenRush.System.Core.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.Enemy
{
    
    public class EnemyController : MonoBehaviour, IAnimatable<EnemyAnimationState>
    {
        [Header("Wiring")]
        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _projectileSpawnRoot;

        [Header("Behaviour")] 
        [SerializeField] private bool _attacksWithBow;
        [SerializeField] private float _bowAttackInterval;

        [SerializeField] private bool _attacksWithThrow;
        [SerializeField] private float _throwInterval;
        
        public bool  HasBow => _attacksWithBow;
        public bool  HasThrow => _attacksWithThrow;
        public bool IsAttacking => _isAttacking;
        public bool IsDead => _isDead;
        
        private bool _canAttack;
        private bool _isAttacking;
        private bool _canMove;
        private bool _isMoving;
        private bool _isDead;
        
        private Dictionary<AttackType, float> _lastAttackTimes;
        private Dictionary<AttackType, float> _attackIntervals;
        private List<AttackType> _availableAttacks;
        
        #region Unity Object Life Cycle
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            ValidateComponents();
            _lastAttackTimes = new Dictionary<AttackType, float>();
            _attackIntervals = new Dictionary<AttackType, float>();
            _availableAttacks = new List<AttackType>();
            if (_attacksWithThrow)
            {
                _lastAttackTimes.Add(AttackType.Throw, Time.time);
                _attackIntervals.Add(AttackType.Throw, _throwInterval);
                _availableAttacks.Add(AttackType.Throw);
            }

            if (_attacksWithBow)
            {
                _lastAttackTimes.Add(AttackType.Bow, Time.time);
                _attackIntervals.Add(AttackType.Bow, _bowAttackInterval);
                _availableAttacks.Add(AttackType.Bow);
            }
        
        }

        private void ValidateComponents()
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<SkinnedMeshRenderer>();
            }

            if (_renderer == null)
            {
                throw new MissingComponentException("[ ENTITY : ENEMY ] No MeshRenderer component attached. ");
            }
            
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider>();
            }

            if (_collider == null)
            {
                throw new MissingComponentException("[ ENTITY : ENEMY ] No MeshCollider component attached. ");
            }
            
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            if (_animator == null)
            {
                throw new MissingComponentException("[ ENTITY : ENEMY ] No Animator component attached. ");
            }
            
        }
        #endregion
        
        #region Animation Interface
            public void TriggerAnimationState(EnemyAnimationState animationState)
            {
                Debug.Log("Triggering animation state: " + animationState);
                _animator.SetTrigger(animationState.ToString());
            }

            public void Stop()
            {
                TriggerAnimationState(EnemyAnimationState.Idle);
            }

            // Called by Event trigger in the Unity Animation Clip
            private void OnProjectileRelease()
            {
                ProjectileSpawnSignal signal = new ProjectileSpawnSignal(_projectileSpawnRoot, transform.forward.normalized);
                SignalBus.FireSignal<ProjectileSpawnSignal>(signal);
            }
            
        #endregion
        
        #region Attack Behaviour

        private int GetAttackMask()
        {
            int bow =  _attacksWithBow ? 1 : 0;
            int throws = _attacksWithThrow ? 2 : 0;
            return bow + throws;
        }

        private AttackType DetermineNextAttack(int attackMask)
            => attackMask switch
            {
                0 => AttackType.None,
                1 => AttackType.Bow,
                2 => AttackType.Throw,
                3 => SelectAttackOption()
            };

        private AttackType SelectAttackOption()
        {
            if (!_canAttack) return AttackType.None;
            if (!_isAttacking) return _availableAttacks[Random.Range(0, _availableAttacks.Count)];
            return _lastAttackTimes
                .First(pair => Time.time - pair.Value >= _attackIntervals[pair.Key])
                .Key;
        }

        public void StartAttack()
        {
            _canAttack = true;
            AttackType attackType = DetermineNextAttack(GetAttackMask());
            if (attackType != AttackType.None) return;
            _isAttacking = true;
            
            switch (attackType)
            {
                case AttackType.Bow: TriggerAnimationState(EnemyAnimationState.AttackBow); break;
                case AttackType.Throw: TriggerAnimationState(EnemyAnimationState.AttackThrow); break;
            }
            
             _lastAttackTimes[attackType] = Time.time;
        }
        
        #endregion

        
    }
}