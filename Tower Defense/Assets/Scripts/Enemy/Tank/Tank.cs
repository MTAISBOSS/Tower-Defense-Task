using System;
using System.Collections.Generic;
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
        [SerializeField] private Health health;
        private IObjectPool<Tank> _objectPool;
        private bool _released;

        private void OnEnable()
        {
            _released = false;
        }

        public void TakeDamage(int amount)
        {
            if (health)
            {
                var currentHealth = health.HealthAmount;
                health.SetHealth(currentHealth - amount);
                if (health.HealthAmount <= 0)
                {
                    HandleDeath();
                }
            }
        }
        private async void HandleDeath()
        {
            if (_released)
            {
                return;
            }

            _released = true;
            ServiceLocator.Instance.Get<EnemyTransformContainer>().Remove(transform);
            movement.IsAbleToMove = false;
            Vector3 forceDirection = Vector3.back + Vector3.up * 0.5f;
            rb.AddForce(forceDirection * destroyImpactForce,ForceMode.Impulse);
            if (explosionEffect)
            {
                explosionEffect.gameObject.SetActive(true);
            }
            await UniTask.Delay(2000);
            explosionEffect.gameObject.SetActive(false);
            rb.isKinematic = true;
            rb.useGravity = true;
            _objectPool.Release(this);
        }
    }
}