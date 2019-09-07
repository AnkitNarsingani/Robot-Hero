using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] TileList tileList;
    [SerializeField] int tileSetSize = 10;
    [SerializeField] float cellSize = 1; 
    [SerializeField] Vector3 cellGameObjectSize = new Vector3(0.7f, 0.1f, 0.7f);

    float valueCheckForSize;


	void Start ()
    {
        valueCheckForSize = tileSetSize - cellSize;
	}
	
	void Update ()
    {
		
	}

    private void OnDrawGizmos()
    {
        if (tileSetSize - cellSize != valueCheckForSize)
        {
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
                    tileSelector.tileList = this.tileList;
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
