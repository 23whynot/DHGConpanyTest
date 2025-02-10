using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Sphere
{
    public class ZoneDestroyer : MonoBehaviour
    {
        private List<GameObject> ballsInZone = new List<GameObject>();

        public void RegisterBall(GameObject ball)
        {
            ballsInZone.Add(ball);
        }

        public void DestroyZone(Color color)
        {
            foreach (GameObject ball in ballsInZone)
            {
                if (ball != null && ball.GetComponent<Renderer>().material.color == color)
                {
                    Rigidbody rb = ball.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.AddExplosionForce(200f, ball.transform.position, 5f);
                    }
                    Destroy(ball, 2f);
                }
            }

            ballsInZone.Clear();
        }
    }
}
