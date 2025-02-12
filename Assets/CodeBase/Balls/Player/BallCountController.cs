using System;
using CodeBase.Sphere;
using Zenject;

namespace CodeBase.Balls.Player
{
    public class BallCountController : IBallCountController
    {
        public event Action<int> OnBallCountChanged;
        public event Action OnBallsEnd;

        private IColorOfZoneProvider _colorOfZoneProvider;
        private int _bonusBallCount;
        private int _ballsOnLevel;

        [Inject]
        public void Construct(IColorOfZoneProvider colorOfZoneProvider) => _colorOfZoneProvider = colorOfZoneProvider;

        public int GetBallCount() => _ballsOnLevel;

        public void OnShoot()
        {
            _ballsOnLevel--;

            if (_ballsOnLevel == 0) OnBallsEnd?.Invoke();

            OnBallCountChanged?.Invoke(_ballsOnLevel);
        }

        public void InitBallCount(int bonusBallCount)
        {
            _bonusBallCount = bonusBallCount;
            _ballsOnLevel = _colorOfZoneProvider.GetCountZone() + _bonusBallCount;
            OnBallCountChanged?.Invoke(_ballsOnLevel);
        }
    }
}