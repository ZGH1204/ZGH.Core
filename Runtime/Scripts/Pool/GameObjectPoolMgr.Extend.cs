using UnityEngine;

namespace ZGH.Core.Pool
{
    public partial class GameObjectPoolMgr
    {
        public GameObjectPooled Get(string poolId, int groupId = 1, Transform parent = null)
        {
            if (!m_AllPool.TryGetValue(poolId, out var pool))
            {
                pool = InitializeObjectsPool(poolId, groupId, 1, 1);
            }

            var go = pool.Get();
            if (parent != null)
            {
                go.transform.SetParent(parent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
            }
            return go;
        }

        public void Release(GameObjectPooled pooledGo)
        {
            if (null != pooledGo)
            {
                pooledGo.OnRelease();
            }
        }

        public void Release(Transform trans)
        {
            Release(trans.GetComponent<GameObjectPooled>());
        }

        public void Release(GameObject go)
        {
            Release(go.GetComponent<GameObjectPooled>());
        }
    }
}