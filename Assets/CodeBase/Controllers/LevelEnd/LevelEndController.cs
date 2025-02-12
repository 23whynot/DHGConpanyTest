using System;
using System.Collections;
using CodeBase.Balls.Player;
using CodeBase.Sphere;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Controllers.LevelEnd
{
    public class LevelEndController : MonoBehaviour, ILevelEndController
    {
        [SerializeField] private UIController uiController;
        
        private bool _levelEnded;

        private IBallCountController _ballCountController;
        private SphereGenerator _sphereGenerator;

        [Inject]
        public void Construct(SphereGenerator sphereGenerator, IBallCountController ballCountController)
        {
            _sphereGenerator = sphereGenerator;
            _ballCountController = ballCountController;
        }

        public event Action OnLevelEnded;

        private void Start()
        {
            _sphereGenerator.OnAllZonesDestroyed += WinLevel;
            _ballCountController.OnBallsEnd += CheckLoseCondition;
        }

        private void WinLevel()
        {
            if (_levelEnded) return;
            _levelEnded = true;
            OnLevelEnded?.Invoke();
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
                OnLevelEnded?.Invoke();
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