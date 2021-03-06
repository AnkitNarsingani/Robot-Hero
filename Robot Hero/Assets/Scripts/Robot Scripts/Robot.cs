﻿using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public abstract class Robot : MonoBehaviour, IPushable
{
    [SerializeField] protected Vector2 staringGridPosition;
    [SerializeField] protected float robotSpeed = 5f;
    [SerializeField] public bool isMoving = false;
    [SerializeField] public Vector2 CurrentGridPosition { get; protected set; }
    [SerializeField] protected float maxInaccuracy = 1;
    [SerializeField] protected int noOfDirections = 2;
    [SerializeField] protected Transform[] AccessableBlocks;
    [SerializeField] protected Material[] defaultMaterials;
    [SerializeField] protected UnityEvent playerMoveEvent;

    [HideInInspector] public Transform currentTile;

    protected Animator animator;
    protected Transform robotHead;
    protected Transform robotLegs;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool raycastHitObject = false;

    private Material highlightedMaterial;

    public static IPushable[] pushables;

    protected abstract void GetAccessibleBlocks();

    public abstract System.Collections.IEnumerator Move(Transform tile);

    protected void Start()
    {
        AccessableBlocks = new Transform[noOfDirections];
        defaultMaterials = new Material[noOfDirections];
        CurrentGridPosition = staringGridPosition;
        if (pushables == null) pushables = FindAllPushables();
        highlightedMaterial = Resources.Load("Highlight Tile") as Material;
        animator = GetComponentInChildren<Animator>();
        robotHead = transform.Find("Top");
        robotLegs = transform.Find("Bottom");
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        currentTile = gridSystem.tileTransforms[(int)staringGridPosition.x + (int)staringGridPosition.y * gridSystem.tileSetSize];

        if (currentTile.GetComponent<TileScript>().occupyAction != null)
            currentTile.GetComponent<TileScript>().occupyAction(gameObject);

        Vector3 startingTilePositon = currentTile.position;
        transform.position = new Vector3(startingTilePositon.x, transform.position.y, startingTilePositon.z);
        GetAccessibleBlocks();
    }

    private void OnEnable()
    {

    }

    protected void Update()
    {
        if (!isMoving)
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

                    animator.SetBool("isTouched", true);

                    StartHighlightingTiles();
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:

                    animator.SetBool("isTouched", false);

                    StopHighlightingTiles();

                    touchEndPos = touch.position;
                    if (touchStartPos != touchEndPos && touchStartPos != Vector2.zero)
                    {
                        float leastDistance = maxInaccuracy + 1;
                        Transform tileToMove = null;
                        foreach (Transform tile in AccessableBlocks)
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
                        {
                            isMoving = true;
                            StartCoroutine(Move(tileToMove));
                        }
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
        for (int i = 0; i < noOfDirections; i++)
        {
            if (AccessableBlocks[i] != null)
                AccessableBlocks[i].GetComponent<MeshRenderer>().material = highlightedMaterial;
        }
    }

    private void StopHighlightingTiles()
    {
        for (int i = 0; i < noOfDirections; i++)
        {
            if (AccessableBlocks[i] != null)
                AccessableBlocks[i].GetComponent<MeshRenderer>().material = defaultMaterials[i];
        }
    }

    private float GetTileDistance(Transform tile, Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance = 0;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 pos = ray.GetPoint(distance);
            return Vector3.Distance(pos, tile.position);
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
        Transform updatedTile = gridSystem.tileTransforms[(int)tempGridPosition.x + (int)tempGridPosition.y * gridSystem.tileSetSize] ?? null;
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
            Vector3 updatedTilePositon = new Vector3(updatedTileScript.transform.position.x, transform.position.y, updatedTileScript.transform.position.z);
            transform.DOMove(updatedTilePositon, 0.2f);
            currentTile.GetComponent<TileScript>().vacateAction(gameObject);
            updatedTile.GetComponent<TileScript>().occupyAction(gameObject);
            currentTile = updatedTile;
            GetAccessibleBlocks();
            return true;
        }

        return false;
    }
}
