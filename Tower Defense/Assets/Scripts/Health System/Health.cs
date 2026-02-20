using System;
using UnityEngine;

namespace Health_System
{
    public class Health : MonoBehaviour
    {
        public Action HealthUpdated;
        private int _healthAmount;
        public int HealthAmount => _healthAmount;
        public int MaxHealth { get; set; }
        public float GetHealthPercentage => (float)_healthAmount / MaxHealth;

        public void SetHealth(int healthAmount)
        {
            _healthAmount = healthAmount;
            HealthUpdated?.Invoke();
        }   
        public void SetMaxHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
            SetHealth(maxHealth);
        }

        
    }
}