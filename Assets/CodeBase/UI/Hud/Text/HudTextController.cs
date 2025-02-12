using CodeBase.Balls.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Hud.Text
{
    public class HudTextController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ballsLeftText;
        
        private const string BallLeftText = "Balls Left:";
        private IBallCountController _ballCountController;
        
        private int _count;

        [Inject]
        public void Construct(IBallCountController ballCountController) => _ballCountController = ballCountController;

        private void Awake()
        {
            _count = _ballCountController.GetBallCount();
            ChangeValue(_count);
            
            _ballCountController.OnBallCountChanged += ChangeValue;
        }

        private void OnDestroy() => _ballCountController.OnBallCountChanged -= ChangeValue;

        private void ChangeValue(int count) => ballsLeftText.text = BallLeftText  + count.ToString();
    }
}