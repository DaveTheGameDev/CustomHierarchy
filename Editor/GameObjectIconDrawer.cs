using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class GameObjectIconDrawer : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix { get; }
        
        private const string IgnoredIcons = "GameObject Icon, Prefab Icon";
        private static GameObject lastGameObject;
        
        public override void DrawGUI(Rect rect)
        {
            if(!CustomHierarchySettings.settings.showGameObjectIcon)
                return;
            
            var image = EditorGUIUtility.ObjectContent( CustomHierarchyEditor.CurrentGameObject, null);
            rect.width = 14;
            rect.height = 14;
            rect.x = 34;
            rect.y += 1;

            if (lastGameObject && Event.current.commandName == "ObjectSelectorClosed")
            {
                Texture2D newIcon = (Texture2D)EditorGUIUtility.GetObjectPickerObject();
                
                var ty = typeof( EditorGUIUtility );
                var mi = ty.GetMethod( "SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static );
                mi.Invoke( null, new object[] { lastGameObject, newIcon } );

                lastGameObject = null;
            }
            
            if (image != null && image.image)
            {
                if(!CustomHierarchySettings.settings.showDefaultGameObjectIcons && IgnoredIcons.Contains(image.image.name))
                    return; 
                
                GUIContent img = new GUIContent(image.image);
                
                if(CustomHierarchySettings.settings.showDefaultGameObjectIcons && IgnoredIcons.Contains(image.image.name))
                {
                    img = new GUIContent((Texture)EditorGUIUtility.Load("GameObject Icon"));
                }

                DrawActiveButton(rect, img);
            }
        }
        
        private static void DrawActiveButton(Rect rect,  GUIContent texture)
        {
            Color c = GUI.color;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            
            GUI.changed = false;

            GUI.Button(rect, texture, GUIStyle.none);
            
            GUI.color = c;

            if (GUI.changed)
            {
                EditorGUIUtility.ShowObjectPicker<Texture2D>(null, false, "", EditorGUIUtility.GetControlID(FocusType.Passive) + 100);
                lastGameObject = CustomHierarchyEditor.CurrentGameObject;
            }
        }
    }
}