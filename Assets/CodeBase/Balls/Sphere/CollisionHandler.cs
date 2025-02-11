using CodeBase.Balls.Player;
using CodeBase.Zone;
using UnityEngine;

namespace CodeBase.Balls.Sphere
{
    public class CollisionHandler : MonoBehaviour
    {
        private IDestroyColorZone _zone;

        public void Initialize(IDestroyColorZone zone)
        {
            _zone = zone;
        }

        public void HandleCollision(Collision other, SphereBall sphereBall)
        {
            if (other.gameObject.TryGetComponent<IPlayerBall>(out IPlayerBall playerBall))
            {
                if (playerBall.GetColor() == _zone.color)
                {
                    _zone.DestroyAllBallsInZone();
                }
            }
            else
            {
                sphereBall.DeactivateBall();
            }
        }
    }
}