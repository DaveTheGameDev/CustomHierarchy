using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class LayerTagDrawer : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix => "---";
        
        private static GameObject lastObject;
        public override void DrawGUI(Rect rect)
        {
            if(!IsValid())
                return;
            
            rect.x = Screen.width - 115;
            rect.width = 50;
                
            DrawTag(rect);
            DrawLayer(rect);
        }

        private static void DrawLayer(Rect rect)
        {
            if (!CustomHierarchySettings.settings.showLayer) 
                return;
            

            string layer = LayerMask.LayerToName(CustomHierarchyEditor.CurrentGameObject.layer);

            Color c = GUI.color;
            if (layer == "Default")
            {
                if(!CustomHierarchySettings.settings.showDefaultLayer)
                    return;
                
                GUI.color = new Color(c.r,c.g,c.b, c.a * 0.5f);
            }

            DrawLayerWithMenu(rect, layer);
            GUI.color = c;
        }

        private static void DrawTag(Rect rect)
        {
            if (CustomHierarchySettings.settings.showTag)
            {
                rect.y -= 7;

                Color c = GUI.color;
                
                if (CustomHierarchyEditor.CurrentGameObject.CompareTag("Untagged"))
                {
                    if(!CustomHierarchySettings.settings.showDefaultTag)
                        return;
                    
                    GUI.color = new Color(c.r,c.g,c.b, c.a * 0.5f);
                }
                
                DrawTagWithMenu(rect, CustomHierarchyEditor.CurrentGameObject.tag);
                GUI.color = c;
            }
        }

        private static void DrawTagWithMenu(Rect rect, string text)
        {
            GUI.changed = false;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.Button(rect, text, CustomHierarchyEditor.LayerStyle);
            
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
        
        private static void DrawLayerWithMenu(Rect rect, string text)
        {
            GUI.changed = false;

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.Button(rect, text, CustomHierarchyEditor.LayerStyle);

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