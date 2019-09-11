using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] protected Vector2 staringGridPosition;
    [SerializeField] protected Vector2 currentGridPosition;
    [SerializeField] protected GameObject[] AccessableBlocks;
    [SerializeField] protected float maxInaccuracy = 1;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool raycastHitObject = false;

    [SerializeField] private Material[] defaultMaterials;
    private Material highlightedMaterial;

    void Start()
    {
        AccessableBlocks = new GameObject[2];
        defaultMaterials = new Material[2];
        currentGridPosition = staringGridPosition;
        highlightedMaterial = Resources.Load("Highlight Tile") as Material;
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        Vector3 startingTilePositon = gridSystem.tileGameObjects[(int)staringGridPosition.x + (int)staringGridPosition.y * gridSystem.tileSetSize].transform.position;
        transform.position = new Vector3(startingTilePositon.x, transform.position.y, startingTilePositon.z);
        GetAccessibleBlocks();
    }

    void Update()
    {
        TakeInput();
    }

    void GetAccessibleBlocks()
    {
        int upTempY = (int)currentGridPosition.y + 1;
        int downTempY = (int)currentGridPosition.y - 1;

        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject up = gridSystem.tileGameObjects[(int)currentGridPosition.x + upTempY * gridSystem.tileSetSize] ?? null;
        GameObject down = gridSystem.tileGameObjects[(int)currentGridPosition.x + downTempY * gridSystem.tileSetSize] ?? null;

        if (up != null && up.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[0] = gridSystem.tileGameObjects[(int)currentGridPosition.x + upTempY * gridSystem.tileSetSize];
            defaultMaterials[0] = AccessableBlocks[0].GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[0] = null;
        if (down != null && down.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[1] = gridSystem.tileGameObjects[(int)currentGridPosition.x + downTempY * gridSystem.tileSetSize];
            defaultMaterials[1] = AccessableBlocks[1].GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[1] = null;
    }

    protected virtual void TakeInput()
    {
        CheckRaycast();

        if (Input.touchCount > 0 && raycastHitObject)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartHighlightingTiles();
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    StopHighlightingTiles();

                    touchEndPos = touch.position;
                    if (touchStartPos != touchEndPos)
                    {
                        float leastDistance = maxInaccuracy + 1;
                        GameObject tileToMove = null;
                        foreach (GameObject tile in AccessableBlocks)
                        {
                            if (tile != null)
                            {
                                float tempCompare = GetTileDistance(tile, touchEndPos);
                                if (tempCompare < leastDistance)
                                {
                                    leastDistance = tempCompare;
                                    tileToMove = tile;
                                }
                            }
                        }
                        if (AccessableBlocks[0] == tileToMove)
                            Move(currentGridPosition.x, currentGridPosition.y + 1);
                        else
                            Move(currentGridPosition.x, currentGridPosition.y - 1);

                    }
                    raycastHitObject = false;
                    break;
            }
        }
    }

    void CheckRaycast()
    {
        if (Input.touchCount > 0)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.name.Equals(gameObject.name))
                    raycastHitObject = true;
            }
        }
    }

    protected virtual void StartHighlightingTiles()
    {
        foreach (GameObject tile in AccessableBlocks)
        {
            if (tile != null)
                tile.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }
    }

    protected virtual void StopHighlightingTiles()
    {
        for (int i = 0; i < 2; i++)
        {
            if (AccessableBlocks[i] != null)
                AccessableBlocks[i].GetComponent<MeshRenderer>().material = defaultMaterials[i];
        }
    }

    float GetTileDistance(GameObject tile, Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 pos = ray.GetPoint(distance);
            return Vector3.Distance(pos, tile.transform.position);
        }
        return maxInaccuracy + 1;
    }

    protected virtual void Move(float x, float y)
    {
        currentGridPosition = new Vector2(x, y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        Vector3 currentTilePositon = gridSystem.tileGameObjects[(int)currentGridPosition.x + (int)currentGridPosition.y * gridSystem.tileSetSize].transform.position;
        transform.position = new Vector3(currentTilePositon.x, transform.position.y, currentTilePositon.z);
        GetAccessibleBlocks();
    }
}
