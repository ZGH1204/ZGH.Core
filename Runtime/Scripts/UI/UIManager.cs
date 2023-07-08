using System.Collections.Generic;
using UnityEngine;
using ZGH.Core.Pool;

namespace ZGH.Core.UI
{
    public partial class UIManager : SingletonMonobehaviour<UIManager>
    {
        public static IViewConfigHandler ConfigHandler;

        private Transform m_UIRoot;
        private Dictionary<UIZOrder, List<UIBaseView>> m_OrderDic = new();

        protected override void Awake()
        {
            m_UIRoot = GameObject.Find("UIRoot").transform;
            m_OrderDic[UIZOrder.Bg] = new List<UIBaseView>(10);
            m_OrderDic[UIZOrder.Common] = new List<UIBaseView>(10);
            m_OrderDic[UIZOrder.Tip] = new List<UIBaseView>(10);
        }

        protected override void Start()
        {
            base.Start();
        }

        public UIBaseView OpenUI(int id, object arg = null)
        {
            var config = ConfigHandler.Get(id);
            var list = m_OrderDic[config.order];

            var view = list.Find((x) => { return x.id == id; });
            if (view) {
                view.Show(arg);
            } else {
                var go = ConfigHandler.GetGameObject(id);
                go.transform.SetParent(m_UIRoot);
                go.transform.Zeroize();
                view = go.GetComponent<UIBaseView>();
                list.Add(view);
                view.Init();
                view.Open(arg);
            }
            UpdateSortingOrder(view, config.order);

            return view;
        }

        public void CloseUI(int id)
        {
            var config = ConfigHandler.Get(id);
            var list = m_OrderDic[config.order];
            var view = list.Find((x) => { return x.id == id; });
            if (view) {
                view.Close();
                list.Remove(view);
                m_OrderDic[UIZOrder.Common].Remove(view);
                GameObjectPoolMgr.Instance.Release(view.gameObject);
            }
        }

        public void CloseAll(UIZOrder order)
        {
            var list = m_OrderDic[order];
            for (var i = 0; i < list.Count; i++) {
                CloseUI(list[i].id);
                i--;
            }
        }

        public void CloseAll()
        {
            CloseAll(UIZOrder.Bg);
            CloseAll(UIZOrder.Common);
            CloseAll(UIZOrder.Tip);
        }

        public bool IsOpenUI(int id)
        {
            var config = ConfigHandler.Get(id);
            var list = m_OrderDic[config.order];
            var view = list.Find((x) => { return x.id == id; });
            return view && view.IsOpen();
        }

        public UIBaseView GetUIView(int id)
        {
            var config = ConfigHandler.Get(id);
            var list = m_OrderDic[config.order];
            return list.Find((x) => { return x.id == id; });
        }

        private void UpdateSortingOrder(UIBaseView view, UIZOrder order)
        {
            var list = m_OrderDic[order];

            if (list.Remove(view)) {
                list.Sort((x, y) => { return y.canvas.sortingOrder - x.canvas.sortingOrder; });
            }
            list.Add(view);
            for (var i = 0; i < list.Count; i++) {
                list[i].canvas.sortingOrder = (int)order + i * 10;
            }
        }
    }
}