using System;
using Com.AsterForge.ShurikenRush.Manager.Shuriken;
using Com.AsterForge.ShurikenRush.Systems.Core.DI.Context;
using Com.AsterForge.ShurikenRush.Systems.Operation;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.OperationGate
{
    [RequireComponent(typeof(Collider))]
    public class OperationGateConnector : MonoBehaviour
    {
        [SerializeField] private OperationGateController _gate;   // assign the OperationGate on parent
        [SerializeField] private bool _destroyAfterUse = true;
        [SerializeField] private string _playerTag = "PlayerCollider";

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
            
            ShurikenManager shurikenManager = GameContext.ShurikenManager;
            if (shurikenManager == null)
            {
                throw new NullReferenceException("[ ENTITY : OPERATION_GATE_CONTROLLER ] MassManager is unassigned in the Global Context Provider.");
            }

            int current = shurikenManager.CurrentCount;
            int next = OperationService.Apply(current, _gate.OperationSpec, 0, int.MaxValue);
            if (next != current)
            {
                shurikenManager.SetCount(next);
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