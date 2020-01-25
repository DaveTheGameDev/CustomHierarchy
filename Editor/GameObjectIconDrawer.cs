using System;
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
            
            rect.width = 12;
            rect.height = 12;
            rect.x = CustomHierarchySettings.settings.iconXOffset;
            rect.y += 2;
            
            if (lastGameObject && Event.current.commandName == "ObjectSelectorClosed")
            {
                Texture2D newIcon = (Texture2D)EditorGUIUtility.GetObjectPickerObject();
                
                Type ty = typeof( EditorGUIUtility );
                MethodInfo methodInfo = ty.GetMethod( "SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static );
                if (methodInfo != null) methodInfo?.Invoke(null, new object[] {lastGameObject, newIcon});

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