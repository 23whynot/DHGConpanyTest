using DG.Tweening;
using UnityEngine;


namespace CodeBase.Sphere
{
    public class SphereRotator : MonoBehaviour
    {
        [Header("Настройки вращения")]
        public float rotationSpeedY = 10f;
        public float rotationDuration = 20f;

        private void Start()
        {
            transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1); 
        }
    }
}