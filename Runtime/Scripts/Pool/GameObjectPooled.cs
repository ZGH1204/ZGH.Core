using UnityEngine;

namespace ZGH.Core.Pool
{
    public class GameObjectPooled : MonoBehaviour
    {
        public bool isRelease = true;
        public System.Action onReleaseEvent;

        private GameObjectPool m_Pool;

        public void Init(GameObjectPool p)
        {
            if (!isRelease) {
                return;
            }

            isRelease = false;
            m_Pool = p;
            m_Pool.onReleaseEvent += OnRelease;
            gameObject.SetActive(true);
        }

        public void DeInit()
        {
            if (isRelease) {
                return;
            }

            isRelease = true;
            gameObject?.SetActive(false);

            onReleaseEvent?.Invoke();
            onReleaseEvent = null;

            m_Pool.Release(this);
            m_Pool.onReleaseEvent -= OnRelease;
            m_Pool = null;
        }

        public void OnRelease()
        {
            // Log.I($"释放 GameObject ：{gameObject.name}");
            DeInit();
        }

        private void OnDestroy()
        {
            if (isRelease) {
                return;
            }

            isRelease = true;

            onReleaseEvent?.Invoke();
            onReleaseEvent = null;

            m_Pool.countAll--;
            m_Pool.onReleaseEvent -= OnRelease;
            m_Pool = null;
        }
    }
}