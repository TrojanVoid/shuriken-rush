using System;
using System.Linq;
using Com.ShurikenRush.System.DIContainer;
using Com.ShurikenRush.System.Input;
using UnityEngine;
using Com.ShurikenRush.World.Mass;

namespace Com.ShurikenRush.World.Entity.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Camera _cam;                       // child camera (optional for future)
        [SerializeField] private MassManager _mass;
        [SerializeField] private InputService _input;
        [SerializeField] private BoxCollider _collider;

        [Header("Motion")]
        [SerializeField] private float _forwardSpeed = 6f;

        private bool _canMove;
        public bool CanMove => _canMove;

        private void Awake()
        {
            if (!_transform) _transform = transform;
            if (!_mass) _mass = GetComponentInChildren<MassManager>(true);
            if (!_input) _input = GetComponentInChildren<InputService>(true);
            if (!_cam) _cam = GetComponentInChildren<Camera>(true);
            if (!_collider)
                _collider = GetComponentsInChildren<BoxCollider>().AsEnumerable()
                    .First(col => col.gameObject.CompareTag("Player"));

            GlobalContextProvider.PlayerController = this;
        }
        

        private void Start()
        {
            // Optional: align input baseline to current world X
            _input?.SnapTo(_mass.GetCentroidWorld().x);
        }

        private void FixedUpdate()
        {
            _transform.position += _transform.forward * (_forwardSpeed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            _mass.Tick(Time.deltaTime, _canMove);

            if (_input && _collider)
            {
                _collider.transform.position = new Vector3(_input.DesiredWorldX + 0.0001f, _collider.transform.position.y, _collider.transform.position.z);
            }
            if (_input && _mass)
            {
                _mass.SetTargetWorldX(_input.DesiredWorldX);
                _mass.Tick(Time.deltaTime, _canMove);
            }
        }

        private void OnDestroy()
        {
            if(GlobalContextProvider.PlayerController == this) GlobalContextProvider.PlayerController = null;
        }

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
            GlobalContextProvider.PlayerCanMove = canMove;
        }
    }
}