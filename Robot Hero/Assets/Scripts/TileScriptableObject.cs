using UnityEngine;

[CreateAssetMenu(fileName = "NewTile", menuName = "ScriptableObjects/New Tile", order = 1)]
public class TileScriptableObject : ScriptableObject
{
    public GameObject tilePrefab;
    public bool canWalk;
}