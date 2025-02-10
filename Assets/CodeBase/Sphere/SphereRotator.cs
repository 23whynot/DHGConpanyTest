using DG.Tweening;
using UnityEngine;


namespace CodeBase.Sphere
{
    public class SphereRotator : MonoBehaviour
    {
        [Header("Настройки вращения")]
        [Tooltip("Скорость вращения вокруг оси Y")]
        public float rotationSpeedY = 10f;

        [Tooltip("Продолжительность одного полного оборота")]
        public float rotationDuration = 20f;

        private void Start()
        {
            // Запускаем бесконечное вращение вокруг оси Y с DOTween
            transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear) // Постоянная скорость вращения
                .SetLoops(-1); // Бесконечное количество повторений
        }
    }
}