using UnityEngine;
using UnityEngine.Tilemaps;

public class PoseManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tuileInterdite;

    public bool PeutPlacerCarte(Vector3Int positionBase, CarteData carte)
    {
        TileBase currentTile = tilemap.GetTile(positionBase);

        if (currentTile != null && currentTile.name == tuileInterdite.name) return false;

        return true;
    }

    public bool PlacerCarte(Vector3Int positionBase, CarteData carte)
    {
        if (!PeutPlacerCarte(positionBase, carte)) return false;

        Vector3 worldPos = tilemap.GetCellCenterWorld(positionBase);
        worldPos.z = 0;

        GameObject instance = Instantiate(carte.prefabTuile, worldPos, Quaternion.identity);
        instance.transform.SetParent(tilemap.transform);

        SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float precisionFactor = 10f;
            sr.sortingOrder = (int)(100 - worldPos.y * precisionFactor);
        }

        tilemap.SetTile(positionBase, tuileInterdite);

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(-1,  0, 0),
            new Vector3Int( 1,  0, 0),
            new Vector3Int( 0,  1, 0),
            new Vector3Int( 0, -1, 0),
            new Vector3Int(-1,  1, 0),
            new Vector3Int( 1,  1, 0),
            new Vector3Int(-1, -1, 0),
            new Vector3Int( 1, -1, 0)
        };

        foreach (var dir in directions)
        {
            Vector3Int adjacentCell = positionBase + dir;

            TileBase existingTile = tilemap.GetTile(adjacentCell);
            if (existingTile == null || existingTile.name != tuileInterdite.name)
                tilemap.SetTile(adjacentCell, tuileInterdite);
        }

        return true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectionManager.Instance.CarteSelectionnee != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = tilemap.WorldToCell(mouseWorld);
            cell.z = 0;

            var carte = SelectionManager.Instance.CarteSelectionnee;

            bool placed = PlacerCarte(cell, carte);

            if (placed)
                SelectionManager.Instance.DeselectionnerCarte();
        }
    }
}
