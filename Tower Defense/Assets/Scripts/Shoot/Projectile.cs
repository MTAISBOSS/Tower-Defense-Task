using System;
using Cysharp.Threading.Tasks;
using Health_System;
using UnityEngine;
using UnityEngine.Pool;

namespace Shoot
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public IObjectPool<Projectile> ObjectPool { set => _objectPool = value; }
        
        [SerializeField]private Rigidbody rb;
        
        private int _damage;
        private IObjectPool<Projectile> _objectPool;
        private float _speed;
        private bool _released;

      
        public void SetDamage(int damage) => _damage = damage;
        public async void SetDespawnTime(int despawnTime)
        {
            await UniTask.Delay(despawnTime);
            _objectPool.Release(this);
        }
        public void SetSpeed(float speed) => _speed = speed;

        private void OnEnable() => _released = false;

        public void Launch(Vector3 direction)
        {
            transform.up = direction;
            rb.AddForce(direction * _speed,ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (_released)
            {
                return;
            }
            if (collider.gameObject.TryGetComponent(typeof(IDamage), out Component component))
            {
                ((IDamage)component).TakeDamage(_damage);
            }
            _released = true;
            _objectPool?.Release(this);
        }
    }
}