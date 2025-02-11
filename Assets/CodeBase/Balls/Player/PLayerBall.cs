using System.Collections.Generic;
using CodeBase.Balls.Sphere;
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


        public bool IsActive { get; private set; }
        
        [Inject]
        public void Construct(IMaterialService playerBallMaterialProvider) => _playerBallMaterialProvider = playerBallMaterialProvider;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<SphereBall>(out SphereBall ball)) Deactivate();
        }

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
            rigidBody.isKinematic = true;
            meshRenderer.material = _playerBallMaterialProvider.GetActualMaterial();
        }

        public void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
            rigidBody.isKinematic = false;
        }
        
        public Color GetColor() => meshRenderer.material.GetColor("_BaseColor"); //TODO
    }
}