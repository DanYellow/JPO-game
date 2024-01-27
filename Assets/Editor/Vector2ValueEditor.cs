using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vector2Value), editorForChildClasses: true)]
public class Vector2ValueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Vector2Value e = target as Vector2Value;

        GUILayout.Label($"Current Value: {e.CurrentValue}");
        // GUILayout.Label(e.CurrentValue.ToString());
        // EditorGUILayout.SelectableLabel(e.CurrentValue.ToString());
        if (GUILayout.Button("Clear value"))
            e.CurrentValue = null;
    }
}
