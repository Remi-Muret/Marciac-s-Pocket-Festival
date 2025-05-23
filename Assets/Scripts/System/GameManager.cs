using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TileDatabase tileDatabase;
    public List<TileData> handTiles;

    public int draw;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DrawTile(draw);
    }

    public void DrawTile(int nombre)
    {
        handTiles.Clear();
        var allTiles = tileDatabase.GetTileData();
        List<TileData> tempPool = new List<TileData>();
        int minTotal = 0, maxTotal = 0;

        foreach (var tile in allTiles)
        {
            minTotal += tile.minFreq;
            maxTotal += tile.maxFreq;

            for (int i = 0; i < tile.minFreq; i++)
                tempPool.Add(tile);
        }

        if (nombre < minTotal)
            nombre = minTotal;
        else if (nombre > maxTotal)
            nombre = maxTotal;

        System.Random rand = new System.Random();
        while (tempPool.Count < nombre)
        {
            var tile = allTiles[rand.Next(allTiles.Count)];

            int count = 0;
            foreach (var t in tempPool)
                if (t == tile) count++;

            if (count < tile.maxFreq)
                tempPool.Add(tile);
        }

        handTiles.AddRange(tempPool.GetRange(0, nombre));
        handTiles.Sort((a, b) => string.Compare(a.tileName, b.tileName));
        HandUI.Instance.ShowCards(handTiles);
    }
}
