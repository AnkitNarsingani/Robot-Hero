using UnityEngine;
using DG.Tweening;

public class DoorScript : TileScript
{
    public bool IsUnlocked { get { return isUnlocked; } }

    [SerializeField] protected bool isUnlocked;

    [SerializeField] protected MeshRenderer exitSignMeshRenderer;
    [SerializeField] protected Transform doorTransform;

    [SerializeField] protected Material unlockedMaterial;
    [SerializeField] protected Material lockedMaterial;

    private int noOfRobotsFinished = 0;

    public override void Occupy(GameObject occupiedRobot)
    {
        if (isUnlocked)
        {
            Robot robot = occupiedRobot.GetComponent<Robot>();

            if (robot is RobotBackForthMovement)
            {
                noOfRobotsFinished++;
                Destroy(occupiedRobot);
            }
            else if (robot is RobotSidewaysMovement)
            {
                noOfRobotsFinished++;
                Destroy(occupiedRobot);
            }

            CheckWinCondition();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Not Unlocked");
#endif
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
        exitSignMeshRenderer.material = unlockedMaterial;
        doorTransform.DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.LocalAxisAdd);
    }

    public void Lock()
    {
        isUnlocked = false;
        exitSignMeshRenderer.material = lockedMaterial;
        doorTransform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast);
    }

    void CheckWinCondition()
    {
        if (noOfRobotsFinished >= 2)
        {
            FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
