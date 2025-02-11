using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Core.ObjPool;
using CodeBase.Sphere;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Balls.Player
{
    public class PlayerBall : MonoBehaviour, IPlayerBall, IPoolableObject
    {
        [SerializeField] private Renderer meshRenderer;
        [SerializeField] private Rigidbody rigidBody;
        
        private Color _ballColor;
        private IColorOfZoneProvider _colorOfZoneProvider;
        private List<Color> _colors;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        public bool IsActive { get; private set; }
        
        [Inject]
        public void Construct(IColorOfZoneProvider colorOfZoneProvider)
        {
            _colorOfZoneProvider = colorOfZoneProvider;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent<SphereBall>(out SphereBall ball)) Deactivate();
        }

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
            rigidBody.isKinematic = true;
            InitializeMaterialColor();
        }

        public void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
            rigidBody.isKinematic = false;
        }

        private Color GetRandomColor() => _colors[Random.Range(0, _colors.Count)];

        private void InitializeMaterialColor()
        {
            _colors = _colorOfZoneProvider.GetColorOfZone();
            _ballColor = GetRandomColor();
            
            meshRenderer.material = new Material(meshRenderer.material);
            meshRenderer.material.SetColor(BaseColor, _ballColor); //TODO
        }

        public Color GetColor() => _ballColor;

        public void Destroy() => Deactivate();
        
    }
    
}