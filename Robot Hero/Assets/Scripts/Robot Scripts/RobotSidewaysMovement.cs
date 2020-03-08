using UnityEngine;
using DG.Tweening;

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
        if (AccessableBlocks[0] == tile) x = 1;
        else if (AccessableBlocks[1] == tile) x = -1;

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
        }

        animator.SetFloat("Move", -x);

        CurrentGridPosition = tempGridPosition;
        yield return new WaitForSeconds(playerFlap.length);


        Vector3 updatedTilePositon = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);

        robotHead.parent = null;
        Vector3 updatedHeadPositon = new Vector3(robotHead.position.x + (x * 0.8f), robotHead.position.y, robotHead.position.z);
        robotHead.DOMove(updatedHeadPositon, 0.2f);

        yield return new WaitForSeconds(0.2f);

        robotLegs.parent = null;
        Vector3 updatedLegsPositon = new Vector3(robotLegs.position.x + (x * 0.8f), robotLegs.position.y, robotLegs.position.z);
        robotLegs.DOMove(updatedLegsPositon, 0.2f);

        yield return new WaitForSeconds(0.2f);

        transform.position = updatedTilePositon;
        robotHead.parent = transform;
        robotLegs.parent = transform;
        animator.SetFloat("Move", 0);
        animator.SetTrigger("GoToIdle");

        currentTile.GetComponent<TileScript>().vacateAction(gameObject);
        tile.GetComponent<TileScript>().occupyAction(gameObject);
        currentTile = tile;

        playerMoveEvent.Invoke();
        GetAccessibleBlocks();

        yield return new WaitForSeconds(0.7f);
        isMoving = false;
    }
}
