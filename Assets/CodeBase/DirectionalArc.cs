using UnityEngine;

namespace CodeBase
{
    public class DirectionalArc : MonoBehaviour
    {
        public GameObject ballPrefab;
        public Transform spawnPoint;
        public float initialSpeed = 20f;
        public ProjectileTrajectory projectileTrajectory;  // Ссылка на компонент расчета траектории

        public float horizontalAdjustmentSpeed = 120f;
        public float verticalAdjustmentSpeed = 300f;

        private float horizontalAngleOffset = 0f;
        private float verticalAngleOffset = 0f;

        void Start()
        {
            projectileTrajectory.SetupTrajectory(spawnPoint, initialSpeed);
            projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
        }

        void Update()
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
                verticalAngleOffset = Mathf.Clamp(verticalAngleOffset - verticalAdjustmentSpeed * Time.deltaTime, -180f, 180f);
                projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            }
            if (Input.GetKey(KeyCode.S))
            {
                verticalAngleOffset = Mathf.Clamp(verticalAngleOffset + verticalAdjustmentSpeed * Time.deltaTime, -180f, 180f);
                projectileTrajectory.DrawTrajectory(horizontalAngleOffset, verticalAngleOffset);
            }

            // Запуск шарика
            if (Input.GetMouseButtonDown(0))
            {
                LaunchBall();
            }
        }

        void LaunchBall()
        {
            GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 velocity = projectileTrajectory.CalculateLaunchVelocity(horizontalAngleOffset, verticalAngleOffset);
            rb.velocity = velocity;
        }
    }
}