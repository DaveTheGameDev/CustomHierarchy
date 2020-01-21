using UnityEditor;
using UnityEngine;

namespace Packages.CustomHierarchy
{
    public class MenuItems
    {
        private static double timeSinceRenameEvent;
        
        [MenuItem("GameObject/Create Header Object", false, 0)]
        private static void CreateHeader()
        {
            GameObject gameObject = new GameObject("--- NAME");
            Selection.activeGameObject = gameObject;
            
            timeSinceRenameEvent = EditorApplication.timeSinceStartup + 0.2d;
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (EditorApplication.timeSinceStartup >= timeSinceRenameEvent)
            {
                EditorApplication.update -= Update;
                EditorWindow.focusedWindow.SendEvent(Event.KeyboardEvent("f2"));
            }
        }
    }
}