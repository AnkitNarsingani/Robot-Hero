using UnityEngine;

public class SwitchDual : SwitchScript
{
    private bool backForthRobotSwitch = false;
    private bool sidewaysRobotSwitch = false;

    public override void Occupy(GameObject occupiedRobot)
    {
        Robot robot = occupiedRobot.GetComponent<Robot>();

        if (robot is RobotSidewaysMovement)
        {
            sidewaysRobotSwitch = true;
        }
        else if (robot is RobotBackForthMovement)
        {
            backForthRobotSwitch = true;
        }

        isOccupied = true;

        if (sidewaysRobotSwitch && backForthRobotSwitch)
            if (door != null)
                door.isUnlocked = true;
            else
            {
#if UNITY_EDITOR
                Debug.Log("Door not assigned to switch " + gameObject.name);
#endif
            }
    }
}
