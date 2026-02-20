using System;
using Enemy.Base;
using UnityEngine;

namespace Turret
{
    public class TurretHeadTurn : MonoBehaviour, ITurn
    {
        public float rotationSpeed;
        public Transform turretBase;
        private Vector3 _direction;

        public void Turn(Transform target)
        {
            _direction = (target.position - transform.position).normalized;
            _direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            turretBase.rotation =
                Quaternion.RotateTowards(turretBase.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public float GetAngleBetweenTargetAndSelf(Transform target)
        {
            var direction = (target.position - transform.position).normalized;
            direction.y = 0;
            return Vector3.Angle(turretBase.forward, direction);
        }
    }
}