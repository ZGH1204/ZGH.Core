using System;
using UnityEngine;

namespace ZGH.Core
{
    public class SingletonUpdater<T> : AUpdater, IDisposable where T : AUpdater, new()
    {
        private static T m_Instance;

        public static T Instance {
            get {
                if (null == m_Instance) {
                    m_Instance = new T();
                    if (null == m_Instance) {
                        Log.E("Error Create Singleton ! ", m_Instance.GetType());
                    }
                }
                return m_Instance;
            }
            set { m_Instance = value; }
        }

        public SingletonUpdater() : base()
        {
        }

        public virtual void Dispose()
        {
            UpdaterMgr.Unregister(m_Instance);
            m_Instance = null;
        }
    }

    public class Singleton<T> where T : class, new()
    {
        private static T m_Instance;

        public static T Instance {
            get {
                if (null == m_Instance) {
                    m_Instance = new T();
                    if (null == m_Instance) {
                        Log.E("Error Create Singleton ! ", m_Instance.GetType());
                    }
                }
                return m_Instance;
            }
            set { m_Instance = value; }
        }

        public virtual void Init()
        {
        }

        public virtual void Dispose()
        {
            m_Instance = null;
        }
    }

    public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    {
        private static T m_Instance;

        public static T Instance {
            get {
                if (null == m_Instance) {
                    m_Instance = (T)FindObjectOfType(typeof(T));
                    if (m_Instance == null) {
                        var go = new GameObject(typeof(T).Name);
                        m_Instance = go.AddComponent<T>();

                        var parent = GameObject.Find("Singleton");
                        if (parent == null) {
                            parent = new GameObject("Singleton");
                            DontDestroyOnLoad(parent);
                        }
                        go.transform.parent = parent.transform;
                    }
                }
                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        public virtual void Dispose()
        {
            m_Instance = null;
            Destroy(gameObject);
        }
    }
}