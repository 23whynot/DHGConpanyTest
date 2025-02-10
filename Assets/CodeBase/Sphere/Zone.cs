using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Sphere
{
    public class Zone
    {
        public Vector3 center;
        public Color color;
        private List<GameObject> ballsInZone = new List<GameObject>();

        public Zone(Vector3 center, Color color)
        {
            this.center = center;
            this.color = color;
        }

        public void RegisterBall(GameObject ball)
        {
            ballsInZone.Add(ball);
        }

        public void DestroyAllBalls()
        {
            foreach (GameObject ball in ballsInZone)
            {
                if (ball != null)
                {
                    Destroy(ball);
                }
            }
            ballsInZone.Clear();
        }

        private void Destroy(GameObject ball)
        {
            
        }
    }
}