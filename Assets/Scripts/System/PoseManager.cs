using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class PoseManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase forbiddenTile;

    public bool CanPlaceTile(Vector3Int positionBase, TileData tileData)
    {
        TileBase currentTile = tilemap.GetTile(positionBase);

        Vector3Int[] directions = null;
        switch (tileData.tileSize)
        {
            case TileData.TileSize.OneByOne:
                directions = new Vector3Int[]
                {
                    new Vector3Int( 0,  0, 0),
                };
                break;
            case TileData.TileSize.TwoByTwo:
                directions = new Vector3Int[]
                {
                    new Vector3Int( 0,  0, 0),
                    new Vector3Int( 1,  0, 0),
                    new Vector3Int( 0,  1, 0),
                    new Vector3Int( 1,  1, 0),
                };
                break;
            case TileData.TileSize.ThreeByThree:
                directions = new Vector3Int[]
                {
                    new Vector3Int( 0,  0, 0),
                    new Vector3Int(-1,  0, 0),
                    new Vector3Int( 1,  0, 0),
                    new Vector3Int( 0,  1, 0),
                    new Vector3Int( 0, -1, 0),
                    new Vector3Int(-1,  1, 0),
                    new Vector3Int( 1,  1, 0),
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int( 1, -1, 0)
                };
                break;
        }

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
            sr.sortingOrder = (int)(100 - worldPos.y * 10);

        Vector3Int[] directions = null;
        switch (tileData.tileSize)
        {
            case TileData.TileSize.OneByOne:
                directions = new Vector3Int[]
                {
                    new Vector3Int( 0,  0, 0),
                };
                break;
            case TileData.TileSize.TwoByTwo:
                directions = new Vector3Int[]
                {
                    new Vector3Int( 0,  0, 0),
                    new Vector3Int( 1,  0, 0),
                    new Vector3Int( 0,  1, 0),
                    new Vector3Int( 1,  1, 0),
                };
                break;
            case TileData.TileSize.ThreeByThree:
                directions = new Vector3Int[]
                {
                    new Vector3Int( 0,  0, 0),
                    new Vector3Int(-1,  0, 0),
                    new Vector3Int( 1,  0, 0),
                    new Vector3Int( 0,  1, 0),
                    new Vector3Int( 0, -1, 0),
                    new Vector3Int(-1,  1, 0),
                    new Vector3Int( 1,  1, 0),
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int( 1, -1, 0)
                };
                break;
        }

        foreach (var dir in directions)
        {
            Vector3Int adjacentCell = positionBase + dir;
            TileBase existingTile = tilemap.GetTile(adjacentCell);

            if (existingTile == null || existingTile.name != forbiddenTile.name)
                tilemap.SetTile(adjacentCell, forbiddenTile);
        }

        return true;
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

}
