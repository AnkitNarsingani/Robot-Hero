using UnityEngine;

public class RobotAlternateMovement : Robot
{
    protected override void GetAccessibleBlocks()
    {
        int upTempY = (int)currentGridPosition.y + 2;
        int downTempY = (int)currentGridPosition.y - 2;
        int leftTempX = (int)currentGridPosition.x - 2;
        int rightTempX = (int)currentGridPosition.x + 2;

        GameObject up = null, down = null, left = null, right = null;
        GridSystem gridSystem = FindObjectOfType<GridSystem>();

        if(upTempY > 0 && upTempY < gridSystem.tileSetSize)
            up = gridSystem.tileGameObjects[(int)currentGridPosition.x + upTempY * gridSystem.tileSetSize] ?? null;
        if (downTempY > 0 && downTempY < gridSystem.tileSetSize)
            down = gridSystem.tileGameObjects[(int)currentGridPosition.x + downTempY * gridSystem.tileSetSize] ?? null;
        if (rightTempX > 0 && rightTempX < gridSystem.tileSetSize)
            right = gridSystem.tileGameObjects[rightTempX + (int)currentGridPosition.y * gridSystem.tileSetSize] ?? null;
        if (leftTempX > 0 && leftTempX < gridSystem.tileSetSize)
            left = gridSystem.tileGameObjects[leftTempX + (int)currentGridPosition.y * gridSystem.tileSetSize] ?? null;


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

        
        if (right != null && right.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[2] = right;
            defaultMaterials[2] = right.GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[2] = null;

        if (left != null && left.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[3] = left;
            defaultMaterials[3] = left.GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[3] = null;
    }

    public override void Move(GameObject tile)
    {
        float y = 0, x = 0;

        if (AccessableBlocks[0] == tile)
            y = 2;
        else if (AccessableBlocks[1] == tile)
            y = -2;
        else if (AccessableBlocks[2] == tile)
            x = 2;
        else
            x = -2;

        Vector2 tempGridPosition = new Vector2(currentGridPosition.x + x, currentGridPosition.y + y);

        if (tile.GetComponent<TileScript>().isOccupied)
        {
            Robot[] robots = FindObjectsOfType<Robot>();
            foreach (Robot robot in robots)
            {
                if (robot.currentGridPosition == tempGridPosition)
                {
                    if (robot.Move(x == 0 ? x : x > 0 ? x - 1 : x + 1, y == 0 ? y : y > 0 ? y - 1 : y + 1))
                        break;
                    else
                        return;
                }
            }
        }

        currentGridPosition = tempGridPosition;
        Vector3 updatedTilePositon = tile.transform.position;
        transform.position = new Vector3(updatedTilePositon.x, transform.position.y, updatedTilePositon.z);
        currentTile.GetComponent<TileScript>().isOccupied = false;
        tile.GetComponent<TileScript>().isOccupied = true;
        currentTile = tile;
        GetAccessibleBlocks();
    }
}
