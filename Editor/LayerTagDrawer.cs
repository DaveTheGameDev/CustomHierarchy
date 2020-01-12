using UnityEngine;

namespace CustomHierarchy
{
    public class LayerTagDrawer : IconBase
    {
        public override void DrawGUI(Rect rect)
        {
            rect.x = Screen.width - 100;
            rect.width = 50;
                
            DrawTag(rect);
            DrawLayer(rect);
        }

        private static void DrawLayer(Rect rect)
        {
            if (CustomHierarchySettings.settings.showLayer)
            {
                if (!CustomHierarchySettings.settings.showTag)
                {
                    rect.y -= 5;
                }

                rect.y += 2;
                string layer = LayerMask.LayerToName(CustomHierarchyEditor.CurrentGameObject.layer);

                if (layer == "Default" && !CustomHierarchySettings.settings.showDefaultLayer)
                    return;
                    
                GUI.Label(rect, layer, CustomHierarchyEditor.LayerStyle);
            }
        }

        private static void DrawTag(Rect rect)
        {
            if (CustomHierarchySettings.settings.showTag)
            {
                rect.y -= 5;

                if (CustomHierarchyEditor.CurrentGameObject.CompareTag("Untagged") && !CustomHierarchySettings.settings.showDefaultTag)
                    return;
                
                GUI.Label(rect, CustomHierarchyEditor.CurrentGameObject.tag, CustomHierarchyEditor.TagStyle);
            }
        }
    }
}