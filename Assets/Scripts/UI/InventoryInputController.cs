using UnityEngine;

/// <summary>
/// InventoryInputController reads the backpack hotkey and toggles the inventory panel.
/// It only handles input, keeping inventory data and UI rendering in separate scripts.
/// Press I to toggle the panel, and press Escape to close it when it is already open.
/// </summary>
public class InventoryInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryPanel inventoryPanel; // UI panel controlled by keyboard input.

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.I; // Key used to open or close the backpack.
    [SerializeField] private KeyCode closeKey = KeyCode.Escape; // Key used only to close the backpack.

    private void Awake()
    {
        if (inventoryPanel == null)
            Debug.LogError("InventoryInputController: inventoryPanel is not assigned.");
    }

    private void Update()
    {
        if (inventoryPanel == null)
            return;

        if (Input.GetKeyDown(toggleKey))
            inventoryPanel.Toggle();

        if (Input.GetKeyDown(closeKey) && inventoryPanel.IsVisible)
            inventoryPanel.Close();
    }
}
