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

    public override void Move(float x)
    {
        currentGridPosition = new Vector2(currentGridPosition.x + x, currentGridPosition.y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        Vector3 updatedTilePositon = gridSystem.tileGameObjects[(int)currentGridPosition.x + (int)currentGridPosition.y * gridSystem.tileSetSize].transform.position;
        transform.position = new Vector3(updatedTilePositon.x, transform.position.y, updatedTilePositon.z);
        GetAccessibleBlocks();
    }
}
