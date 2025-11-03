using System;
using Com.ShurikenRush.System.DIContainer;
using UnityEngine;

namespace Com.ShurikenRush.World.Entity.LevelGate
{
    public enum GateTriggerType
    {
        Undefined,
        LevelStart,
        LevelEnd,
    }
    public class LevelGateController : MonoBehaviour
    {
        [SerializeField] private MeshCollider _collider;
        [SerializeField] private GateTriggerType _triggerType;

        public void Awake()
        {
            CheckComponents();
            
            Debug.Log($"[ ENTITY : LEVEL_GATE_CONTROLLER ] Level Gate Controller for " +
                      $"{(_triggerType == GateTriggerType.LevelStart ? "LEVEL_START" :"LEVEL_END")} has been initialized.");
        }

        private void CheckComponents()
        {
            if (!_collider)
            {
                _collider = GetComponent<MeshCollider>();
            }

            if (_triggerType == GateTriggerType.Undefined)
            {
                throw new MissingFieldException(
                    "[ ENTITY : LEVEL_GATE_CONTROLLER ] No trigger type selected for the Level Gate");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Check");
            if (!other.CompareTag("Player"))
            {
                Debug.Log("Fail 1");
                return;
            }
            if (GlobalContextProvider.PlayerController == null)
            {
                Debug.Log("Fail 2");
                throw new MissingFieldException(
                    "[ ENTITY : LEVEL_GATE_CONTROLLER ] No player controller registered at Global Context Provider.");
            }
            GlobalContextProvider.PlayerController.SetCanMove(_triggerType == GateTriggerType.LevelStart);
            Debug.Log("Pass");
        }
        
    }
}