using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventoryPanel shows the current inventory list in prebuilt slot views.
/// It does not own item data and does not read keyboard input.
/// </summary>
public class InventoryPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager; // Data source for the backpack.
    [SerializeField] private CanvasGroup canvasGroup;           // Controls panel visibility without destroying objects.
    [SerializeField] private InventorySlotView[] slotViews;     // Fixed visible slot views in the panel.
    [SerializeField] private Text emptyMessageText;             // Message shown when no items exist.

    [Header("State")]
    [SerializeField] private bool startsVisible;                // Whether the backpack is open at scene start.

    public bool IsVisible { get; private set; }                 // Current panel visibility state.

    private void Awake()
    {
        ValidateReferences();
        SetVisible(startsVisible);
    }

    private void OnEnable()
    {
        if (inventoryManager != null)
            inventoryManager.InventoryChanged += Refresh;
    }

    private void Start()
    {
        if (inventoryManager != null)
            Refresh(inventoryManager.Items);
    }

    private void OnDisable()
    {
        if (inventoryManager != null)
            inventoryManager.InventoryChanged -= Refresh;
    }

    /// <summary>
    /// Opens the panel when closed, closes it when open.
    /// </summary>
    public void Toggle()
    {
        SetVisible(!IsVisible);
    }

    /// <summary>
    /// Sets UI visibility while keeping the same objects alive in the scene.
    /// </summary>
    public void SetVisible(bool isVisible)
    {
        IsVisible = isVisible;

        if (canvasGroup == null)
            return;

        canvasGroup.alpha = isVisible ? 1f : 0f;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }

    private void Refresh(IReadOnlyList<InventoryItem> items)
    {
        if (slotViews == null)
            return;

        for (int slotIndex = 0; slotIndex < slotViews.Length; slotIndex++)
            RefreshSlot(slotIndex, items);

        if (emptyMessageText != null)
            emptyMessageText.enabled = items == null || items.Count == 0;
    }

    private void RefreshSlot(int slotIndex, IReadOnlyList<InventoryItem> items)
    {
        InventorySlotView slotView = slotViews[slotIndex];

        if (slotView == null)
            return;

        if (items != null && slotIndex < items.Count)
            slotView.SetItem(items[slotIndex]);
        else
            slotView.SetEmpty();
    }

    private void ValidateReferences()
    {
        if (inventoryManager == null)
            Debug.LogError("InventoryPanel: inventoryManager is not assigned.");

        if (canvasGroup == null)
            Debug.LogError("InventoryPanel: canvasGroup is not assigned.");

        if (slotViews == null || slotViews.Length == 0)
            Debug.LogError("InventoryPanel: slotViews is empty. Assign slot views in the Inspector.");
    }
}
