using UnityEngine;

namespace Enemy.Base
{
    [CreateAssetMenu(fileName = "Enemy Config",menuName = "Configs/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        public GameObject prefab;
        public EnemyType type;
        public int health;
        public int damage;
        public float speed;
    }

    public enum EnemyType : byte
    {
        Tank,
        Trooper,
        Helicopter
    }
}