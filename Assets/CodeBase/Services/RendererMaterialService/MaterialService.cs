using System.Collections.Generic;
using CodeBase.Sphere;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Services.RendererMaterialService
{
    public class MaterialService : IMaterialService
    {
        private readonly List<Material> _materials = new List<Material>();
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

                  private IColorOfZoneProvider _colorOfZoneProvider;

        [Inject]
        public void Construct(IColorOfZoneProvider colorOfZoneProvider) => _colorOfZoneProvider = colorOfZoneProvider;

        public void Init(Material material, int layersCount)
        {
            CreateMaterialsFromColors(material, layersCount);
        }

        public Material GetMaterial(Color color)
        {
            return _materials.Find(material =>
                material.HasProperty(BaseColor) && material.GetColor(BaseColor) == color);
        }

        public Material GetActualMaterial() => _materials[Random.Range(0, _materials.Count)];

        public void DeleteMaterial(Color color)
        {
            Material materialToRemove = _materials.Find(material =>
                material.HasProperty(BaseColor) && material.GetColor(BaseColor) == color);
                _materials.Remove(materialToRemove);
        }

        private void CreateMaterialsFromColors(Material material, int layersCount)
        {
            List<Color> colors = _colorOfZoneProvider.GetColorsOfZone();

            foreach (Color color in colors)
            {
                CreateMaterial(material, color, layersCount);
            }
        }

        private void CreateMaterial(Material material, Color color, int layersCount)
        {
            for (int i = 0; i < layersCount; i++)
            {
                Material newMaterial = new Material(material);
                newMaterial.SetColor(BaseColor, color);
                _materials.Add(newMaterial);
            }
        }
    }
}
