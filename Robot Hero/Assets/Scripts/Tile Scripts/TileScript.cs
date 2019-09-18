using UnityEngine;

public class TileScript : MonoBehaviour
{
    public bool canWalk;
    protected bool isOccupied = false;

    public bool IsOccupied
    {
        get
        {
            return isOccupied;
        }
    }
    public virtual void Occupy()
    {
        isOccupied = true;
    }

    public virtual void Occupy(GameObject occupiedRobot)
    {
        isOccupied = true;
    }

    public virtual void Vacate()
    {
        isOccupied = false;
    }
}
