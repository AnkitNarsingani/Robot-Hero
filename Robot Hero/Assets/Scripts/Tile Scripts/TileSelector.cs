using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField] public TileList tileListScriptableObject;
    [SerializeField] public Vector2 positionOnGrid;

    void Awake()
    {
        Destroy(gameObject);
    }
}
