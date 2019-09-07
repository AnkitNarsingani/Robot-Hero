using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [HideInInspector] public TileList tileList;

    void Awake()
    {
        Destroy(gameObject);
    }
}
