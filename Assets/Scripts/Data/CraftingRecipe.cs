using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("制作结果")]
    public EquipmentData result;
    public int resultAmount = 1;

    [Header("所需材料")]
    public RecipeMaterial[] materials;
}

[Serializable]
public class RecipeMaterial
{
    public MaterialData material;
    public int amount = 1;
}
