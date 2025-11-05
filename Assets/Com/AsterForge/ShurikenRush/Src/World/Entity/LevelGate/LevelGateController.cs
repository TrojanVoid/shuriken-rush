using System;
using Com.AsterForge.ShurikenRush.System.Core.DI;
using TMPro;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.LevelGate
{
    public enum GateTriggerType
    {
        Undefined,
        NoEffect,
        LevelStart,
        LevelEnd,
        StartFight,
    }
    public class LevelGateController : MonoBehaviour
    {
        [SerializeField] private GateTriggerType _triggerType;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private MeshCollider _collider;
        [SerializeField] private TextMeshPro _text;
        
        

        public void Awake()
        {
            CheckComponents();
            
            Debug.Log($"[ ENTITY : LEVEL_GATE_CONTROLLER ] Level Gate Controller for " +
                      $"{(_triggerType == GateTriggerType.LevelStart ? "LEVEL_START" :"LEVEL_END")} has been initialized.");
        }

        private void CheckComponents()
        {
            if (!_renderer)
            {
                _renderer = GetComponent<MeshRenderer>();
            }

            if (!_renderer)
            {
                throw new MissingComponentException("[ ENTITY : LEVEL_GATE_CONTROLLER ] No renderer component found.");
            }
            if (!_collider)
            {
                _collider = GetComponent<MeshCollider>();
            }

            if (!_text)
            {
                throw new MissingComponentException(" [ ENTITY : LEVEL_GATE_CONTROLLER ] No TextMeshPro component found.");
            }

            if (_triggerType == GateTriggerType.Undefined)
            {
                throw new MissingFieldException(
                    "[ ENTITY : LEVEL_GATE_CONTROLLER ] No trigger type selected for the Level Gate");
            }
        }

        public void Disable()
        {
            _renderer.enabled = false;
            _collider.enabled = false;
            _text.renderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("PlayerCollider"))
                return;
            if (GameContext.PlayerController == null)
            {
                throw new MissingFieldException(
                    "[ ENTITY : LEVEL_GATE_CONTROLLER ] No player controller registered at Global Context Provider.");
            }

            if (_triggerType == GateTriggerType.LevelStart)
            {
                GameContext.PlayerController.SetCanMoveHorizontal(true);
            }

            if (_triggerType == GateTriggerType.LevelEnd)
            {
            }

            if (_triggerType == GateTriggerType.StartFight)
            {
                GameContext.MassCanThrow = true;
                GameContext.PlayerController.SetCanMoveVertical(false);
                GameContext.ShurikenManager.BeginThrowing();
            }

            Disable();
        }
        
    }
}