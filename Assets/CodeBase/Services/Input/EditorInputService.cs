namespace CodeBase.Services.Input
{
    public class EditorInputService : InputService
    {
        private const float MouseSensitivity = 2f;
        private bool _wasHolding = false;

        public override bool IsHolding()
        {
            bool isHoldingNow = UnityEngine.Input.GetMouseButton(0);

            // Вызываем событие, если кнопка отпущена
            if (_wasHolding && !isHoldingNow)
            {
                InvokeReleaseEvent();
            }

            _wasHolding = isHoldingNow;
            return isHoldingNow;
        }

        public override float GetHorizontal()
        {
            return UnityEngine.Input.GetAxis("Mouse X") * MouseSensitivity;
        }

        public override float GetVertical()
        {
            return UnityEngine.Input.GetAxis("Mouse Y") * MouseSensitivity;
        }
    }
}