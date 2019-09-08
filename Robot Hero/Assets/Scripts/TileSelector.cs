using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [HideInInspector] public TileList tileListScriptableObject;
    [HideInInspector] public Vector2 positionOnGrid;

    void Awake()
    {
        Destroy(gameObject);
    }
}
