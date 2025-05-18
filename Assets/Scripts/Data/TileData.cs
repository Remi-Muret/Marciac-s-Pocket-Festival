using System;
using UnityEngine;

[Serializable]
public class TileData
{
    public string tileName;

    public enum TileSize
    {        
        OneByOne,
        TwoByTwo,
        ThreeByThree,
    }

    public TileSize tileSize;
    public Sprite icone;
    public int score;
    public GameObject tilePrefab;
}
