using UnityEngine;

public class SwitchScript : TileScript
{
    [Header("The door this switch should Unlock")]
    [SerializeField] private DoorScript door = null;


    public override void Occupy(GameObject occupiedRobot)
    {
        isOccupied = true;
        if (door != null)
            door.IsUnlocked = true;
        else
        {
#if UNITY_EDITOR
            Debug.Log("Door to unlock not assigned");
#endif
        }
    }
}
