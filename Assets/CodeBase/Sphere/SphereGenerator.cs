using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Sphere
{
    public class SphereGenerator : MonoBehaviour
    {
        
        [SerializeField] private GameObject ballPrefab;
        [Header("Настройки генерации")]
        [Tooltip("Количество слоёв сфер (минимум 1, максимум 3)")]
        [Range(1, 3)] public int layerCount = 3;

        [Tooltip("Цвета зон. Каждый цвет соответствует одной зоне на сферах")]
        public List<Color> zoneColors = new List<Color>();

        private List<List<Zone>> sphereLayers = new List<List<Zone>>();
        private int baseBallCount = 500; // Базовое количество шариков для внешнего слоя
        private float outerSphereRadius = 7f; // Радиус внешней сферы
        private float innerSphereRadius = 5f; // Радиус внутренней сферы
        private float noiseScale = 3f; // Масштаб шума для неровных границ зон
        private float borderWidth = 0.4f; // Ширина размытых границ между зонами

        private void Start()
        {
            if (zoneColors.Count == 0)
            {
                Debug.LogError("Добавьте хотя бы один цвет в список zoneColors!");
                return;
            }

            GenerateLayers(); // Запускаем генерацию всех слоёв сфер
        }

        private void GenerateLayers()
        {
            sphereLayers.Clear();

            // Рассчитываем радиус каждого слоя автоматически
            float radiusStep = (outerSphereRadius - innerSphereRadius) / Mathf.Max(1, layerCount - 1);

            for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
            {
                float currentRadius = outerSphereRadius - (radiusStep * layerIndex);

                // Динамически рассчитываем количество шариков в зависимости от радиуса
                int ballCountForLayer = Mathf.CeilToInt(baseBallCount * Mathf.Pow(currentRadius / outerSphereRadius, 2));

                // Генерация зон для текущего слоя
                List<Zone> currentLayerZones = new List<Zone>();
                GenerateZones(currentLayerZones);
                sphereLayers.Add(currentLayerZones);

                // Генерация шариков в текущем слое
                GenerateSphere(currentRadius, ballCountForLayer, currentLayerZones);
            }
        }

        private void GenerateZones(List<Zone> zones)
        {
            zones.Clear();

            for (int i = 0; i < zoneColors.Count; i++)
            {
                Vector3 randomPos = Random.onUnitSphere * 2.0f; // Случайное размещение зон
                zones.Add(new Zone(randomPos, zoneColors[i]));
            }
        }

        private void GenerateSphere(float radius, int count, List<Zone> zones)
        {
            for (int i = 0; i < count; i++)
            {
                // Используем алгоритм Фибоначчи для плотного распределения шариков по сфере
                float theta = Mathf.Acos(1 - 2 * (i + 0.5f) / count);
                float phi = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;

                Vector3 position = new Vector3(
                    radius * Mathf.Sin(theta) * Mathf.Cos(phi),
                    radius * Mathf.Sin(theta) * Mathf.Sin(phi),
                    radius * Mathf.Cos(theta)
                );

                GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, transform);

                Color zoneColor = GetColorByPosition(position, zones);
                ball.GetComponent<Renderer>().material.color = zoneColor;

                Zone zone = GetZoneByColor(zoneColor, zones);
                if (zone != null)
                {
                    zone.RegisterBall(ball);
                }
            }
        }

        private Color GetColorByPosition(Vector3 pos, List<Zone> zones)
        {
            float bestMatch = float.MaxValue;
            Color selectedColor = Color.black;

            foreach (Zone zone in zones)
            {
                float noise = Mathf.PerlinNoise(pos.x * noiseScale, pos.y * noiseScale) * 2 - 1;
                float distance = Vector3.Distance(pos, zone.center) + noise * borderWidth;

                if (distance < bestMatch)
                {
                    bestMatch = distance;
                    selectedColor = zone.color;
                }
            }

            return selectedColor;
        }

        private Zone GetZoneByColor(Color color, List<Zone> zones)
        {
            foreach (Zone zone in zones)
            {
                if (zone.color == color)
                {
                    return zone;
                }
            }

            return null;
        }

   
    }
}
