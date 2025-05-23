using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PoseManager : MonoBehaviour
{
    public static PoseManager Instance;

    public Tilemap tilemap;
    public TileBase forbiddenTile;
    public List<GameObject> characterPrefabs;
    public int spawnCount;

    private Dictionary<Vector3Int, Dictionary<string, int>> influenceMap = new Dictionary<Vector3Int, Dictionary<string, int>>();
    private List<GameObject> spawnedCharacters = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (SelectionManager.Instance.selectedTile != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = tilemap.WorldToCell(mouseWorld);
            cell.z = 0;

            var tile = SelectionManager.Instance.selectedTile;

            if (!CanPlaceTile(cell, tile))
                SelectionManager.Instance.spriteRenderer.color = Color.red;
            else
                SelectionManager.Instance.spriteRenderer.color = Color.white;

            if (Input.GetMouseButtonDown(0))
            {
                bool placed = PlaceTile(cell, tile);
                if (placed)
                    SelectionManager.Instance.DeselectTile();
            }
        }
    }

    public bool CanPlaceTile(Vector3Int positionBase, TileData tileData)
    {
        TileBase currentTile = tilemap.GetTile(positionBase);

        Vector3Int[] directions = GetOccupiedOffsets(tileData.tileSize);
        foreach (var dir in directions)
        {
            Vector3Int adjacentCell = positionBase + dir;
            TileBase existingTile = tilemap.GetTile(adjacentCell);

            if (existingTile.name == forbiddenTile.name) return false;
        }

        return true;
    }

    public bool PlaceTile(Vector3Int positionBase, TileData tileData)
    {
        if (!CanPlaceTile(positionBase, tileData)) return false;

        Vector3 worldPos = tilemap.GetCellCenterWorld(positionBase);
        worldPos.z = 0;

        GameObject instance = Instantiate(tileData.tilePrefab, worldPos, Quaternion.identity);
        instance.transform.SetParent(tilemap.transform);

        SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = (int)(100 - worldPos.y * 10);

            if (tileData.tileSize == TileData.TileSize.ThreeByFour)
                sr.sortingOrder -= 1;
            if (tileData.tileSize == TileData.TileSize.TwoByTwo)
                sr.sortingOrder -= 2;
        }

        Vector3Int[] directions = GetOccupiedOffsets(tileData.tileSize);
        List<Vector3Int> occupiedCells = new List<Vector3Int>();

        foreach (var dir in directions)
        {
            Vector3Int occupiedCell = positionBase + dir;
            TileBase existingTile = tilemap.GetTile(occupiedCell);

            if (existingTile.name != forbiddenTile.name)
                tilemap.SetTile(occupiedCell, forbiddenTile);

            occupiedCells.Add(occupiedCell);
        }

        ApplyInfluence(occupiedCells.ToArray(), tileData);
        ScoreManager.Instance.AddScore(tileData, GetInfluencesAt(positionBase, tileData));
        HandUI.Instance.CheckIfAllButtonsDisabled();
        AudioManager.Instance.PlaySFX("Poof SFX", 1f);

        return true;
    }

    public Dictionary<string, int> GetInfluencesAt(Vector3Int positionBase, TileData tileData)
    {
        Dictionary<string, int> combinedInfluences = new Dictionary<string, int>();
        HashSet<string> countedInfluences = new HashSet<string>();

        Vector3Int[] directions = GetOccupiedOffsets(tileData.tileSize);

        foreach (var dir in directions)
        {
            Vector3Int pos = positionBase + dir;
            if (influenceMap.TryGetValue(pos, out var influences))
            {
                foreach (var kvp in influences)
                {
                    if (countedInfluences.Contains(kvp.Key)) continue;

                    countedInfluences.Add(kvp.Key);
                    combinedInfluences[kvp.Key] = kvp.Value;
                }
            }
        }

        return combinedInfluences;
    }

    public void SpawnCharacters()
    {
        List<Vector3Int> validPositions = GetAllValidTilePositions();
        if (validPositions.Count == 0 || characterPrefabs.Count == 0) return;

        spawnedCharacters.Clear();

        int spawnsDone = 0;

        while (spawnsDone < spawnCount && validPositions.Count > 0)
        {
            int index = Random.Range(0, validPositions.Count);
            Vector3Int randomCell = validPositions[index];
            validPositions.RemoveAt(index);

            Vector3 worldPos = tilemap.GetCellCenterWorld(randomCell);
            GameObject prefab = characterPrefabs[Random.Range(0, characterPrefabs.Count)];
            GameObject instance = Instantiate(prefab, worldPos, Quaternion.identity);
            instance.transform.SetParent(tilemap.transform);

            SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = (int)(100 - worldPos.y * 10);

            tilemap.SetTile(randomCell, forbiddenTile);
            spawnedCharacters.Add(instance);

            spawnsDone++;
        }
    }

    public List<GameObject> GetSpawnedCharacters()
    {
        return spawnedCharacters;
    }

    Vector3Int[] GetOccupiedOffsets(TileData.TileSize size)
    {
        switch (size)
        {
            case TileData.TileSize.OneByOne:
                return new Vector3Int[]
                {
                new Vector3Int(0, 0, 0)
                };

            case TileData.TileSize.TwoByTwo:
                return new Vector3Int[]
                {
                new Vector3Int(0, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(1, 1, 0)
                };

            case TileData.TileSize.ThreeByThree:
                return new Vector3Int[]
                {
                new Vector3Int(0, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, -1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(-1, -1, 0),
                new Vector3Int(1, -1, 0)
                };

            case TileData.TileSize.ThreeByFour:
                return new Vector3Int[]
                {
                new Vector3Int(0, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, -1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(-1, -1, 0),
                new Vector3Int(1, -1, 0),
                new Vector3Int(-1, 2, 0),
                new Vector3Int(0, 2, 0),
                new Vector3Int(1, 2, 0),
                };

            default:
                return new Vector3Int[] { new Vector3Int(0, 0, 0) };
        }
    }

    Vector3Int[] GetSurroundingCells(Vector3Int center, int radius)
    {
        List<Vector3Int> results = new List<Vector3Int>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x != 0 && y != 0) continue;

                results.Add(new Vector3Int(center.x + x, center.y + y, 0));
            }
        }

        return results.ToArray();
    }

    List<Vector3Int> GetAllValidTilePositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        List<Vector3Int> validPositions = new List<Vector3Int>();

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null && tile.name != forbiddenTile.name)
                validPositions.Add(pos);
        }

        return validPositions;
    }

    void ApplyInfluence(Vector3Int[] occupiedCells, TileData tileData)
    {
        int radius;
        switch (tileData.tileName)
        {
            case "Enceinte":
                radius = 3;
                break;

            case "Agent de sécurité":
                radius = 4;
                break;

            default:
                radius = 1;
                break;
        }

        HashSet<Vector3Int> ignoredCells = new HashSet<Vector3Int>(occupiedCells);

        foreach (var center in occupiedCells)
        {
            Vector3Int[] area = GetSurroundingCells(center, radius);

            foreach (var pos in area)
            {
                if (ignoredCells.Contains(pos)) continue;

                if (!influenceMap.ContainsKey(pos))
                    influenceMap[pos] = new Dictionary<string, int>();

                if (!influenceMap[pos].ContainsKey(tileData.tileName))
                    influenceMap[pos][tileData.tileName] = 0;

                influenceMap[pos][tileData.tileName]++;
            }
        }
    }
}
