using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Controllers.Renderer;
using CodeBase.Core.ObjPool;
using CodeBase.Services.RendererMaterialService;
using UnityEngine;
using Zenject;

namespace CodeBase.Balls.Player
{
    public class PlayerBall : MonoBehaviour, IPlayerBall, IPoolableObject
    {
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private Rigidbody rigidBody;
        
        private Color _ballColor;
        private List<Color> _colors;
        private IMaterialService _playerBallMaterialProvider;
        private IHudInputProvider _hudInputProvider;

        public bool IsActive { get; private set; }

        [Inject]
        public void Construct(IMaterialService playerBallMaterialProvider, IHudInputProvider hudInputProvider)
        {
            _playerBallMaterialProvider = playerBallMaterialProvider;
            _hudInputProvider = hudInputProvider;
        }

        private void Start() => _hudInputProvider.OnButtonClick += ChangeColor;
        
        private void OnDestroy() => _hudInputProvider.OnButtonClick -= ChangeColor;

        public Color GetColor() => meshRenderer.material.GetColor("_BaseColor"); //TODO

        public void ChangeColor() => meshRenderer.material = _playerBallMaterialProvider.GetActualMaterial();

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
            rigidBody.isKinematic = true;
            ChangeColor();
        }

        public void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
            rigidBody.isKinematic = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<SphereBall>(out SphereBall ball))
            {
                
            }
            else
            {
                Deactivate();
            }
        }
    }
}