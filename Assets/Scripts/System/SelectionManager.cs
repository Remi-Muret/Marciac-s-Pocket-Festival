using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    public TileData selectedTile { get; private set; }
    public int CalculatedSortingOrder { get; private set; }
    public Tilemap tilemap;

    [HideInInspector] public SpriteRenderer spriteRenderer;

    private GameObject tilePreview;
    private GameObject involvedButton;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (selectedTile != null && tilePreview != null && tilemap != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = tilemap.transform.position.z;

            Vector3Int cellPos = tilemap.WorldToCell(mouseWorld);
            Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPos);
            cellCenter.z = 0;
            tilePreview.transform.position = cellCenter;

            if (spriteRenderer != null)
            {
                float precisionFactor = 10f;
                spriteRenderer.sortingOrder = (int)(100 - cellCenter.y * precisionFactor);
            }

            TileBase currentTile = tilemap.GetTile(cellPos);
        }
    }

    public void SelectTile(TileData tileData, GameObject button)
    {
        selectedTile = tileData;
    
        involvedButton = button;
        involvedButton.SetActive(false);

        if (tileData != null)
        {
            tilePreview = Instantiate(tileData.tilePrefab);
            Vector3 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0;
            tilePreview.transform.position = startPos;

            tilePreview.transform.SetParent(tilemap.transform);
            spriteRenderer = tilePreview.GetComponent<SpriteRenderer>();

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            CalculatedSortingOrder = (int)(100 - mouseWorldPos.y * 10);
        }
    }

    public void DeselectTile()
    {
        selectedTile = null;

        if (tilePreview != null)
            Destroy(tilePreview);
    }
}
