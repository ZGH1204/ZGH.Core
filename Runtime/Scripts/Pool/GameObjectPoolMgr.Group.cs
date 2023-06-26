using System.Collections.Generic;

namespace ZGH.Core.Pool
{
    public class GameObjectPoolGroup
    {
        public int id;
        public List<GameObjectPool> pools => m_Pools;

        private List<GameObjectPool> m_Pools = new();
        private List<string> m_TempList = new();
        private bool m_IsAutoRelease;
        private float m_ClearWaitTime;

        public GameObjectPoolGroup(int _id)
        {
            InitGroup(_id);
        }

        public void InitGroup(int _id)
        {
            id = _id;
            groupConfigHandler.GetConfig(_id, out m_IsAutoRelease, out m_ClearWaitTime);
        }

        public void AddPool(GameObjectPool pool)
        {
            if (m_Pools.Contains(pool)) {
                Log.E($"当前组 {id} 已经存在对象池 {pool.id}？");
                return;
            }

            m_Pools.Add(pool);
        }

        public bool RemovePool(GameObjectPool pool)
        {
            var id = m_Pools.IndexOf(pool);
            if (id >= 0) {
                m_Pools.RemoveAt(id);
            }
            return id > 0;
        }

        public bool RemovePool(int id)
        {
            if (id >= 0 && id < m_Pools.Count) {
                m_Pools.RemoveAt(id);
                return true;
            }
            return false;
        }

        public List<string> CheckAutoClear()
        {
            m_TempList.Clear();
            if (!m_IsAutoRelease) {
                return m_TempList;
            }

            for (int i = 0; i < m_Pools.Count; i++) {
                if (m_Pools[i].CheckAutoClear(m_ClearWaitTime)) {
                    m_TempList.Add(m_Pools[i].id);
                    if (RemovePool(i)) {
                        i--;
                    }
                }
            }
            return m_TempList;
        }

        public void Clear()
        {
            m_TempList.Clear();
            m_Pools.Clear();
        }

        #region static api

        public static IGroupConfigHandler groupConfigHandler;

        private static Stack<GameObjectPoolGroup> m_StackPools = new(10);

        public static GameObjectPoolGroup Pop(int id)
        {
            if (m_StackPools.TryPop(out var group)) {
                group.InitGroup(id);
            } else {
                group = new GameObjectPoolGroup(id);
            }
            return group;
        }

        public static void Push(GameObjectPoolGroup group)
        {
            group.Clear();
            m_StackPools.Push(group);
        }

        #endregion static api
    }
}