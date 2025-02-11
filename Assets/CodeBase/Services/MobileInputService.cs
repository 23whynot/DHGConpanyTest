using System;
using UnityEngine;

namespace CodeBase.Services
{
    public class MobileInputService : InputService
    {
        private readonly float _sensitivity = 0.1f;
        private readonly float _smoothing = 0.05f;  
        private readonly float _minDeltaThreshold = 0.01f;  

        private float _horizontalSmooth;
        private float _verticalSmooth;
        private bool _wasHolding; 
        public override bool IsHolding()
        {
            bool isHoldingNow = Input.touchCount > 0 && 
                                (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved);

            
            if (_wasHolding && !isHoldingNow)
            {
                InvokeReleaseEvent();
            }

            _wasHolding = isHoldingNow;
            return isHoldingNow;
        }

        public override float GetHorizontal()
        {
            if (Input.touchCount <= 0) return 0f;
            Touch touch = Input.GetTouch(0);
            float rawHorizontal = touch.deltaPosition.x * _sensitivity;

            _horizontalSmooth = Mathf.Lerp(_horizontalSmooth, rawHorizontal, _smoothing);

            if (Mathf.Abs(_horizontalSmooth) < _minDeltaThreshold)
            {
                _horizontalSmooth = 0f;
            }

            return _horizontalSmooth;
        }

        public override float GetVertical()
        {
            if (Input.touchCount <= 0) return 0f;
            Touch touch = Input.GetTouch(0);
            float rawVertical = touch.deltaPosition.y * _sensitivity;

            _verticalSmooth = Mathf.Lerp(_verticalSmooth, rawVertical, _smoothing);
            
            if (Mathf.Abs(_verticalSmooth) < _minDeltaThreshold)
            {
                _verticalSmooth = 0f;
            }

            return _verticalSmooth;
        }
    }
}
