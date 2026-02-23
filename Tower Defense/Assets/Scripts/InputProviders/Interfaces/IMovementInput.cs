using UnityEngine;

namespace GameSystemsPackage.Movement.Interfaces
{
    public interface IMovementInput
    {
        Vector2 GetMovementInput();
        bool GetJumpInput();
        bool GetSprintInput();
        Vector2 GetLookInput();
        bool GetMouseDownInput(int id);
        bool GetMouseInput(int id);
        bool GetInteractionKey();
        float GetMouseY();
        float GetMouseX();
        float GetMouseScroll();
    }
}