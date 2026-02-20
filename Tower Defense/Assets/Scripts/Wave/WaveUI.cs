using System;
using Cysharp.Threading.Tasks;
using Enemy;
using TMPro;
using UnityEngine;

namespace Wave
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waveLabel;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private float showTextDuration;
        private void Start() => spawner.WaveStarted += ShowStartedWave;
        private async void ShowStartedWave(int wave)
        {
            waveLabel.gameObject.SetActive(true);
            waveLabel.text = $"Wave {wave + 1} has Started";
            await UniTask.Delay((int)(showTextDuration * 1000));
            waveLabel.text = String.Empty;
        }
    }
}