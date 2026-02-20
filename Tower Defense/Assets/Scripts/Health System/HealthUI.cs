using System;
using UnityEngine;
using UnityEngine.UI;

namespace Health_System
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Image fillBar;

        private void Start() => health.HealthUpdated += UpdateVisual;
        private void UpdateVisual() => fillBar.fillAmount = Mathf.Clamp01(health.GetHealthPercentage);
    }
}