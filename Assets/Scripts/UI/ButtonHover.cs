using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TileData tileData;

    public void Init(TileData data)
    {
        tileData = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SelectionManager.Instance.selectedTile != null) return;

        TooltipUI.Instance.ShowTooltip(tileData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }
}
