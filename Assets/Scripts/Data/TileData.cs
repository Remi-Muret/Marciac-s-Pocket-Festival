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
        ThreeByFour,
    }

    public TileSize tileSize;
    public Sprite icone;
    public int score;
    public int minFreq;
    public int maxFreq;
    [TextArea] public string description;
    public GameObject tilePrefab;
}
