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
        private void Start()
        {
            spawner.WaveStarted += ShowStartedWave;
            spawner.WavesEnded += ShowWaveEnded;
        }

        private void ShowWaveEnded()
        {
            waveLabel.text = "Waves Ended! Good Job";
        }

        private void ShowStartedWave(int wave)
        {
            waveLabel.text = $"Wave {wave + 1} has Started";
        }
    }
}