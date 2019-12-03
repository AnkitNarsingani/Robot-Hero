using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Robot : MonoBehaviour, IPushable
{
    [SerializeField] protected Vector2 staringGridPosition;
    [SerializeField] public Vector2 CurrentGridPosition { get; protected set; }
    [SerializeField] protected float maxInaccuracy = 1;
    [SerializeField] protected int noOfDirections = 2;
    [SerializeField] protected GameObject[] AccessableBlocks;
    [SerializeField] protected Material[] defaultMaterials;
    [SerializeField] protected UnityEvent playerMoveEvent;

    [ReadOnly] public GameObject currentTile;

    protected Animator animator;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool raycastHitObject = false;

    protected static GameObject lastHighlightedRobot;

    private Material highlightedMaterial;

    public static IPushable[] pushables;

    protected abstract void GetAccessibleBlocks();

    public abstract void Move(GameObject tile);

    protected void Start()
    {
        AccessableBlocks = new GameObject[noOfDirections];
        defaultMaterials = new Material[noOfDirections];
        CurrentGridPosition = staringGridPosition;
        pushables = FindAllPushables();
        highlightedMaterial = Resources.Load("Highlight Tile") as Material;
        animator = GetComponentInChildren<Animator>();
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        currentTile = gridSystem.tileGameObjects[(int)staringGridPosition.x + (int)staringGridPosition.y * gridSystem.tileSetSize];

        if (currentTile.GetComponent<TileScript>().occupyAction != null)
            currentTile.GetComponent<TileScript>().occupyAction(gameObject);

        Vector3 startingTilePositon = currentTile.transform.position;
        transform.position = new Vector3(startingTilePositon.x, transform.position.y, startingTilePositon.z);
        GetAccessibleBlocks();
    }

    private void OnEnable()
    {

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
        if (lastHighlightedRobot != null)
            lastHighlightedRobot.GetComponent<Robot>().StopHighlightingTiles();

        lastHighlightedRobot = gameObject;

        foreach (GameObject tile in AccessableBlocks)
        {
            if (tile != null)
                tile.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }
    }

    private void StopHighlightingTiles()
    {
        lastHighlightedRobot = null;

        for (int i = 0; i < noOfDirections; i++)
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

    private IPushable[] FindAllPushables()
    {
        IPushable[] pushables = FindObjectsOfType<MonoBehaviour>().OfType<IPushable>().ToArray();
        return pushables;
    }

    public bool Push(float x, float y)
    {
        Vector2 tempGridPosition = new Vector2(CurrentGridPosition.x + x, CurrentGridPosition.y + y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject updatedTile = gridSystem.tileGameObjects[(int)tempGridPosition.x + (int)tempGridPosition.y * gridSystem.tileSetSize] ?? null;
        TileScript updatedTileScript = updatedTile.GetComponent<TileScript>();
        if (updatedTile != null && updatedTileScript.canWalk)
        {
            if (updatedTileScript.IsOccupied)
            {
                foreach (IPushable pushable in pushables)
                {
                    if (pushable.CurrentGridPosition == tempGridPosition)
                    {
                        if (!pushable.Push(x, y))
                            return false;
                    }
                }
            }
            CurrentGridPosition = tempGridPosition;
            transform.position = new Vector3(updatedTile.transform.position.x, transform.position.y, updatedTile.transform.position.z);
            currentTile.GetComponent<TileScript>().vacateAction(gameObject);
            updatedTile.GetComponent<TileScript>().occupyAction(gameObject);
            currentTile = updatedTile;
            GetAccessibleBlocks();
            return true;
        }

        return false;
    }
}
