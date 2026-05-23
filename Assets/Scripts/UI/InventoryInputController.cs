using UnityEngine;

/// <summary>
/// InventoryInputController reads the backpack hotkey and toggles the inventory panel.
/// It only handles input, keeping inventory data and UI rendering in separate scripts.
/// </summary>
public class InventoryInputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryPanel inventoryPanel; // UI panel controlled by the hotkey.

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.I; // Key used to open or close the backpack.

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
    }
}
