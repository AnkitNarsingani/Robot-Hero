using UnityEngine;

public class RobotSidewaysMovement : Robot
{
    protected override void GetAccessibleBlocks()
    {
        int leftTempX = (int)currentGridPosition.x - 1;
        int rightTempX = (int)currentGridPosition.x + 1;

        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject right = gridSystem.tileGameObjects[rightTempX + (int)currentGridPosition.y * gridSystem.tileSetSize] ?? null;
        GameObject left = gridSystem.tileGameObjects[leftTempX + (int)currentGridPosition.y * gridSystem.tileSetSize] ?? null;

        if (right != null && right.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[0] = right;
            defaultMaterials[0] = right.GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[0] = null;

        if (left != null && left.GetComponent<TileScript>().canWalk)
        {
            AccessableBlocks[1] = left;
            defaultMaterials[1] = left.GetComponent<MeshRenderer>().material;
        }
        else
            AccessableBlocks[1] = null;
    }

    public override void Move(GameObject tile)
    {
        float x = 0;
        if (AccessableBlocks[0] == tile)
            x = 1;
        else if (AccessableBlocks[1] == tile)
            x = -1;

        currentGridPosition = new Vector2(currentGridPosition.x + x, currentGridPosition.y);
        if (tile.GetComponent<TileScript>().isOccupied)
        {
            Robot[] robots = FindObjectsOfType<Robot>();
            foreach (Robot robot in robots)
            {
                if (robot.currentGridPosition == currentGridPosition)
                {
                    if (robot.Move(x))
                        break;
                    else
                        return;
                }     
            }
        }
        
        Vector3 updatedTilePositon = tile.transform.position;
        transform.position = new Vector3(updatedTilePositon.x, transform.position.y, updatedTilePositon.z);
        currentTile.GetComponent<TileScript>().isOccupied = false;
        tile.GetComponent<TileScript>().isOccupied = true;
        currentTile = tile;
        GetAccessibleBlocks();
    }

    public override bool Move(float y)
    {
        Vector3 tempGridPosition = new Vector2(currentGridPosition.x, currentGridPosition.y + y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject updatedTile = gridSystem.tileGameObjects[(int)tempGridPosition.x + (int)tempGridPosition.y * gridSystem.tileSetSize] ?? null;
        if (updatedTile != null && updatedTile.GetComponent<TileScript>().canWalk)
        {
            currentGridPosition = tempGridPosition;
            transform.position = new Vector3(updatedTile.transform.position.x, transform.position.y, updatedTile.transform.position.z);
            currentTile.GetComponent<TileScript>().isOccupied = false;
            updatedTile.GetComponent<TileScript>().isOccupied = true;
            currentTile = updatedTile;
            GetAccessibleBlocks();
            return true;
        }

        return false;
    }
}
