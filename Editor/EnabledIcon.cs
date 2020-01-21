using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class EnabledIcon : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix => "---";
        
        private static Texture2D eye;
        private static Texture2D eyeClosed;

        public EnabledIcon()
        {
            eye = Resources.Load("eye") as Texture2D;
            eyeClosed = Resources.Load("eyeClosed") as Texture2D;
        }

        public override void DrawGUI(Rect rect)
        {
            if(!IsValid())
                return;
            
            rect.width = 14;
            rect.height = 14;
            rect.x = Screen.width - 35;
            if (CustomHierarchySettings.settings.showEnabledIcon)
            {
                Texture2D currentEye = CustomHierarchyEditor.CurrentGameObject.activeSelf ? eye : eyeClosed;
                DrawActiveButton(rect, CustomHierarchyEditor.CurrentGameObject, new GUIContent(currentEye));
            }
        }

        private static void DrawActiveButton(Rect rect, GameObject gameObject, GUIContent texture)
        {
            Color c = GUI.color;
            GUI.color = new Color(c.r, c.g, c.b, texture.image == eye ? .6f : .25f);

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            
            GUI.changed = false;

            GUI.Button(rect, texture, GUIStyle.none);
            
            GUI.color = c;

            if (GUI.changed)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
        
        protected override bool IsValid()
        {
            return !base.IsValid();
        }
    }
}