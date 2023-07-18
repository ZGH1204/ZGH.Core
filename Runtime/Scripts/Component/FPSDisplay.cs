using UnityEngine;

namespace ZGH.Core
{
    public class FPSDisplay : MonoBehaviour
    {
        [Tooltip("是否显示")]
        public bool isShow = true;

        [Tooltip("字体大小")]
        public int fontSize = 46;

        [Tooltip("距离边缘")]
        public int margin = 50;

        [Tooltip("显示位置")]
        public TextAnchor alignment = TextAnchor.UpperLeft;

        private GUIStyle m_GuiStyle;
        private Rect m_Rect;
        private int m_Frames;
        private float m_Fps;
        private float m_LastInterval;

        private void Start()
        {
            m_GuiStyle = new GUIStyle();
            m_GuiStyle.fontStyle = FontStyle.Bold;        //字体加粗
            m_GuiStyle.fontSize = fontSize;               //字体大小
            m_GuiStyle.normal.textColor = Color.green;    //字体颜色
            m_GuiStyle.alignment = alignment;             //对其方式

            m_Rect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
            m_LastInterval = Time.realtimeSinceStartup;
            m_Frames = 0;
            m_Fps = 0.0f;
        }

        private void Update()
        {
            if (!isShow) {
                return;
            }

            m_Frames++;
            var timeNow = Time.realtimeSinceStartup;
            if (timeNow > m_LastInterval + 1.0f) {
                m_Fps = m_Frames / (timeNow - m_LastInterval);
                m_Frames = 0;
                m_LastInterval = timeNow;
            }
        }

        private void OnGUI()
        {
            if (!isShow) {
                return;
            }

            if (m_GuiStyle.fontSize != fontSize) {
                m_GuiStyle.fontSize = fontSize;
            }
            if (m_GuiStyle.alignment != alignment) {
                m_GuiStyle.alignment = alignment;
            }
            if (m_Rect.x != margin) {
                m_Rect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
            }

            var col = Color.red;
            col = m_Fps >= 20f ? Color.yellow : col;
            col = m_Fps >= 30f ? Color.green : col;
            m_GuiStyle.normal.textColor = col;
            GUI.Label(m_Rect, "FPS: " + m_Fps.ToString("F1"), m_GuiStyle);
        }
    }
}