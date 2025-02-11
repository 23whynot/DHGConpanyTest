using CodeBase.Factory;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase
{
    public class DirectionalArc : MonoBehaviour
    {
        [Header("Ball Configuration")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float initialSpeed = 50f;
        
        [Header("Trajectory Settings")]
        [SerializeField] private ProjectileTrajectory projectileTrajectory;
        [SerializeField] private float horizontalAdjustmentSpeed = 60f;
        [SerializeField] private float verticalAdjustmentSpeed = 30f;

        private GameObject _currentBall;
        private bool _firstPress = true;
        private float _horizontalAngleOffset;
        private float _verticalAngleOffset;

        private IGameFactory _gameFactory;
        private IInputService _inputService;

        [Inject]
        public void Construct(IGameFactory gameFactory, IInputService inputService)
        {
            _gameFactory = gameFactory;
            _inputService = inputService;
        }

        private void Awake() => _gameFactory.Init();

        private void Start()
        {
            SpawnBall();
            projectileTrajectory.SetupTrajectory(spawnPoint, initialSpeed);
            projectileTrajectory.DrawTrajectory(_horizontalAngleOffset, _verticalAngleOffset);
            _inputService.OnRelease += LaunchBall;
        }

        private void Update()
        {
            if (!_inputService.IsHolding()) return;
            
            if (_firstPress)
            {
                _verticalAngleOffset = 10f;
                _firstPress = false;
            }
            else
            {
                _verticalAngleOffset = Mathf.Clamp(_verticalAngleOffset + _inputService.GetVertical() * verticalAdjustmentSpeed * Time.deltaTime, -45f, 45f);
            }
            
            _horizontalAngleOffset += _inputService.GetHorizontal() * horizontalAdjustmentSpeed * Time.deltaTime;
            
            projectileTrajectory.DrawTrajectory(_horizontalAngleOffset, _verticalAngleOffset);
        }

        private void OnDestroy() => _inputService.OnRelease -= LaunchBall;

        private void LaunchBall()
        {
            
            Rigidbody rb = _currentBall.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = projectileTrajectory.CalculateLaunchVelocity(_horizontalAngleOffset, _verticalAngleOffset);
            
            SpawnBall();
            
            _firstPress = true;
        }

        private void SpawnBall() => _currentBall = _gameFactory.CreatePlayerBall(spawnPoint);
    }
}
