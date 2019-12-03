using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] public bool canWalk;
    protected bool isOccupied = false;
    public delegate void Action(GameObject occupiedRobot);
    public Action occupyAction;
    public Action vacateAction;

    public bool IsOccupied
    {
        get
        {
            return isOccupied;
        }
    }

    private void OnEnable()
    {
        occupyAction += Occupy;
        vacateAction += Vacate;
    }

    private void OnDisable()
    {
        occupyAction -= Occupy;
        vacateAction -= Vacate;
    }

    public virtual void Occupy(GameObject occupiedRobot)
    {
        isOccupied = true;
    }

    public virtual void Vacate(GameObject occupiedRobot)
    {
        isOccupied = false;
    }
}
