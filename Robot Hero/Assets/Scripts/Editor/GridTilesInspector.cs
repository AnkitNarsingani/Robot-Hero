﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSelector))]
[CanEditMultipleObjects]
public class GridTilesInspector : Editor
{
    string[] options;
    int index = 0;
    [SerializeField] TileSelector tileSelector;

    private SerializedProperty _value;

    private void OnEnable()
    {
        _value = serializedObject.FindProperty("positionOnGrid");
    }

    void Init()
    {
        tileSelector = (TileSelector)target;
        options = new string[tileSelector.tileListScriptableObject.tilesList.Count];

        for (int i = 0; i < tileSelector.tileListScriptableObject.tilesList.Count; i++)
        {
            options[i] = tileSelector.tileListScriptableObject.tilesList[i].ToString();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_value, new GUIContent("Position On Grid"));
        serializedObject.ApplyModifiedProperties();

        Init();
        index = EditorGUILayout.Popup("Tile:", index, options);
        if (GUILayout.Button("Create"))
            InstantiateBlock();
    }

    void InstantiateBlock()
    {
        if (tileSelector.tileListScriptableObject.tilesList[index].tilePrefab != null)
        {
            GameObject newTile = Instantiate(tileSelector.tileListScriptableObject.tilesList[index].tilePrefab, tileSelector.transform.position, Quaternion.identity);
            newTile.transform.localScale = tileSelector.transform.localScale;
            GridSystem gridSystem = FindObjectOfType<GridSystem>();
            newTile.transform.parent = gridSystem.transform;
            newTile.name = newTile.name + " [" + tileSelector.positionOnGrid.x + "] [" + tileSelector.positionOnGrid.y + "]";
            gridSystem.tileTransforms[(int)tileSelector.positionOnGrid.x + (int)tileSelector.positionOnGrid.y * gridSystem.tileSetSize] = newTile.transform;
            Undo.RegisterCreatedObjectUndo(newTile, "Object " + newTile.name);
            Undo.DestroyObjectImmediate(tileSelector.gameObject);
        }
        else
            Debug.Log("Prefab not assigned to: " + tileSelector.tileListScriptableObject.tilesList[index].name);
    }
}