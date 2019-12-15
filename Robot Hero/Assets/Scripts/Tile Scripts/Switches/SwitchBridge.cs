using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBridge : SwitchScript
{
    [SerializeField] public List<GameObject> bridgeTiles = new List<GameObject>();

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
}