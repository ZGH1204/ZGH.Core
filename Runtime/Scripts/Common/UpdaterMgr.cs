using System.Collections.Generic;

namespace ZGH.Core
{
    public static class UpdaterMgr
    {
        public static bool IsPause;

        private static List<AUpdater> m_Updaters = new();

        public static void Register(AUpdater updater)
        {
            m_Updaters.Add(updater);
            m_Updaters.Sort((x, y) => { return x.priority - y.priority; });
        }

        public static void Unregister(AUpdater updater)
        {
            m_Updaters.Remove(updater);
        }

        public static void UnregisterAll()
        {
            m_Updaters.Clear();
        }

        public static void Update()
        {
            if (IsPause) {
                return;
            }

            var count = m_Updaters.Count;
            for (int i = 0; i < count; i++) {
                m_Updaters[i].Update();
                if (count != m_Updaters.Count) {
                    return;
                }
            }
        }

        public static void FixUpdate()
        {
            if (IsPause) {
                return;
            }

            var count = m_Updaters.Count;
            for (int i = 0; i < count; i++) {
                m_Updaters[i].FixUpdate();
                if (count != m_Updaters.Count) {
                    return;
                }
            }
        }
    }

    public abstract class AUpdater
    {
        public int priority;
        public bool isPause = false;

        public AUpdater()
        {
            UpdaterMgr.Register(this);
        }

        public virtual void Update()
        { }

        public virtual void FixUpdate()
        { }
    }
}