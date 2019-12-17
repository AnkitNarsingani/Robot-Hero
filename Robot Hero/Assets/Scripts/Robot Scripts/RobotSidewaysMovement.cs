using UnityEngine;

public class RobotSidewaysMovement : Robot
{
    protected override void GetAccessibleBlocks()
    {
        int leftTempX = (int)CurrentGridPosition.x - 1;
        int rightTempX = (int)CurrentGridPosition.x + 1;

        GridSystem gridSystem = FindObjectOfType<GridSystem>();
        GameObject right = null, left = null;

        if (rightTempX >= 0 && rightTempX <= gridSystem.tileSetSize)
            right = gridSystem.tileGameObjects[rightTempX + (int)CurrentGridPosition.y * gridSystem.tileSetSize];
        if (leftTempX >= 0 && leftTempX <= gridSystem.tileSetSize)
            left = gridSystem.tileGameObjects[leftTempX + (int)CurrentGridPosition.y * gridSystem.tileSetSize];

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

    public override System.Collections.IEnumerator Move(GameObject tile)
    {
        float x = 0;
        if (AccessableBlocks[0] == tile)
            x = 1;
        else if (AccessableBlocks[1] == tile)
            x = -1;

        Vector2 tempGridPosition = new Vector2(CurrentGridPosition.x + x, CurrentGridPosition.y);
        if (tile.GetComponent<TileScript>().IsOccupied)
        {
            foreach (IPushable pushable in pushables)
            {
                if (pushable.CurrentGridPosition == tempGridPosition)
                {
                    if (pushable.Push(x, 0))
                        break;
                }
            }
            yield return null;
            CurrentGridPosition = tempGridPosition;
            Vector3 updatedTilePositon = tile.transform.position;
            transform.position = new Vector3(updatedTilePositon.x, transform.position.y, updatedTilePositon.z);
            currentTile.GetComponent<TileScript>().vacateAction(gameObject);
            tile.GetComponent<TileScript>().occupyAction(gameObject);
            currentTile = tile;
            GetAccessibleBlocks();
        }
    }
}
