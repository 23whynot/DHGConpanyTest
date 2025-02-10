using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Zone;
using UnityEngine;

namespace CodeBase.Sphere
{
    public class SphereGenerator : MonoBehaviour, IColorOfZoneProvider
    {
        [SerializeField] private SphereBall ballPrefab; 
        [SerializeField] private Transform nonRotationalParent;
        [SerializeField] private Material material;

        [Header("Настройки генерации")]
        [Range(1, 3)] public int layerCount = 3; // Количество слоёв сфер

        [Tooltip("Цвета зон. Каждый цвет соответствует одной зоне на сферах")]
        public List<Color> zoneColors = new List<Color>();

        private readonly int _baseBallCount = 350; 
        private readonly float _outerSphereRadius = 6f; 
        private readonly float _innerSphereRadius = 3f; 
        private readonly float _noiseScale = 3f; 
        private readonly float _borderWidth = 0.4f;
        

        private void Start() => GenerateLayers();

        public List<Color> GetColorOfZone() => zoneColors;

        private void GenerateLayers()
        {
            // Рассчитываем шаг радиуса между слоями
            float radiusStep = (_outerSphereRadius - _innerSphereRadius) / Mathf.Max(1, layerCount - 1);

            for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
            {
                // Радиус текущего слоя
                float currentRadius = _outerSphereRadius - (radiusStep * layerIndex);

                // Рассчитываем количество шариков для текущего слоя
                int ballCountForLayer =
                    Mathf.CeilToInt(_baseBallCount * Mathf.Pow(currentRadius / _outerSphereRadius, 2));

                // Генерируем зоны для текущего слоя
                List<ColorZone> currentLayerZones = new List<ColorZone>();
                GenerateZones(currentLayerZones);

                // Генерируем шарики на текущем слое
                GenerateSphere(currentRadius, ballCountForLayer, currentLayerZones);
            }
        }

        private void GenerateZones(List<ColorZone> zones)
        {
            zones.Clear();

            // Создаём зону для каждого цвета из списка
            for (int i = 0; i < zoneColors.Count; i++)
            {
                // Случайная позиция на поверхности сферы
                Vector3 randomPos = Random.onUnitSphere * 2.0f;
                zones.Add(new ColorZone(randomPos, zoneColors[i], nonRotationalParent, new Material(material)));
            }
        }

        private void GenerateSphere(float radius, int count, List<ColorZone> zones)
        {
            for (int i = 0; i < count; i++)
            {
                // Алгоритм Фибоначчи для равномерного распределения шариков
                float theta = Mathf.Acos(1 - 2 * (i + 0.5f) / count);
                float phi = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;

                // Позиция шарика на сфере
                Vector3 position = new Vector3(
                    radius * Mathf.Sin(theta) * Mathf.Cos(phi),
                    radius * Mathf.Sin(theta) * Mathf.Sin(phi),
                    radius * Mathf.Cos(theta)
                );

                // Создаём шарик в сцене
                SphereBall ball = Instantiate(ballPrefab, position, Quaternion.identity, transform);

                // Определяем зону на основе позиции шарика и шума
                ColorZone zone = GetZoneByPosition(position, zones);

                ball.Init(zone, zone.GetMaterial()); // Связываем шарик с зоной
                
                zone.RegisterBall(ball);
            }
        }

        private ColorZone GetZoneByPosition(Vector3 pos, List<ColorZone> zones)
        {
            float bestMatch = float.MaxValue;
            ColorZone selectedZone = null;

            foreach (ColorZone zone in zones)
            {
                // Генерация шума для плавных границ зон
                float noise = Mathf.PerlinNoise(pos.x * _noiseScale, pos.y * _noiseScale) * 2 - 1;
                float distance = Vector3.Distance(pos, zone.center) + noise * _borderWidth;

                if (distance < bestMatch)
                {
                    bestMatch = distance;
                    selectedZone = zone;
                }
            }

            return selectedZone; // Возвращаем ближайшую зону
        }
    }
}