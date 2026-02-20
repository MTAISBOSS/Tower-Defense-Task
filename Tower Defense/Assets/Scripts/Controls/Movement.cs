using System;
using Service_Locator;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Controls
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        private float _speed;

        private Quaternion _lookRotation;
        public bool IsAbleToMove { get; set; }
        public float GetSpeed() => _speed;
        public void SetSpeed(float speed) => _speed = speed;
        private void OnEnable() => ServiceLocator.Instance.Get<BatchMovementService>().AddAgent(this);
        private void OnDisable() => ServiceLocator.Instance.Get<BatchMovementService>().RemoveAgent(this);

        private void Awake()
        {
            IsAbleToMove = true;
        }

        public void Move(Vector3 position, Vector3 rotateDirection)
        {
            if (!IsAbleToMove)
            {
                return;
            }
            if (rb)
            {
                _lookRotation = Quaternion.LookRotation(rotateDirection);
                rb.position = position;
                rb.rotation = Quaternion.RotateTowards(rb.rotation,_lookRotation,Time.deltaTime*50);
            }
        }

        
    }
}