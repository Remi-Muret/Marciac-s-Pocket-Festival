using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(TileData tile)
    {
        nameText.text = tile.tileName;
        descriptionText.text = tile.description;

        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
