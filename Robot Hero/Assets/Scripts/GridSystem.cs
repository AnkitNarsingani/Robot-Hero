using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] TileList tileList;
    [SerializeField] int tileSetSize = 10;
    [SerializeField] float cellSize = 1;
    [SerializeField] bool shouldAutoAdjustValueOfCell = false;
    [SerializeField] Vector3 cellGameObjectSize = new Vector3(0.7f, 0.1f, 0.7f);

    public GameObject[,] tileGameObjects;
    float valueCheckForSize;

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

            tileGameObjects = new GameObject[tileSetSize, tileSetSize];

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

            valueCheckForSize = tileSetSize - cellSize;
        }

        for (int i = 0; i < tileSetSize; i++)
        {
            for (int j = 0; j < tileSetSize; j++)
            {
                Gizmos.DrawWireCube(new Vector3(i * cellSize, 0, j * cellSize), new Vector3(cellSize, 0, cellSize));
            }
        }
    }
}
