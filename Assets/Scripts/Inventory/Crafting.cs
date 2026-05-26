using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingRecipe[] allRecipes;

    public IReadOnlyList<CraftingRecipe> AllRecipes => allRecipes;

    private void Awake()
    {
        if (inventory == null) inventory = GetComponent<Inventory>();
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        if (recipe == null || recipe.result == null || recipe.materials == null) return false;
        foreach (var mat in recipe.materials)
        {
            if (mat.material == null || inventory.GetMaterialCount(mat.material) < mat.amount)
                return false;
        }
        return true;
    }

    public bool Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe)) return false;

        // 扣除材料
        foreach (var mat in recipe.materials)
            inventory.RemoveMaterial(mat.material, mat.amount);

        // 生成装备
        for (int i = 0; i < recipe.resultAmount; i++)
            inventory.AddEquipment(recipe.result);

        Debug.Log($"Crafting: 制作了 {recipe.result.itemName} x{recipe.resultAmount}");
        return true;
    }
}
