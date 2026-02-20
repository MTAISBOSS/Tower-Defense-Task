using System;
using Controls;
using Cysharp.Threading.Tasks;
using Enemy.Base;
using Health_System;
using Service_Locator;
using Shoot;
using Turret;
using UnityEngine;
using UnityEngine.Pool;
using Update_Service;

namespace Enemy.Tank
{
    public class Tank : MonoBehaviour, IEnemy, IDamage
    {
        public IObjectPool<Tank> ObjectPool { set => _objectPool = value; }
        [Header("Controll Settings")]
        [SerializeField] private Movement movement;
        
        [Header("Death Related Settings")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float destroyImpactForce = 100f;
        [SerializeField] private ParticleSystem explosionEffect;

        private Health _health;
        private IObjectPool<Tank> _objectPool;

        public void TakeDamage(int amount)
        {
            if (_health)
            {
                var currentHealth = _health.HealthAmount;
                _health.SetHealth(currentHealth - amount);
                if (_health.HealthAmount <= 0)
                {
                    HandleDeath();
                }
            }
        }
        private void HandleDeath()
        {
            ServiceLocator.Instance.Get<EnemyTransformContainer>().Remove(transform);
            movement.IsAbleToMove = false;
            Vector3 forceDirection = Vector3.back + Vector3.up + Vector3.left * 0.5f;
            rb.AddForce(forceDirection * destroyImpactForce,ForceMode.Impulse);
            if (explosionEffect)
            {
                explosionEffect.Play();
            }
            UniTask.Delay(4000);
            rb.isKinematic = true;
            rb.useGravity = true;
            UniTask.Delay(3000);
            _objectPool.Release(this);
        }
    }
}