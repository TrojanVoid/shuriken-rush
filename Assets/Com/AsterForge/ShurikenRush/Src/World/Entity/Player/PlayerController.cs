using System.Linq;
using Com.AsterForge.ShurikenRush.Manager.Shuriken;
using Com.AsterForge.ShurikenRush.System.Core.DI;
using Com.AsterForge.ShurikenRush.System.Core.InputBroadcast;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Camera _cam;                       // child camera (optional for future)
        [SerializeField] private ShurikenManager shuriken;
        [SerializeField] private InputService _input;
        [SerializeField] private BoxCollider _collider;

        [Header("Motion")]
        [SerializeField] private float _forwardSpeed = 6f;

        private bool _canMoveHorizontal = false;
        private bool _canMoveVertical = true;
        public bool CanMoveHorizontal => _canMoveHorizontal;
        public bool CanMoveVertical => _canMoveVertical;

        private void Awake()
        {
            if (!_transform) _transform = transform;
            if (!shuriken) shuriken = GameContext.ShurikenManager;
            if (!_input) _input = GetComponentInChildren<InputService>(true);
            if (!_cam) _cam = GetComponentInChildren<Camera>(true);
            if (!_collider)
                _collider = GetComponentsInChildren<BoxCollider>().AsEnumerable()
                    .First(col => col.gameObject.CompareTag("Player"));

            GameContext.PlayerController = this;
        }

        private void Start()
        {
            // Optional: align input baseline to current world X
            _input?.SnapTo(shuriken.GetCentroidWorld().x);
        }

        private void FixedUpdate()
        {
            if(_canMoveVertical)
                _transform.position += _transform.forward * (_forwardSpeed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            shuriken.Tick(Time.deltaTime, _canMoveHorizontal, _canMoveVertical);

            if (_input && _collider)
            {
                _collider.transform.position = new Vector3(_input.DesiredWorldX + 0.0001f, _collider.transform.position.y, _collider.transform.position.z);
            }
            if (_input && shuriken)
            {
                shuriken.SetTargetWorldX(_input.DesiredWorldX);
                shuriken.Tick(Time.deltaTime, _canMoveHorizontal, _canMoveVertical);
            }
        }

        private void OnDestroy()
        {
            if(GameContext.PlayerController == this) GameContext.PlayerController = null;
        }

        public void SetCanMoveHorizontal(bool canMove)
        {
            _canMoveHorizontal = canMove;
            GameContext.PlayerCanMoveHorizontal = canMove;
        }

        public void SetCanMoveVertical(bool canMove)
        {
            _canMoveVertical = canMove;
            GameContext.PlayerCanMoveVertical = canMove;
        }
    }
}