using System;
using Com.ShurikenRush.System.DIContainer;
using UnityEngine;

namespace Com.ShurikenRush.World.Entity.LevelGate
{
    public enum GateTriggerType
    {
        Undefined,
        NoEffect,
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
            if (_triggerType == GateTriggerType.NoEffect || !other.CompareTag("Player"))
                return;
            if (GlobalContextProvider.PlayerController == null)
            {
                throw new MissingFieldException(
                    "[ ENTITY : LEVEL_GATE_CONTROLLER ] No player controller registered at Global Context Provider.");
            }

            if (_triggerType == GateTriggerType.LevelStart)
            {
                GlobalContextProvider.PlayerController.SetCanMoveHorizontal(true);
            }

            if (_triggerType == GateTriggerType.LevelEnd)
            {
                GlobalContextProvider.PlayerController.SetCanMoveVertical(false);
            }
            Debug.Log("Pass");
        }
        
    }
}