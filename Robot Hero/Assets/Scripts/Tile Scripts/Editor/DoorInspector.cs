using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DoorScript), false)]
[CanEditMultipleObjects]
public class DoorInspector : GameTilesInspector
{
    private SerializedProperty isUnlocked;

    private SerializedProperty exitSignMeshRenderer;
    private SerializedProperty doorTransform;

    private SerializedProperty unlockedMaterial;
    private SerializedProperty lockedMaterial;

    protected override void OnEnable()
    {
        base.OnEnable();
        isUnlocked = serializedObject.FindProperty("isUnlocked");

        exitSignMeshRenderer = serializedObject.FindProperty("exitSignMeshRenderer");
        doorTransform = serializedObject.FindProperty("doorTransform");

        unlockedMaterial = serializedObject.FindProperty("unlockedMaterial");
        lockedMaterial = serializedObject.FindProperty("lockedMaterial");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(exitSignMeshRenderer, new GUIContent("Exit Sign"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(doorTransform, new GUIContent("Door Prefab"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(unlockedMaterial, new GUIContent("Unlocked Material"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(lockedMaterial, new GUIContent("Locked Material"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(isUnlocked, new GUIContent("Is Unlocked"));


        serializedObject.ApplyModifiedProperties();
    }
}