using CodeBase.Balls.Player;
using CodeBase.Sphere;
using UnityEngine;

namespace CodeBase.Balls.Sphere
{
    public class SphereBall : MonoBehaviour, IDestroyableNotifier
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Renderer renderer;
        private IDestroyColorZone _zone;
        private Color _color;
        

        public void SetZoneAndColor(IDestroyColorZone zone, Color color)
        {
            _zone = zone;
            _color = color;
            
            renderer.material.color = _color;
        }

        private void OnCollisionEnter(Collision other)
        {
            
            if (other.gameObject.TryGetComponent<IPlayerBall>(out IPlayerBall playerBall))
            {
                if (playerBall.GetColor().linear == _color.linear) _zone.DestroyAllBallsInZone();
                
              //  playerBall.Destroy();
            }
        }

        public void ZoneDestroed()
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.down, ForceMode.Impulse);
        }
    }
}