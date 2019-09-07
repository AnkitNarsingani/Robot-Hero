using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileSelector))]
public class TileInspector : Editor
{
    string[] options;
    int index = 0;
    TileSelector tileSelector;

    void Init()
    {
        tileSelector = (TileSelector)target;
        options = new string[tileSelector.tileList.tilesList.Count];

        for (int i = 0; i < tileSelector.tileList.tilesList.Count; i++)
        {
            options[i] = tileSelector.tileList.tilesList[i].ToString();
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
        if (tileSelector.tileList.tilesList[index].tilePrefab != null)
        {
            GameObject g = Instantiate(tileSelector.tileList.tilesList[index].tilePrefab, tileSelector.transform.position, Quaternion.identity);
            g.transform.localScale = tileSelector.transform.localScale;
            DestroyImmediate(tileSelector.gameObject);
        } 
        else
            Debug.Log("Prefab not assigned to: " + tileSelector.tileList.tilesList[index].name);
    }
}
