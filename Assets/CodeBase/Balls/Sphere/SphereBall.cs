using CodeBase.Zone;
using UnityEngine;

namespace CodeBase.Balls.Sphere
{
    public class SphereBall : MonoBehaviour, IDestroyableNotifier
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private CollisionHandler collisionHandler;

        private IDestroyColorZone _zone;
        private MaterialController _materialController;

        private void Awake() => _materialController = new MaterialController(meshRenderer);

        public void Init(IDestroyColorZone zone, Material material)
        {
            _zone = zone;
            
            collisionHandler.Initialize(zone);
            
            _materialController.SetMaterial(material);
        }

        public void DeactivateBall() => gameObject.SetActive(false);

        public void ZoneDestroy()
        {
            rb.isKinematic = false;
            effect.Play();
            ChangeParentTransform(_zone.GetNonRotationalParent());
        }

        private void OnCollisionEnter(Collision other)
        {
            collisionHandler.HandleCollision(other, this);
        }

        private void ChangeParentTransform(Transform parent) => transform.SetParent(parent);
    }
}