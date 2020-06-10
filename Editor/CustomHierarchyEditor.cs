using UnityEngine;
using UnityEditor;

namespace CustomHierarchy
{
    [InitializeOnLoad]
    public static class CustomHierarchyEditor
    {
        public static GameObject CurrentGameObject { get; private set; }

        public static GUIStyle HeaderStyle;
        public static GUIStyle TagStyle;
        public static GUIStyle LayerStyle;

        private static readonly CustomHierarchyDrawer[] Icons;
       
        static CustomHierarchyEditor()
        {
            //Order here matters.
            Icons = new CustomHierarchyDrawer[]
            {
                
                new WarningIcon(),
                new EnabledIcon(), 
                new LayerTagDrawer(),
                new GameObjectIconDrawer(),
                new StaticIcon(),
                new HeaderDrawer(),
                new OutlineDrawer()
            };

            CustomHierarchySettings.Load();

            EditorApplication.RepaintHierarchyWindow();

            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
        }

        private static void HierarchyWindowItemOnGui(int instanceId, Rect selectionRect)
        {
            if(!CustomHierarchySettings.settings.enabled)
                return;

            CurrentGameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            
            if (CurrentGameObject == null)
                return;

            // Draw GUI for each drawer
            foreach (CustomHierarchyDrawer iconBase in Icons)
            {
                iconBase.DrawGUI(new Rect(selectionRect));
            }
        }
    }
}