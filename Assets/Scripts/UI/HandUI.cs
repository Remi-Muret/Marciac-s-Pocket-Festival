using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HandUI : MonoBehaviour
{
    public static HandUI Instance;

    public GameObject tileButtonPrefab;
    public Transform handTilesContainer;
    public UnityEvent OnAllButtonsDisabled;

    void Awake()
    {
        Instance = this;

        if (OnAllButtonsDisabled == null)
            OnAllButtonsDisabled = new UnityEvent();
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

            HandButtonHover hover = bouton.GetComponent<HandButtonHover>();
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
        {
            EndGameManager.Instance.TriggerEndGameSequence();
            OnAllButtonsDisabled?.Invoke();
        }
    }
}
