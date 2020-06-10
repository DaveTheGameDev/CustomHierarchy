using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class LayerTagDrawer : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix => "---";
        
        private static GameObject lastObject;
        private static Texture2D tagIcon;
        private static Texture2D layerIcon;
        
        public LayerTagDrawer()
        {
            tagIcon = Resources.Load<Texture2D>("tag");
            layerIcon = Resources.Load<Texture2D>("layer");
        }
        
        public override void DrawGUI(Rect rect)
        {
            if(!IsValid())
                return;
            
            rect.width = 10;
            rect.height = 10;
            rect.y += 10;
            rect.x = Screen.width -65;
                
            DrawTag(rect);
            
            rect.x = Screen.width -80;
            rect.y -= 7;
            DrawLayer(rect);
        }

        private static void DrawLayer(Rect rect)
        {
            if (!CustomHierarchySettings.settings.showLayer) 
                return;
            
            DrawLayerWithMenu(rect);
        }

        private static void DrawTag(Rect rect)
        {
            if (CustomHierarchySettings.settings.showTag)
            {
                rect.y -= 7;
                DrawTagWithMenu(rect);
            }
        }

        private static void DrawTagWithMenu(Rect rect)
        {
            GUI.changed = false;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.Button(rect, new GUIContent(tagIcon), GUIStyle.none);
            
            if (GUI.changed)
            {
                lastObject = CustomHierarchyEditor.CurrentGameObject;
                GenericMenu menu = new GenericMenu();

                foreach (string tag in UnityEditorInternal.InternalEditorUtility.tags)
                {
                    AddMenuItemForTag(menu, $"Tag/{tag}", tag);
                }

                menu.ShowAsContext();
            }
        }
        
        private static void DrawLayerWithMenu(Rect rect)
        {
            GUI.changed = false;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            rect.y -= 1;
            GUI.Button(rect, new GUIContent(layerIcon), GUIStyle.none);

            if (GUI.changed)
            {
                lastObject = CustomHierarchyEditor.CurrentGameObject;
                GenericMenu menu = new GenericMenu();

                ArrayList layerNames = new ArrayList();
                for (int i = 0; i <= 31; i++)
                {
                    string layerN = LayerMask.LayerToName(i);
                    if (layerN.Length > 0)
                        layerNames.Add(layerN);
                }
                
                foreach (string layer in layerNames)
                {
                    AddMenuItemForLayer(menu, $"Layer/{layer}", layer);
                }

                menu.ShowAsContext();
            }
        }

        
        private static void AddMenuItemForTag(GenericMenu menu, string menuPath, string tag)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            menu.AddItem(new GUIContent(menuPath), lastObject.CompareTag(tag), OnTagSelected, tag);
        }
        
        private static void OnTagSelected(object userdata)
        {
            lastObject.tag = (string)userdata;
            lastObject = null;
        }
        
        private static void AddMenuItemForLayer(GenericMenu menu, string menuPath, string layer)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            string currentLayer = LayerMask.LayerToName(lastObject.layer);
            menu.AddItem(new GUIContent(menuPath), currentLayer.Equals(layer), OnLayerSelected, layer);
        }

        private static void OnLayerSelected(object userdata)
        {
            lastObject.layer = LayerMask.NameToLayer((string)userdata);
            lastObject = null;
        }
        
        protected override bool IsValid()
        {
            return !base.IsValid();
        }
    }
}