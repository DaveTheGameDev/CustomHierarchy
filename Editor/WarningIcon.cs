using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class WarningIcon : IconBase
    {
        private static Texture2D warningComponent;

        public WarningIcon()
        {
            warningComponent = Resources.Load("warning") as Texture2D;
        }

        public override void DrawGUI(Rect rect)
        {
            if (!CustomHierarchySettings.settings.showMissingScriptIcon || !CustomHierarchyEditor.CurrentGameObject)
                return;

            // if (!CustomHierarchySettings.settings.showEnabledIcon)
            // {
            //     rect.x = Screen.width - 30;
            // }
            // else
            // {
            //     rect.x = Screen.width - 45;
            // }

            rect.x = Screen.width - 60;
            
            foreach (var script in CustomHierarchyEditor.CurrentGameObject.GetComponentsInChildren<MonoBehaviour>())
            {
                if (script == null)
                {
                    DrawActiveButton(rect, CustomHierarchyEditor.CurrentGameObject, new GUIContent(warningComponent));
                }
            }
        }
        
        private static void DrawActiveButton(Rect rect, GameObject gameObject, GUIContent texture)
        {
#if UNITY_2019_3
           
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            
            GUI.changed = false;

            GUI.Button(rect, texture, GUIStyle.none);
  
            if (GUI.changed)
            {
                if (EditorUtility.DisplayDialog("Remove Missing Scripts", "Are You Sure?", "Yes", "No"))
                {
                    EditorApplication.delayCall += () => 
                        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                }
            }
#else
 GUI.DrawTexture(rect, texture.image);
#endif
        }
    }
}