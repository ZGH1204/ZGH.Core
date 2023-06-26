using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace ZGH.Core
{
    public static partial class Utils
    {
        public static void DontDestroy(this GameObject go)
        {
            UnityEngine.Object.DontDestroyOnLoad(go);
        }

        public static void DontDestroy(this Transform go)
        {
            UnityEngine.Object.DontDestroyOnLoad(go);
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (go == null) {
                return null;
            }

            if (!go.TryGetComponent<T>(out var cp)) {
                cp = go.AddComponent<T>();
            }
            return cp;
        }

        public static T GetOrAddComponent<T>(this Transform t) where T : Component
        {
            if (t == null) {
                return null;
            }

            return t.gameObject.GetOrAddComponent<T>();
        }

        public static void RemoveComponentIfExists<T>(this GameObject go) where T : Component
        {
            if (go.TryGetComponent<T>(out var comp)) {
                UnityEngine.Object.Destroy(comp);
            }
        }

        public static GameObject FindOrNew(this GameObject go, string name)
        {
            if (go == null) {
                return null;
            }

            return go.transform.FindOrNew(name).gameObject;
        }

        public static Transform FindOrNew(this Transform t, string name)
        {
            if (t == null) {
                return null;
            }

            var child = t.Find(name);
            if (child == null) {
                child = new GameObject(name).transform;
                child.SetParent(t);
                child.Zeroize();
            }
            return child;
        }

        public static void Zeroize(this Transform t)
        {
            if (t == null) {
                return;
            }

            t.localPosition = Vector3.zero;
            t.localScale = Vector3.one;
            t.localEulerAngles = Vector3.zero;
        }

        public static void SetVisible(this Transform t, bool boo)
        {
            if (t == null) {
                return;
            }

            t.gameObject.SetActive(boo);
        }

        public static void SetUIFullScreen(this Transform t, float left = 0, float right = 0)
        {
            if (t == null) {
                return;
            }

            if (t.TryGetComponent<RectTransform>(out var rt)) {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = new Vector2(left, 0);
                rt.offsetMax = new Vector2(right, 0);
            }
        }

        public static void HideAllChild(this Transform t)
        {
            if (t == null) {
                return;
            }

            var count = t.childCount;
            for (var i = 0; i < count; i++) {
                t.GetChild(i).SetVisible(false);
            }
        }

        public static float CalcDistance(List<Vector3> wps)
        {
            float dis = 0;
            for (var i = 0; i < wps.Count - 1; i++) {
                dis += Vector3.Distance(wps[i], wps[i + 1]);
            }
            return dis;
        }

        public static int GetRandom(int maxVal)
        {
            var randomBytes = new byte[4];
            var rngCrypto = new RNGCryptoServiceProvider();
            rngCrypto.GetBytes(randomBytes);
            return BitConverter.ToInt32(randomBytes, 0) / maxVal;
        }
    }
}