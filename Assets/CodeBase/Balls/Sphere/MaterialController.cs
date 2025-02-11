using UnityEngine;

namespace CodeBase.Balls.Sphere
{
    public class MaterialController
    {
        private Renderer _renderer;

        public MaterialController(Renderer renderer)
        {
            _renderer = renderer;
        }

        public void SetMaterial(Material material)
        {
            _renderer.material = material;
        }
    }
}