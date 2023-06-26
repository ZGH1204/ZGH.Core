using UnityEngine;

namespace ZGH.Core.Pool
{
    public interface IPrefabLoadHandler
    {
        public GameObject Load(string path);

        public void Clear(string path);
    }

    public interface IGroupConfigHandler
    {
        void GetConfig(int id, out bool isAutoRelease, out float clearWaitTime);
    }
}