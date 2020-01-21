using CustomHierarchy;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class HeaderDrawer : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix => "---";
        
        public override void DrawGUI(Rect rect)
        {
            if(!IsValid())
                return;
            
            Rect wholeRect = new Rect(rect);
            rect.width = Screen.width;
            rect.x = 60;
            wholeRect.width = Screen.width;
            wholeRect.x = 0;
                
            Color color = CustomHierarchySettings.settings.headerColor;

            if (SelectedObject == Selection.activeGameObject)
            {
                color = Settings.selectionColor;
            } 
            else if (wholeRect.Contains(Event.current.mousePosition))
            {
                color = Settings.headerHoverColor;
            }
            EditorGUI.DrawRect(rect, color);

            rect.yMax -= 2;
            Rect r = new Rect(rect) {x = 10};

            EditorGUI.LabelField(r, SelectedObject.name.Substring(3).ToUpperInvariant(), CustomHierarchyEditor.HeaderStyle);
        }
    }
}