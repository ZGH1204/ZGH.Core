//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using UnityEngine;
//using UnityEngine.Pool;

//namespace ZGH.Core.Pool
//{
//    public class GameObjectPoolMgrEditor
//    {
//        [SerializeField] private string m_ExpDesc;
//        [SerializeField] private List<PoolInfo> m_AllPoolInfo = ListPool<PoolInfo>.Get();
//        [SerializeField] private List<PoolInfo> m_CachePoolInfo = ListPool<PoolInfo>.Get();

//        private void OnUpdateEditor()
//        {
//            m_AllPoolInfo.ForEach(x => GenericPool<PoolInfo>.Release(x));
//            m_AllPoolInfo.Clear();
//            foreach (var groupKV in GameObjectPoolMgr allGroup) {
//                foreach (var pool in groupKV.Value.pools) {
//                    m_AllPoolInfo.Add(PoolInfo.Get(groupKV.Key, pool));
//                }
//            }
//            m_AllPoolInfo.Sort((x, y) => (int)x.groupID - (int)y.groupID);

//            m_CachePoolInfo.ForEach(x => GenericPool<PoolInfo>.Release(x));
//            m_CachePoolInfo.Clear();
//            PoolInfo.Get(m_CachePoolInfo);

//            if (m_AllPoolInfo.Count != allPool.Count) {
//                m_ExpDesc = "对象池异常！";
//            }
//        }

//        [Serializable]
//        private class PoolInfo
//        {
//            public string id;
//            public float deathTime;

//            public int countAll;
//            public int countInactive;
//            public int countActive;

//            public int groupID;

//            private static List<FieldInfo> m_FieldInfos;

//            public static PoolInfo Get(int groupId, GameObjectPool pool)
//            {
//                if (m_FieldInfos == null) {
//                    Assembly assembly = Assembly.GetExecutingAssembly();
//                    Type type = assembly.GetType("Core.Pool.GameObjectPool");
//                    m_FieldInfos = type.GetRuntimeFields().ToList();
//                }

//                var poolInfo = GenericPool<PoolInfo>.Get();
//                FieldInfo m_clearTime_Field = m_FieldInfos.Find(x => x.Name == "m_clearTime");
//                FieldInfo countAll_Field = m_FieldInfos.Find(x => x.Name == "countAll");

//                poolInfo.id = $"【{poolInfo.groupID}】{pool.id}";
//                poolInfo.countAll = (int)countAll_Field.GetValue(pool);
//                poolInfo.countInactive = pool.countInactive;
//                poolInfo.countActive = pool.countActive;

//                var deathTime = (float)m_clearTime_Field.GetValue(pool) - Time.time;
//                poolInfo.deathTime = deathTime < 0f ? -1f : deathTime;
//                poolInfo.groupID = groupId;

//                return poolInfo;
//            }

//            public static void Get(List<PoolInfo> poolInfos)
//            {
//                if (m_FieldInfos == null) {
//                    Assembly assembly = Assembly.GetExecutingAssembly();
//                    Type type = assembly.GetType("Core.Pool.GameObjectPool");
//                    m_FieldInfos = type.GetRuntimeFields().ToList();
//                }

//                var field = m_FieldInfos.Find(x => x.Name == "m_StackPools");
//                var cachePool = field.GetValue(null) as Stack<GameObjectPool>;
//                foreach (var pool in cachePool) {
//                    poolInfos.Add(Get(0, pool));
//                }
//            }
//        }
//    }
//}