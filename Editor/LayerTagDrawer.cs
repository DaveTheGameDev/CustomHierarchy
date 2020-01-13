using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class LayerTagDrawer : IconBase
    {
        public override void DrawGUI(Rect rect)
        {
            rect.x = Screen.width - 115;
            rect.width = 50;
                
            DrawTag(rect);
            DrawLayer(rect);
        }

        private static void DrawLayer(Rect rect)
        {
            if (!CustomHierarchySettings.settings.showLayer) 
                return;
            
            if (!CustomHierarchySettings.settings.showTag)
            {
                rect.y -= 5;
            }

            rect.y += 2;
            string layer = LayerMask.LayerToName(CustomHierarchyEditor.CurrentGameObject.layer);

            if (layer == "Default" && !CustomHierarchySettings.settings.showDefaultLayer)
                return;

            GUI.changed = false;

            DrawLayerWithMenu(rect, layer);
        }

        private static void DrawTag(Rect rect)
        {
            if (CustomHierarchySettings.settings.showTag)
            {
                rect.y -= 5;

                if (CustomHierarchyEditor.CurrentGameObject.CompareTag("Untagged") && !CustomHierarchySettings.settings.showDefaultTag)
                    return;
                
                DrawTagWithMenu(rect, CustomHierarchyEditor.CurrentGameObject.tag);
            }
        }

        private static void DrawTagWithMenu(Rect rect, string text)
        {
            GUI.changed = false;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.Button(rect, text, CustomHierarchyEditor.LayerStyle);

            Event e = Event.current;
            
            if (!e.isMouse && e.button != 1)
            {
                return;
            }

            if (GUI.changed)
            {
                GenericMenu menu = new GenericMenu();

                foreach (string tag in UnityEditorInternal.InternalEditorUtility.tags)
                {
                    AddMenuItemForTag(menu, $"Tag/{tag}", tag);
                }

                menu.ShowAsContext();
            }
        }
        
        private static void DrawLayerWithMenu(Rect rect, string text)
        {
            GUI.changed = false;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.Button(rect, text, CustomHierarchyEditor.LayerStyle);

            Event e = Event.current;
            
            if (!e.isMouse && e.button != 1)
            {
                return;
            }

            if (GUI.changed)
            {
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
            menu.AddItem(new GUIContent(menuPath), CustomHierarchyEditor.CurrentGameObject.CompareTag(tag), OnTagSelected, tag);
        }
        
        private static void OnTagSelected(object userdata)
        {
            CustomHierarchyEditor.CurrentGameObject.tag = (string)userdata;
        }
        
        private static void AddMenuItemForLayer(GenericMenu menu, string menuPath, string layer)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            string currentLayer = LayerMask.LayerToName(CustomHierarchyEditor.CurrentGameObject.layer);
            menu.AddItem(new GUIContent(menuPath), currentLayer.Equals(layer), OnLayerSelected, layer);
        }

        private static void OnLayerSelected(object userdata)
        {
            CustomHierarchyEditor.CurrentGameObject.layer = LayerMask.NameToLayer((string)userdata);
        }
    }
}