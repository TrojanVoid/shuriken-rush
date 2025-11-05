using System;
using Com.AsterForge.ShurikenRush.System.Core.Signal;
using UnityEngine;

namespace Com.AsterForge.ShurikenRush.World.Entity.Player
{
    public class PlayerHitboxConnector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger && (other.CompareTag("EnemyProjectile") || other.CompareTag("Enemy"))  )
            {
                SignalBus.FireSignal<PlayerHitSignal>(new PlayerHitSignal(5));
            }
        }
        
    }
}