using System.Collections.Generic;
using UnityEngine;
using CodeBase.Balls.Sphere;
using CodeBase.Zone;
using System.Linq;

namespace CodeBase.Sphere
{
    public class SphereGenerator : MonoBehaviour, IColorOfZoneProvider
    {
        [SerializeField] private SphereBall ballPrefab;
        [SerializeField] private Transform nonRotationalParent;
        [SerializeField] private Material material;
        
        [Range(1, 3)] public int layerCount = 3;
        public List<Color> zoneColors = new List<Color>();

        private const int BaseBallCount = 350;
        private const float OuterSphereRadius = 6f;
        private const float InnerSphereRadius = 3f;
        private const float NoiseScale = 3f;
        private const float BorderWidth = 0.4f;

        private void Start() => GenerateLayers();

        public List<Color> GetColorOfZone() => zoneColors;

        private void GenerateLayers()
        {
            float radiusStep = (OuterSphereRadius - InnerSphereRadius) / Mathf.Max(1, layerCount - 1);

            for (int i = 0; i < layerCount; i++)
            {
                float currentRadius = OuterSphereRadius - radiusStep * i;
                int ballCount = Mathf.CeilToInt(BaseBallCount * Mathf.Pow(currentRadius / OuterSphereRadius, 2));
                GenerateSphere(currentRadius, ballCount, CreateZones());
            }
        }

        private List<ColorZone> CreateZones() => zoneColors.Select(color => new ColorZone(Random.onUnitSphere * 2f, color, nonRotationalParent, new Material(material))).ToList();

        private void GenerateSphere(float radius, int ballCount, List<ColorZone> zones)
        {
            foreach (var position in GenerateSpherePositions(ballCount, radius))
            {
                ColorZone zone = GetClosestZone(position, zones);

                SphereBall ballInstance = Instantiate(ballPrefab, position, Quaternion.identity, transform);
                ballInstance.Init(zone, zone.GetMaterial());
                zone.RegisterBall(ballInstance);
            }
        }

        private IEnumerable<Vector3> GenerateSpherePositions(int ballCount, float radius)
        {
            for (int i = 0; i < ballCount; i++)
            {
                float theta = Mathf.Acos(1 - 2f * (i + 0.5f) / ballCount);
                float phi = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;
                yield return radius * new Vector3(
                    Mathf.Sin(theta) * Mathf.Cos(phi),
                    Mathf.Sin(theta) * Mathf.Sin(phi),
                    Mathf.Cos(theta)
                );
            }
        }

        private ColorZone GetClosestZone(Vector3 position, List<ColorZone> zones)
        {
            ColorZone closestZone = null;
            float minDistance = float.MaxValue;

            foreach (var zone in zones)
            {
                float distance = Vector3.Distance(position, zone.center) +
                                 (Mathf.PerlinNoise(position.x * NoiseScale, position.y * NoiseScale) * 2 - 1) * BorderWidth;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestZone = zone;
                }
            }

            return closestZone;
        }
    }
}
