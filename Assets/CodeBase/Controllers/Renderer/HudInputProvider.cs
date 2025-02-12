using System;
using CodeBase.Balls.Player;
using Zenject;

namespace CodeBase.Controllers.Renderer
{
    public class HudInputProvider : IHudInputProvider ,IChangeColorButtonListener
    {
        public event Action OnButtonClick;
        public void OnClick() => OnButtonClick?.Invoke();
    }
}