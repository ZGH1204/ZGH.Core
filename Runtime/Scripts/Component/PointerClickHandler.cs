using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZGH.Core
{
    public class PointerClickHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action<Vector2> onClick;
        public Action<Vector2> onUp;
        public Action<Vector2> onDown;

        private void Awake()
        {
            var m_Flash = gameObject.GetOrAddComponent<FlashGraphic>();
            m_Flash.FlashColor = new Color32(200, 200, 200, 255);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(eventData.position);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            onDown?.Invoke(eventData.position);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            onUp?.Invoke(eventData.position);
        }
    }
}