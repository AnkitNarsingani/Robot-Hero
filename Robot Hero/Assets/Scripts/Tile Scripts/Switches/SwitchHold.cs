using UnityEngine;
using DG.Tweening;

public class SwitchHold : SwitchScript
{
    public override void Occupy(GameObject occupiedRobot)
    {
        isOccupied = true;
        transform.GetChild(0).DOMoveY(0, 0.25f);
        if (door != null)
            door.Unlock();
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
        transform.GetChild(0).DOMoveY(0.04f, 0.25f);
        if (door != null)
            door.Lock();
        else
        {
#if UNITY_EDITOR
            Debug.Log("Door not assigned to switch " + gameObject.name);
#endif
        }
    }
}
