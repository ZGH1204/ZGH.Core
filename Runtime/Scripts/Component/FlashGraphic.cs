﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ZGH.Core
{
    public class FlashGraphic : UIBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public float DecayTime = 0.15f;
        public Color DefaultColor = new(1, 1, 1, 1);
        public Color FlashColor = new Color(1, 1, 1, 1) * 0.78125f;
        public Graphic Target;

        private bool _isHoldingUntilNextPress;

        protected override void Awake()
        {
            Target = GetComponent<Graphic>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Target.CrossFadeColor(FlashColor, 0f, true, true);
            _isHoldingUntilNextPress = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isHoldingUntilNextPress) {
                Target.CrossFadeColor(DefaultColor, DecayTime, true, true);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!_isHoldingUntilNextPress) {
                Target.CrossFadeColor(DefaultColor, 0f, true, true);
            }
        }

#if UNITY_EDITOR

        protected void Update()
        {
            if (!Application.isPlaying && Target != null) {
                Target.CrossFadeColor(DefaultColor, 0, true, true);
            }
        }

#endif

        public void Flash()
        {
            Target.CrossFadeColor(FlashColor, 0f, true, true);
            Target.CrossFadeColor(DefaultColor, DecayTime, true, true);
            _isHoldingUntilNextPress = false;
        }

        public void FlashAndHoldUntilNextPress()
        {
            Target.CrossFadeColor(FlashColor, 0f, true, true);
            _isHoldingUntilNextPress = true;
        }
    }
}