using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingRecipeListItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image backgroundImage;

    private static readonly Color NormalColor = new Color(0.42f, 0.30f, 0.52f, 0.90f);
    private static readonly Color SelectedColor = new Color(0.55f, 0.40f, 0.70f, 0.95f);
    private static readonly Color CannotCraftColor = new Color(0.35f, 0.35f, 0.35f, 0.70f);

    public CraftingRecipe Recipe { get; private set; }
    private Action<CraftingRecipe> onClickCallback;

    public void Setup(CraftingRecipe recipe, bool canCraft, bool isSelected, Action<CraftingRecipe> onClick)
    {
        Recipe = recipe;
        onClickCallback = onClick;

        if (recipe.result != null)
        {
            if (iconImage != null)
            {
                iconImage.enabled = recipe.result.icon != null;
                iconImage.sprite = recipe.result.icon;
            }
            if (nameText != null) nameText.text = recipe.result.itemName;
        }

        UpdateColor(canCraft, isSelected);
    }

    public void UpdateSelection(bool isSelected, bool canCraft)
    {
        UpdateColor(canCraft, isSelected);
    }

    private void UpdateColor(bool canCraft, bool isSelected)
    {
        if (backgroundImage == null) return;
        if (!canCraft)
            backgroundImage.color = CannotCraftColor;
        else if (isSelected)
            backgroundImage.color = SelectedColor;
        else
            backgroundImage.color = NormalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke(Recipe);
    }
}
