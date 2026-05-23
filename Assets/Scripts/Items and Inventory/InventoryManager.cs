using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InventoryManager owns the backpack item list and notifies UI when the data changes.
/// It does not create UI objects, handle input, or apply equipment effects.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Header("Starting Items")]
    [SerializeField] private List<InventoryItem> startingItems = new List<InventoryItem>(); // Items loaded when the scene starts.

    private readonly List<InventoryItem> currentItems = new List<InventoryItem>(); // Runtime inventory content.

    public event Action<IReadOnlyList<InventoryItem>> InventoryChanged; // Raised after the inventory list changes.

    public IReadOnlyList<InventoryItem> Items => currentItems; // Read-only view for UI.

    private void Awake()
    {
        LoadStartingItems();
    }

    private void Start()
    {
        NotifyInventoryChanged();
    }

    /// <summary>
    /// Adds one item and merges it with an existing stack when allowed.
    /// </summary>
    public void AddItem(InventoryItem item)
    {
        if (!IsValidItem(item))
            return;

        InventoryItem existingItem = FindStackableItem(item);

        if (existingItem != null)
            existingItem.quantity += Mathf.Max(1, item.quantity);
        else
            currentItems.Add(item.Copy());

        NotifyInventoryChanged();
    }

    /// <summary>
    /// Removes the item at a slot index when later UI buttons need it.
    /// </summary>
    public void RemoveItemAt(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= currentItems.Count)
            return;

        currentItems.RemoveAt(itemIndex);
        NotifyInventoryChanged();
    }

    private void LoadStartingItems()
    {
        currentItems.Clear();

        foreach (InventoryItem startingItem in startingItems)
        {
            if (IsValidItem(startingItem))
                currentItems.Add(startingItem.Copy());
        }
    }

    private InventoryItem FindStackableItem(InventoryItem item)
    {
        if (!item.canStack)
            return null;

        return currentItems.Find(currentItem => currentItem.canStack && currentItem.itemId == item.itemId);
    }

    private bool IsValidItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogError("InventoryManager: item is null.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.itemId))
        {
            Debug.LogError("InventoryManager: itemId is empty. Set itemId in the Inspector.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(item.displayName))
        {
            Debug.LogError("InventoryManager: displayName is empty. Set displayName in the Inspector.");
            return false;
        }

        return true;
    }

    private void NotifyInventoryChanged()
    {
        InventoryChanged?.Invoke(currentItems);
    }
}
