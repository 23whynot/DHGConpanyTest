using CodeBase.Balls.Player;
using CodeBase.Zone;
using UnityEngine;

namespace CodeBase.Balls.Sphere
{
    public class SphereBall : MonoBehaviour, IDestroyableNotifier
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Renderer renderer;
        private IDestroyColorZone _zone;
        private Color _color;

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        public void Init(IDestroyColorZone zone, Material material)
        {
            _zone = zone;

            renderer.material = material;
        }

        private void OnCollisionEnter(Collision other)
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
                gameObject.SetActive(false);
            }
            
            
        }

        private void ChangeParentTransform(Transform parent) => transform.SetParent(parent);

        public void ZoneDestroy()
        {
            rb.isKinematic = false;
            ChangeParentTransform(_zone.GetNonRotationalParent());
        }
    }
}