using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Balls.Sphere;
using CodeBase.Services.RendererMaterialService;
using CodeBase.Zone;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Sphere
{
    public class SphereGenerator : MonoBehaviour, IColorOfZoneProvider, ICoroutineRunner
    {
        [SerializeField] private SphereBall ballPrefab;
        [SerializeField] private Transform nonRotationalParent;
        [SerializeField] private Material material;
        [Range(1, 3)]
        [SerializeField] private int layerCount = 3;
        [SerializeField] private List<Color> zoneColors = new List<Color>();
        
        private const int BaseBallsPerLayer = 350;
        private const float OuterSphereRadius = 6f;
        private const float InnerSphereRadius = 3f;
        private const float NoiseScale = 3f;
        private const float BorderWidth = 0.4f;
        private static readonly float GoldenAngle = Mathf.PI * (1 + Mathf.Sqrt(5));
        
        public event Action OnAllZonesDestroyed;

        private IMaterialService _materialService;
        private DiContainer _diContainer;
        private List<ColorZone> _activeZones = new List<ColorZone>();

        [Inject]
        public void Construct(IMaterialService materialService, DiContainer diContainer)
        {
            _materialService = materialService;
            _diContainer = diContainer;
        }

        private void Awake() => _materialService.Init(material, layerCount);
        private void Start() => GenerateLayers();
        public List<Color> GetColorsOfZone() => zoneColors;
        public int GetCountZone() => zoneColors.Count * layerCount;
        public int GetCountActiveZone() => _activeZones.Count; 

        private void GenerateLayers()
        {
            float radiusStep = (OuterSphereRadius - InnerSphereRadius) / Mathf.Max(1, layerCount - 1);

            for (int i = 0; i < layerCount; i++)
            {
                float currentRadius = OuterSphereRadius - radiusStep * i;
                int ballCount = Mathf.CeilToInt(BaseBallsPerLayer * Mathf.Pow(currentRadius / OuterSphereRadius, 2));
                GenerateSphere(currentRadius, ballCount, CreateZones());
            }
        }

        private List<ColorZone> CreateZones()
        {
            var zones = zoneColors.Select(color =>
                new ColorZone(Random.onUnitSphere * 2f, color, nonRotationalParent, this, _materialService)).ToList();
        
            _activeZones.AddRange(zones); 
            zones.ForEach(zone => zone.OnZoneDestroyed += ZoneDestroyed);
            return zones;
        }

        private void GenerateSphere(float radius, int ballCount, List<ColorZone> zones)
        {
            foreach (var position in GenerateSpherePositions(ballCount, radius))
            {
                ColorZone zone = GetClosestZone(position, zones);
                SphereBall ballInstance = _diContainer.InstantiatePrefabForComponent<SphereBall>(
                    ballPrefab, position, Quaternion.identity, transform);
                ballInstance.Init(zone);
                zone.RegisterBall(ballInstance);
            }
        }

        private IEnumerable<Vector3> GenerateSpherePositions(int ballCount, float radius)
        {
            for (int i = 0; i < ballCount; i++)
            {
                float theta = Mathf.Acos(1 - 2f * (i + 0.5f) / ballCount);
                float phi = GoldenAngle * i;
                yield return radius * new Vector3(
                    Mathf.Sin(theta) * Mathf.Cos(phi),
                    Mathf.Sin(theta) * Mathf.Sin(phi),
                    Mathf.Cos(theta)
                );
            }
        }

        private ColorZone GetClosestZone(Vector3 position, List<ColorZone> zones)
        {
            return zones
                .OrderBy(zone => Vector3.Distance(position, zone.Center) +
                    (Mathf.PerlinNoise(position.x * NoiseScale, position.y * NoiseScale) * 2 - 1) * BorderWidth)
                .First();
        }

        private void ZoneDestroyed(ColorZone zone)
        {
            _activeZones.Remove(zone); 
            if (_activeZones.Count == 0)
            {
                OnAllZonesDestroyed?.Invoke(); 
            }
        }
    }
}
