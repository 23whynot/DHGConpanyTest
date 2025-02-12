using System;

namespace CodeBase.Controllers.Renderer
{
    public interface IHudInputProvider
    {
        event Action OnButtonClick;
    }
}