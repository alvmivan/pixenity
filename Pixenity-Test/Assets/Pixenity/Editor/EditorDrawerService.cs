using System;
using UnityEditor;
using UnityEngine;

namespace Pixenity.Editor
{
    public class EditorDrawerService : DrawerService
    {
        private static readonly GUILayoutOption[] BoolFieldsOptions = {};
        public DrawableField<bool> CreateBool(string label="")
        {
            return new EditorBoolField(label, null, BoolFieldsOptions);
        }
    }

    public class EditorBoolField : DrawableField<bool>
    {
        public event ValueChanged<bool> OnChange = (x,y)=>{};

        private readonly string label;
        private readonly GUIStyle guiStyle;
        private readonly GUILayoutOption[] options;
        
        public bool Value { get; set; }

        public EditorBoolField(string label, GUIStyle guiStyle=null, params GUILayoutOption[] options)
        {
            this.label = label;
            this.guiStyle = guiStyle;
            this.options = options;
        }

        bool DrawField()
        {
            var style = guiStyle != null;
            var labeled = !string.IsNullOrEmpty(label);

            if (style && labeled)   return GUILayout.Toggle(Value, label, guiStyle, options);
            if (style && !labeled)  return EditorGUILayout.Toggle(Value, guiStyle, options);
            if (!style && !labeled) return EditorGUILayout.Toggle(Value, options);
            if (!style && labeled)  return GUILayout.Toggle(Value, label, options);
            return false;//unreached
        }
        
        public void Draw()
        {
            var oldValue = Value;

            var newValue = DrawField();

            Value = newValue;
            if (oldValue != newValue)
            {
                OnChange(oldValue, newValue);
            }
        }

        public void Dispose()
        {
            OnChange = null;
        }
    }
}