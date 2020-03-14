using UnityEngine;
using DG.Tweening;

public class RobotAlternateMovement : Robot
{
    protected override void GetAccessibleBlocks()
    {
        int upTempY = (int)CurrentGridPosition.y + 2;
        int downTempY = (int)CurrentGridPosition.y - 2;
        int leftTempX = (int)CurrentGridPosition.x - 2;
        int rightTempX = (int)CurrentGridPosition.x + 2;

        GameObject up = null, down = null, left = null, right = null;
        GridSystem gridSystem = FindObjectOfType<GridSystem>();

        if (upTempY >= 0 && upTempY <= gridSystem.tileSetSize)
            up = gridSystem.tileGameObjects[(int)CurrentGridPosition.x + upTempY * gridSystem.tileSetSize];

        if (downTempY >= 0 && downTempY <= gridSystem.tileSetSize)
            down = gridSystem.tileGameObjects[(int)CurrentGridPosition.x + downTempY * gridSystem.tileSetSize];


        if (rightTempX >= 0 && rightTempX <= gridSystem.tileSetSize)
            right = gridSystem.tileGameObjects[rightTempX + (int)CurrentGridPosition.y * gridSystem.tileSetSize];

        if (leftTempX >= 0 && leftTempX <= gridSystem.tileSetSize)
            left = gridSystem.tileGameObjects[leftTempX + (int)CurrentGridPosition.y * gridSystem.tileSetSize];


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

    public override System.Collections.IEnumerator Move(GameObject tile)
    {
        float y = 0, x = 0;

        if (AccessableBlocks[0] == tile) y = 2;
        else if (AccessableBlocks[1] == tile) y = -2;
        else if (AccessableBlocks[2] == tile) x = 2;
        else x = -2;

        Vector2 tempGridPosition = new Vector2(CurrentGridPosition.x + x, CurrentGridPosition.y + y);

        if (tile.GetComponent<TileScript>().IsOccupied)
        {
            foreach (IPushable pushable in pushables)
            {
                if (pushable.CurrentGridPosition == tempGridPosition)
                {
                    if (pushable.Push(x == 0 ? x : x > 0 ? x - 1 : x + 1,
                        y == 0 ? y : y > 0 ? y - 1 : y + 1))
                        break;
                }
            }
        }

        CurrentGridPosition = tempGridPosition;

        Vector3 updatedTilePositon = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        transform.DOLookAt(updatedTilePositon, 0.25f, AxisConstraint.Y);
        yield return new WaitForSeconds(0.25f);

        transform.DOJump(updatedTilePositon, 0.5f, 1, 0.5f);
        yield return new WaitForSeconds(0.25f);

        currentTile.GetComponent<TileScript>().vacateAction(gameObject);
        tile.GetComponent<TileScript>().occupyAction(gameObject);
        currentTile = tile;
        GetAccessibleBlocks();

        yield return new WaitForSeconds(0.7f);
        isMoving = false;
    }
}
