using UnityEngine;

namespace CodeBase.Sphere
{
    public class PlayerBall : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Renderer renderer = GetComponent<Renderer>();
            Color ballColor = renderer.material.color;

            ZoneDestroyer zoneManager = FindObjectOfType<ZoneDestroyer>();
            if (zoneManager != null)
            {
                zoneManager.DestroyZone(ballColor);
            }

            Destroy(gameObject); 
        }
    }
}