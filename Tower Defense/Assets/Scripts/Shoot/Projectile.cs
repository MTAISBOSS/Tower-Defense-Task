using System;
using Cysharp.Threading.Tasks;
using Health_System;
using UnityEngine;
using UnityEngine.Pool;

namespace Shoot
{
    public class Projectile : MonoBehaviour
    {
        public IObjectPool<Projectile> ObjectPool { get; set; }

        private int _damage;
        private IObjectPool<Projectile> _objectPool;
      
        public void SetDamage(int damage) => _damage = damage;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(typeof(IDamage), out Component component))
            {
                ((IDamage)component).TakeDamage(_damage);
            }
        }

        public void SetDespawnTime(int despawnTime)
        {
            UniTask.Delay(despawnTime);
            _objectPool.Release(this);
        }
    }
}