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
        return GetMaxCraftCount(recipe) > 0;
    }

    /// <summary>
    /// 计算当前材料最多能合成多少次（不是产物个数，是合成次数）。
    /// </summary>
    public int GetMaxCraftCount(CraftingRecipe recipe)
    {
        if (recipe == null || recipe.result == null || recipe.materials == null) return 0;
        int max = int.MaxValue;
        foreach (var mat in recipe.materials)
        {
            if (mat.material == null || mat.amount <= 0) return 0;
            int owned = inventory.GetMaterialCount(mat.material);
            max = Mathf.Min(max, owned / mat.amount);
        }
        return max == int.MaxValue ? 0 : max;
    }

    /// <summary>
    /// 合成指定次数。返回实际合成次数。
    /// </summary>
    public int Craft(CraftingRecipe recipe, int count)
    {
        if (recipe == null || count <= 0) return 0;
        int max = GetMaxCraftCount(recipe);
        int actual = Mathf.Min(count, max);
        if (actual <= 0) return 0;

        foreach (var mat in recipe.materials)
            inventory.RemoveMaterial(mat.material, mat.amount * actual);

        for (int i = 0; i < actual * recipe.resultAmount; i++)
            inventory.AddEquipment(recipe.result);

        return actual;
    }
}
