using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class EquipmentData : ItemData
{
    [Header("装备信息")]
    public EquipmentType equipmentType;

    [Header("属性加成")]
    public EquipmentModifier[] modifiers;

    [Header("特效")]
    public ItemEffect[] effects;

    [Header("制作配方（为空则不可制作）")]
    public CraftingRecipe[] recipes;

    private void OnValidate()
    {
        itemType = ItemType.Equipment;
    }
}
