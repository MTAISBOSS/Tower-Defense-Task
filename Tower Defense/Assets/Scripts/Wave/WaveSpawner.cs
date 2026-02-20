using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy.Base;
using UnityEngine;
using ZLinq;

namespace Wave
{
    public class WaveSpawner
    {
        public async UniTask SpawnWave(WaveConfig waveConfig, Vector3 spawnPoint, CancellationTokenSource cts)
        {
            var spawnTasks = waveConfig.enemyBatches.AsValueEnumerable()
                .Select(o => SpawnEnemyBatch(o, spawnPoint, cts));
            if (waveConfig.duration < 0)
            {
                await UniTask.WhenAll(spawnTasks.ToArray());
            }
            else
            {
                var delayTask = UniTask.Delay((int)(waveConfig.duration * 1000),cancellationToken:cts.Token);
                await UniTask.WhenAny(UniTask.WhenAll(spawnTasks.ToArray()), delayTask);
            }
        }

        private async UniTask SpawnEnemyBatch(EnemyBatch enemyBatch, Vector3 spawnPosition,
            CancellationTokenSource cancellationTokenSource)
        {
            for (int i = 0; i < enemyBatch.totalEnemiesAmount; i++)
            {
                await UniTask.Delay((int)(enemyBatch.timeBetweenSpawn * 1000),
                    cancellationToken: cancellationTokenSource.Token);
                enemyBatch.factory.Create(spawnPosition, enemyBatch.config);
            }
        }
    }
}