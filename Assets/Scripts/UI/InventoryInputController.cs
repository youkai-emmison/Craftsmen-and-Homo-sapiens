using UnityEngine;

public class InventoryInputController : MonoBehaviour
{
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;
    [SerializeField] private KeyCode closeKey = KeyCode.Escape;

    private void Update()
    {
        if (inventoryPanel == null) return;

        if (Input.GetKeyDown(toggleKey))
            inventoryPanel.Toggle();

        if (Input.GetKeyDown(closeKey) && inventoryPanel.IsVisible)
            inventoryPanel.Close();
    }
}
