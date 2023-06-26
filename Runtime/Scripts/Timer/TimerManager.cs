using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZGH.Core
{
    public static class TimerManager
    {
        private static List<Timer> m_Timers = new(100);

        public static int AddTimer(float interval, Func<bool> func)
        {
            if (interval <= 0) {
                Log.E("interval <= 0 ???");
            }

            var id = m_Timers.FindIndex((x) => { return x.interval <= 0f; });
            if (id >= 0) {
                var timer = m_Timers[id];
                timer.Init(interval, func);
                m_Timers[id] = timer;
            }
            else {
                id = m_Timers.Count;
                m_Timers.Add(new Timer().Init(interval, func));
            }
            return id;
        }

        public static void DelTimer(int id)
        {
            if (id < 0) {
                return;
            }

            var timer = m_Timers[id];
            timer.interval = -1f;
            timer.func = null;
            m_Timers[id] = timer;
        }

        public static void SetInterval(int id, float interval)
        {
            if (id < 0) {
                return;
            }

            var timer = m_Timers[id];
            timer.interval = interval;
            m_Timers[id] = timer;
        }

        public static void Update()
        {
            for (var i = 0; i < m_Timers.Count; i++) {
                var timer = m_Timers[i];
                if (timer.IsValid() && timer.CanInvoke()) {
                    if (timer.func.Invoke()) {
                        m_Timers[i] = timer.Init(-1f, null);
                    }

                    timer = m_Timers[i];
                    if (timer.IsValid()) {
                        timer.expireTime = timer.interval + Time.time;
                        m_Timers[i] = timer;
                    }
                }
            }
        }

        public static string Dump()
        {
            var validCount = m_Timers.FindAll((x) => {
                return x.interval > 0;
            }).Count;
            return string.Format($"存活定时器 {validCount}个,  回池定时器 {m_Timers.Count - validCount}个");
        }

        private struct Timer
        {
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

            public bool CanInvoke()
            {
                return expireTime <= Time.time;
            }
        }
    }
}