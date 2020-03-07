using UnityEngine;
using DG.Tweening;

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

    public override System.Collections.IEnumerator Move(GameObject tile)
    {
        ChangeState(true);

        float y = 0;
        if (AccessableBlocks[0] == tile) y = 1;
        else if (AccessableBlocks[1] == tile) y = -1;

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

        animator.SetFloat("Move", y);

        CurrentGridPosition = tempGridPosition;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        Vector3 updatedTilePositon = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);

        robotHead.parent = null;
        Vector3 updatedHeadPositon = new Vector3(robotHead.position.x, robotHead.position.y, robotHead.position.z + (y * 0.8f));
        robotHead.DOMove(updatedHeadPositon, 0.2f);

        yield return new WaitForSeconds(0.2f);

        robotLegs.parent = null;
        Vector3 updatedLegsPositon = new Vector3(robotLegs.position.x, robotLegs.position.y, robotLegs.position.z + (y * 0.8f));
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

        ChangeState(false);
    }
}
