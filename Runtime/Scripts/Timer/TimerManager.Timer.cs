using System;
using UnityEngine;
using UnityEngine.Pool;

namespace ZGH.Core
{
    public partial class TimerManager
    {
        private class Timer
        {
            public int id;
            public float interval;
            public float expireTime;
            public Func<bool> func;

            public Timer Init(float v, Func<bool> f)
            {
                interval = v;
                expireTime = v + Time.time;
                func = f;
                return this;
            }

            public bool IsValid()
            {
                return interval > 0f && func != null;
            }

            #region static api

            public static int Idx = 0;

            public static Timer Pop(float v, Func<bool> f)
            {
                var t = GenericPool<Timer>.Get();
                t.Init(v, f);
                t.id = ++Idx;
                // Log.I($"[Core定时器] Id={t.id} 添加");
                return t;
            }

            public static void Push(Timer t)
            {
                // Log.I($"[Core定时器] Id={t.id} 删除");
                t.Init(float.MinValue, null);
                t.id = -1;
                GenericPool<Timer>.Release(t);
            }

            #endregion static api
        }
    }
}