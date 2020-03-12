using UnityEngine;

public class DoorScript : TileScript
{
    public bool IsUnlocked { get { return isUnlocked; } }

    [SerializeField] protected bool isUnlocked;
    [SerializeField] protected Color unlockedColor;
    [SerializeField] protected Color lockedColor;

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
    }

    public void Lock()
    {
        isUnlocked = false;
    }

    void CheckWinCondition()
    {
        if (noOfRobotsFinished >= 2)
        {
            FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
