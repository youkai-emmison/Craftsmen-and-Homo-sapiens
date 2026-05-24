using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventoryItemDetailPanel displays the item currently selected in the backpack.
/// It only renders text and does not change inventory data or apply item effects.
/// </summary>
public class InventoryItemDetailPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text itemNameText;     // Selected item name.
    [SerializeField] private Text quantityText;     // Selected item stack count.
    [SerializeField] private Text descriptionText;  // Selected item description.

    private const string EmptyNameText = "No item selected";
    private const string EmptyQuantityText = "Quantity: -";
    private const string EmptyDescriptionText = "Click an item slot to view details.";

    private void Awake()
    {
        ValidateReferences();
        Clear();
    }

    /// <summary>
    /// Shows details for a selected item, or the empty state when the item is null.
    /// </summary>
    public void ShowItem(InventoryItem item)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        SetText(itemNameText, item.displayName);
        SetText(quantityText, $"Quantity: {Mathf.Max(1, item.quantity)}");
        SetText(descriptionText, string.IsNullOrWhiteSpace(item.description) ? "No description yet." : item.description);
    }

    /// <summary>
    /// Resets the panel when an empty slot is selected.
    /// </summary>
    public void Clear()
    {
        SetText(itemNameText, EmptyNameText);
        SetText(quantityText, EmptyQuantityText);
        SetText(descriptionText, EmptyDescriptionText);
    }

    private void SetText(Text targetText, string value)
    {
        if (targetText != null)
            targetText.text = value;
    }

    private void ValidateReferences()
    {
        if (itemNameText == null)
            Debug.LogError("InventoryItemDetailPanel: itemNameText is not assigned.");

        if (quantityText == null)
            Debug.LogError("InventoryItemDetailPanel: quantityText is not assigned.");

        if (descriptionText == null)
            Debug.LogError("InventoryItemDetailPanel: descriptionText is not assigned.");
    }
}
