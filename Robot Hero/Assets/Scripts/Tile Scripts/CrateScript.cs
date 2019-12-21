using UnityEngine;

public class CrateScript : MonoBehaviour, IPushable
{
    [SerializeField] protected Vector2 staringGridPosition;

    public Vector2 CurrentGridPosition { get; private set; }

    public GameObject currentTile;

    void Start()
    {
        CurrentGridPosition = staringGridPosition;

        currentTile = FindObjectOfType<GridSystem>().tileGameObjects[(int)staringGridPosition.x + (int)staringGridPosition.y * FindObjectOfType<GridSystem>().tileSetSize];

        if (currentTile.GetComponent<TileScript>().occupyAction != null)
            currentTile.GetComponent<TileScript>().occupyAction(gameObject);

        Vector3 startingTilePositon = currentTile.transform.position;
        transform.position = new Vector3(startingTilePositon.x, transform.position.y, startingTilePositon.z);
    }

    bool IPushable.Push(float x, float y)
    {
        Vector2 tempGridPosition = new Vector2(CurrentGridPosition.x + x, CurrentGridPosition.y + y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject updatedTile = gridSystem.tileGameObjects[(int)tempGridPosition.x + (int)tempGridPosition.y * gridSystem.tileSetSize] ?? null;
        TileScript updatedTileScript = updatedTile.GetComponent<TileScript>();
        if (updatedTile != null && updatedTileScript.canWalk)
        {
            if (updatedTileScript.IsOccupied)
            {
                foreach (IPushable pushable in Robot.pushables)
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
            return true;
        }

        return false;
    }
}
