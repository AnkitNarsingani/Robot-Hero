﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSelector))]
public class TileInspector : Editor
{
    string[] options;
    int index = 0;
    [SerializeField] TileSelector tileSelector;

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
            newTile.transform.parent = FindObjectOfType<GridSystem>().transform;
            newTile.AddComponent<TileScript>().canWalk = tileSelector.tileListScriptableObject.tilesList[index].canWalk;
            GridSystem gridSystem = FindObjectOfType<GridSystem>();
            gridSystem.tileGameObjects[(int)tileSelector.positionOnGrid.x + (int)tileSelector.positionOnGrid.y * gridSystem.tileSetSize] = newTile;
            DestroyImmediate(tileSelector.gameObject);
        }
        else
            Debug.Log("Prefab not assigned to: " + tileSelector.tileListScriptableObject.tilesList[index].name);
    }
}
