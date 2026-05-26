using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Crafting crafting;
    [SerializeField] private Transform recipeListParent;
    [SerializeField] private GameObject recipeButtonPrefab;
    [SerializeField] private Text recipeNameText;
    [SerializeField] private Text recipeMaterialsText;
    [SerializeField] private Button craftButton;
    [SerializeField] private ItemTooltip tooltip;

    private CraftingRecipe selectedRecipe;
    private readonly List<GameObject> spawnedButtons = new List<GameObject>();

    private void Start()
    {
        if (craftButton != null)
            craftButton.onClick.AddListener(OnCraftClicked);

        RefreshRecipeList();
    }

    public void RefreshRecipeList()
    {
        if (crafting == null || recipeListParent == null || recipeButtonPrefab == null) return;

        // 清除旧按钮
        foreach (var btn in spawnedButtons)
            if (btn != null) Destroy(btn);
        spawnedButtons.Clear();

        IReadOnlyList<CraftingRecipe> recipes = crafting.AllRecipes;
        if (recipes == null) return;

        for (int i = 0; i < recipes.Count; i++)
        {
            CraftingRecipe recipe = recipes[i];
            if (recipe == null || recipe.result == null) continue;

            GameObject btn = Instantiate(recipeButtonPrefab, recipeListParent);
            btn.SetActive(true);

            Text btnText = btn.GetComponentInChildren<Text>();
            if (btnText != null) btnText.text = recipe.result.itemName;

            Button btnComp = btn.GetComponent<Button>();
            if (btnComp != null)
            {
                CraftingRecipe captured = recipe;
                bool canCraft = crafting.CanCraft(captured);

                // 按钮颜色提示
                ColorBlock colors = btnComp.colors;
                colors.normalColor = canCraft ? Color.white : new Color(0.6f, 0.6f, 0.6f);
                btnComp.colors = colors;

                btnComp.onClick.AddListener(() => SelectRecipe(captured));
            }

            spawnedButtons.Add(btn);
        }
    }

    private void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        if (recipe == null || recipe.result == null) return;

        if (recipeNameText != null)
            recipeNameText.text = recipe.result.itemName;

        if (recipeMaterialsText != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var mat in recipe.materials)
            {
                if (mat.material == null) continue;
                int owned = crafting != null ? 0 : 0;
                // 通过 Crafting 组件获取材料数量 (需要 Inventory 引用)
                sb.AppendLine($"{mat.material.itemName}: {mat.amount}");
            }
            recipeMaterialsText.text = sb.ToString();
        }

        if (craftButton != null)
            craftButton.interactable = crafting.CanCraft(recipe);

        if (tooltip != null)
            tooltip.ShowRecipe(recipe, crafting.CanCraft(recipe));
    }

    private void OnCraftClicked()
    {
        if (selectedRecipe == null || crafting == null) return;

        if (crafting.Craft(selectedRecipe))
        {
            // 制作成功，刷新列表和详情
            RefreshRecipeList();
            SelectRecipe(selectedRecipe);
        }
    }
}
