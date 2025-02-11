using CodeBase.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase
{
    public class DirectionalArc : MonoBehaviour
    {
        [Header("Ball Configuration")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float initialSpeed = 50f;

        [Header("Trajectory Settings")]
        [SerializeField] private ProjectileTrajectory projectileTrajectory; 
        [SerializeField] private float horizontalAdjustmentSpeed = 60;
        [SerializeField] private float verticalAdjustmentSpeed = 30f;

        private float horizontalAngleOffset = 0f;
        private float verticalAngleOffset = 0f;
        private GameObject currentBall;
        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Awake() => InitGameFactory();

        private void Start()
        {
            SpawnBall();
            InitializeTrajectory();
        }

        private void Update() => HandleInput();

        private void InitializeTrajectory()
        {
            projectileTrajectory.SetupTrajectory(spawnPoint, initialSpeed);
            projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
        }


        private void HandleInput()
        {
            HandleHorizontalInput();
            HandleVerticalInput();

            if (Input.GetMouseButtonDown(0))
            {
                LaunchBall();
            }
        }

        private void HandleHorizontalInput()
        {
            if (Input.GetKey(KeyCode.A))
            {
                AdjustHorizontalAngle(-horizontalAdjustmentSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                AdjustHorizontalAngle(horizontalAdjustmentSpeed);
            }
        }

        private void HandleVerticalInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                AdjustVerticalAngle(-verticalAdjustmentSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                AdjustVerticalAngle(verticalAdjustmentSpeed);
            }
        }

        private void AdjustHorizontalAngle(float adjustment)
        {
            horizontalAngleOffset += adjustment * Time.deltaTime;
            projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
        }

        private void AdjustVerticalAngle(float adjustment)
        {
            verticalAngleOffset = Mathf.Clamp(verticalAngleOffset + adjustment * Time.deltaTime, -180f, 180f);
            projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
        }

        private void LaunchBall()
        {
            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            
            rb.isKinematic = false;
            rb.velocity = projectileTrajectory.CalculateLaunchVelocity(horizontalAngleOffset, verticalAngleOffset);
            
            SpawnBall();
        }

        private void SpawnBall() => currentBall = _gameFactory.CreatePlayerBall(at: spawnPoint);

        private void InitGameFactory() => _gameFactory.Init();
    }
}
