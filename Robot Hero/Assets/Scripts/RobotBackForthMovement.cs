﻿using UnityEngine;

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

    public override void Move(GameObject tile)
    {
        float y = 0;
        if (AccessableBlocks[0] == tile)
            y = 1;
        else if (AccessableBlocks[1] == tile)
            y = -1;

        Vector2 tempGridPosition = new Vector2(currentGridPosition.x, currentGridPosition.y + y);
        if (tile.GetComponent<TileScript>().isOccupied)
        {
            Robot[] robots = FindObjectsOfType<Robot>();
            foreach (Robot robot in robots)
            {
                if (robot.currentGridPosition == tempGridPosition)
                {
                    if (robot.Move(y))
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

    public override bool Move(float x)
    {
        Vector3 tempGridPosition = new Vector2(currentGridPosition.x + x, currentGridPosition.y);
        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject updatedTile = gridSystem.tileGameObjects[(int)tempGridPosition.x + (int)tempGridPosition.y * gridSystem.tileSetSize] ?? null;
        if(updatedTile != null && updatedTile.GetComponent<TileScript>().canWalk)
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
