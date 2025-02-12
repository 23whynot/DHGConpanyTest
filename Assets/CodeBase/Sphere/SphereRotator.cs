using System;
using DG.Tweening;
using UnityEngine;


namespace CodeBase.Sphere
{
    public class SphereRotator : MonoBehaviour
    {
        [Header("Налаштування обертання")]
        public float rotationSpeedY = 10f;
        public float rotationDuration = 20f;
        
        private Tween _tween;

        private void Start()
        {
            _tween = transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1); 
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}