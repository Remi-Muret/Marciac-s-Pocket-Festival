using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TileDatabase tileDatabase;
    public List<TileData> handTiles;

    public int draw;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DrawTile(draw);
    }

    public void DrawTile(int nombre)
    {
        handTiles.Clear();
        var allTiles = tileDatabase.GetTileData();

        for (int i = 0; i < nombre; i++)
        {
            var tile = allTiles[Random.Range(0, allTiles.Count)];
            handTiles.Add(tile);
        }

        HandUIController.Instance.ShowCards(handTiles);
    }
}
