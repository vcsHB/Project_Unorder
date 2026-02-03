#if UNITY_EDITOR
using UnityEditor;
using Project_Unorder.FlowSystem;
using UnityEngine;

[CustomEditor(typeof(VolumeControlCue))]
public class VolumeControlCueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_effectType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_duration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_easeType"));

        var targetValues = serializedObject.FindProperty("_targetValues");
        var effectType = (VolumeControlCue.VolumeEffectType)serializedObject.FindProperty("_effectType").enumValueIndex;

        EditorGUILayout.Space();
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();

        var intensityProp = targetValues.FindPropertyRelative("Intensity");

        switch (effectType)
        {
            case VolumeControlCue.VolumeEffectType.ColorAdjustments:
                intensityProp.floatValue = EditorGUILayout.Slider("Exposure", intensityProp.floatValue, -5f, 5f);
                EditorGUILayout.PropertyField(targetValues.FindPropertyRelative("Contrast"));
                EditorGUILayout.PropertyField(targetValues.FindPropertyRelative("Saturation"));
                EditorGUILayout.PropertyField(targetValues.FindPropertyRelative("FilterColor"));
                break;

            case VolumeControlCue.VolumeEffectType.Vignette:
            case VolumeControlCue.VolumeEffectType.ChromaticAberration:
                intensityProp.floatValue = EditorGUILayout.Slider("Intensity (0~1)", intensityProp.floatValue, 0f, 1f);
                break;

            case VolumeControlCue.VolumeEffectType.LensDistortion:
                intensityProp.floatValue = EditorGUILayout.Slider("Distortion (-1~1)", intensityProp.floatValue, -1f, 1f);
                break;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Reset to Default Values"))
        {
            targetValues.FindPropertyRelative("Intensity").floatValue = 0f;
            targetValues.FindPropertyRelative("Contrast").floatValue = 0f;
            targetValues.FindPropertyRelative("Saturation").floatValue = 0f;
            targetValues.FindPropertyRelative("FilterColor").colorValue = Color.white;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif