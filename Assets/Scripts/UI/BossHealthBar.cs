// Script purpose: Shows a top-screen Boss health bar after the Boss is active or damaged.
// Key Inspector variables:
// - bossTag: Tag used to locate the Boss GameObject in the scene.
// - bossName: Text shown above the bar.
// - barWidth / barHeight: Screen-space size of the top Boss health bar.
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("Boss Lookup")]
    [SerializeField] private string bossTag = "Boss";
    [SerializeField] private bool showOnlyWhenBossActivated = true;

    [Header("Style")]
    [SerializeField] private string bossName = "Evil Wizard";
    [SerializeField] private float barWidth = 600f;
    [SerializeField] private float barHeight = 28f;
    [SerializeField] private Color fillColor = new Color(0.85f, 0.08f, 0.12f, 1f);
    [SerializeField] private Color backgroundColor = new Color(0.12f, 0.10f, 0.10f, 1f);

    private Entity bossEntity;
    private Enemy_EvilWizard evilWizardBoss;
    private RectTransform fillRect;
    private GameObject canvasRoot;

    private void Awake()
    {
        CreateUI();
        SetVisible(false);
    }

    private void Update()
    {
        if (bossEntity == null)
            TryFindBossByTag();

        if (bossEntity == null)
        {
            SetVisible(false);
            return;
        }

        bool shouldShowBar = ShouldShowBar();
        SetVisible(shouldShowBar);

        if (!shouldShowBar) return;

        UpdateFill();
    }

    private void TryFindBossByTag()
    {
        GameObject bossObject = GameObject.FindGameObjectWithTag(bossTag);
        if (bossObject == null) return;

        bossEntity = bossObject.GetComponent<Entity>();
        evilWizardBoss = bossObject.GetComponent<Enemy_EvilWizard>();

        if (bossEntity == null)
            Debug.LogError("BossHealthBar: Boss tagged object does not have an Entity component.", bossObject);
    }

    private bool ShouldShowBar()
    {
        if (bossEntity.currentHealth <= 0f) return false;
        if (!showOnlyWhenBossActivated) return true;

        if (evilWizardBoss != null)
            return evilWizardBoss.IsBossActivated || bossEntity.currentHealth < bossEntity.maxHealth;

        return bossEntity.currentHealth < bossEntity.maxHealth;
    }

    private void SetVisible(bool isVisible)
    {
        if (canvasRoot != null && canvasRoot.activeSelf != isVisible)
            canvasRoot.SetActive(isVisible);
    }

    private void UpdateFill()
    {
        if (fillRect == null || bossEntity == null) return;

        float healthRatio = bossEntity.maxHealth > 0f
            ? Mathf.Clamp01(bossEntity.currentHealth / bossEntity.maxHealth)
            : 0f;

        fillRect.sizeDelta = new Vector2(barWidth * healthRatio, barHeight);
    }

    private void CreateUI()
    {
        canvasRoot = new GameObject("BossHealthBarCanvas");

        Canvas canvas = canvasRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200;

        CanvasScaler scaler = canvasRoot.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasRoot.AddComponent<GraphicRaycaster>();

        GameObject panel = new GameObject("BarPanel");
        panel.transform.SetParent(canvasRoot.transform, false);

        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.pivot = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -40f);
        panelRect.sizeDelta = new Vector2(barWidth + 8f, barHeight + 8f);

        CreateBackground(panel.transform);
        CreateFill(panel.transform);
        CreateNameText(panel.transform);
    }

    private void CreateBackground(Transform parent)
    {
        GameObject background = new GameObject("Background");
        background.transform.SetParent(parent, false);

        RectTransform backgroundRect = background.AddComponent<RectTransform>();
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        background.AddComponent<Image>().color = backgroundColor;
    }

    private void CreateFill(Transform parent)
    {
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(parent, false);

        fillRect = fill.AddComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0.5f);
        fillRect.anchorMax = new Vector2(0f, 0.5f);
        fillRect.pivot = new Vector2(0f, 0.5f);
        fillRect.anchoredPosition = new Vector2(4f, 0f);
        fillRect.sizeDelta = new Vector2(barWidth, barHeight);

        fill.AddComponent<Image>().color = fillColor;
    }

    private void CreateNameText(Transform parent)
    {
        GameObject nameObject = new GameObject("BossName");
        nameObject.transform.SetParent(parent, false);

        Text nameText = nameObject.AddComponent<Text>();
        nameText.text = bossName;
        nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        nameText.fontSize = 16;
        nameText.fontStyle = FontStyle.Bold;
        nameText.color = new Color(0.92f, 0.85f, 0.72f, 1f);
        nameText.alignment = TextAnchor.MiddleCenter;

        RectTransform nameRect = nameObject.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0f, 1f);
        nameRect.anchorMax = new Vector2(1f, 1f);
        nameRect.pivot = new Vector2(0.5f, 0f);
        nameRect.anchoredPosition = new Vector2(0f, 4f);
        nameRect.sizeDelta = new Vector2(0f, 22f);
    }
}
