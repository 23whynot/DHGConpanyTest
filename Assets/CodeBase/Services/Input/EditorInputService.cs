using UnityEngine;
using UnityEngine.EventSystems; // Добавь этот using

namespace CodeBase.Services.Input
{
    public class EditorInputService : InputService
    {
        private const float MouseSensitivity = 2f;
        private bool _wasHolding = false;

        public override bool IsHolding()
        {
            
            if (IsPointerOverUI()) return false;

            bool isHoldingNow = UnityEngine.Input.GetMouseButton(0);

            if (_wasHolding && !isHoldingNow) InvokeReleaseEvent();

            _wasHolding = isHoldingNow;
            return isHoldingNow;
        }

        public override float GetHorizontal()
        {
            if (IsPointerOverUI()) return 0f;
            return UnityEngine.Input.GetAxis("Mouse X") * MouseSensitivity;
        }

        public override float GetVertical()
        {
            if (IsPointerOverUI()) return 0f;
            return UnityEngine.Input.GetAxis("Mouse Y") * MouseSensitivity;
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }
    }
}