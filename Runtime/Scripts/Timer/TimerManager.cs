using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZGH.Core
{
    public partial class TimerManager : SingletonUpdater<TimerManager>
    {
        private List<int> m_Timers;
        private Dictionary<int, Timer> m_TimerDic;

        public TimerManager() : base()
        {
            m_Timers = new(100);
            m_TimerDic = new Dictionary<int, Timer>(100);
        }

        public override void Dispose()
        {
            base.Dispose();
            Timer.Idx = 0;
            m_Timers.Clear();
            m_TimerDic.Clear();
        }

        public override void Update()
        {
            base.Update();

            m_Timers.Clear();
            foreach (var id in m_TimerDic.Keys) {
                m_Timers.Add(id);
            }
            m_Timers.ForEach(id => {
                if (m_TimerDic.TryGetValue(id, out var t) && t.IsValid() && t.expireTime <= Time.time) {
                    if (t.func.Invoke()) {
                        DelTimer(id);
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

        public void DelTimer(int id)
        {
            if (id <= 0) {
                return;
            }

            if (m_TimerDic.TryGetValue(id, out var t)) {
                m_TimerDic.Remove(id);
                Timer.Push(t);
            }
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