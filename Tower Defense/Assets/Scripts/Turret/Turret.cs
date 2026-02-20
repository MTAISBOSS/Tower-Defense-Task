using System;
using Enemy.Base;
using Service_Locator;
using Shoot;
using UnityEngine;
using Update_Service;
using Logger = Utilities.Logger;

namespace Turret
{
    public class Turret : MonoBehaviour,IUpdate
    {
        [SerializeField] private Transform target;
        [SerializeField] private TurretHeadTurn headTurn;
        [SerializeField] private TurretBarrelTurn barrelTurn;
        [SerializeField] private Gun gun;
        [SerializeField] private float targetingAngleThreshold;
        private float _angle;
        private EnemyTransformContainer _enemyContainer;

        private void OnEnable()
        {
            ServiceLocator.Instance.Get<UpdateManager>().Register(this);
            _enemyContainer = ServiceLocator.Instance.Get<EnemyTransformContainer>();
        }
        private void OnDisable() => ServiceLocator.Instance.Get<UpdateManager>().UnRegister(this);
        
        public void Update()
        {
            target = _enemyContainer.Get(transform);
            if (!target)
            {
                return;
            }
            headTurn.Turn(target);
            _angle = headTurn.GetAngleBetweenTargetAndSelf(target);
            if (Mathf.Abs(_angle) < targetingAngleThreshold)
            {
                barrelTurn.Turn(target);
            }
        }
    }
}
