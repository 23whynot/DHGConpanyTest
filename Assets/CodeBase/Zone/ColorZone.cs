using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Sphere;
using UnityEngine;

namespace CodeBase.Zone
{
    public class ColorZone : IDestroyColorZone 
    {
        public Vector3 center;
        public Color color;
        
        private List<GameObject> _ballsInZone = new List<GameObject>();

        public ColorZone(Vector3 center, Color color)
        {
            this.center = center;
            this.color = color;
        }

        public void RegisterBall(GameObject ball)
        {
            _ballsInZone.Add(ball);
        }

        public void DestroyAllBallsInZone()
        {
            foreach (GameObject ball in _ballsInZone)
            {
                IDestroyableNotifier destroyable = ball.GetComponent<IDestroyableNotifier>();
                destroyable.ZoneDestroed();
            }
            _ballsInZone.Clear();
        }
    }
}