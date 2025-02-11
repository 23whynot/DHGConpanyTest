using System;

namespace CodeBase.Services
{
    public interface IInputService
    {
        bool IsHolding();
        event Action OnRelease;  
        float GetHorizontal();
        float GetVertical();
    }
}