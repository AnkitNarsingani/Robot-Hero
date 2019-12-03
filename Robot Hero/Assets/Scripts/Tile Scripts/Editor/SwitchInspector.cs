using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchScript), false)]
[CanEditMultipleObjects]
public class SwitchInspector : GameTilesInspector
{
    private SerializedProperty door;

    protected override void OnEnable()
    {
        base.OnEnable();
        door = serializedObject.FindProperty("door");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(door, new GUIContent("Door"));

        base.OnInspectorGUI();
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}
