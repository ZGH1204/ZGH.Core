using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Pool;

namespace ZGH.Core.Pool
{
    public partial class GameObjectPoolMgr : SingletonMonobehaviour<GameObjectPoolMgr>
    {
        public Dictionary<string, GameObjectPool> allPool => m_AllPool;

        private Dictionary<string, GameObjectPool> m_AllPool = DictionaryPool<string, GameObjectPool>.Get();
        private Dictionary<int, GameObjectPoolGroup> m_AllGroup = new();
        private List<string> m_tempList = new(10);

        protected override void Start()
        {
            base.Start();
        }

        public void OnUpdate()
        {
            if (m_AllPool.Count == 0) {
                return;
            }

            foreach (var group in m_AllGroup.Values) {
                var list = group.CheckAutoClear();
                if (list.Count > 0) {
                    m_tempList.AddRange(list);
                }
            }

            foreach (var id in m_tempList) {
                ClearPool(id);
            }
            m_tempList.Clear();
        }

        public void AddPool(string path, int groupId, int initCount = 0, int retainCount = 1)
        {
            InitializeObjectsPool(path, groupId, initCount, retainCount);
        }

        private GameObjectPool InitializeObjectsPool(string poodId, int groupId, int initCount = 0, int retainCount = 1)
        {
            if (!m_AllGroup.TryGetValue(groupId, out var group)) {
                group = GameObjectPoolGroup.Pop(groupId);
                m_AllGroup.Add(groupId, group);
            }
            if (!m_AllPool.TryGetValue(poodId, out var pool)) {
                pool = CreatePool(poodId, initCount, retainCount);
            }
            group.AddPool(pool);

            return pool;
        }

        private GameObjectPool CreatePool(string poodId, int initCount, int retainCount)
        {
            Log.I($"对象池创建：{poodId}");
            if (!m_AllPool.TryGetValue(poodId, out var pool)) {
                pool = GameObjectPool.Pop(poodId, initCount, retainCount);
                m_AllPool.Add(poodId, pool);
            }

            return pool;
        }

        public void ClearPool(string poodId)
        {
            Log.I($"对象池删除：{poodId}");
            if (m_AllPool.TryGetValue(poodId, out var pool)) {
                foreach (var group in m_AllGroup.Values) {
                    group.RemovePool(pool);
                }

                m_AllPool.Remove(poodId);
                GameObjectPool.Push(pool);
            }
        }

        public void ClearAllPool(bool gc = true)
        {
            var poolNames = m_AllPool.Keys.ToList();
            foreach (var id in poolNames) {
                ClearPool(id);
            }

            DictionaryPool<string, GameObjectPool>.Release(m_AllPool);
            if (gc) {
                GC.Collect();
            }
        }
    }
}