using System;
using System.Threading;
using Enemy.Base;
using UnityEngine;
using Wave;
using Logger = Utilities.Logger;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public Action<int> WaveStarted;
        public Action WavesEnded;
        
        [SerializeField] private WaveConfig[] waveConfigs;
        [SerializeField] private Transform[] spawnPoints;
        
        private WaveSpawner _waveSpawner;

        private void Start()
        {
            Initialize();
        }

        async void Initialize()
        {
            using var cts = new CancellationTokenSource();
            _waveSpawner = new WaveSpawner();
            for (var i = 0; i < waveConfigs.Length; i++)
            {
                var waveConfig = waveConfigs[i];
                Logger.LogWarning($"wave {i} started");
                WaveStarted?.Invoke(i);
                var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                await _waveSpawner.SpawnWave(waveConfig, spawnPoint, cts);
                Logger.LogWarning($"wave {i} ended");
            }
            Logger.LogWarning("waves ended");
            WavesEnded?.Invoke();
        }
    }
}