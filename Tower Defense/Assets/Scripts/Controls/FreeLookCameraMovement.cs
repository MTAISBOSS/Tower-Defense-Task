using System;
using Script.Input;
using Service_Locator;
using UnityEngine;
using Update_Service;

namespace Controls
{
    public class FreeLookCameraMovement : MonoBehaviour, IUpdate
    {
        [SerializeField] private Vector4 limit;
        [SerializeField] private float speed;

        private InputHandler _inputHandler;
        private float _horizontalInput;
        private float _verticalInput;
        private Vector3 _finalPosition;
        private void OnEnable() => ServiceLocator.Instance.Get<UpdateManager>().Register(this);
        private void OnDisable() => ServiceLocator.Instance.Get<UpdateManager>().UnRegister(this);
        
        private void Start()
        {
            _inputHandler = InputHandler.Instance;
        }
        public void Update()
        {
            _horizontalInput = _inputHandler.GetHorizontal()* speed; 
            _verticalInput = _inputHandler.GetVertical()* speed;
            _finalPosition = transform.position + new Vector3(_horizontalInput, 0, _verticalInput);
            _finalPosition.x = Mathf.Clamp(_finalPosition.x, limit.x, limit.y);
            _finalPosition.z = Mathf.Clamp(_finalPosition.z, limit.z, limit.w);
            transform.position = _finalPosition;
        }
    }
}
