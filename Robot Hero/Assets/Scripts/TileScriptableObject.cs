using UnityEngine;

[CreateAssetMenu(fileName = "NewTile", menuName = "ScriptableObjects/Tile ScriptableObject", order = 1)]
public class TileScriptableObject : ScriptableObject
{
    public GameObject tilePrefab;
    public bool canWalk;
}