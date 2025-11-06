using UnityEngine;

namespace Com.AsterForge.ShurikenRush.Systems.Level.Data
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "AsterForge/ShurikenRush/Level Config")]
    public class LevelConfigSO : ScriptableObject
    {
        [Tooltip("Total number of levels available in the game.")]
        [Min(1)] public int maxLevelCount = 10;
    }
}