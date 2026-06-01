// Script purpose: Reads keyboard shortcuts for opening and closing the inventory panels.
// Key Inspector variables:
// - inventoryPanel: UI panel that owns backpack, crafting, skill tree, and settings tabs.
// - backpackKey / craftingKey / skillTreeKey / settingsKey: Tab toggle shortcuts.
// - closeKey: Shortcut used to close the visible panel.
using UnityEngine;

public class InventoryInputController : MonoBehaviour
{
    // Panel controlled by these shortcuts.
    [SerializeField] private InventoryPanel inventoryPanel;

    // Keyboard shortcuts kept serialized so they can be changed in the Inspector.
    [SerializeField] private KeyCode backpackKey = KeyCode.B;
    [SerializeField] private KeyCode craftingKey = KeyCode.N;
    [SerializeField] private KeyCode skillTreeKey = KeyCode.M;
    [SerializeField] private KeyCode settingsKey = KeyCode.P;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    private void Update()
    {
        if (inventoryPanel == null) return;

        if (DialoguePanelController.IsAnyDialogueOpen)
        {
            return;
        }

        if (Input.GetKeyDown(backpackKey))
            inventoryPanel.ToggleTab(0);
        if (Input.GetKeyDown(craftingKey))
            inventoryPanel.ToggleTab(1);
        if (Input.GetKeyDown(skillTreeKey))
            inventoryPanel.ToggleTab(2);
        if (Input.GetKeyDown(settingsKey))
            inventoryPanel.ToggleTab(3);

        if (Input.GetKeyDown(closeKey) && inventoryPanel.IsVisible)
            inventoryPanel.Close();
    }
}
