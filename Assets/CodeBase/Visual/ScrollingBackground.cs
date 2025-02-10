using UnityEngine;

namespace CodeBase.Visual
{
    public class ScrollingBackground : MonoBehaviour
    {
        public float scrollSpeed = 0.1f;  // Скорость прокрутки
        private Material backgroundMaterial;  // Ссылка на материал

        void Start()
        {
            // Получаем материал из рендерера объекта
            backgroundMaterial = GetComponent<Renderer>().material;
        }

        void Update()
        {
            // Рассчитываем новое смещение текстуры
            float newOffsetY = Mathf.Repeat(backgroundMaterial.mainTextureOffset.y + (scrollSpeed * Time.deltaTime), 1f);
        
            // Применяем смещение
            backgroundMaterial.mainTextureOffset = new Vector2(0f, newOffsetY);
        }
    }
}