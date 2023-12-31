﻿using UnityEngine;

namespace ZGH.Core.UI
{
    public enum UIStatus
    {
        Close = 0,
        Loading = 1,
        Open = 2,
        Hide = 3,
    }

    [DisallowMultipleComponent]
    public class UIBaseView : MonoBehaviour
    {
        [HideInInspector] public int id = -1;
        [HideInInspector] public Canvas canvas;

        public System.Action onCloseCB;

        private UIStatus m_UIStatus;
        private bool m_initFlag = false;

        public void Init()
        {
            if (m_initFlag) {
                return;
            }
            m_initFlag = true;

            canvas = transform.GetComponent<Canvas>();
            canvas.overrideSorting = true;

            OnInit();

            var cfg = UIManager.ConfigHandler.Get(id);
            if (cfg != null && cfg.isFullScreen) {
                transform.SetUIFullScreen();
            }
            var bg = transform.Find("bg");
            if (bg != null) {
                bg.SetUIFullScreen();
            }

            //if (bg != null) {
            //    bg.SetVisible(false);
            //}
            //var img_bg = transform.Find("img_bg");
            //if (img_bg != null) {
            //    img_bg.SetVisible(false);
            //}
        }

        public void Open(object arg = null)
        {
            Log.I($"打开界面 {id}");
            OnRegister();
            OnOpen(arg);

            m_UIStatus = UIStatus.Open;
        }

        public void Close()
        {
            if (m_UIStatus == UIStatus.Close) {
                return;
            }
            Log.I($"关闭界面 {id}");

            m_UIStatus = UIStatus.Close;
            m_initFlag = false;

            OnUnRegister();
            OnClose();

            onCloseCB?.Invoke();
            onCloseCB = null;
        }

        public void Show(object arg = null)
        {
            OnShow(arg);
            gameObject.SetActive(true);
            m_UIStatus = UIStatus.Open;
        }

        public void Hide()
        {
            OnHide();
            gameObject.SetActive(false);
            m_UIStatus = UIStatus.Hide;
        }

        public bool IsOpen()
        {
            return m_UIStatus == UIStatus.Open;
        }

        public void CloseSelf()
        {
            UIManager.Instance.CloseUI(id);
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnOpen(object arg = null)
        {
        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnShow(object arg = null)
        {
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnRegister()
        {
        }

        protected virtual void OnUnRegister()
        {
        }
    }
}