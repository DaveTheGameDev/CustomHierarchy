using System;
using UnityEngine;
using UnityEditor;

namespace CustomHierarchy
{
    
[InitializeOnLoad]
public static class CustomHierarchyEditor
{
    private static Texture2D eye;
    private static Texture2D eyeClosed;
    private static Texture2D warningComponent;

    public static GUIStyle HeaderStyle;
    
    
    static CustomHierarchyEditor()
    {
        eye = Resources.Load("eye") as Texture2D;
        eyeClosed = Resources.Load("eyeClosed") as Texture2D;
        warningComponent = Resources.Load("warning") as Texture2D;

        CustomHierarchySettings.Load();

        EditorApplication.RepaintHierarchyWindow();
        
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
    }

    static void HierarchyWindowItemOnGui(int instanceId, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

        // Rect and Label
        DrawHeader(ref selectionRect, gameObject);
        
        // Toggle Active Button
        DrawIcons(ref selectionRect, gameObject);
    }

    private static void DrawIcons(ref Rect selectionRect, GameObject gameObject)
    {
        if (gameObject != null && !gameObject.name.StartsWith("---", StringComparison.Ordinal))
        {
            selectionRect.x = Screen.width - 30;
            selectionRect.yMin += 3f;
            selectionRect.yMax += 4;
            selectionRect.width = 12;
            selectionRect.height = 12;

            if (CustomHierarchySettings.settings.showEnabledIcon)
            {
                Texture2D currentEye = gameObject.activeSelf ? eye : eyeClosed;

                if (TextureButton(selectionRect, new GUIContent(currentEye)))
                {
                    gameObject.SetActive(!gameObject.activeSelf);
                }
            }

            if (CustomHierarchySettings.settings.showMissingScriptIcon)
            {
                if (!CustomHierarchySettings.settings.showEnabledIcon)
                {
                    selectionRect.x = Screen.width - 30;
                }
                else
                {
                    selectionRect.x = Screen.width - 45;
                }


                foreach (var script in gameObject.GetComponentsInChildren<MonoBehaviour>())
                {
                    if (script == null)
                    {
                        GUI.DrawTexture(selectionRect, warningComponent);
                    }
                }
            }
        }
    }

    private static void DrawHeader(ref Rect selectionRect, GameObject gameObject)
    {
        if (gameObject != null && gameObject.name.StartsWith("---", StringComparison.Ordinal))
        {
            var rect = selectionRect;
            rect.width = Screen.width;
            rect.xMin = CustomHierarchySettings.settings.headerXStartOffset;
            rect.xMax = Screen.width - CustomHierarchySettings.settings.headerXEndOffset;

            EditorGUI.DrawRect(rect, CustomHierarchySettings.settings.headerColor);

            selectionRect.yMax -= 2;

            EditorGUI.LabelField(selectionRect, gameObject.name.Substring(3).ToUpperInvariant(), HeaderStyle);
        }
    }


    [MenuItem("GameObject/Create Header Object", false, 0)]
    static void CreateHeader()
    {
        var gameObject = new GameObject("--- NAME");
    }

    public static bool TextureButton(Rect rect, GUIContent texture, params GUILayoutOption[] options)
    {
        Color c = GUI.color;
        GUI.color = new Color(c.r,c.g,c.b, texture.image == eye ? .7f : .25f);
        
        var position = rect;

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
        bool button = GUI.Button(position, texture, GUIStyle.none);
            
        GUI.color = c;
        
        return button;
    }

}

}