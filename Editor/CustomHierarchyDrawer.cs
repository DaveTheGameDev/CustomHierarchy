using System;
using UnityEngine;

namespace CustomHierarchy
{
    public abstract class CustomHierarchyDrawer
    {
        protected static GameObject SelectedObject => CustomHierarchyEditor.CurrentGameObject;
        protected static CustomHierarchySettings.Settings Settings => CustomHierarchySettings.settings;
        
        protected abstract string ValidObjectNamePrefix { get; }
        
        public abstract void DrawGUI(Rect rect);

        protected virtual bool IsValid()
        {
            return SelectedObject.name.StartsWith(ValidObjectNamePrefix, StringComparison.Ordinal);
        }
    }
}