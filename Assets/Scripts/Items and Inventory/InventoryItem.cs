using UnityEngine;

/// <summary>
/// InventoryItem stores one inventory entry that can be shown by the backpack UI.
/// It is data only: it does not create UI, read input, or change player stats.
/// </summary>
[System.Serializable]
public class InventoryItem
{
    [Header("Identity")]
    public string itemId;          // Unique id used when stackable items merge.
    public string displayName;     // Player-facing name shown in the slot.
    [TextArea] public string description; // Short note for later tooltip work.

    [Header("Stacking")]
    public int quantity = 1;       // Amount shown in the slot.
    public bool canStack = true;   // Whether another item with the same id can merge.

    [Header("Visual")]
    public Sprite icon;            // Optional icon displayed by InventorySlotView.
    public Color itemColor = Color.white; // Color swatch used when no icon exists.

    /// <summary>
    /// Creates a separate runtime copy so scene defaults are not mutated by play mode edits.
    /// </summary>
    public InventoryItem Copy()
    {
        return new InventoryItem
        {
            itemId = itemId,
            displayName = displayName,
            description = description,
            quantity = quantity,
            canStack = canStack,
            icon = icon,
            itemColor = itemColor
        };
    }
}
