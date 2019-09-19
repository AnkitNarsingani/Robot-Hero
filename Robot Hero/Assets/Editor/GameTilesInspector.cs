using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileScript), true)]
[CanEditMultipleObjects]
public class GameTilesInspector : Editor
{
    private SerializedProperty canWalk;
    private SerializedProperty door;

    private void OnEnable()
    {

        canWalk = serializedObject.FindProperty("canWalk");
        door = serializedObject.FindProperty("door");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(canWalk, new GUIContent("Can Walk"));
        EditorGUILayout.PropertyField(canWalk, new GUIContent("Is Unlocked"));
        if (door != null)
            EditorGUILayout.PropertyField(door, new GUIContent("Door"));
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Revert Tile"))
            Revert();
    }

    void Revert()
    {
        TileScript tileScript = target as TileScript;
        GameObject newTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newTile.transform.localScale = tileScript.transform.localScale;
        newTile.transform.position = tileScript.transform.position;
        newTile.transform.SetParent(tileScript.transform.parent);
        TileSelector tileSelector = newTile.AddComponent<TileSelector>();
        tileSelector.tileListScriptableObject = Resources.Load("Tile List") as TileList;
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        int index = System.Array.FindIndex(gridSystem.tileGameObjects, item => item == tileScript.gameObject);
        if (index != -1)
        {
            tileSelector.positionOnGrid.x = (int)index % gridSystem.tileSetSize;
            tileSelector.positionOnGrid.y = (int)index / gridSystem.tileSetSize;
            newTile.name = "Tile [" + tileSelector.positionOnGrid.x + "] [" + tileSelector.positionOnGrid.y + "]";
        }
        DestroyImmediate(tileScript.gameObject);
    }
}
