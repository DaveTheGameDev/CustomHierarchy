using System;
using UnityEngine;
using UnityEditor;

namespace CustomHierarchy
{
    [InitializeOnLoad]
    public static class CustomHierarchyEditor
    {
        public static GameObject CurrentGameObject { get; private set; }
      

        public static GUIStyle HeaderStyle;
        public static GUIStyle TagStyle;
        public static GUIStyle LayerStyle;

        private static IconBase[] icons;
        static CustomHierarchyEditor()
        {
            icons = new IconBase[]
            {
                new WarningIcon(),
                new EnabledIcon(), 
                new LayerTagDrawer(),
            };

            CustomHierarchySettings.Load();

            EditorApplication.RepaintHierarchyWindow();

            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
        }

        static void HierarchyWindowItemOnGui(int instanceId, Rect selectionRect)
        {
            GUI.depth = int.MaxValue;
            var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            CurrentGameObject = gameObject;
            
            if (gameObject == null)
                return;

            // Rect and Label
            DrawHeader(ref selectionRect, gameObject);

            // Toggle Active Button
            DrawIcons(ref selectionRect, gameObject);
        }

        private static void DrawIcons(ref Rect selectionRect, GameObject gameObject)
        {
            if (gameObject != null && !gameObject.name.StartsWith("---", StringComparison.Ordinal))
            {
                Rect rect = new Rect(selectionRect);
                rect.x = Screen.width - 30;
                rect.yMin += 3f;
                rect.yMax += 4;
                rect.width = 12;
                rect.height = 12;

                foreach (IconBase iconBase in icons)
                {
                    iconBase.DrawGUI(rect);
                }
            }
        }
        private static void DrawHeader(ref Rect selectionRect, GameObject gameObject)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (gameObject != null && gameObject.name.StartsWith("---", StringComparison.Ordinal))
            {
                Rect rect = new Rect(selectionRect);
                rect.width = Screen.width;
                rect.xMin = CustomHierarchySettings.settings.headerXStartOffset;
                rect.xMax = Screen.width - CustomHierarchySettings.settings.headerXEndOffset;

                EditorGUI.DrawRect(rect, CustomHierarchySettings.settings.headerColor);

                if (CustomHierarchySettings.settings.showOutline)
                    DrawOutline(rect, CustomHierarchySettings.settings.outlineSize,
                        CustomHierarchySettings.settings.outlineColor);

                rect.yMax -= 2;

                EditorGUI.LabelField(rect, gameObject.name.Substring(3).ToUpperInvariant(), HeaderStyle);
            }
        }

        private static void DrawOutline(Rect rect, float size, Color color)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            Color color1 = GUI.color;
            GUI.color *= color;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, size), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - size, rect.width, size), EditorGUIUtility.whiteTexture);

            GUI.DrawTexture(new Rect(rect.x, rect.y + 1f, size, rect.height - size), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - size, rect.y + 1f, size, rect.height - size),
                EditorGUIUtility.whiteTexture);

            GUI.color = color1;
        }


        [MenuItem("GameObject/Create Header Object", false, 0)]
        static void CreateHeader()
        {
            var gameObject = new GameObject("--- NAME");
        }

       
    }
}