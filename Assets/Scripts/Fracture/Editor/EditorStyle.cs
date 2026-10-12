using UnityEngine;
using UnityEditor;

namespace EsteboFracture.Editor
{
    public static class CustomEditorStyle
    {
        public static GUIStyle TitleStyle => new GUIStyle(EditorStyles.largeLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
            fontStyle = FontStyle.Bold,
        };

        public static GUIStyle HeaderStyleCentered => new GUIStyle(EditorStyles.largeLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Normal
        };

        public static GUIStyle HeaderStyleLeft => new GUIStyle(EditorStyles.whiteLargeLabel)
        {
            alignment = TextAnchor.MiddleLeft,
        };
    }
}
