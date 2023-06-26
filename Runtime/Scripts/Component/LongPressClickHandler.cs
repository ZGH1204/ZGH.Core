using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZGH.Core
{
    public class LongPressClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action<Vector2> onDown;
        public Action<Vector2> onLongPress;

        private float m_PressTime = 0;
        private const float PRESS_TIME = 1f;

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown?.Invoke(eventData.position);

            m_PressTime += Time.deltaTime;
            if (m_PressTime >= PRESS_TIME)
            {
                onLongPress?.Invoke(eventData.position);
                m_PressTime = 0f;
            }

            Debug.Log("OnPointerDown" + eventData.position);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp" + eventData.position);
        }
    }
}