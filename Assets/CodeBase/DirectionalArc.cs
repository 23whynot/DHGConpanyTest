using CodeBase.Balls.Player;
using CodeBase.Core.ObjPool;
using UnityEngine;
using Zenject;

namespace CodeBase
{
    public class DirectionalArc : MonoBehaviour
    {
        public GameObject ballPrefab;
        public Transform spawnPoint;
        public float initialSpeed = 20f;
        public ProjectileTrajectory projectileTrajectory; // Ссылка на компонент расчета траектории

        public float horizontalAdjustmentSpeed = 120f;
        public float verticalAdjustmentSpeed = 300f;

        private float horizontalAngleOffset = 0f;
        private float verticalAngleOffset = 0f;
        
        private int _preLoadCount = 5;
        private ObjectPool _pool;

        [Inject]
        public void Construct(ObjectPool pool)
        {
            _pool = pool;
        }
        private void Start()
        {
            projectileTrajectory.SetupTrajectory(spawnPoint, initialSpeed);
            projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            
            _pool.RegisterPrefab<PlayerBall>(ballPrefab ,_preLoadCount);
        }

        private void Update()
        {
            // Управление поворотом влево и вправо
            if (Input.GetKey(KeyCode.A))
            {
                horizontalAngleOffset -= horizontalAdjustmentSpeed * Time.deltaTime;
                projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            }

            if (Input.GetKey(KeyCode.D))
            {
                horizontalAngleOffset += horizontalAdjustmentSpeed * Time.deltaTime;
                projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            }

            // Управление вертикальным углом вверх/вниз
            if (Input.GetKey(KeyCode.W))
            {
                verticalAngleOffset = Mathf.Clamp(verticalAngleOffset - verticalAdjustmentSpeed * Time.deltaTime, -180f,
                    180f);
                projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            }

            if (Input.GetKey(KeyCode.S))
            {
                verticalAngleOffset = Mathf.Clamp(verticalAngleOffset + verticalAdjustmentSpeed * Time.deltaTime, -180f,
                    180f);
                projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            }

            // Запуск шарика
            if (Input.GetMouseButtonDown(0))
            {
                LaunchBall();
            }
        }

        private void LaunchBall()
        {

            PlayerBall ball = _pool.GetObject<PlayerBall>();
            ball.transform.position = spawnPoint.position;
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 velocity = projectileTrajectory.CalculateLaunchVelocity(horizontalAngleOffset, verticalAngleOffset);
            rb.velocity = velocity;
            ball.Activate();
        }
    }
}