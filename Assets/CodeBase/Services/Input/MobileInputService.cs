using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace CodeBase.Services.Input
{
    public class MobileInputService : InputService
    {
        private readonly float _sensitivity = 0.05f;
        private readonly float _minDeltaThreshold = 0.005f;
        private bool _wasHolding;

        public override bool IsHolding()
        {
            if (UnityEngine.Input.touchCount == 0) return false;
            if (IsTouchOverUI()) return false;

            bool isHoldingNow = UnityEngine.Input.GetTouch(0).phase == TouchPhase.Stationary || 
                                UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved;

            if (_wasHolding && !isHoldingNow)
            {
                InvokeReleaseEvent();
            }

            _wasHolding = isHoldingNow;
            return isHoldingNow;
        }

        public override float GetHorizontal()
        {
            if (UnityEngine.Input.touchCount == 0 || IsTouchOverUI()) return 0f;
            Touch touch = UnityEngine.Input.GetTouch(0);
            float rawHorizontal = touch.deltaPosition.x * _sensitivity;

            return Mathf.Abs(rawHorizontal) >= _minDeltaThreshold ? rawHorizontal : 0f;
        }

        public override float GetVertical()
        {
            if (UnityEngine.Input.touchCount == 0 || IsTouchOverUI()) return 0f;
            Touch touch = UnityEngine.Input.GetTouch(0);
            float rawVertical = touch.deltaPosition.y * _sensitivity;

            return Mathf.Abs(rawVertical) >= _minDeltaThreshold ? rawVertical : 0f;
        }

        private bool IsTouchOverUI()
        {
            if (EventSystem.current == null) return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = UnityEngine.Input.GetTouch(0).position
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}
