using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchBridge), false)]
[CanEditMultipleObjects]
[ExecuteInEditMode]
public class SwitchBridgeInspector : GameTilesInspector
{
    private SerializedProperty bridgeTiles;

    protected override void OnEnable()
    {
        base.OnEnable();
        bridgeTiles = serializedObject.FindProperty("bridgeTiles");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(bridgeTiles, new GUIContent("Bridge Tiles"));

        base.OnInspectorGUI();   
        serializedObject.ApplyModifiedProperties();
    }
}

