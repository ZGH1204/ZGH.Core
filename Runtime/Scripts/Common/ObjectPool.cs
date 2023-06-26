using System;
using System.Collections.Generic;

namespace Core
{
    public class ObjectPool<T> : IDisposable where T : class
    {
        public int countAll;
        public int countActive => countAll - countInactive;
        public int countInactive => m_List.Count;

        protected List<T> m_List;
        protected int m_MaxSize;
        protected bool m_CollectionCheck;

        public ObjectPool(bool check = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            m_MaxSize = maxSize;
            m_CollectionCheck = check;
            m_List = new List<T>(defaultCapacity);
        }

        protected virtual T m_OnCreate()
        {
            return default;
        }

        protected virtual void m_OnRelease(T t)
        {
        }

        protected virtual void m_OnDestroy(T t)
        {
        }

        protected virtual void m_OnGet(T t)
        {
        }

        public T Get()
        {
            T val;
            if (m_List.Count == 0) {
                val = m_OnCreate();
                countAll++;
            } else {
                var index = m_List.Count - 1;
                val = m_List[index];
                m_List.RemoveAt(index);
            }

            m_OnGet(val);
            return val;
        }

        public void Release(T element)
        {
            if (m_CollectionCheck && m_List.Count > 0) {
                for (var i = 0; i < m_List.Count; i++) {
                    if (element == m_List[i]) {
                        throw new InvalidOperationException("Trying to release an object that has already been released to the m_AllPool.");
                    }
                }
            }

            m_OnRelease(element);
            if (countInactive < m_MaxSize) {
                m_List.Add(element);
            } else {
                countAll--;
                m_OnDestroy(element);
            }
        }

        public virtual void Clear()
        {
            foreach (var item in m_List) {
                m_OnDestroy(item);
            }

            m_List.Clear();
            countAll = 0;
        }

        public void Dispose()
        {
            Clear();
        }
    }
}