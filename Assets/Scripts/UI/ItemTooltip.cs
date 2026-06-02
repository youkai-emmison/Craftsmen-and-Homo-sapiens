using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI effectsText;
    [SerializeField] private CanvasGroup canvasGroup;

    private RectTransform rectTransform;
    private Canvas parentCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        Hide();
    }

    public void ShowEquipment(EquipmentData data)
    {
        if (data == null) { Hide(); return; }

        SetIcon(data.icon);
        SetName(data.itemName);
        SetType(data.equipmentType.ToString());
        SetDescription(data.description);
        SetStats(data.modifiers);
        SetEffects(data.effects);
        PositionAtMouse();
        SetVisible(true);
    }

    public void ShowMaterial(MaterialData data)
    {
        if (data == null) { Hide(); return; }

        SetIcon(data.icon);
        SetName(data.itemName);
        SetType("Material");
        SetDescription(data.description);
        ClearStats();
        ClearEffects();
        PositionAtMouse();
        SetVisible(true);
    }

    public void ShowRecipe(CraftingRecipe recipe, bool canCraft)
    {
        if (recipe == null || recipe.result == null) { Hide(); return; }

        SetIcon(recipe.result.icon);
        SetName(recipe.result.itemName);
        SetType(recipe.result.equipmentType.ToString() + " (Recipe)");
        SetDescription(recipe.result.description);
        SetStats(recipe.result.modifiers);
        SetRecipeMaterials(recipe, canCraft);
        PositionAtMouse();
        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
    }

    private void SetIcon(Sprite sprite)
    {
        if (iconImage == null) return;
        if (sprite != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = sprite;
            iconImage.color = Color.white;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    private void SetName(string name)
    {
        if (nameText != null) nameText.text = name;
    }

    private void SetType(string type)
    {
        if (typeText != null) typeText.text = type;
    }

    private void SetDescription(string desc)
    {
        if (descriptionText == null) return;
        descriptionText.text = !string.IsNullOrEmpty(desc) ? desc : "";
    }

    private void SetStats(EquipmentModifier[] modifiers)
    {
        if (statsText == null) return;
        if (modifiers == null || modifiers.Length == 0) { statsText.text = ""; return; }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (var mod in modifiers)
        {
            string prefix = mod.modifierType == ModifierType.Percent
                ? $"{mod.value * 100:F0}%"
                : $"+{mod.value:F0}";
            sb.AppendLine($"{mod.statType}: {prefix}");
        }
        statsText.text = sb.ToString();
    }

    private void ClearStats()
    {
        if (statsText != null) statsText.text = "";
    }

    private void SetEffects(ItemEffect[] effects)
    {
        if (effectsText == null) return;
        if (effects == null || effects.Length == 0) { effectsText.text = ""; return; }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("<b>Effects:</b>");
        foreach (var effect in effects)
        {
            if (effect != null)
                sb.AppendLine($"- {effect.effectName}: {effect.description}");
        }
        effectsText.text = sb.ToString();
    }

    private void ClearEffects()
    {
        if (effectsText != null) effectsText.text = "";
    }

    private void SetRecipeMaterials(CraftingRecipe recipe, bool canCraft)
    {
        if (effectsText == null) return;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("<b>Materials:</b>");
        foreach (var mat in recipe.materials)
        {
            if (mat.material != null)
            {
                string status = canCraft ? "<color=green>OK</color>" : "<color=red>Insufficient</color>";
                sb.AppendLine($"- {mat.material.itemName} x{mat.amount} {status}");
            }
        }
        effectsText.text = sb.ToString();
    }

    private void PositionAtMouse()
    {
        if (rectTransform == null || parentCanvas == null) return;

        Vector2 mousePos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            mousePos,
            parentCanvas.worldCamera,
            out Vector2 localPos);

        // 偏移避免遮挡鼠标
        localPos += new Vector2(15f, -15f);

        // 边界检查
        RectTransform canvasRect = parentCanvas.transform as RectTransform;
        if (canvasRect != null)
        {
            Vector2 tooltipSize = rectTransform.sizeDelta;
            if (localPos.x + tooltipSize.x > canvasRect.sizeDelta.x * 0.5f)
                localPos.x -= tooltipSize.x + 30f;
            if (localPos.y - tooltipSize.y < -canvasRect.sizeDelta.y * 0.5f)
                localPos.y += tooltipSize.y + 30f;
        }

        rectTransform.anchoredPosition = localPos;
    }

    private void SetVisible(bool visible)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            gameObject.SetActive(visible);
        }
    }
}
