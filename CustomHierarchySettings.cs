using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class CustomHierarchySettings : ScriptableObject
    {
        [Serializable]
        public struct Settings
        {
            public bool showMissingScriptIcon;
            public bool showEnabledIcon;
            public Color headerColor;
            public Color fontColor;
            public int fontSize;
            public TextAnchor textAnchor;
            public FontStyle fontStyle;
            public GUIStyleState styleState;
        }
        
        public enum Position
        {
            Left,
            Center,
            Right
        }

        private static string MyCustomSettingsPath = null;
        public static Settings settings;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (string.IsNullOrEmpty(MyCustomSettingsPath))
            {
                MyCustomSettingsPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "ProjectSettings/Custom Hierarchy Settings.asset");
            }
            
            if (!Load())
            {
                settings.showMissingScriptIcon = true;
                settings.showEnabledIcon = true;
                settings.textAnchor = TextAnchor.MiddleCenter;
                settings.headerColor = Color.grey;
                settings.fontColor = Color.black;
                settings.styleState.textColor = settings.fontColor;
                
                settings.fontSize = 14;
                Save();
            }
        }

        private static CustomHierarchySettings GetOrCreateSettings()
        {
            CustomHierarchySettings settingsAsset = CreateInstance<CustomHierarchySettings>();

            Init();
            
            EditorApplication.RepaintHierarchyWindow();
            
            return settingsAsset;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
        
        static class CustomHierarchySettingsRegister
        {
            [SettingsProvider]
            public static SettingsProvider CustomHierarchySettingsProvider()
            {
                var provider = new SettingsProvider("Preferences/Custom Hierarchy", SettingsScope.User)
                {
                    label = "Custom Hierarchy",
                    
                    guiHandler = searchContext =>
                    {
                        EditorGUI.BeginChangeCheck();
                        
                        settings.showMissingScriptIcon = EditorGUILayout.Toggle("Show Missing Script Icon", settings.showMissingScriptIcon);
                        settings.showEnabledIcon = EditorGUILayout.Toggle("Show Enable Icon", settings.showEnabledIcon);
                        settings.headerColor = EditorGUILayout.ColorField("Header Color", settings.headerColor);
                        settings.fontColor = EditorGUILayout.ColorField("Font Color", settings.fontColor);
                        settings.fontSize = EditorGUILayout.IntField("Header Font Size", settings.fontSize);
                        settings.fontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", settings.fontStyle);
                        settings.textAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Header Text Position", settings.textAnchor);
                        
                        if (EditorGUI.EndChangeCheck())
                        {
                            settings.styleState.textColor = settings.fontColor;
                
                            CustomHierarchyEditor.HeaderStyle = new GUIStyle
                            {
                                fontSize = settings.fontSize,
                                alignment = settings.textAnchor,
                                fontStyle = settings.fontStyle,
                                normal = settings.styleState
                            };
                            
                            EditorApplication.RepaintHierarchyWindow();
                            
                           Save();
                        }
                    },

                    // Populate the search keywords to enable smart search filtering and label highlighting:
                    //keywords = new HashSet<string>(new[] { "Number", "Some String" })
                };

                return provider;
            }
        }

        public static bool Load()
        {
            if (File.Exists(MyCustomSettingsPath))
            {
                var data = File.ReadAllText(MyCustomSettingsPath);
                settings = JsonUtility.FromJson<Settings>(data);

                settings.styleState.textColor = settings.fontColor;
                
                CustomHierarchyEditor.HeaderStyle = new GUIStyle
                {
                    fontSize = settings.fontSize,
                    alignment = settings.textAnchor,
                    fontStyle = settings.fontStyle,
                    normal = settings.styleState
                };

                return true;
            }

            return false;
        }
        public static void Save()
        {
            string data = JsonUtility.ToJson(settings, true);

            using (StreamWriter file =  new StreamWriter(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "ProjectSettings/Custom Hierarchy Settings.asset")))
            {
                file.Write(data);
            }
        }
        
    }
}