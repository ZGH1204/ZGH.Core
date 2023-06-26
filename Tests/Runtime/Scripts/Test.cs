using UnityEngine;
using ZGH.Core;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Log.I(Utils.ToHourMinSec(4545));
    }

    // Update is called once per frame
    private void TestPool()
    {
        //GameObjectPoolMgr m_PoolManager = GameObjectPoolMgr.Instance;
        //GameObjectPoolGroup.groupConfigHandler = new PoolGroupConfigHandler();
        //GameObjectPool.prefabLoadHandler = new PoolPrefabLoadHandler();

        //  GameObjectPoolMgr.Instance.OnUpdate();
    }
}