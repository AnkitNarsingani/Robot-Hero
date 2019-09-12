using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBackForthMovement : Robot
{
    protected override void GetAccessibleBlocks()
    {
        int upTempY = (int)currentGridPosition.y + 1;
        int downTempY = (int)currentGridPosition.y - 1;

        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject up = gridSystem.tileGameObjects[(int)currentGridPosition.x + upTempY * gridSystem.tileSetSize] ?? null;
        GameObject down = gridSystem.tileGameObjects[(int)currentGridPosition.x + downTempY * gridSystem.tileSetSize] ?? null;

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

    public override void Move(float y)
    {
        currentGridPosition = new Vector2(currentGridPosition.x, currentGridPosition.y + y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        Vector3 updatedTilePositon = gridSystem.tileGameObjects[(int)currentGridPosition.x + (int)currentGridPosition.y * gridSystem.tileSetSize].transform.position;
        transform.position = new Vector3(updatedTilePositon.x, transform.position.y, updatedTilePositon.z);
        GetAccessibleBlocks();
    }
}
