using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public CarteData CarteSelectionnee { get; private set; }
    private GameObject apercuTuile;
    private GameObject boutonAssocie;
    private SpriteRenderer apercuSpriteRenderer;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileAutorisee;
    [SerializeField] private TileBase tileInterdite;

    public int CalculatedSortingOrder { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SelectionnerCarte(CarteData carte, GameObject bouton)
    {
        CarteSelectionnee = carte;
    
        boutonAssocie = bouton;
        boutonAssocie.SetActive(false);

        if (carte != null)
        {
            apercuTuile = Instantiate(carte.prefabTuile);
            Vector3 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0;
            apercuTuile.transform.position = startPos;

            apercuTuile.transform.SetParent(tilemap.transform);
            apercuSpriteRenderer = apercuTuile.GetComponent<SpriteRenderer>();

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            CalculatedSortingOrder = (int)(100 - mouseWorldPos.y * 10);
        }
    }

    public void DeselectionnerCarte()
    {
        CarteSelectionnee = null;

        if (apercuTuile != null)
            Destroy(apercuTuile);
    }

    private void Update()
    {
        if (CarteSelectionnee != null && apercuTuile != null && tilemap != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = tilemap.transform.position.z;

            Vector3Int cellPos = tilemap.WorldToCell(mouseWorld);
            Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPos);
            cellCenter.z = 0;
            apercuTuile.transform.position = cellCenter;

            if (apercuSpriteRenderer != null)
            {
                float precisionFactor = 10f;
                apercuSpriteRenderer.sortingOrder = (int)(100 - cellCenter.y * precisionFactor);
            }

            TileBase currentTile = tilemap.GetTile(cellPos);

            if (currentTile == tileInterdite)
                apercuSpriteRenderer.color = Color.red;
            else
                apercuSpriteRenderer.color = Color.white;
        }
    }
}
