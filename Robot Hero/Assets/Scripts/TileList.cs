using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileList", menuName = "ScriptableObjects/Tile List", order = 1)]
public class TileList : ScriptableObject
{
    public List<TileScriptableObject> tilesList = new List<TileScriptableObject>();
}
