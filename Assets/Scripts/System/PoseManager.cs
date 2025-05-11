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
