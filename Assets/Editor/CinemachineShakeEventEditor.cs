using UnityEditor;
using UnityEngine;

// https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f6
[CustomEditor(typeof(CinemachineShakeEventChannel), editorForChildClasses: true)]
public class CinemachineShakeEventEditor : Editor
{
    float time;
    float intensity;

    CameraShakeTypeValue dynamicShakeType = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        CinemachineShakeEventChannel e = target as CinemachineShakeEventChannel;

        time = EditorGUILayout.FloatField("duration ", time);
        intensity = EditorGUILayout.FloatField("magnitude ", intensity);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("SO");
        dynamicShakeType = (CameraShakeTypeValue)EditorGUILayout.ObjectField(dynamicShakeType, typeof(CameraShakeTypeValue), false);
        EditorGUILayout.EndHorizontal();

        CameraShakeTypeValue shakeType = new CameraShakeTypeValue();

        if (dynamicShakeType)
        {
            shakeType = dynamicShakeType;
        }
        else
        {
            shakeType.time = time;
            shakeType.intensity = intensity;
        }

        if (GUILayout.Button("Raise"))
        {
            e.Raise(shakeType);
        }
    }
}
