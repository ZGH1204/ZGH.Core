using System;
using System.Collections.Generic;

namespace ZGH.Core
{
    using ListDelegatePool = UnityEngine.Pool.ListPool<Delegate>;

    public class EventManager : Singleton<EventManager>
    {
        public bool isSafeCheck = true;

        private readonly Dictionary<int, List<Delegate>> m_EventDic;

        public EventManager()
        {
            m_EventDic = new Dictionary<int, List<Delegate>>(128);
        }

        #region Add

        public void AddEvent(int id, Delegate cb)
        {
            if (m_EventDic.TryGetValue(id, out var list)) {
                if (list.IndexOf(cb) < 0)
                    list.Add(cb);
            }
            else {
                var l = ListDelegatePool.Get();
                l.Add(cb);
                m_EventDic.Add(id, l);
            }
        }

        public void Add(int id, Action cb)
        {
            AddEvent(id, cb);
        }

        public void Add<T1>(int id, Action<T1> cb)
        {
            AddEvent(id, cb);
        }

        public void Add<T1, T2>(int id, Action<T1, T2> cb)
        {
            AddEvent(id, cb);
        }

        public void Add<T1, T2, T3>(int id, Action<T1, T2, T3> cb)
        {
            AddEvent(id, cb);
        }

        public void Add<T1, T2, T3, T4>(int id, Action<T1, T2, T3, T4> cb)
        {
            AddEvent(id, cb);
        }

        #endregion Add

        #region Remove

        private void RemoveEvent(int id, Delegate cb)
        {
            if (m_EventDic.TryGetValue(id, out var list)) {
                list.Remove(cb);
                if (list.Count == 0) {
                    m_EventDic.Remove(id);
                    ListDelegatePool.Release(list);
                }
            }
        }

        public void Remove(int id, Action cb)
        {
            Delegate d = cb;
            RemoveEvent(id, d);
        }

        public void Remove<T1>(int id, Action<T1> cb)
        {
            Delegate d = cb;
            RemoveEvent(id, d);
        }

        public void Remove<T1, T2>(int id, Action<T1, T2> cb)
        {
            Delegate d = cb;
            RemoveEvent(id, d);
        }

        public void Remove<T1, T2, T3>(int id, Action<T1, T2, T3> cb)
        {
            Delegate d = cb;
            RemoveEvent(id, d);
        }

        public void Remove<T1, T2, T3, T4>(int id, Action<T1, T2, T3, T4> cb)
        {
            Delegate d = cb;
            RemoveEvent(id, d);
        }

        #endregion Remove

        #region Fire

        private List<Delegate> GetEventList(int id)
        {
            if (m_EventDic.TryGetValue(id, out var list)) {
                if (isSafeCheck) {
                    var type = list[0].GetType();
                    for (var i = 1; i < list.Count; i++) {
                        if (type != list[i].GetType()) {
                            Log.W($"[SafeCheck] 事件ID:{id} 存在不同类型的委托事件, 请检查!!!");
                            break;
                        }
                    }
                }
                return list;
            }
            return null;
        }

        public void Fire<T1, T2, T3, T4>(int id, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var eventList = GetEventList(id);
            eventList?.ForEach((cb) => { (cb as Action<T1, T2, T3, T4>)?.Invoke(arg1, arg2, arg3, arg4); });
        }

        public void Fire<T1, T2, T3>(int id, T1 arg1, T2 arg2, T3 arg3)
        {
            var eventList = GetEventList(id);
            eventList?.ForEach((cb) => { (cb as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3); });
        }

        public void Fire<T1, T2>(int id, T1 arg1, T2 arg2)
        {
            var eventList = GetEventList(id);
            eventList?.ForEach((cb) => { (cb as Action<T1, T2>)?.Invoke(arg1, arg2); });
        }

        public void Fire<T1>(int id, T1 arg1)
        {
            var eventList = GetEventList(id);
            eventList?.ForEach((cb) => { (cb as Action<T1>)?.Invoke(arg1); });
        }

        public void Fire(int id)
        {
            var eventList = GetEventList(id);
            eventList?.ForEach((cb) => { (cb as Action)?.Invoke(); });
        }

        #endregion Fire
    }
}