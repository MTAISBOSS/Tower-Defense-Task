using System;
using Enemy.Base;
using Enemy.Tank;
using Service_Locator;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Update_Service;

namespace Shoot
{
    public class Gun : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private Projectile projectile;
        [SerializeField] ProjectileSetting projectileSetting;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private int defaultCapacity = 20;

        [Header("Gun Settings")] [SerializeField]
        private float timeBetweenShots;
        [SerializeField] private float detectRange;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Vector3 additionalOffset;
        
        private IObjectPool<Projectile> _objectPool;
        private EnemyTransformContainer _enemyContainer;
        private Transform _target;
        private float _lastTime;
        private float nextTimeToShoot;
        private bool _isInRange;
        private Vector3 _projectileDirection;

        private void Awake()
        {
            _objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true,
                defaultCapacity, maxSize);
            _enemyContainer = ServiceLocator.Instance.Get<EnemyTransformContainer>();

        }
        
        private Projectile CreateProjectile()
        {
            Projectile proj = Instantiate(projectile);
            proj.ObjectPool = _objectPool;
            proj.SetDamage(projectileSetting.damage);
            proj.SetDespawnTime((int)(projectileSetting.despawnTime * 1000));
            proj.SetSpeed(projectileSetting.speed);
            proj.transform.position = shootPoint.position;
            return proj;
        }
        private void OnGetFromPool(Projectile proj)
        {
            proj.gameObject.SetActive(true);
        }
        private void OnReleaseToPool(Projectile proj)
        {
            proj.gameObject.SetActive(false);
        }
        private void OnDestroyPooledObject(Projectile proj)
        {
            Destroy(proj.gameObject);
        }

        public void Shoot()
        {
            _target = _enemyContainer.Get(transform);
            if (!_target)
            {
                return;
            }

            _isInRange = Vector3.Distance(_target.position, transform.position) < detectRange;
            if (!_isInRange)
            {
                return;
            }
            if (_objectPool != null && Time.time > nextTimeToShoot)
            {
                Projectile proj = _objectPool.Get();
                if (!proj)
                {
                    return;
                }

                _projectileDirection = ((_target.position + additionalOffset) - shootPoint.position).normalized;
                proj.Launch(_projectileDirection);
                nextTimeToShoot = Time.time + timeBetweenShots;
            }
        }
    }

    [Serializable]
    public struct ProjectileSetting
    {
        public float despawnTime;
        public int damage;
        public float speed;
    }
}