using UnityEngine;

namespace Enemy.Base
{
    public abstract class EnemyFactory : ScriptableObject
    {
        public EnemyType type;
        public abstract IEnemy Create(Vector3 spawnPosition, EnemyConfig config);
    }
}
