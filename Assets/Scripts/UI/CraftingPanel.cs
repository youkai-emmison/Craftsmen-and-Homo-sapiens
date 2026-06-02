using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Crafting crafting;
    [SerializeField] private Inventory inventory;

    [Header("Recipe List (Left)")]
    [SerializeField] private Transform recipeListParent;
    [SerializeField] private CraftingRecipeListItem recipeItemPrefab;

    [Header("Crafting Detail (Right)")]
    [SerializeField] private Image resultIcon;
    [SerializeField] private TextMeshProUGUI resultNameText;
    [SerializeField] private TextMeshProUGUI resultDescText;
    [SerializeField] private TextMeshProUGUI materialsText;
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Button craftButton;
    [SerializeField] private TextMeshProUGUI craftButtonText;

    [Header("Tooltip")]
    [SerializeField] private ItemTooltip tooltip;

    private CraftingRecipe selectedRecipe;
    private int craftCount = 1;
    private readonly List<CraftingRecipeListItem> spawnedItems = new List<CraftingRecipeListItem>();

    private void Start()
    {
        if (crafting == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                if (crafting == null) crafting = player.GetComponent<Crafting>();
                if (inventory == null) inventory = player.GetComponent<Inventory>();
            }
        }

        if (quantitySlider != null)
        {
            quantitySlider.wholeNumbers = true;
            quantitySlider.minValue = 1;
            quantitySlider.onValueChanged.AddListener(OnSliderChanged);
        }

        if (craftButton != null)
            craftButton.onClick.AddListener(OnCraftClicked);

        RefreshRecipeList();
    }

    public void HideTooltip()
    {
        if (tooltip != null) tooltip.Hide();
    }

    public void RefreshRecipeList()
    {
        if (crafting == null || recipeListParent == null || recipeItemPrefab == null) return;

        var recipes = crafting.AllRecipes;
        if (recipes == null) return;

        // 确保有足够的列表项
        while (spawnedItems.Count < recipes.Count)
        {
            var newItem = Instantiate(recipeItemPrefab, recipeListParent);
            spawnedItems.Add(newItem);
        }

        // 隐藏多余的
        for (int i = recipes.Count; i < spawnedItems.Count; i++)
            spawnedItems[i].gameObject.SetActive(false);

        // 填充
        for (int i = 0; i < recipes.Count; i++)
        {
            var recipe = recipes[i];
            if (recipe == null || recipe.result == null)
            {
                spawnedItems[i].gameObject.SetActive(false);
                continue;
            }

            spawnedItems[i].gameObject.SetActive(true);
            bool canCraft = crafting.CanCraft(recipe);
            bool isSelected = recipe == selectedRecipe;
            spawnedItems[i].Setup(recipe, canCraft, isSelected, OnRecipeClicked);
        }

        // 如果当前选中的配方被刷新了，更新详情
        if (selectedRecipe != null)
            RefreshDetail();
    }

    private void OnRecipeClicked(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        craftCount = 1;

        // 更新列表选中状态
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (!spawnedItems[i].gameObject.activeSelf) continue;
            bool isSelected = spawnedItems[i].Recipe == recipe;
            bool canCraft = crafting.CanCraft(spawnedItems[i].Recipe);
            spawnedItems[i].UpdateSelection(isSelected, canCraft);
        }

        RefreshDetail();

        if (tooltip != null)
            tooltip.ShowRecipe(recipe, crafting.CanCraft(recipe));
    }

    private void RefreshDetail()
    {
        if (selectedRecipe == null || selectedRecipe.result == null) return;

        var result = selectedRecipe.result;

        // 产物信息
        if (resultIcon != null)
        {
            resultIcon.enabled = result.icon != null;
            resultIcon.sprite = result.icon;
        }
        if (resultNameText != null)
            resultNameText.text = result.itemName;
        if (resultDescText != null)
            resultDescText.text = result.description ?? "";

        // 材料列表
        UpdateMaterialsText();

        // 滑块
        int maxCount = crafting.GetMaxCraftCount(selectedRecipe);
        if (quantitySlider != null)
        {
            quantitySlider.maxValue = Mathf.Max(1, maxCount);
            quantitySlider.value = 1;
        }
        craftCount = 1;
        UpdateQuantityText();
        UpdateCraftButton();
    }

    private void UpdateMaterialsText()
    {
        if (materialsText == null || selectedRecipe == null) return;

        var sb = new StringBuilder();
        foreach (var mat in selectedRecipe.materials)
        {
            if (mat.material == null) continue;
            int owned = inventory != null ? inventory.GetMaterialCount(mat.material) : 0;
            int required = mat.amount * craftCount;
            string colorTag = owned >= required ? "green" : "red";
            sb.AppendLine($"<color={colorTag}>{mat.material.itemName}: {owned}/{required}</color>");
        }
        materialsText.text = sb.ToString();
    }

    private void OnSliderChanged(float value)
    {
        craftCount = Mathf.Max(1, Mathf.RoundToInt(value));
        UpdateQuantityText();
        UpdateMaterialsText();
        UpdateCraftButton();
    }

    private void UpdateQuantityText()
    {
        if (quantityText != null)
            quantityText.text = craftCount.ToString();
    }

    private void UpdateCraftButton()
    {
        if (craftButton == null || crafting == null || selectedRecipe == null) return;
        int maxCount = crafting.GetMaxCraftCount(selectedRecipe);
        craftButton.interactable = craftCount <= maxCount && craftCount > 0;
        if (craftButtonText != null)
            craftButtonText.text = "Craft";
    }

    private void OnCraftClicked()
    {
        if (selectedRecipe == null || crafting == null) return;

        int made = crafting.Craft(selectedRecipe, craftCount);
        if (made > 0)
        {
            // 制作成功，刷新
            int maxCount = crafting.GetMaxCraftCount(selectedRecipe);
            if (quantitySlider != null)
                quantitySlider.maxValue = Mathf.Max(1, maxCount);

            // 如果剩余材料不够当前数量，调整滑块
            if (craftCount > maxCount)
            {
                craftCount = Mathf.Max(1, maxCount);
                if (quantitySlider != null) quantitySlider.value = craftCount;
            }

            UpdateMaterialsText();
            UpdateCraftButton();
            RefreshRecipeList();
        }
    }
}
