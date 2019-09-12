using UnityEngine;

public abstract class Robot : MonoBehaviour
{
    [SerializeField] protected Vector2 staringGridPosition;
    [SerializeField] public Vector2 currentGridPosition;
    [SerializeField] protected float maxInaccuracy = 1;
    [SerializeField] protected int noOfDirections = 2;
    [SerializeField] protected GameObject[] AccessableBlocks;
    [SerializeField] protected Material[] defaultMaterials;

    public GameObject currentTile;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool raycastHitObject = false;

    private Material highlightedMaterial;

    protected abstract void GetAccessibleBlocks();

    public abstract void Move(GameObject tile);
    public abstract bool Move(float x);

    protected void Start()
    {
        AccessableBlocks = new GameObject[noOfDirections];
        defaultMaterials = new Material[noOfDirections];
        currentGridPosition = staringGridPosition;
        highlightedMaterial = Resources.Load("Highlight Tile") as Material;
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        currentTile = gridSystem.tileGameObjects[(int)staringGridPosition.x + (int)staringGridPosition.y * gridSystem.tileSetSize];
        currentTile.GetComponent<TileScript>().isOccupied = true;
        Vector3 startingTilePositon = currentTile.transform.position;
        transform.position = new Vector3(startingTilePositon.x, transform.position.y, startingTilePositon.z);
        GetAccessibleBlocks();
    }

    protected void Update()
    {
        TakeInput();
    }

    protected void TakeInput()
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
                    if (touchStartPos != touchEndPos && touchStartPos != Vector2.zero)
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

                        if (tileToMove != null)
                            Move(tileToMove);
                    }
                    raycastHitObject = false;
                    touchStartPos = Vector2.zero;
                    break;
            }
        }
    }

    private void CheckRaycast()
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

    private void StartHighlightingTiles()
    {
        foreach (GameObject tile in AccessableBlocks)
        {
            if (tile != null)
                tile.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }
    }

    private void StopHighlightingTiles()
    {
        for (int i = 0; i < 2; i++)
        {
            if (AccessableBlocks[i] != null)
                AccessableBlocks[i].GetComponent<MeshRenderer>().material = defaultMaterials[i];
        }
    }

    private float GetTileDistance(GameObject tile, Vector2 touchPosition)
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
}
