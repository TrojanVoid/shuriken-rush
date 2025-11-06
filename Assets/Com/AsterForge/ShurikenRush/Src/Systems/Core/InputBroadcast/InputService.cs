using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Core.InputBroadcast
{
    public class InputService : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _groundY = 0f;
        [SerializeField] private float _sensitivity = 1.0f;   // world units per normalized drag
        [SerializeField] private float _smoothTime = 0.06f;    // horizontal target smoothing (seconds)
        [SerializeField] private float _minX = -3.0f;
        [SerializeField] private float _maxX =  3.0f;

        private Plane _groundPlane;
        private bool _hasAnchor;
        private Vector3 _dragAnchorWorld;
        private float _targetWorldX;
        private float _smoothedWorldX;
        private float _velX;

        public float DesiredWorldX => _smoothedWorldX;

        private void Awake()
        {
            if (!_camera) _camera = Camera.main;
            _groundPlane = new Plane(Vector3.up, new Vector3(0f, _groundY, 0f));
            _targetWorldX = 0f;
            _smoothedWorldX = 0f;
        }

        private void Update()
        {
            if (!GameContext.PlayerCanMoveHorizontal)
                return;
            
            // Begin drag
            if (GetPrimaryDown())
            {
                _hasAnchor = TryRayToPlane(out _dragAnchorWorld);
            }

            // Dragging
            if (_hasAnchor && GetPrimary())
            {
                if (TryRayToPlane(out var worldHit))
                {
                    float deltaX = (worldHit.x - _dragAnchorWorld.x) * _sensitivity;
                    float desired = Mathf.Clamp(_smoothedWorldX + deltaX, _minX, _maxX);
                    _targetWorldX = desired;
                    _dragAnchorWorld = worldHit; // incremental dragging
                }
            }

            // Release
            if (GetPrimaryUp())
            {
                _hasAnchor = false;
            }

            // Smooth target
            _smoothedWorldX = Mathf.SmoothDamp(_smoothedWorldX, _targetWorldX, ref _velX, _smoothTime, Mathf.Infinity, Time.deltaTime);
        }

        private bool TryRayToPlane(out Vector3 hitWorld)
        {
            hitWorld = default;
            Ray ray = _camera.ScreenPointToRay(GetPrimaryPosition());
            if (_groundPlane.Raycast(ray, out float dist))
            {
                hitWorld = ray.GetPoint(dist);
                return true;
            }
            return false;
        }

        private static bool GetPrimaryDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0) || 
                   (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.touches[0].phase == TouchPhase.Began);
        }
        private static bool GetPrimary()
        {
            return UnityEngine.Input.GetMouseButton(0) || 
                   (UnityEngine.Input.touchCount > 0 && 
                        (UnityEngine.Input.touches[0].phase == TouchPhase.Moved || UnityEngine.Input.touches[0].phase == TouchPhase.Stationary));
        }
        private static bool GetPrimaryUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0) || 
                   (UnityEngine.Input.touchCount > 0 && 
                        (UnityEngine.Input.touches[0].phase == TouchPhase.Ended || UnityEngine.Input.touches[0].phase == TouchPhase.Canceled));
        }
        private static Vector2 GetPrimaryPosition()
        {
            return UnityEngine.Input.touchCount > 0 ? 
                    UnityEngine.Input.touches[0].position : 
                    (Vector2)UnityEngine.Input.mousePosition;
        }

        public void SetBounds(float minX, float maxX) { _minX = minX; _maxX = maxX; }
        public void SetGroundY(float groundY) { _groundY = groundY; _groundPlane = new Plane(Vector3.up, new Vector3(0f, _groundY, 0f)); }
        public void SnapTo(float worldX) { _targetWorldX = _smoothedWorldX = Mathf.Clamp(worldX, _minX, _maxX); _velX = 0f; }
    }
}
