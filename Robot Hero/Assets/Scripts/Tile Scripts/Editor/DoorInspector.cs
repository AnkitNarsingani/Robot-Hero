using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorScript), false)]
[CanEditMultipleObjects]
public class DoorInspector : GameTilesInspector
{
    private SerializedProperty isUnlocked;

    protected override void OnEnable()
    {
        base.OnEnable();
        isUnlocked = serializedObject.FindProperty("isUnlocked");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(isUnlocked, new GUIContent("Is Unlocked"));

        base.OnInspectorGUI();
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}
