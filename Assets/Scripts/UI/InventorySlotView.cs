using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private Image slotBackgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Text quantityText;

    [Header("Colors")]
    [SerializeField] private Color emptyColor = new Color(0.18f, 0.18f, 0.22f, 0.75f);
    [SerializeField] private Color filledColor = new Color(0.42f, 0.30f, 0.52f, 0.90f);

    public int SlotIndex { get; set; }
    public EquipmentData CurrentEquipmentData { get; private set; }
    public MaterialData CurrentMaterialData { get; private set; }
    public bool IsEmpty => CurrentEquipmentData == null && CurrentMaterialData == null;

    private Action<int> onClickCallback;
    private Action<int> onDiscardCallback;
    private ItemTooltip tooltip;
    private bool isDragging;

    private void Awake()
    {
        DisableChildRaycasts();
    }

    public void SetTooltip(ItemTooltip t) { tooltip = t; }

    public void SetCallbacks(Action<int> onClick, Action<int> onDiscard)
    {
        onClickCallback = onClick;
        onDiscardCallback = onDiscard;
    }

    public void SetEquipment(EquipmentData data)
    {
        CurrentEquipmentData = data;
        CurrentMaterialData = null;
        if (data == null) { SetEmpty(); return; }

        SetBackground(filledColor);
        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = data.icon;
            iconImage.color = data.icon != null ? Color.white : Color.gray;
        }
        if (quantityText != null) quantityText.text = "";
    }

    public void SetMaterial(MaterialData data, int amount)
    {
        CurrentEquipmentData = null;
        CurrentMaterialData = data;
        if (data == null) { SetEmpty(); return; }

        SetBackground(filledColor);
        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = data.icon;
            iconImage.color = data.icon != null ? Color.white : Color.gray;
        }
        if (quantityText != null) quantityText.text = amount > 1 ? amount.ToString() : "";
    }

    public void SetEmpty()
    {
        CurrentEquipmentData = null;
        CurrentMaterialData = null;
        SetBackground(emptyColor);
        if (iconImage != null) iconImage.enabled = false;
        if (quantityText != null) quantityText.text = "";
    }

    // ── Click ──

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        // Ctrl+Click 丢弃
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            onDiscardCallback?.Invoke(SlotIndex);
            return;
        }

        onClickCallback?.Invoke(SlotIndex);
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

        // 通知全局 DragHandler
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

    // ── Helpers ──

    private void SetBackground(Color c)
    {
        if (slotBackgroundImage != null) slotBackgroundImage.color = c;
    }

    private void DisableChildRaycasts()
    {
        if (iconImage != null) iconImage.raycastTarget = false;
        if (quantityText != null) quantityText.raycastTarget = false;
    }
}
