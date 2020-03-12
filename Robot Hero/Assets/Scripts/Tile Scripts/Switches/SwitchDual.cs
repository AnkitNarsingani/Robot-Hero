using UnityEngine;
using DG.Tweening;

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
            transform.GetChild(0).DOMoveY(0, 0.25f);
        }
        else if (robot is RobotBackForthMovement)
        {
            backForthRobotSwitch = true;
            transform.GetChild(1).DOMoveY(0, 0.25f);
        }

        isOccupied = true;

        if (sidewaysRobotSwitch && backForthRobotSwitch)
            if (door != null)
                door.Unlock();
            else
            {
#if UNITY_EDITOR
                Debug.Log("Door not assigned to switch " + gameObject.name);
#endif
            }
    }
}
