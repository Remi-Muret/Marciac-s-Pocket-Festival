using UnityEngine;
using UnityEngine.EventSystems;

public class HandButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float animationSpeed = 10f;

    private TileData tileData;
    private Vector3 originalScale;
    private Vector3 targetScale;

    public void Init(TileData data)
    {
        tileData = data;
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SelectionManager.Instance.selectedTile != null) return;

        TooltipUI.Instance.ShowTooltip(tileData);
        targetScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
        targetScale = originalScale;
    }
}
