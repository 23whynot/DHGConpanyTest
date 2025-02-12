using System.Collections.Generic;
using CodeBase.Constants;
using CodeBase.Sphere;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Services.RendererMaterialService
{
    public class MaterialService : IMaterialService
    {
        private readonly List<Material> _materials = new List<Material>();

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
                material.HasProperty(ColorConstants.BaseColorOnMaterial) && material.GetColor(ColorConstants.BaseColorOnMaterial) == color);
        }

        public Material GetActualMaterial() => _materials[Random.Range(0, _materials.Count)];

        public void DeleteMaterial(Color color)
        {
            Material materialToRemove = _materials.Find(material =>
                material.HasProperty(ColorConstants.BaseColorOnMaterial) && material.GetColor(ColorConstants.BaseColorOnMaterial) == color);
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
                newMaterial.SetColor(ColorConstants.BaseColorOnMaterial, color);
                _materials.Add(newMaterial);
            }
        }
    }
}
