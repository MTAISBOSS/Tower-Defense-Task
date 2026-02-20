using UnityEngine;
using Logger = Utilities.Logger;

namespace Enemy.Base
{
    [CreateAssetMenu(menuName = "Configs/Enemy Batch",fileName = "Enemy Batch")]
    public class EnemyBatch : ScriptableObject
    {
        public EnemyFactory factory;
        public EnemyConfig config;
        public float timeBetweenSpawn;
        public int totalEnemiesAmount;
        private void OnValidate()
        {
            if (config)
            {
                if (config.type != factory.type)
                {
                    Logger.LogError("You have assigned the enemy setting with different type");
                    config = null;
                }
            }
        }
    }
}