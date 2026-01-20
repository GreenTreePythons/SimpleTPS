using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
    public class UITouchPanel : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private float m_Sensitivity = 1f;

        public event Action<Vector2> OnLookDelta;

        private int m_ActivePointerId = int.MinValue;
        private bool m_IsDragging;

        public void OnPointerDown(PointerEventData eventData)
        {
            // 멀티터치 안전: 최초로 잡은 포인터만 추적
            if (m_IsDragging) return;

            m_IsDragging = true;
            m_ActivePointerId = eventData.pointerId;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!m_IsDragging) return;
            if (eventData.pointerId != m_ActivePointerId) return;

            // eventData.delta = 프레임 간 픽셀 이동량
            Vector2 delta = eventData.delta * m_Sensitivity;
            OnLookDelta?.Invoke(delta);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!m_IsDragging) return;
            if (eventData.pointerId != m_ActivePointerId) return;

            m_IsDragging = false;
            m_ActivePointerId = int.MinValue;

            // 마지막에 0 델타 전달(선택)
            OnLookDelta?.Invoke(Vector2.zero);
        }
    }
}