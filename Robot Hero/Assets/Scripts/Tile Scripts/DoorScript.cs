using UnityEngine;

public class DoorScript : TileScript
{
    int noOfRobotsFinished = 0;

    public override void Occupy(GameObject occupiedRobot)
    {
        Robot robot = occupiedRobot.GetComponent<Robot>();

        if(robot is RobotBackForthMovement)
        {
            noOfRobotsFinished++;
            Destroy(occupiedRobot);
        }
        else if(robot is RobotSidewaysMovement)
        {
            noOfRobotsFinished++;
            Destroy(occupiedRobot);
        }

        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if(noOfRobotsFinished >= 2)
        {
            FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
