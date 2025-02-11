using CodeBase.Services;

public abstract class InputService : IInputService
{
    public abstract bool IsHolding();
    public abstract bool IsRelease();
    public abstract float GetHorizontal();
    public abstract float GetVertical();
}