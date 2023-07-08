using UnityEngine;

namespace ZGH.Core.UI
{
    public interface IViewConfigHandler
    {
        UIViewConfig Get(int id);
        GameObject GetGameObject(int id);
    }
}