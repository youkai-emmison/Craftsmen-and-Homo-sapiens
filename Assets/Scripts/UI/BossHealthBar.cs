using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Boss 血条 UI —— 屏幕顶部显示，血量减少时从右往左缩。
/// 通过 Tag "Boss" 查找目标。
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    #region Inspector

    [Header("样式")]
    [SerializeField] private string bossName = "Evil Wizard";
    [SerializeField] private float barWidth = 600f;
    [SerializeField] private float barHeight = 28f;
    [SerializeField] private Color fillColor = new Color(0.85f, 0.08f, 0.12f, 1f);
    [SerializeField] private Color bgColor = new Color(0.12f, 0.10f, 0.10f, 1f);

    #endregion

    #region 运行时状态

    private Entity bossEntity;
    private RectTransform fillRect;
    private GameObject canvasRoot;

    #endregion

    #region 生命周期

    private void Awake()
    {
        CreateUI();
        if (canvasRoot != null)
            canvasRoot.SetActive(false);
    }

    private void Update()
    {
        // 持续尝试查找 Boss
        if (bossEntity == null)
        {
            TryFindBoss();
            if (bossEntity == null) return;
        }

        // 显示血条
        if (canvasRoot != null && !canvasRoot.activeSelf)
            canvasRoot.SetActive(true);

        UpdateFill();
    }

    #endregion

    #region 查找 Boss

    private void TryFindBoss()
    {
        try
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
            if (bossObj != null)
            {
                bossEntity = bossObj.GetComponent<Entity>();
                Debug.Log($"BossHealthBar: 找到 Boss: {bossObj.name}");
            }
        }
        catch (System.Exception)
        {
            // Tag "Boss" 不存在，静默忽略
        }
    }

    #endregion

    #region 血条更新

    private void UpdateFill()
    {
        if (fillRect == null || bossEntity == null) return;

        float ratio = bossEntity.maxHealth > 0f
            ? Mathf.Clamp01(bossEntity.currentHealth / bossEntity.maxHealth)
            : 0f;

        fillRect.anchorMax = new Vector2(ratio, 1f);
    }

    #endregion

    #region UI 自动生成

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

        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(panel.transform, false);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        bg.AddComponent<Image>().color = bgColor;

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(panel.transform, false);
        fillRect = fill.AddComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0f, 0f);
        fillRect.anchorMax = new Vector2(1f, 1f);
        fillRect.pivot = new Vector2(0f, 0.5f);
        fillRect.offsetMin = new Vector2(4f, 4f);
        fillRect.offsetMax = new Vector2(-4f, -4f);
        fill.AddComponent<Image>().color = fillColor;

        GameObject nameObj = new GameObject("BossName");
        nameObj.transform.SetParent(panel.transform, false);
        Text nameText = nameObj.AddComponent<Text>();
        nameText.text = bossName;
        nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        nameText.fontSize = 16;
        nameText.fontStyle = FontStyle.Bold;
        nameText.color = new Color(0.92f, 0.85f, 0.72f, 1f);
        nameText.alignment = TextAnchor.MiddleCenter;
        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0f, 1f);
        nameRect.anchorMax = new Vector2(1f, 1f);
        nameRect.pivot = new Vector2(0.5f, 0f);
        nameRect.anchoredPosition = new Vector2(0f, 4f);
        nameRect.sizeDelta = new Vector2(0f, 22f);
    }

    #endregion
}
