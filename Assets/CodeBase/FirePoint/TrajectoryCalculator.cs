using UnityEngine;

namespace CodeBase.FirePoint
{
    public class TrajectoryCalculator
    {
        public static void FillTrajectoryPoints(Vector3 startPosition, Vector3 velocity, Vector3[] points, float maxTime)
        {
            int resolution = points.Length;
            for (int i = 0; i < resolution; i++)
            {
                float t = (i / (float)resolution) * maxTime;
                points[i] = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;
            }
        }
    }
}