using UnityEngine;
using UnityEditor;

public class TileEditor : EditorWindow
{
    private GameObject gridGameObject;
    private TileList tileList;
    public int tileSetSize = 10;
    private float cellSize = 1;
    private bool shouldAutoAdjustValueOfCell = false;
    Vector3 cellGameObjectSize = new Vector3(0.7f, 0.1f, 0.7f);

    private float valueCheckForSize;

    [MenuItem("Window/Tile Editor")]
    public static void ShowWindow()
    {
        TileEditor window = GetWindow<TileEditor>("Tile Editor");
        window.minSize = new Vector2(400, 200);
    }

    private void OnEnable()
    {
        try
        {
            gridGameObject = FindObjectOfType<GridSystem>().gameObject;
        }
        catch(System.NullReferenceException)
        {
            Debug.Log("Could not find Grid System in the current scene, created a new Grid System");
            gridGameObject = new GameObject("Grid System");
            gridGameObject.AddComponent<GridSystem>();
        }
        
        tileList = Resources.Load("Tile List") as TileList;
    }

    private void OnGUI()
    {
        DrawVariables();
        if (GUILayout.Button("Create"))
        {
            ResetTiles();
        }
    }

    private void DrawVariables()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grid Anchor");
        gridGameObject = EditorGUILayout.ObjectField(gridGameObject, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tile List");
        tileList = EditorGUILayout.ObjectField(tileList, typeof(ScriptableObject), false) as TileList;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grid Size");
        tileSetSize = EditorGUILayout.IntField(tileSetSize);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Cell Size");
        cellSize = EditorGUILayout.FloatField(cellSize);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tile Size");
        cellGameObjectSize = EditorGUILayout.Vector3Field("", cellGameObjectSize);
        EditorGUILayout.EndHorizontal();
    }

    private void ResetTiles()
    {
        GridSystem gridSystem = gridGameObject.GetComponent<GridSystem>();
        gridSystem.tileGameObjects = new GameObject[tileSetSize * tileSetSize];
        gridSystem.tileSetSize = tileSetSize;
        gridSystem.cellSize = cellSize;

        while (gridGameObject.transform.childCount != 0)
            DestroyImmediate(gridGameObject.transform.GetChild(0).gameObject);

        for (int i = 0; i < tileSetSize; i++)
        {
            for (int j = 0; j < tileSetSize; j++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.name = "Tile [" + i + "] [" + j + "]";
                tile.transform.localScale = cellGameObjectSize;
                tile.transform.parent = gridGameObject.transform;
                tile.transform.localPosition = new Vector3(i * cellSize, 0, j * cellSize);
                TileSelector tileSelector = tile.AddComponent<TileSelector>();
                tileSelector.tileListScriptableObject = this.tileList;
                tileSelector.positionOnGrid = new Vector2(i, j);
            }
        }
  
        gridSystem.tileSetSize = tileSetSize;
        gridSystem.cellSize = cellSize;
    }
}
