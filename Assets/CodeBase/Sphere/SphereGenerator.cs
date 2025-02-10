using System.Collections.Generic;
using CodeBase.Balls.Sphere;
using CodeBase.Zone;
using UnityEngine;

namespace CodeBase.Sphere
{
    public class SphereGenerator : MonoBehaviour, IColorOfZoneProvider
    {
        [SerializeField] private GameObject ballPrefab; // Префаб шарика

        [Header("Настройки генерации")] [Tooltip("Количество слоёв сфер (минимум 1, максимум 3)")] [Range(1, 3)]
        public int layerCount = 3; // Количество слоёв сфер

        [Tooltip("Цвета зон. Каждый цвет соответствует одной зоне на сферах")]
        public List<Color> zoneColors = new List<Color>(); // Список цветов зон

        private int baseBallCount = 350; // Базовое количество шариков для внешнего слоя
        private float outerSphereRadius = 6f; // Радиус внешней сферы
        private float innerSphereRadius = 3f; // Радиус внутренней сферы
        private float noiseScale = 3f; // Масштаб шума для неровностей границ зон
        private float borderWidth = 0.4f; // Ширина размытых границ между зонами
        private IColorOfZoneProvider _colorOfZoneProviderImplementation;

        private void Start() => GenerateLayers();

        public List<Color> GetColorOfZone() => zoneColors;

        private void GenerateLayers()
        {
            // Рассчитываем шаг радиуса между слоями
            float radiusStep = (outerSphereRadius - innerSphereRadius) / Mathf.Max(1, layerCount - 1);

            for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
            {
                // Радиус текущего слоя
                float currentRadius = outerSphereRadius - (radiusStep * layerIndex);

                // Рассчитываем количество шариков для текущего слоя
                int ballCountForLayer =
                    Mathf.CeilToInt(baseBallCount * Mathf.Pow(currentRadius / outerSphereRadius, 2));

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
                zones.Add(new ColorZone(randomPos, zoneColors[i]));
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
                GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, transform);

                // Определяем зону на основе позиции шарика и шума
                ColorZone zone = GetZoneByPosition(position, zones);

                // Устанавливаем ссылку на зону в компонент SphereBall
                SphereBall sphereBall = ball.GetComponent<SphereBall>();
                sphereBall.SetZoneAndColor(zone, zone.color); // Связываем шарик с зоной

                // Регистрируем шарик в соответствующей зоне
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
                float noise = Mathf.PerlinNoise(pos.x * noiseScale, pos.y * noiseScale) * 2 - 1;
                float distance = Vector3.Distance(pos, zone.center) + noise * borderWidth;

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