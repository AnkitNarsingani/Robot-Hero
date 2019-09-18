using UnityEngine;

public class DoorScript : TileScript
{
    int noOfRobotsFinished = 0;

    [SerializeField] public bool isUnlocked = true;

    public override void Occupy()
    {
        Robot[] robots = FindObjectsOfType<Robot>();
        foreach (Robot robot in robots)
        {
            if(robot.currentTile == gameObject)
            {
                Occupy(robot.gameObject);
                break;
            }
        }
    }

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

    void CheckWinCondition()
    {
        if(noOfRobotsFinished >= 2)
        {
            FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
