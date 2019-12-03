using UnityEngine;

public class RobotBackForthMovement : Robot
{
    protected override void GetAccessibleBlocks()
    {
        int upTempY = (int)CurrentGridPosition.y + 1;
        int downTempY = (int)CurrentGridPosition.y - 1;

        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject up = null, down = null;

        if (upTempY >= 0 && upTempY <= gridSystem.tileSetSize)
            up = gridSystem.tileGameObjects[(int)CurrentGridPosition.x + upTempY * gridSystem.tileSetSize];
        if (downTempY >= 0 && downTempY <= gridSystem.tileSetSize)
            down = gridSystem.tileGameObjects[(int)CurrentGridPosition.x + downTempY * gridSystem.tileSetSize];

        if (up != null && up.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[0] = up;
            defaultMaterials[0] = up.GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[0] = null;

        if (down != null && down.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[1] = down;
            defaultMaterials[1] = down.GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[1] = null;
    }

    public override void Move(GameObject tile)
    {
        float y = 0;
        if (AccessableBlocks[0] == tile)
            y = 1;
        else if (AccessableBlocks[1] == tile)
            y = -1;

        Vector2 tempGridPosition = new Vector2(CurrentGridPosition.x, CurrentGridPosition.y + y);
        if (tile.GetComponent<TileScript>().IsOccupied)
        {
            foreach (IPushable pushable in pushables)
            {
                if (pushable.CurrentGridPosition == tempGridPosition)
                {
                    if (pushable.Push(0, y))
                        break;
                }
            }
        }
        CurrentGridPosition = tempGridPosition;
        Vector3 updatedTilePositon = tile.transform.position;
        transform.position = new Vector3(updatedTilePositon.x, transform.position.y, updatedTilePositon.z);
        currentTile.GetComponent<TileScript>().vacateAction(gameObject);
        tile.GetComponent<TileScript>().occupyAction(gameObject);
        currentTile = tile;
        playerMoveEvent.Invoke();
        GetAccessibleBlocks();
    }
}
