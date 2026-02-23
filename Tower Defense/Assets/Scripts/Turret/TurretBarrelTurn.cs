using UnityEngine;

namespace Turret
{
    public class TurretBarrelTurn : MonoBehaviour, ITurn
    {
        public float rotationSpeed;
        public Transform turretBarrel;
        public Transform barrelHead;
        private Vector3 _direction;

        public void Turn(Transform target)
        {
            _direction = (target.position - barrelHead.position).normalized;
            Vector3 localDirection = transform.InverseTransformDirection(_direction);
            float angle = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            angle = -1 * Mathf.Clamp(angle, -30, 30);
            Quaternion rotation = Quaternion.Euler(angle, 0, 0);
            turretBarrel.localRotation =
                Quaternion.RotateTowards(turretBarrel.localRotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}