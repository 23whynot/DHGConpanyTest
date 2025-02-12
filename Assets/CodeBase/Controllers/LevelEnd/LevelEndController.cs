using System.Collections;
using CodeBase.Balls.Player;
using CodeBase.Sphere;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Controllers.LevelEnd
{
    public class LevelEndController : MonoBehaviour
    {
        [SerializeField] private UIController uiController;
        private IBallCountController _ballCountController;
        private SphereGenerator _sphereGenerator;
        private bool _levelEnded;

        [Inject]
        public void Construct(SphereGenerator sphereGenerator, IBallCountController ballCountController)
        {
            _sphereGenerator = sphereGenerator;
            _ballCountController = ballCountController;
        }

        private void Start()
        {
            _sphereGenerator.OnAllZonesDestroyed += WinLevel; 
            _ballCountController.OnBallsEnd += CheckLoseCondition;
        }

        private void WinLevel()
        {
            if (_levelEnded) return;
            _levelEnded = true;
            uiController.ShowWinScreen();
        }

        private void CheckLoseCondition()
        {
            if (_levelEnded) return;
            StartCoroutine(WaitForLose());
        }

        private IEnumerator WaitForLose()
        {
            yield return new WaitForSeconds(3);
            if (!_levelEnded && _sphereGenerator.GetCountActiveZone() > 0)
            {
                _levelEnded = true;
                uiController.ShowLoseScreen();
            }
        }

        private void OnDestroy()
        {
            _sphereGenerator.OnAllZonesDestroyed -= WinLevel;
            _ballCountController.OnBallsEnd -= CheckLoseCondition;
        }
    }
}