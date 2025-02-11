using System;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        public event Action OnRelease;  // Событие для отпускания кнопки

        public abstract bool IsHolding();
        public abstract float GetHorizontal();
        public abstract float GetVertical();

        // Метод для вызова события
        protected void InvokeReleaseEvent()
        {
            OnRelease?.Invoke();
        }
    }
}