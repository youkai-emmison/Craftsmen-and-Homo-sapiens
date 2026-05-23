using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventorySlotView renders one backpack slot.
/// It receives item data from InventoryPanel and never changes inventory contents directly.
/// </summary>
public class InventorySlotView : MonoBehaviour
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

    private void Awake()
    {
        ValidateReferences();
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
        SetBackground(emptyColor);

        if (iconImage != null)
            iconImage.enabled = false;

        SetName(EmptySlotText);
        SetQuantity(0);
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
}
