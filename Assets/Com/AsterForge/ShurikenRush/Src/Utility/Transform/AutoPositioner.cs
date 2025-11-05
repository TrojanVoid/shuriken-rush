using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Utility.Transform
{
    [ExecuteAlways]
    public class AutoPositioner : MonoBehaviour
    {
        private enum PositionType
        {
            Left,
            Middle,
            Right
        }

        [SerializeField] private PositionType _positionType;
        [SerializeField] private float _leftOffsetX;
        [SerializeField] private float _middleOffsetX;
        [SerializeField] private float _rightOffsetX;

        private void OnValidate()
        {
            if (!Application.isPlaying)
                ApplyOffset();
        }

        private void ApplyOffset()
        {
            float offsetX = _positionType switch
            {
                PositionType.Left => _leftOffsetX,
                PositionType.Middle => _middleOffsetX,
                PositionType.Right => _rightOffsetX,
                _ => 0f
            };

            var pos = transform.localPosition;
            pos.x = offsetX;
            transform.localPosition = pos;
        }
    }
}