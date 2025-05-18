using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/TileDatabase")]
public class TileDatabase : ScriptableObject
{
    [SerializeField] private List<TileData> tileData = new();

    public List<TileData> GetTileData() => tileData;
}
