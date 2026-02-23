using GameSystemsPackage.Movement.Interfaces;
using Script.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystemsPackage.Movement.InputProviders
{
    [System.Serializable]
    public class KeyboardMouseInput : IMovementInput
    {
        [Header("Key Bindings")] [SerializeField]
        private KeyCode jumpKey = KeyCode.Space;

        [SerializeField] private KeyCode interactKey = KeyCode.E;

        [Header("Mouse Settings")] [SerializeField]
        private float mouseSensitivity = 2f;

        [SerializeField] private bool invertY = false;

        private Vector2 lookInput;

        public Vector2 GetMovementInput()
        {
            return new Vector2(
                InputHandler.Instance.GetHorizontal(),
                InputHandler.Instance.GetVertical()
            ).normalized;
        }

        public bool GetJumpInput()
        {
            return InputHandler.Instance.GetJump();
        }

        public bool GetSprintInput()
        {
            return InputHandler.Instance.GetSprint();
        }

        public bool GetMouseDownInput(int id)
        {
            return InputHandler.Instance.GetMouseButtonDown(id);
        }

        public bool GetMouseInput(int id)
        {
            return InputHandler.Instance.GetMouseButton(id);
        }

        public bool GetInteractionKey()
        {
            return InputHandler.Instance.GetKey(interactKey);
        }

        public float GetMouseY()
        {
            return InputHandler.Instance.GetMouseY();
        }

        public float GetMouseX()
        {
            return InputHandler.Instance.GetMouseX();
        }

        public float GetMouseScroll()
        {
            return InputHandler.Instance.GetMouseScrollWheel();
        }


        public Vector2 GetLookInput()
        {
            lookInput.x = GetMouseX() * mouseSensitivity;
            lookInput.y = GetMouseY() * mouseSensitivity * (invertY ? 1 : -1);
            return lookInput;
        }
    }
}