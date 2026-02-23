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
        private Vector3 _spawnPosition;

        public TankFactory()
        {
            _objectPool = new ObjectPool<Tank>(CreateTank, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true,
                defaultCapacity, maxSize);
        }


        private Tank CreateTank()
        {
            Tank enemy = Instantiate(_enemyConfig.prefab,_spawnPosition,Quaternion.identity).GetComponent<Tank>();
            enemy.ObjectPool = _objectPool;
            return enemy;
        }

        private void OnGetFromPool(Tank tank)
        {
            tank.gameObject.SetActive(true);
            tank.transform.position = _spawnPosition;
            tank.transform.rotation = Quaternion.Euler(0,0,0);
            tank.GetComponent<Movement>().IsAbleToMove = true;
            if (tank.TryGetComponent(typeof(Health_System.Health), out Component health))
            {
                ((Health_System.Health)health).SetMaxHealth(_enemyConfig.health);
            }
        }

        private void OnReleaseToPool(Tank tank)
        {
            tank.transform.position = _spawnPosition;
            tank.GetComponent<Movement>().IsAbleToMove = false;
            tank.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(Tank tank)
        {
            Destroy(tank.gameObject);
        }

        public override IEnemy Create(Vector3 spawnPosition, EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
            _spawnPosition = spawnPosition;
            Tank tank = _objectPool.Get();
            
            GameObject enemy =tank.gameObject;

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