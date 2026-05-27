using UnityEngine;

public class InventoryInputController : MonoBehaviour
{
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private KeyCode backpackKey = KeyCode.B;
    [SerializeField] private KeyCode craftingKey = KeyCode.N;
    [SerializeField] private KeyCode skillTreeKey = KeyCode.M;
    [SerializeField] private KeyCode settingsKey = KeyCode.P;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    private void Update()
    {
        if (inventoryPanel == null) return;

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
