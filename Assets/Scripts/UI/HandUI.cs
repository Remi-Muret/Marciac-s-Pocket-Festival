using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    public static HandUI Instance;

    public GameObject tileButtonPrefab;
    public Transform handTilesContainer;

    void Awake()
    {
        Instance = this;
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

            ButtonHover hover = bouton.GetComponent<ButtonHover>();
            hover.Init(tile);

            buttonComponent.onClick.AddListener(() => 
            {
                if (SelectionManager.Instance.selectedTile == null)
                {
                    SelectionManager.Instance.SelectTile(tile, bouton);
                    TooltipUI.Instance.HideTooltip();
                }
            });
        }
    }

    public void CheckIfAllButtonsDisabled()
    {
        int activeButtons = 0;

        foreach (Transform child in handTilesContainer)
        {
            if (child.gameObject.activeSelf)
                activeButtons++;
        }

        if (activeButtons == 0)
            FadeUI.Instance.Fade();
    }
}
