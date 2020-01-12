using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class CustomHierarchyWindow : EditorWindow
    {
        [MenuItem("Window/CustomHierarchy")]
        private static void ShowWindow()
        {
            var window = GetWindow<CustomHierarchyWindow>();
            window.titleContent = new GUIContent("Custom Hierarchy");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}