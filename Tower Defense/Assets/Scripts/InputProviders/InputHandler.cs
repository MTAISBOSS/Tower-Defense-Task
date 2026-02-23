using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Script.Input
{
    public class InputHandler : MonoBehaviour
    {
        public static event Action<Vector2> OnMoveInput;

        private InputSystem_Actions _inputAction;
        private Mouse _mouse;

        public Vector2 MoveInput { get; private set; }
        public bool GetSprint() => _inputAction.Player.Sprint.IsPressed();
        public bool GetSprintDown() => _inputAction.Player.Sprint.WasPressedThisFrame();
        public bool GetSprintUp() => _inputAction.Player.Sprint.WasReleasedThisFrame();

        #region Singleton

        public static InputHandler Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeInput();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        private void InitializeInput()
        {
            _inputAction = new InputSystem_Actions();
            _mouse = Mouse.current;

            _inputAction.Player.Move.performed += OnMovePerformed;
            _inputAction.Player.Move.canceled += OnMoveCanceled;
        }

        private void OnEnable()
        {
            _inputAction?.Enable();
        }

        private void OnDisable()
        {
            _inputAction?.Disable();
        }

        #region Input Callbacks

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            OnMoveInput?.Invoke(MoveInput);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
            OnMoveInput?.Invoke(MoveInput);
        }

        #endregion

        private void OnDestroy()
        {
            if (_inputAction != null)
            {
                _inputAction.Player.Move.performed -= OnMovePerformed;
                _inputAction.Player.Move.canceled -= OnMoveCanceled;
            }
        }

        public void EnablePlayerInput() => _inputAction.Player.Enable();
        public void DisablePlayerInput() => _inputAction.Player.Disable();

        public void EnableUIInput() => _inputAction.UI.Enable();
        public void DisableUIInput() => _inputAction.UI.Disable();

        public float GetVertical()
        {
            return MoveInput.y;
        }

        public float GetHorizontal()
        {
            return MoveInput.x;
        }

        public bool GetJump()
        {
            return _inputAction.Player.Jump.WasPressedThisFrame();
        }

        public bool GetJumpHeld()
        {
            return _inputAction.Player.Jump.IsPressed();
        }

        public float GetMouseScrollWheel()
        {
            if (_mouse != null)
            {
                return _mouse.scroll.ReadValue().y;
            }
            return 0f;
        }

        public bool GetKey(KeyCode keyCode)
        {
            if (Keyboard.current == null) return false;

            KeyControl key = GetKeyControl(keyCode);
            return key != null && key.isPressed;
        }

        public bool GetKeyDown(KeyCode keyCode)
        {
            if (Keyboard.current == null) return false;

            KeyControl key = GetKeyControl(keyCode);
            return key != null && key.wasPressedThisFrame;
        }

        public bool GetKeyUp(KeyCode keyCode)
        {
            if (Keyboard.current == null) return false;

            KeyControl key = GetKeyControl(keyCode);
            return key != null && key.wasReleasedThisFrame;
        }

        private KeyControl GetKeyControl(KeyCode keyCode)
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return null;

            switch (keyCode)
            {
                case KeyCode.LeftShift: return keyboard.leftShiftKey;
                case KeyCode.RightShift: return keyboard.rightShiftKey;
                case KeyCode.LeftControl: return keyboard.leftCtrlKey;
                case KeyCode.RightControl: return keyboard.rightCtrlKey;
                case KeyCode.LeftAlt: return keyboard.leftAltKey;
                case KeyCode.RightAlt: return keyboard.rightAltKey;
                case KeyCode.Space: return keyboard.spaceKey;
                case KeyCode.Return: return keyboard.enterKey;
                case KeyCode.Escape: return keyboard.escapeKey;
                case KeyCode.Tab: return keyboard.tabKey;
                case KeyCode.A: return keyboard.aKey;
                case KeyCode.B: return keyboard.bKey;
                case KeyCode.C: return keyboard.cKey;
                case KeyCode.D: return keyboard.dKey;
                case KeyCode.E: return keyboard.eKey;
                case KeyCode.F: return keyboard.fKey;
                case KeyCode.G: return keyboard.gKey;
                case KeyCode.H: return keyboard.hKey;
                case KeyCode.I: return keyboard.iKey;
                case KeyCode.J: return keyboard.jKey;
                case KeyCode.K: return keyboard.kKey;
                case KeyCode.L: return keyboard.lKey;
                case KeyCode.M: return keyboard.mKey;
                case KeyCode.N: return keyboard.nKey;
                case KeyCode.O: return keyboard.oKey;
                case KeyCode.P: return keyboard.pKey;
                case KeyCode.Q: return keyboard.qKey;
                case KeyCode.R: return keyboard.rKey;
                case KeyCode.S: return keyboard.sKey;
                case KeyCode.T: return keyboard.tKey;
                case KeyCode.U: return keyboard.uKey;
                case KeyCode.V: return keyboard.vKey;
                case KeyCode.W: return keyboard.wKey;
                case KeyCode.X: return keyboard.xKey;
                case KeyCode.Y: return keyboard.yKey;
                case KeyCode.Z: return keyboard.zKey;
                case KeyCode.Alpha0: return keyboard.digit0Key;
                case KeyCode.Alpha1: return keyboard.digit1Key;
                case KeyCode.Alpha2: return keyboard.digit2Key;
                case KeyCode.Alpha3: return keyboard.digit3Key;
                case KeyCode.Alpha4: return keyboard.digit4Key;
                case KeyCode.Alpha5: return keyboard.digit5Key;
                case KeyCode.Alpha6: return keyboard.digit6Key;
                case KeyCode.Alpha7: return keyboard.digit7Key;
                case KeyCode.Alpha8: return keyboard.digit8Key;
                case KeyCode.Alpha9: return keyboard.digit9Key;
                default: return null;
            }
        }

        public bool GetMouseButton(int button)
        {
            if (_mouse == null) return false;

            switch (button)
            {
                case 0: return _mouse.leftButton.isPressed;
                case 1: return _mouse.rightButton.isPressed;
                case 2: return _mouse.middleButton.isPressed;
                default: return false;
            }
        }

        public bool GetMouseButtonDown(int button)
        {
            if (_mouse == null) return false;

            switch (button)
            {
                case 0: return _mouse.leftButton.wasPressedThisFrame;
                case 1: return _mouse.rightButton.wasPressedThisFrame;
                case 2: return _mouse.middleButton.wasPressedThisFrame;
                default: return false;
            }
        }

        public bool GetMouseButtonUp(int button)
        {
            if (_mouse == null) return false;

            switch (button)
            {
                case 0: return _mouse.leftButton.wasReleasedThisFrame;
                case 1: return _mouse.rightButton.wasReleasedThisFrame;
                case 2: return _mouse.middleButton.wasReleasedThisFrame;
                default: return false;
            }
        }

        public float GetAxis(string axisName)
        {
            switch (axisName)
            {
                case "Horizontal":
                    return GetHorizontal();
                case "Vertical":
                    return GetVertical();
                case "Mouse X":
                    return GetMouseX();
                case "Mouse Y":
                    return GetMouseY();
                case "Mouse ScrollWheel":
                    return GetMouseScrollWheel();
                default:
                    return 0f;
            }
        }

        public bool GetButton(string buttonName)
        {
            switch (buttonName)
            {
                case "Jump":
                    return GetJumpHeld();
                case "Sprint":
                    return GetSprint();
                default:
                    return false;
            }
        }

        public bool GetButtonDown(string buttonName)
        {
            switch (buttonName)
            {
                case "Jump":
                    return GetJump();
                case "Sprint":
                    return GetSprintDown();
                default:
                    return false;
            }
        }

        public float GetMouseX()
        {
            if (_mouse != null)
            {
                return _mouse.delta.ReadValue().x;
            }
            return 0f;
        }

        public float GetMouseY()
        {
            if (_mouse != null)
            {
                return _mouse.delta.ReadValue().y;
            }
            return 0f;
        }

        public bool GetButtonUp(string buttonName)
        {
            switch (buttonName)
            {
                case "Jump":
                    return _inputAction.Player.Jump.WasReleasedThisFrame();
                case "Sprint":
                    return GetSprintUp();
                default:
                    return false;
            }
        }

        public bool GetAnyKeyDown()
        {
            return _inputAction.Player.Move.WasPressedThisFrame();
        }
    }
}