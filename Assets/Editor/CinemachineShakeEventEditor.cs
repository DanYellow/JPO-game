using UnityEditor;
using UnityEngine;

// https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f6
[CustomEditor(typeof(CinemachineShakeEventChannel), editorForChildClasses: true)]
public class CinemachineShakeEventEditor : Editor
{
    float duration;
    float intensity;

    CameraShakeType dynamicShakeType = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        CinemachineShakeEventChannel e = target as CinemachineShakeEventChannel;

        GUILayout.Label("Test", EditorStyles.boldLabel);
        duration = EditorGUILayout.FloatField("duration ", duration);
        intensity = EditorGUILayout.FloatField("magnitude ", intensity);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("SO");
        dynamicShakeType = (CameraShakeType)EditorGUILayout.ObjectField(dynamicShakeType, typeof(CameraShakeType), false);
        EditorGUILayout.EndHorizontal();

        CameraShakeType shakeType = new CameraShakeType();

        if (dynamicShakeType)
        {
            shakeType = dynamicShakeType;
        }
        else
        {
            shakeType.duration = duration;
            shakeType.intensity = intensity;
        }

        if (GUILayout.Button("Raise"))
        {
            e.Raise(shakeType);
        }
    }
}