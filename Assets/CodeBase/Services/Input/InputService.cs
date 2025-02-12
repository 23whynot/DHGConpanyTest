using System;

namespace CodeBase.Services.Input
{
    public abstract class InputService : IInputService
    {
        public event Action OnRelease;  

        public abstract bool IsHolding();
        public abstract float GetHorizontal();
        public abstract float GetVertical();

        
        protected void InvokeReleaseEvent()
        {
            OnRelease?.Invoke();
        }
    }
}