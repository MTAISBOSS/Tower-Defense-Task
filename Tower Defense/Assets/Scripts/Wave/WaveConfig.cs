using System.Collections.Generic;
using Enemy.Base;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "Wave Config", menuName = "Configs/Wave")]
    public class WaveConfig : ScriptableObject
    {
        public List<EnemyBatch> enemyBatches = new List<EnemyBatch>();

        [Tooltip(
            "This is the total duration of Wave, if set to -1 then it depends on all enemies to spawn not just time")]
        public float duration = -1;
    }
}