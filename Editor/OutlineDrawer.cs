using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class OutlineDrawer : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix { get; }
        
        public override void DrawGUI(Rect rect)
        {
            if(!CustomHierarchySettings.settings.showOutline)
                return;
            
            float size = CustomHierarchySettings.settings.outlineSize;
            Color c = GUI.color;
            GUI.color =  CustomHierarchySettings.settings.outlineColor;
            GUI.DrawTexture(new Rect(0, rect.yMax - size, Screen.width, size), EditorGUIUtility.whiteTexture);
            GUI.color = c;
        }
    }
}