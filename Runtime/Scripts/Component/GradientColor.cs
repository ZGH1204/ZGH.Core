using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DD
{
    public enum GradientMode
    {
        Single,
        HorizontalGradient,
        VerticalGradient,
        FourCornersGradient
    }

    public class GradientColor : BaseMeshEffect
    {
        public GradientMode colorMode = GradientMode.HorizontalGradient;
        public Color topLeft = Color.white;
        public Color topRight = Color.white;
        public Color bottomLeft = Color.white;
        public Color bottomRight = Color.white;

        public bool isSplit = true;
        public Color centerColor;
        [Range(0.0f, 1.0f)] public float split = 0.5f;

        private void HorizontalLayout(VertexHelper vh)
        {
            var v_bottomLeft = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_bottomLeft, 0);

            var v_topLeft = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_topLeft, 1);

            var v_topRight = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_topRight, 2);

            var v_bottomRight = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_bottomRight, 3);

            var v_topCenter = UIVertex.simpleVert;
            v_topCenter.position = Vector4.Lerp(v_topLeft.position, v_topRight.position, split);
            v_topCenter.color = centerColor;
            v_topCenter.uv0 = Vector4.Lerp(v_topLeft.uv0, v_topRight.uv0, split);

            var v_bottomCenter = UIVertex.simpleVert;
            v_bottomCenter.position = Vector4.Lerp(v_bottomLeft.position, v_bottomRight.position, split);
            v_bottomCenter.color = centerColor;
            v_bottomCenter.uv0 = Vector4.Lerp(v_bottomLeft.uv0, v_bottomRight.uv0, split);

            var m_Verts = ListPool<UIVertex>.Get();
            m_Verts.Add(v_bottomLeft);
            m_Verts.Add(v_topLeft);
            m_Verts.Add(v_topCenter);

            m_Verts.Add(v_topRight);
            m_Verts.Add(v_bottomRight);
            m_Verts.Add(v_bottomCenter);

            var m_Indices = ListPool<int>.Get();
            m_Indices.Add(0);
            m_Indices.Add(1);
            m_Indices.Add(2);

            m_Indices.Add(2);
            m_Indices.Add(5);
            m_Indices.Add(0);

            m_Indices.Add(5);
            m_Indices.Add(2);
            m_Indices.Add(3);

            m_Indices.Add(3);
            m_Indices.Add(4);
            m_Indices.Add(5);

            vh.Clear();
            vh.AddUIVertexStream(m_Verts, m_Indices);

            ListPool<UIVertex>.Release(m_Verts);
            ListPool<int>.Release(m_Indices);
        }

        private void VerticalLayout(VertexHelper vh)
        {
            var v_bottomLeft = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_bottomLeft, 0);

            var v_topLeft = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_topLeft, 1);

            var v_topRight = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_topRight, 2);

            var v_bottomRight = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref v_bottomRight, 3);

            var v_leftCenter = UIVertex.simpleVert;
            v_leftCenter.position = Vector4.Lerp(v_bottomLeft.position, v_topLeft.position, split);
            v_leftCenter.color = centerColor;
            v_leftCenter.uv0 = Vector4.Lerp(v_bottomLeft.uv0, v_topLeft.uv0, split);

            var v_rightCenter = UIVertex.simpleVert;
            v_rightCenter.position = Vector4.Lerp(v_bottomRight.position, v_topRight.position, split);
            v_rightCenter.color = centerColor;
            v_rightCenter.uv0 = Vector4.Lerp(v_bottomRight.uv0, v_topRight.uv0, split);

            var m_Verts = ListPool<UIVertex>.Get();
            m_Verts.Add(v_bottomLeft);
            m_Verts.Add(v_leftCenter);
            m_Verts.Add(v_topLeft);
            m_Verts.Add(v_topRight);
            m_Verts.Add(v_rightCenter);
            m_Verts.Add(v_bottomRight);

            var m_Indices = ListPool<int>.Get();
            m_Indices.Add(0);
            m_Indices.Add(1);
            m_Indices.Add(4);

            m_Indices.Add(4);
            m_Indices.Add(5);
            m_Indices.Add(0);

            m_Indices.Add(1);
            m_Indices.Add(2);
            m_Indices.Add(3);

            m_Indices.Add(3);
            m_Indices.Add(4);
            m_Indices.Add(1);
            
            vh.Clear();
            vh.AddUIVertexStream(m_Verts, m_Indices);

            ListPool<UIVertex>.Release(m_Verts);
            ListPool<int>.Release(m_Indices);
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            var uI = UIVertex.simpleVert;
            for (var i = 0; i < vh.currentVertCount; i++) {
                vh.PopulateUIVertex(ref uI, i);
                var c = i == 0 ? bottomLeft : Color.white;
                c = i == 1 ? topLeft : c;
                c = i == 2 ? topRight : c;
                c = i == 3 ? bottomRight : c;
                uI.color = c;
                vh.SetUIVertex(uI, i);
            }

            if (isSplit) {
                centerColor.a = 1.0f;
                if (colorMode == GradientMode.HorizontalGradient) {
                    HorizontalLayout(vh);
                } else if (colorMode == GradientMode.VerticalGradient) {
                    VerticalLayout(vh);
                }
            }
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(GradientColor))]
    public class GradientColorEditor2 : Editor
    {
        private SerializedProperty m_ColorMode;
        private SerializedProperty m_TopLeftColor;
        private SerializedProperty m_TopRightColor;
        private SerializedProperty m_BottomLeftColor;
        private SerializedProperty m_BottomRightColor;

        private SerializedProperty m_IsSplit;
        private SerializedProperty m_Split;
        private SerializedProperty m_CenterColor;

        private void OnEnable()
        {
            m_ColorMode = serializedObject.FindProperty("colorMode");
            m_TopLeftColor = serializedObject.FindProperty("topLeft");
            m_TopRightColor = serializedObject.FindProperty("topRight");
            m_BottomLeftColor = serializedObject.FindProperty("bottomLeft");
            m_BottomRightColor = serializedObject.FindProperty("bottomRight");

            m_IsSplit = serializedObject.FindProperty("isSplit");
            m_CenterColor = serializedObject.FindProperty("centerColor");
            m_Split = serializedObject.FindProperty("split");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ColorMode, new GUIContent("Gradient Mode"));
            if (EditorGUI.EndChangeCheck()) {
                switch ((GradientMode)m_ColorMode.enumValueIndex) {
                    case GradientMode.Single:
                        m_TopRightColor.colorValue = m_TopLeftColor.colorValue;
                        m_BottomLeftColor.colorValue = m_TopLeftColor.colorValue;
                        m_BottomRightColor.colorValue = m_TopLeftColor.colorValue;
                        break;

                    case GradientMode.HorizontalGradient:
                        m_BottomLeftColor.colorValue = m_TopLeftColor.colorValue;
                        m_BottomRightColor.colorValue = m_TopRightColor.colorValue;
                        break;

                    case GradientMode.VerticalGradient:
                        m_TopRightColor.colorValue = m_TopLeftColor.colorValue;
                        m_BottomRightColor.colorValue = m_BottomLeftColor.colorValue;
                        break;
                }
            }
            Rect rect;
            switch ((GradientMode)m_ColorMode.enumValueIndex) {
                case GradientMode.Single:
                    EditorGUI.BeginChangeCheck();
                    rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                    EditorGUI.PrefixLabel(rect, new GUIContent("Colors"));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / (EditorGUIUtility.wideMode ? 1f : 2f);
                    DrawColorProperty(rect, m_TopLeftColor);
                    if (EditorGUI.EndChangeCheck()) {
                        m_TopRightColor.colorValue = m_TopLeftColor.colorValue;
                        m_BottomLeftColor.colorValue = m_TopLeftColor.colorValue;
                        m_BottomRightColor.colorValue = m_TopLeftColor.colorValue;
                    }
                    break;

                case GradientMode.HorizontalGradient:
                    rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                    EditorGUI.PrefixLabel(rect, new GUIContent("Colors"));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2f;

                    EditorGUI.BeginChangeCheck();
                    DrawColorProperty(rect, m_TopLeftColor);
                    if (EditorGUI.EndChangeCheck()) {
                        m_BottomLeftColor.colorValue = m_TopLeftColor.colorValue;
                    }

                    rect.x += rect.width;

                    EditorGUI.BeginChangeCheck();
                    DrawColorProperty(rect, m_TopRightColor);
                    if (EditorGUI.EndChangeCheck()) {
                        m_BottomRightColor.colorValue = m_TopRightColor.colorValue;
                    }
                    break;

                case GradientMode.VerticalGradient:
                    rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                    EditorGUI.PrefixLabel(rect, new GUIContent("Colors"));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / (EditorGUIUtility.wideMode ? 1f : 2f);
                    rect.height = EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2);

                    EditorGUI.BeginChangeCheck();
                    DrawColorProperty(rect, m_TopLeftColor);
                    if (EditorGUI.EndChangeCheck()) {
                        m_TopRightColor.colorValue = m_TopLeftColor.colorValue;
                    }

                    rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / (EditorGUIUtility.wideMode ? 1f : 2f);
                    rect.height = EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2);

                    EditorGUI.BeginChangeCheck();
                    DrawColorProperty(rect, m_BottomLeftColor);
                    if (EditorGUI.EndChangeCheck()) {
                        m_BottomRightColor.colorValue = m_BottomLeftColor.colorValue;
                    }
                    break;

                case GradientMode.FourCornersGradient:
                    rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                    EditorGUI.PrefixLabel(rect, new GUIContent("Colors"));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2f;
                    rect.height = EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2);

                    DrawColorProperty(rect, m_TopLeftColor);
                    rect.x += rect.width;
                    DrawColorProperty(rect, m_TopRightColor);

                    rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2f;
                    rect.height = EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2);

                    DrawColorProperty(rect, m_BottomLeftColor);
                    rect.x += rect.width;
                    DrawColorProperty(rect, m_BottomRightColor);
                    break;
            }

            if ((GradientMode)m_ColorMode.enumValueIndex == GradientMode.VerticalGradient ||
                (GradientMode)m_ColorMode.enumValueIndex == GradientMode.HorizontalGradient) {
                EditorGUILayout.PropertyField(m_IsSplit, new GUIContent("Is Split"));
                if (m_IsSplit.boolValue) {
                    rect = EditorGUILayout.GetControlRect();
                    EditorGUI.PrefixLabel(rect, new GUIContent("    Center Color"));
                    rect.x += EditorGUIUtility.labelWidth;
                    rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2f;
                    DrawColorProperty(rect, m_CenterColor);
                    EditorGUILayout.PropertyField(m_Split, new GUIContent("    Split"));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawColorProperty(Rect rect, SerializedProperty property)
        {
            var oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            if (EditorGUIUtility.wideMode) {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60f, rect.height), property, GUIContent.none);
                rect.x += 62f;
                rect.width = Mathf.Min(80f, rect.width - 70f);
            } else {
                rect.height /= 2f;
                rect.width = Mathf.Min(100f, rect.width - 5f);
                EditorGUI.PropertyField(rect, property, GUIContent.none);
                rect.y += rect.height;
            }

            EditorGUI.BeginChangeCheck();
            var colorString = EditorGUI.TextField(rect, string.Format("#{0}", ColorUtility.ToHtmlStringRGBA(property.colorValue)));
            if (EditorGUI.EndChangeCheck()) {
                Color color;
                if (ColorUtility.TryParseHtmlString(colorString, out color)) {
                    property.colorValue = color;
                }
            }
            EditorGUI.indentLevel = oldIndent;
        }
    }

#endif
}