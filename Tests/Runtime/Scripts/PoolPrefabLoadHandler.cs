//using ZGH.Core.Pool;
//using System.Collections.Generic;
//using UnityEditor.VersionControl;
//using UnityEngine;
//using xasset;



//namespace Core.Pool
//{
//    public enum EGroupID : int
//    {
//        Common = 1,
//        UI = 2,
//        Battle = 3,
//    }

//    public class PoolGroupConfigHandler : IGroupConfigHandler
//    {
//        public class GroupData
//        {
//            public bool isAutoRelease;
//            public float clearWaitTime;
//        }

//        private static Dictionary<EGroupID, GroupData> GroupConfig = new() {
//            [EGroupID.Common] = new() {
//                isAutoRelease = true,
//                clearWaitTime = 3f,
//            },
//            [EGroupID.UI] = new() {
//                isAutoRelease = true,
//                clearWaitTime = 3f,
//            },
//            [EGroupID.Battle] = new() {
//                isAutoRelease = true,
//                clearWaitTime = 3f,
//            }
//        };

//        void IGroupConfigHandler.GetConfig(int id, out bool isAutoRelease, out float clearWaitTime)
//        {
//            if (GroupConfig.TryGetValue((EGroupID)id, out var d)) {
//                isAutoRelease = d.isAutoRelease;
//                clearWaitTime = d.clearWaitTime;
//            } else {
//                isAutoRelease = false;
//                clearWaitTime = 3f;
//            }
//        }
//    }

//    public class PoolPrefabLoadHandler : IPrefabLoadHandler
//    {
//        private Dictionary<string, AssetRequest> m_AllPool;

//        void IPrefabLoadHandler.Clear(string path)
//        {
//            if (m_AllPool.TryGetValue(path, out var request)) {
//                request.Release();
//                m_AllPool.Remove(path);
//            }
//        }

//        GameObject IPrefabLoadHandler.Load(string path)
//        {
//            if (m_AllPool.TryGetValue(path, out var request)) {
//                return request.asset as GameObject;
//            }

//            request = Asset.Load(path, typeof(GameObject));
//            m_AllPool.Add(path, request);
//            return request.asset as GameObject;
//        }
//    }
//}