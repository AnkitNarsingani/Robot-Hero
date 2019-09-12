using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour
{
    [SerializeField] private TileList tileList;
    [SerializeField] public int tileSetSize = 10;
    [SerializeField] private float cellSize = 1;
    [SerializeField] private bool shouldAutoAdjustValueOfCell = false;
    [SerializeField] Vector3 cellGameObjectSize = new Vector3(0.7f, 0.1f, 0.7f);

    [SerializeField] public GameObject[] tileGameObjects;

    private float valueCheckForSize;

    private void Awake()
    {
        valueCheckForSize = tileSetSize - cellSize;
    }

    private void OnDrawGizmos()
    {
        float diffrenceToCheck = tileSetSize - cellSize;
        if (diffrenceToCheck != valueCheckForSize)
        {
            if (shouldAutoAdjustValueOfCell)
                cellGameObjectSize.Set(cellSize, 0.1f, cellSize);
            else
            {
                if (cellGameObjectSize.x > cellSize || cellGameObjectSize.y > cellSize)
                    Debug.Log("WARNING: cellGameObjectSize is greater than cellSize");
            }

            Reset();

            valueCheckForSize = tileSetSize - cellSize;
        }
    }

    private void Reset()
    {
        tileGameObjects = new GameObject[tileSetSize * tileSetSize];

        while (transform.childCount != 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        for (int i = 0; i < tileSetSize; i++)
        {
            for (int j = 0; j < tileSetSize; j++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.name = "Tile [" + i + "] [" + j + "]";
                tile.transform.localScale = cellGameObjectSize;
                tile.transform.position = new Vector3(i * cellSize, 0, j * cellSize);
                tile.transform.parent = gameObject.transform;
                TileSelector tileSelector = tile.AddComponent<TileSelector>();
                tileSelector.tileListScriptableObject = this.tileList;
                tileSelector.positionOnGrid = new Vector2(i, j);
            }
        }
    }
}
