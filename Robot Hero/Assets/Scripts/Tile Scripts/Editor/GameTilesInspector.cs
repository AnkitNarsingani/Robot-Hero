using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileScript), true)]
[CanEditMultipleObjects]
public class GameTilesInspector : Editor
{
    protected SerializedProperty canWalk;

    protected virtual void OnEnable()
    {
        canWalk = serializedObject.FindProperty("canWalk");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(canWalk, new GUIContent("Can Walk"));

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Revert Tile"))
            Revert();
    }

    protected void Revert()
    {
        TileScript tileScript = target as TileScript;
        GameObject newTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newTile.transform.localScale = tileScript.transform.localScale;
        newTile.transform.position = tileScript.transform.position;
        newTile.transform.SetParent(tileScript.transform.parent);
        TileSelector tileSelector = newTile.AddComponent<TileSelector>();
        tileSelector.tileListScriptableObject = Resources.Load("Tile List") as TileList;
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        int index = gridSystem.tileTransforms.FindIndex(item => item == tileScript.transform);
        if (index != -1)
        {
            tileSelector.positionOnGrid.x = index % gridSystem.tileSetSize;
            tileSelector.positionOnGrid.y = index / gridSystem.tileSetSize;
            newTile.name = "Tile [" + tileSelector.positionOnGrid.x + "] [" + tileSelector.positionOnGrid.y + "]";
        }
        DestroyImmediate(tileScript.gameObject);
    }
}
