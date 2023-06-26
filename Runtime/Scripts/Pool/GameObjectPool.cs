using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZGH.Core.Pool
{
    public class GameObjectPool : ObjectPool<GameObjectPooled>
    {
        public string id;
        public bool isAuto = true;
        public Action onReleaseEvent;

        private GameObject m_prefab;
        private float m_clearTime = -1f;

        public GameObjectPool(string _id, int initCount, int maxSize) : base(true, maxSize, maxSize)
        {
            InitPool(_id, initCount, maxSize);
        }

        private void InitPool(string _id, int initCount, int maxSize)
        {
            id = _id;
            prefabLoadHandler.Load(_id);

            for (var i = 0; i < initCount; ++i) {
                var go = m_OnCreate();
                countAll++;
                m_List.Add(go);
            }
        }

        public bool CheckAutoClear(float clearWaitTime)
        {
            if (!isAuto || countActive != 0) {
                m_clearTime = -1f;
                return false;
            }

            if (m_clearTime < 0f) {
                m_clearTime = Time.time + clearWaitTime;
            }
            if (Time.time >= m_clearTime) {
                m_clearTime = -1f;
                return true;
            }

            return false;
        }

        protected override GameObjectPooled m_OnCreate()
        {
            var _go = GameObject.Instantiate(m_prefab, Vector3.zero, Quaternion.identity, GameObjectPoolMgr.Instance.transform);
            var go = _go.AddComponent<GameObjectPooled>();
            go.transform.SetVisible(false);
            return go;
        }

        protected override void m_OnRelease(GameObjectPooled go)
        {
            // 回池回调 to do
        }

        protected override void m_OnGet(GameObjectPooled go)
        {
            go.Init(this);
        }

        protected override void m_OnDestroy(GameObjectPooled go)
        {
            GameObject.Destroy(go.gameObject);
        }

        public override void Clear()
        {
            onReleaseEvent?.Invoke();
            base.Clear();

            prefabLoadHandler.Clear(id);
            m_prefab = null;
        }

        #region static api

        public static IPrefabLoadHandler prefabLoadHandler;

        private static Stack<GameObjectPool> m_StackPools = new(10);

        public static GameObjectPool Pop(string id, int initCount, int maxSize)
        {
            if (m_StackPools.TryPop(out var pool)) {
                pool.InitPool(id, initCount, maxSize);
            } else {
                pool = new GameObjectPool(id, initCount, maxSize);
            }
            return pool;
        }

        public static void Push(GameObjectPool pool)
        {
            pool.Clear();
            m_StackPools.Push(pool);
        }

        #endregion static api
    }
}