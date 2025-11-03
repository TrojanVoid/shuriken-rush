using UnityEngine;

namespace Com.ShurikenRush.World.Entity.Shuriken
{
    public class ShurikenController : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private MeshRenderer _renderer;

        public Transform CachedTransform => _transform;

        private void Awake()
        {
            if (!_transform) _transform = transform;
            if (_rb)
            {
                _rb.isKinematic = true;
                _rb.useGravity = false;
            }
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void SetLocalPosition(in Vector3 lp)
        {
            _transform.localPosition = lp;
        }

        public void RotateY(float degrees)
        {
            _transform.Rotate(0f, degrees, 0f, Space.Self);
        }
    }
}