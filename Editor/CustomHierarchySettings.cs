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
            public bool enabled;
            public bool showLayer;
            public bool showTag;
            public bool showStaticFlags;
            public bool showDefaultTag;
            public bool showDefaultLayer;
            
            public bool showMissingScriptIcon;
            public bool showEnabledIcon;
            public bool showGameObjectIcon;
            public bool showDefaultGameObjectIcons;
            public bool showOutline;
           
            public Color outlineColor;
            public Color headerColor;
            public Color fontColor;
            public Color selectionColor;

            public float headerXOffset;
            public float iconXOffset;
            
            public int outlineSize;
            public int fontSize;
            
            public TextAnchor textAnchor;
            public FontStyle fontStyle;
            public GUIStyleState styleState;
            
            public float headerXStartOffset;
            public float headerXEndOffset;
           
            public int tagFontSize;
            public Color headerHoverColor;
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
                settings.enabled = true;
                settings.showMissingScriptIcon = true;
                settings.showEnabledIcon = true;
                settings.showStaticFlags = true;
                settings.showGameObjectIcon = true;
                settings.showDefaultGameObjectIcons = true;
                settings.showTag = true;
                settings.showLayer = true;
                settings.showDefaultLayer = true;
                settings.showDefaultTag = true;
                
                settings.showOutline = true;
                settings.outlineSize = 1;
                
                settings.headerXEndOffset = 20;
                settings.headerXStartOffset = 60;
                settings.textAnchor = TextAnchor.MiddleCenter;
                settings.fontStyle = FontStyle.Bold;
                
                settings.headerXOffset = 30;
                settings.iconXOffset = 4;
                
                settings.fontSize = 12;
                settings.tagFontSize = 7;
                
                settings.outlineColor = new Color(0.61f, 0.61f, 0.61f);
                settings.fontColor = Color.black;
                settings.headerColor = new Color(0.76f, 0.76f, 0.76f);
                settings.headerHoverColor = new Color(0.7f, 0.7f, 0.7f);
                settings.selectionColor = new Color(0.23f, 0.45f, 0.69f);

                settings.styleState = new GUIStyleState {textColor = settings.fontColor};

                ApplyTextStyles();
                
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
                        
                        settings.enabled = EditorGUILayout.Toggle("Enabled", settings.enabled);
                        settings.showMissingScriptIcon = EditorGUILayout.Toggle("Show Missing Script Icon", settings.showMissingScriptIcon);
                        settings.showStaticFlags = EditorGUILayout.Toggle("Show Static Flags Icon", settings.showStaticFlags);
                        settings.showEnabledIcon = EditorGUILayout.Toggle("Show Enable Icon", settings.showEnabledIcon);
                        settings.showGameObjectIcon = EditorGUILayout.Toggle("Show GameObject Icon", settings.showGameObjectIcon);
                        settings.showDefaultGameObjectIcons = EditorGUILayout.Toggle("Show Default GameObject Icon", settings.showDefaultGameObjectIcons);
                        settings.showLayer = EditorGUILayout.Toggle("Show Layer", settings.showLayer);
                        settings.showTag = EditorGUILayout.Toggle("Show Tag", settings.showTag);
                        settings.showDefaultLayer = EditorGUILayout.Toggle("Show Default Layer", settings.showDefaultLayer);
                        settings.showDefaultTag = EditorGUILayout.Toggle("Show Default Tag", settings.showDefaultTag);
                        
                        
                        settings.showOutline = EditorGUILayout.Toggle("Show Outline", settings.showOutline);
                        
                        settings.outlineSize = EditorGUILayout.IntSlider("Outline Size", settings.outlineSize, 1, 5);
 
                        settings.headerXOffset = EditorGUILayout.FloatField("Header X Start Offset", settings.headerXOffset);
                        settings.iconXOffset = EditorGUILayout.FloatField("Icon X End Offset", settings.iconXOffset);
                        
                        settings.headerColor = EditorGUILayout.ColorField(new GUIContent("Header Color"), settings.headerColor, true, false, false);
                        settings.outlineColor = EditorGUILayout.ColorField(new GUIContent("Outline Color"), settings.outlineColor, true, false, false);
                        settings.fontColor = EditorGUILayout.ColorField(new GUIContent("Header Font Color"), settings.fontColor, true, false, false);
                        settings.selectionColor = EditorGUILayout.ColorField(new GUIContent("Header Selection Color"), settings.selectionColor, true, false, false);
                        settings.headerHoverColor = EditorGUILayout.ColorField(new GUIContent("Header Hover Color"), settings.headerHoverColor, true, false, false);
                        settings.fontSize = EditorGUILayout.IntSlider("Header Font Size", settings.fontSize, 1, 20);
                        settings.tagFontSize = EditorGUILayout.IntSlider("Tag Font Size", settings.tagFontSize, 1, 20);
         
                        settings.fontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", settings.fontStyle);
                        settings.textAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Header Text Position", settings.textAnchor);
                        
                        if (EditorGUI.EndChangeCheck())
                        {
                            settings.styleState.textColor = settings.fontColor;

                            ApplyTextStyles();

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
                
                ApplyTextStyles();

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
        
        private static void ApplyTextStyles()
        {
            CustomHierarchyEditor.HeaderStyle = new GUIStyle
            {
                fontSize = settings.fontSize,
                alignment = settings.textAnchor,
                fontStyle = settings.fontStyle,
                normal = settings.styleState
            };

            CustomHierarchyEditor.TagStyle = new GUIStyle
            {
                fontSize = settings.tagFontSize,
                alignment = TextAnchor.MiddleRight,
                fontStyle = settings.fontStyle,
                //normal = settings.styleState
            };

            CustomHierarchyEditor.LayerStyle = new GUIStyle
            {
                fontSize = settings.tagFontSize,
                alignment = TextAnchor.LowerRight,
                fontStyle = settings.fontStyle,
                //normal = settings.styleState
            };
        }
    }
}