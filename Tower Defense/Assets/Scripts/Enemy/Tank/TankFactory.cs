using Controls;
using Enemy.Base;
using Service_Locator;
using UnityEngine;
using UnityEngine.Pool;

namespace Enemy.Tank
{
    [CreateAssetMenu(fileName = "Tank Factory", menuName = "Configs/Factories/Tank Factory")]
    public class TankFactory : EnemyFactory
    {
        public int maxSize = 100;
        public int defaultCapacity = 20;
        private EnemyConfig _enemyConfig;
        private IObjectPool<Tank> _objectPool;

        public TankFactory()
        {
            _objectPool = new ObjectPool<Tank>(CreateTank, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true,
                defaultCapacity, maxSize);
        }


        private Tank CreateTank()
        {
            Tank enemy = Instantiate(_enemyConfig.prefab).GetComponent<Tank>();
            enemy.ObjectPool = _objectPool;
            return enemy;
        }

        private void OnGetFromPool(Tank tank)
        {
            tank.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(Tank tank)
        {
            tank.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(Tank tank)
        {
            Destroy(tank.gameObject);
        }

        public override IEnemy Create(Vector3 spawnPosition, EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
            Tank tank = _objectPool.Get();
            
            GameObject enemy =tank.gameObject;
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;

            if (enemy.TryGetComponent(typeof(Movement), out Component moveComponent))
            {
                ((Movement)moveComponent).SetSpeed(enemyConfig.speed);
            }
            if (enemy.TryGetComponent(typeof(Health_System.Health), out Component health))
            {
                ((Health_System.Health)health).SetMaxHealth(enemyConfig.health);
            }
            
            ServiceLocator.Instance.Get<EnemyTransformContainer>().Add(enemy.transform);
            return tank;
        }
    }
}