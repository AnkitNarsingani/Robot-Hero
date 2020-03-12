using UnityEngine;
using DG.Tweening;

public class SwitchScript : TileScript
{
    [Header("The door this switch should Unlock")]
    [SerializeField] protected DoorScript door = null;

    public override void Occupy(GameObject occupiedRobot)
    {
        isOccupied = true;
        transform.GetChild(0).DOMoveY(0, 0.25f);
        if (door != null)
        {
            door.Unlock();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Door not assigned to switch " + gameObject.name);
#endif
        }
    }
}
