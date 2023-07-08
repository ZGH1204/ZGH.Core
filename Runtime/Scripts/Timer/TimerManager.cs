using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZGH.Core
{
    public partial class TimerManager : SingletonUpdater<TimerManager>
    {
        private List<Timer> m_Timers;
        private Dictionary<int, Timer> m_TimerDic;

        public TimerManager() : base()
        {
            m_Timers = new(100);
            m_TimerDic = new Dictionary<int, Timer>(100);
        }

        public override void Dispose()
        {
            base.Dispose();
            m_Timers.Clear();
            m_TimerDic.Clear();
        }

        public override void Update()
        {
            base.Update();

            m_Timers.Clear();
            foreach (var t in m_TimerDic.Values) {
                m_Timers.Add(t);
            }
            m_Timers.ForEach(t => {
                if (t.IsValid() && t.CanInvoke()) {
                    if (t.func.Invoke()) {
                        DelTimer(ref t.id);
                    } else {
                        t.expireTime = t.interval + Time.time;
                    }
                }
            });
        }

        public int AddTimer(float interval, Func<bool> func)
        {
            if (interval <= 0) {
                Log.E("interval <= 0 ???");
                return -1;
            }

            var timer = Timer.Pop(interval, func);
            m_TimerDic[timer.id] = timer;
            return timer.id;
        }

        public void DelTimer(ref int id)
        {
            if (id <= 0) {
                return;
            }

            if (m_TimerDic.TryGetValue(id, out var t)) {
                m_TimerDic.Remove(id);
                Timer.Push(t);
            }
            id = -1;
        }

        public void SetInterval(int id, float interval)
        {
            if (id <= 0) {
                Log.E("id <= 0 ???");
                return;
            }
            if (interval <= 0) {
                Log.E("interval <= 0 ???");
            }

            if (m_TimerDic.TryGetValue(id, out var t)) {
                t.interval = interval;
            }
        }
    }
}