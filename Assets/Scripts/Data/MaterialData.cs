using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Inventory/Material")]
public class MaterialData : ItemData
{
    private void OnValidate()
    {
        itemType = ItemType.Material;
    }
}
