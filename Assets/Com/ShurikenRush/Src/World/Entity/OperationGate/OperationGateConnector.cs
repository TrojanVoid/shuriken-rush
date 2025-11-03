using System;
using Com.ShurikenRush.System.DIContainer;
using Com.ShurikenRush.System.Operation;
using Com.ShurikenRush.World.Mass;
using UnityEngine;

namespace Com.ShurikenRush.World.Entity.OperationGate
{
    [RequireComponent(typeof(Collider))]
    public class OperationGateConnector : MonoBehaviour
    {
        [SerializeField] private OperationGateController _gate;   // assign the OperationGate on parent
        [SerializeField] private bool _destroyAfterUse = true;
        [SerializeField] private string _playerTag = "Player";

        private bool _consumed;

        private void Reset()
        {
            if (!_gate) _gate = GetComponentInParent<OperationGateController>();
            var col = GetComponent<Collider>();
            if (col) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_consumed) return;
            if (!other.CompareTag(_playerTag)) return;
            
            MassManager massManager = GlobalContextProvider.MassManager;
            if (massManager == null)
            {
                throw new NullReferenceException("[ ENTITY : OPERATION_GATE_CONTROLLER ] MassManager is unassigned in the Global Context Provider.");
            }

            int current = massManager.CurrentCount;
            int next = OperationService.Apply(current, _gate.OperationSpec, 0, int.MaxValue);
            if (next != current)
            {
                massManager.SetCount(next);
            }

            _consumed = true;
            if (_destroyAfterUse) Destroy(gameObject);
            else
            {
                var col = GetComponent<Collider>();
                if (col) col.enabled = false;
            }
        }
    }
}