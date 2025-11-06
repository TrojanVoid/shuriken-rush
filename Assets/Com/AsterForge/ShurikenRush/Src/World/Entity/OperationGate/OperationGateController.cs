using Com.AsterForge.ShurikenRush.Systems.Operation;
using TMPro;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.OperationGate
{
    [ExecuteAlways]
    public class OperationGateController : MonoBehaviour
    {
        [Header("Operation")]
        [SerializeField] private OperationSpec _operation;

        [Header("Visuals")]
        [SerializeField] private MeshRenderer _quadRenderer;

        [SerializeField] private MeshRenderer _holeRenderer;
        [SerializeField] private Material _positiveMat;
        [SerializeField] private Material _negativeMat;
        [SerializeField] private Material _positiveHoleMat;
        [SerializeField] private Material _negativeHoleMat;
        
        [SerializeField] private TextMeshPro _label;

        public OperationSpec OperationSpec => _operation;

        private void OnValidate()
        {
            ApplyVisuals();
        }

        private void Reset()
        {
            // Try auto-wire children if named as per prefab description
            if (!_quadRenderer)
            {
                var quad = transform.Find("Quad");
                if (quad) _quadRenderer = quad.GetComponent<MeshRenderer>();
            }
            if (!_label)
            {
                var text = transform.Find("Quad/Text");
                if (text) _label = text.GetComponent<TextMeshPro>();
            }

            if (!_holeRenderer)
            {
                throw new MissingComponentException("[ ENTITY : OPERATION_GATE_CONTROLLER ] Missing Hole Renderer");
            }
            ApplyVisuals();
        }

        private void ApplyVisuals()
        {
            if (_label)
            {
                _label.text = OperationService.ToDisplay(_operation);
                _label.richText = false;
            }
            
            var positive = OperationService.IsPositive(_operation);
            var targetMat = positive ? _positiveMat : _negativeMat;
            
            if (_quadRenderer)
            {
                
                if (targetMat && Application.isEditor)
                {
#if UNITY_EDITOR
                    // ensure material slot updated in edit/runtime
                    var mats = _quadRenderer.sharedMaterials;
                    if (mats.Length == 0) mats = new Material[1];
                    mats[0] = targetMat;
                    _quadRenderer.sharedMaterials = mats;
#endif
                }
                else if (targetMat)
                {
                    var mats = _quadRenderer.materials;
                    if (mats.Length == 0) mats = new Material[1];
                    mats[0] = targetMat;
                    _quadRenderer.materials = mats;
                }
            }
            
            targetMat = positive ? _positiveHoleMat : _negativeHoleMat;

            if (_holeRenderer)
            {
                if (targetMat && Application.isEditor)
                {
#if UNITY_EDITOR
                    var mats = _holeRenderer.sharedMaterials;
                    if (mats.Length == 0) mats = new Material[1];
                    mats[0] = targetMat;
                    _holeRenderer.sharedMaterials = mats;
#endif
                }
                else if (targetMat)
                {
                    var mats = _holeRenderer.materials;
                    if (mats.Length == 0) mats = new Material[1];
                    mats[0] = targetMat;
                    _holeRenderer.materials = mats;
                }
            }
        }
    }
}