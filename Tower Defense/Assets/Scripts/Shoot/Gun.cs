using System;
using Enemy.Tank;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

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
        
        
        private IObjectPool<Projectile> _objectPool;

        private void Awake()
        {
            _objectPool = new ObjectPool<Projectile>(CreateTank, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, true,
                defaultCapacity, maxSize);
        }


        private Projectile CreateTank()
        {
            Projectile proj = Instantiate(projectile);
            proj.SetDamage(projectileSetting.damage);
            proj.SetDespawnTime((int)(projectileSetting.despawnTime * 1000));
            proj.ObjectPool = _objectPool;
            return proj;
        }

        private void OnGetFromPool(Projectile tank)
        {
            tank.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(Projectile tank)
        {
            tank.gameObject.SetActive(false);
        }

        private void OnDestroyPooledObject(Projectile tank)
        {
            Destroy(tank.gameObject);
        }
    }

    [Serializable]
    public struct ProjectileSetting
    {
        public float despawnTime;
        public int damage;
    }
}