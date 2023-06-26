using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ZGH.Core
{
    public class DragHandler : MonoBehaviour, IDragHandler
    {
        public Action<Vector2> onDrag;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData.position);
        }
    }
}