using CodeBase.Controllers.Renderer;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Hud.Input
{
    public class HudInputController : MonoBehaviour
    {
        [Header("Hud кнопки")] 
        [SerializeField] private Button _changeColorButton;

        private IChangeColorButtonListener _changeColorButtonListener;

        [Inject]
        public void Construct(IChangeColorButtonListener changeColorButtonListener) => _changeColorButtonListener = changeColorButtonListener;

        private void Start() => _changeColorButton.onClick.AddListener(_changeColorButtonListener.OnClick);

        private void OnDestroy() => _changeColorButton.onClick.RemoveListener(_changeColorButtonListener.OnClick);
    }
}