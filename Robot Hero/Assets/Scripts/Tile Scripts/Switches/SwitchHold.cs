using UnityEngine;

public class SwitchHold : SwitchScript
{
    public override void Occupy(GameObject occupiedRobot)
    {
        isOccupied = true;
        if (door != null)
            door.isUnlocked = true;
        else
        {
#if UNITY_EDITOR
            Debug.Log("Door not assigned to switch " + gameObject.name);
#endif
        }
    }

    public override void Vacate(GameObject occupiedRobot)
    {
        isOccupied = true;
        if (door != null)
            door.isUnlocked = false;
        else
        {
#if UNITY_EDITOR
            Debug.Log("Door not assigned to switch " + gameObject.name);
#endif
        }
    }
}
