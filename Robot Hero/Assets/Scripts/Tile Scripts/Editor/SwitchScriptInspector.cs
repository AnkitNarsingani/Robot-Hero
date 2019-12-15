using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchScript), true)]
[CanEditMultipleObjects]
public class SwitchScriptInspector : GameTilesInspector
{
    private SerializedProperty doorEditor;

    protected override void OnEnable()
    {
        base.OnEnable();
        doorEditor = serializedObject.FindProperty("door");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(doorEditor, new GUIContent("Door"));

        base.OnInspectorGUI();  
        serializedObject.ApplyModifiedProperties();
    }
}
