using UnityEngine;
using UnityEngine.Tilemaps;

public class PoseManager : MonoBehaviour
{
    public Tilemap tilemap;

    public bool PeutPlacerCarte(Vector3Int positionBase, CarteData carte)
    {
        if (tilemap.HasTile(positionBase))
            return false;

        return true;
    }

    public void PlacerCarte(Vector3Int positionBase, CarteData carte)
    {
        if (!PeutPlacerCarte(positionBase, carte)) return;

        Vector3 worldPos = tilemap.GetCellCenterWorld(positionBase);
        worldPos.z = 0;

        GameObject instance = Instantiate(carte.prefabTuile, worldPos, Quaternion.identity);
        instance.transform.SetParent(tilemap.transform);

        SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float precisionFactor = 10f;
            sr.sortingOrder = (int)(100 - positionBase.y * precisionFactor);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectionManager.Instance.CarteSelectionnee != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = tilemap.WorldToCell(mouseWorld);

            var carte = SelectionManager.Instance.CarteSelectionnee;

            PlacerCarte(cell, carte);

            SelectionManager.Instance.DeselectionnerCarte();
        }
    }
}
