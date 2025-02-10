using UnityEngine;

namespace CodeBase
{
    public class ProjectileTrajectory : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public int resolution = 100;

        private Transform spawnPoint;
        private float initialSpeed;

        public void SetupTrajectory(Transform spawn, float speed)
        {
            spawnPoint = spawn;
            initialSpeed = speed;

            // Настройка LineRenderer
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.01f;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.yellow, 0.0f),
                    new GradientColorKey(Color.clear, 1.0f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            );
            lineRenderer.colorGradient = gradient;
        }

        public void DrawTrajectory(float horizontalAngleOffset, float verticalAngleOffset)
        {
            Vector3[] points = new Vector3[resolution];
            Vector3 startPosition = spawnPoint.position;
            Vector3 velocity = CalculateLaunchVelocity(horizontalAngleOffset, verticalAngleOffset);

            for (int i = 0; i < resolution; i++)
            {
                float t = (i / (float)resolution) * 2f;
                Vector3 point = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;
                points[i] = point;
            }

            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }

        public Vector3 CalculateLaunchVelocity(float horizontalAngleOffset, float verticalAngleOffset)
        {
            Vector3 baseDirection = Vector3.forward;

            // Поворот горизонтального направления
            Quaternion horizontalRotation = Quaternion.Euler(0, horizontalAngleOffset, 0);
            Vector3 adjustedDirection = horizontalRotation * baseDirection;

            // Добавляем вертикальную составляющую
            float verticalRad = Mathf.Deg2Rad * verticalAngleOffset;
            adjustedDirection.y = Mathf.Sin(verticalRad);

            // Нормализуем и умножаем на начальную скорость
            adjustedDirection = adjustedDirection.normalized * initialSpeed;

            return adjustedDirection;
        }
    }
}
