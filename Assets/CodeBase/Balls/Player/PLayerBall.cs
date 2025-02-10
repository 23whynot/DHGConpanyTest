using System.Collections.Generic;
using CodeBase.Core.ObjPool;
using CodeBase.Sphere;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Balls.Player
{
    public class PlayerBall : MonoBehaviour, IPlayerBall, IPoolableObject
    {
        [SerializeField] private Renderer renderer;
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

        private void Start()
        {
            _colors = _colorOfZoneProvider.GetColorOfZone();
            _ballColor = GetRandomColor();
            
            renderer.material = new Material(renderer.material);
            renderer.material.SetColor(BaseColor, _ballColor);
        }

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        private Color GetRandomColor() => _colors[Random.Range(0, _colors.Count)];

        public Color GetColor() => _ballColor;

        public void Destroy()
        {
            Deactivate();
        }
    }
}