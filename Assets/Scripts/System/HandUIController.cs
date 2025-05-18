using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour
{
    public static HandUIController Instance { get; private set; }

    public GameObject tileButtonPrefab;
    public Transform handTilesContainer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void ShowCards(List<TileData> playerHand)
    {
        foreach (Transform child in handTilesContainer)
            Destroy(child.gameObject);

        foreach (var tile in playerHand)
        {
            GameObject bouton = Instantiate(tileButtonPrefab, handTilesContainer);
            Button buttonComponent = bouton.GetComponent<Button>();
            Image imageComponent = bouton.GetComponent<Image>();

            imageComponent.sprite = tile.icone;

            buttonComponent.onClick.AddListener(() => 
            {
                if (SelectionManager.Instance.selectedTile == null)
                    SelectionManager.Instance.SelectTile(tile, bouton);
            });
        }
    }
}
