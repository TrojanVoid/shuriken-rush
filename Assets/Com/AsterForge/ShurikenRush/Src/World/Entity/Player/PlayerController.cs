
using Com.AsterForge.ShurikenRush.Manager.Shuriken;
using Com.AsterForge.ShurikenRush.System.Core.DI;
using Com.AsterForge.ShurikenRush.System.Core.InputBroadcast;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] private Camera _cam;                       // child camera (optional for future)
        [SerializeField] private InputService _input;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private BoxCollider _hitbox;

        [Header("Motion")]
        [SerializeField] private float _forwardSpeed = 6f;
        
        public bool CanMoveHorizontal => _canMoveHorizontal;
        public bool CanMoveVertical => _canMoveVertical;
        
        private bool _canMoveVertical = true;
        private bool _canMoveHorizontal = false;

        private ShurikenManager _shurikenManager;

        private void Awake()
        {
            if (!_shurikenManager) _shurikenManager = GameContext.ShurikenManager;
            if (!_input) _input = GetComponentInChildren<InputService>(true);
            if (!_cam) _cam = GetComponentInChildren<Camera>(true);
            if (!_collider) _collider = transform.Find("Collider").GetComponent<BoxCollider>();
            if(!_hitbox) _hitbox = transform.Find("Hitbox").GetComponent<BoxCollider>();

            GameContext.PlayerController = this;
        }

        private void Start()
        {
            // Optional: align input baseline to current world X
            _input?.SnapTo(_shurikenManager.GetCentroidWorld().x);
        }

        private void FixedUpdate()
        {
            if(_canMoveVertical)
                transform.position += transform.forward * (_forwardSpeed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            _shurikenManager.Tick(Time.deltaTime, _canMoveHorizontal, _canMoveVertical);

            if (_input && _collider && _hitbox)
            {
                _collider.transform.position = new Vector3(_input.DesiredWorldX + 0.0001f, _collider.transform.position.y, _collider.transform.position.z);
                _hitbox.transform.position =  new Vector3(_input.DesiredWorldX + 0.0001f, _collider.transform.position.y, _collider.transform.position.z);
            }
            if (_input && _shurikenManager)
            {
                _shurikenManager.SetTargetWorldX(_input.DesiredWorldX);
                _shurikenManager.Tick(Time.deltaTime, _canMoveHorizontal, _canMoveVertical);
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