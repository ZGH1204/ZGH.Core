﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZGH.Core
{
    public static partial class ComponentExtension
    {
        private static Vector2 v2 = new();
        private static Vector3 v3 = new();
        private static Vector4 v4 = new();

        public static void SetUIWidth(this Transform t, float x)
        {
            if (t.TryGetComponent<RectTransform>(out var rt)) {
                v2.Set(x, rt.sizeDelta.y);
                rt.sizeDelta = v2;
            }
        }

        public static void SetUIHeight(this Transform t, float y)
        {
            if (t.TryGetComponent<RectTransform>(out var rt)) {
                v2.Set(rt.sizeDelta.x, y);
                rt.sizeDelta = v2;
            }
        }

        public static void SetUISize(this Transform t, float x, float y)
        {
            if (t.TryGetComponent<RectTransform>(out var rt)) {
                v2.Set(x, y);
                rt.sizeDelta = v2;
            }
        }

        public static void AddClickEvent(this Transform t, Action<Vector2> cb)
        {
            t.GetOrAddComponent<Graphic>();
            t.GetOrAddComponent<PointerClickHandler>().onClick = cb;
        }

        public static void AddDownEvent(this Transform t, Action<Vector2> cb)
        {
            t.GetOrAddComponent<Graphic>();
            t.GetOrAddComponent<PointerClickHandler>().onDown = cb;
        }

        public static void AddUpEvent(this Transform t, Action<Vector2> cb)
        {
            t.GetOrAddComponent<Graphic>();
            t.GetOrAddComponent<PointerClickHandler>().onUp = cb;
        }

        public static void RemoveClickEvent(this Transform t)
        {
            if (t.TryGetComponent<PointerClickHandler>(out var comp)) {
                comp.onClick = null;
            }
        }
    }
}