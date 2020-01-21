using System;
using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CustomHierarchy
{
    public class StaticIcon : CustomHierarchyDrawer
    {
        protected override string ValidObjectNamePrefix => "---";
        
        private static Texture2D staticIcon;
        private static GameObject  lastObject;

        public StaticIcon()
        {
            staticIcon = Resources.Load<Texture2D>("static");
        }

        public override void DrawGUI(Rect rect)
        {
            if(!IsValid())
                return;
            
            if(!CustomHierarchySettings.settings.showStaticFlags)
                return;
            
            rect.width = 12;
            rect.height = 12;
            rect.y += 2;
            rect.x = Screen.width - 47;
            
            DrawActiveButton(rect, CustomHierarchyEditor.CurrentGameObject, new GUIContent(staticIcon));
        }
        
        private static void DrawActiveButton(Rect rect, GameObject gameObject, GUIContent texture)
        {
#if UNITY_2019_3
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            
            GUI.changed = false;

            GUI.Button(rect, texture, GUIStyle.none);
            
            if (GUI.changed)
            {
                lastObject = CustomHierarchyEditor.CurrentGameObject;
                GenericMenu menu = new GenericMenu();

                ArrayList flagNames = new ArrayList();
                string[] flags = Enum.GetNames(typeof(StaticEditorFlags));
                
                foreach (string flag in flags)
                {
                    flagNames.Add(flag);
                }

                AddMenuItem(menu, "Nothing", "Nothing");
                AddMenuItem(menu, "Everything", "Everything");
                
                foreach (string layer in flagNames)
                {
#if UNITY_2019_3
                    if(layer == "LightmapStatic")
                        continue;
#endif
                    AddMenuItem(menu, layer, layer);
                }
    
                menu.ShowAsContext();
            }
#else
 GUI.DrawTexture(rect, texture.image);
#endif
        }
        
        private static void AddMenuItem(GenericMenu menu, string menuPath, string flag)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            var flags = GameObjectUtility.GetStaticEditorFlags(CustomHierarchyEditor.CurrentGameObject);
            if(Enum.TryParse(flag, true, out StaticEditorFlags e))
            {
                menu.AddItem(new GUIContent(menuPath), flags.HasFlag(e), OnTagSelected, flag);
            }
            
            if (flag == "Everything")
            {
                StaticEditorFlags editorFlags = StaticEditorFlags.ContributeGI |
                                                StaticEditorFlags.OccludeeStatic |
                                                StaticEditorFlags.OccluderStatic |
                                                StaticEditorFlags.BatchingStatic |
                                                StaticEditorFlags.NavigationStatic |
                                                StaticEditorFlags.OffMeshLinkGeneration |
                                                StaticEditorFlags.ReflectionProbeStatic;

                bool hasAll = flags == editorFlags;
                menu.AddItem(new GUIContent(menuPath), hasAll, OnTagSelected, flag);
            }

            if (flag == "Nothing")
            {
                bool hasNone = flags == default;
                menu.AddItem(new GUIContent(menuPath), hasNone, OnTagSelected, flag);
            }
        }
        
        private static void OnTagSelected(object userdata)
        {
           StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(lastObject);

           if ((string) userdata == "Everything")
           {
               flags = 
                        StaticEditorFlags.ContributeGI |
                        StaticEditorFlags.OccludeeStatic|
                        StaticEditorFlags.OccluderStatic|
                        StaticEditorFlags.BatchingStatic |
                        StaticEditorFlags.NavigationStatic |
                        StaticEditorFlags.OffMeshLinkGeneration |
                        StaticEditorFlags.ReflectionProbeStatic;
           }
           else if ((string) userdata == "Nothing")
           {
               flags = default;
           }
           else if (Enum.TryParse((string)userdata, true, out StaticEditorFlags e))
           {
               if (flags.HasFlag(e))
               {
                   flags &= ~e;
               }
               else
               {
                   flags |= e;
               }
           }
           
           GameObjectUtility.SetStaticEditorFlags(lastObject, flags);
           lastObject = null;
        }

        protected override bool IsValid()
        {
            return !base.IsValid();
        }
    }
}