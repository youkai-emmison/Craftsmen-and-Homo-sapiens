using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 滚动背包列表中的单个物品项，支持拖拽、点击和悬停提示。
/// </summary>
public class InventoryListItem : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text quantityText;
    [SerializeField] private Image backgroundImage;

    public EquipmentData CurrentEquipmentData { get; private set; }
    public MaterialData CurrentMaterialData { get; private set; }
    public bool IsEmpty => CurrentEquipmentData == null && CurrentMaterialData == null;
    public int ItemIndex { get; private set; }

    private Action<int> onClickCallback;
    private Action<int> onDiscardCallback;
    private ItemTooltip tooltip;
    private bool isDragging;

    public void Setup(int index, EquipmentData data, Action<int> onClick, Action<int> onDiscard, ItemTooltip tip)
    {
        ItemIndex = index;
        CurrentEquipmentData = data;
        CurrentMaterialData = null;
        onClickCallback = onClick;
        onDiscardCallback = onDiscard;
        tooltip = tip;

        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = data.icon;
            iconImage.color = data.icon != null ? Color.white : Color.gray;
        }
        if (quantityText != null) quantityText.text = "";
        if (backgroundImage != null)
            backgroundImage.color = new Color(0.42f, 0.30f, 0.52f, 0.90f);
    }

    public void Setup(int index, MaterialData data, int amount, Action<int> onClick, Action<int> onDiscard, ItemTooltip tip)
    {
        ItemIndex = index;
        CurrentEquipmentData = null;
        CurrentMaterialData = data;
        onClickCallback = onClick;
        onDiscardCallback = onDiscard;
        tooltip = tip;

        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = data.icon;
            iconImage.color = data.icon != null ? Color.white : Color.gray;
        }
        if (quantityText != null) quantityText.text = amount > 1 ? amount.ToString() : "";
        if (backgroundImage != null)
            backgroundImage.color = new Color(0.35f, 0.35f, 0.45f, 0.85f);
    }

    // ── Click ──

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            onDiscardCallback?.Invoke(ItemIndex);
            return;
        }

        onClickCallback?.Invoke(ItemIndex);
    }

    // ── Tooltip ──

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip == null || isDragging) return;
        if (CurrentEquipmentData != null)
            tooltip.ShowEquipment(CurrentEquipmentData);
        else if (CurrentMaterialData != null)
            tooltip.ShowMaterial(CurrentMaterialData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null) tooltip.Hide();
    }

    // ── Drag ──

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty) { eventData.pointerDrag = null; return; }
        isDragging = true;
        if (tooltip != null) tooltip.Hide();

        if (DragHandler.Instance != null)
            DragHandler.Instance.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragHandler.Instance != null)
            DragHandler.Instance.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (DragHandler.Instance != null)
            DragHandler.Instance.OnEndDrag(eventData);
    }
}
