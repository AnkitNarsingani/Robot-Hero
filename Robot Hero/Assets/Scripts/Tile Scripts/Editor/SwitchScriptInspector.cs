﻿using UnityEngine;
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
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(doorEditor, new GUIContent("Door"));

        
        serializedObject.ApplyModifiedProperties();
    }
}
