using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// InventorySlotView renders one backpack slot.
/// It receives item data from InventoryPanel and never changes inventory contents directly.
/// Click handling reports the current item back to InventoryPanel for detail display.
/// </summary>
public class InventorySlotView : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private Image slotBackgroundImage; // Slot background image.
    [SerializeField] private Image iconImage;           // Item icon or color swatch.
    [SerializeField] private Text itemNameText;         // Item display name.
    [SerializeField] private Text quantityText;         // Stack count text.

    [Header("Colors")]
    [SerializeField] private Color emptyColor = new Color(0.18f, 0.18f, 0.22f, 0.75f); // Empty slot background.
    [SerializeField] private Color filledColor = new Color(0.42f, 0.30f, 0.52f, 0.90f); // Filled slot background.

    private const string EmptySlotText = "Empty";

    private InventoryItem currentItem;                 // Runtime item shown by this slot.
    private Action<InventoryItem> itemClickedCallback; // Callback owned by InventoryPanel.

    private void Awake()
    {
        ValidateReferences();
        DisableChildRaycasts();
    }

    /// <summary>
    /// Displays an item in this slot.
    /// </summary>
    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            SetEmpty();
            return;
        }

        currentItem = item;
        SetBackground(filledColor);
        SetIcon(item);
        SetName(item.displayName);
        SetQuantity(item.quantity);
    }

    /// <summary>
    /// Displays the empty slot state.
    /// </summary>
    public void SetEmpty()
    {
        currentItem = null;
        SetBackground(emptyColor);

        if (iconImage != null)
            iconImage.enabled = false;

        SetName(EmptySlotText);
        SetQuantity(0);
    }

    /// <summary>
    /// Stores the callback used when this slot is clicked.
    /// </summary>
    public void SetClickCallback(Action<InventoryItem> callback)
    {
        itemClickedCallback = callback;
    }

    /// <summary>
    /// Sends the currently displayed item to InventoryPanel. Null means the slot is empty.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        itemClickedCallback?.Invoke(currentItem);
    }

    private void SetBackground(Color targetColor)
    {
        if (slotBackgroundImage != null)
            slotBackgroundImage.color = targetColor;
    }

    private void SetIcon(InventoryItem item)
    {
        if (iconImage == null)
            return;

        iconImage.enabled = true;
        iconImage.sprite = item.icon;
        iconImage.color = item.icon != null ? Color.white : item.itemColor;
    }

    private void SetName(string itemName)
    {
        if (itemNameText != null)
            itemNameText.text = itemName;
    }

    private void SetQuantity(int quantity)
    {
        if (quantityText == null)
            return;

        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    private void ValidateReferences()
    {
        if (slotBackgroundImage == null)
            Debug.LogError("InventorySlotView: slotBackgroundImage is not assigned.");

        if (iconImage == null)
            Debug.LogError("InventorySlotView: iconImage is not assigned.");

        if (itemNameText == null)
            Debug.LogError("InventorySlotView: itemNameText is not assigned.");

        if (quantityText == null)
            Debug.LogError("InventorySlotView: quantityText is not assigned.");
    }

    private void DisableChildRaycasts()
    {
        // The slot background receives clicks; child visuals should not steal pointer events.
        if (iconImage != null)
            iconImage.raycastTarget = false;

        if (itemNameText != null)
            itemNameText.raycastTarget = false;

        if (quantityText != null)
            quantityText.raycastTarget = false;
    }
}
