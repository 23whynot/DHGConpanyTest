using CodeBase.Balls.Player;
using CodeBase.Services.RendererMaterialService;
using CodeBase.Zone;
using UnityEngine;
using Zenject;


namespace CodeBase.Balls.Sphere
{
    public class SphereBall : MonoBehaviour, IDestroyableNotifier
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private Renderer meshRenderer;
        
        private IColorZone _zone;
        private IMaterialService _sphereBallMaterialProvider;

        [Inject]
        public void Construct(IMaterialService sphereBallMaterialProvider) => _sphereBallMaterialProvider = sphereBallMaterialProvider;


        public void Init(IColorZone zone)
        {
            _zone = zone;
            meshRenderer.material = _sphereBallMaterialProvider.GetMaterial(zone.Color);
        }

        public void ZoneDestroy()
        {
            rb.isKinematic = false;
            effect.Play();
            ChangeParentTransform(_zone.GetNonRotationalParent());
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IPlayerBall>(out IPlayerBall playerBall))
            {
                if (playerBall.GetColor() == _zone.Color) _zone.DestroyAllBallsInZone();
            }
            else
            {
                DeactivateBall();
            }
        }

        private void DeactivateBall() => gameObject.SetActive(false);

        private void ChangeParentTransform(Transform parent) => transform.SetParent(parent);
    }
}