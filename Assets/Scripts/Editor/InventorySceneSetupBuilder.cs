using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// InventorySceneSetupBuilder creates the minimal backpack UI in the open scene.
/// It is an Editor-only setup tool, so runtime code does not auto-create UI objects.
/// </summary>
public static class InventorySceneSetupBuilder
{
    private const string InventorySystemName = "InventorySystem"; // Root object for inventory data and input.
    private const string InventoryCanvasName = "InventoryCanvas"; // Canvas object that owns backpack UI.
    private const string InventoryPanelName = "InventoryPanel";   // Backpack panel object toggled by I.
    private const int SlotCount = 8;                               // Fixed slots for the first backpack test.
    private const string UiArtRoot = "Assets/Art/Kenney/FantasyUIBorders/PNG/Default"; // CC0 UI art used by the builder.
    private const string PanelSpritePath = UiArtRoot + "/Panel/panel-025.png";          // Large backpack frame.
    private const string SlotSpritePath = UiArtRoot + "/Panel/panel-008.png";           // Reused frame for item slots.
    private const string DetailSpritePath = UiArtRoot + "/Border/panel-border-025.png"; // Detail panel frame.

    [MenuItem("Tools/Inventory/Create Minimal Inventory UI")]
    public static void CreateMinimalInventoryUi()
    {
        RebuildInventoryObjects();
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("InventorySceneSetupBuilder: Minimal inventory UI created. Press I in Play Mode to toggle it.");
    }

    public static void CreateInSampleScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);
        CreateMinimalInventoryUi();
    }

    public static void ValidateSampleSceneInventory()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);

        InventoryInputController inputController = UnityEngine.Object.FindObjectOfType<InventoryInputController>();
        InventoryPanel inventoryPanel = UnityEngine.Object.FindObjectOfType<InventoryPanel>();
        Canvas inventoryCanvas = GameObject.Find(InventoryCanvasName)?.GetComponent<Canvas>();

        Require(inputController != null, "Inventory validation failed: InventoryInputController is missing.");
        Require(inventoryPanel != null, "Inventory validation failed: InventoryPanel is missing.");
        Require(inventoryCanvas != null, "Inventory validation failed: InventoryCanvas is missing.");

        ValidatePanelReferences(inventoryPanel);
        ValidateInputReference(inputController);

        Debug.Log("InventorySceneSetupBuilder: Inventory scene validation passed.");
    }

    public static void AddDetailPanelToSampleScene()
    {
        Debug.Log("InventorySceneSetupBuilder: Detail panel is no longer used (merged into ItemTooltip).");
    }

    public static void RebuildInventoryObjects()
    {
        RemoveExistingObject(InventorySystemName);
        RemoveExistingObject(InventoryCanvasName);
        EnsureEventSystem();

        InventoryManager inventoryManager = CreateInventorySystem();
        InventoryPanel inventoryPanel = CreateInventoryCanvas(inventoryManager);

        AssignInputController(inventoryManager, inventoryPanel);
        MarkSceneDirty();
    }

    private static InventoryManager CreateInventorySystem()
    {
        GameObject inventorySystemObject = new GameObject(InventorySystemName);
        InventoryManager inventoryManager = inventorySystemObject.AddComponent<InventoryManager>();
        inventorySystemObject.AddComponent<InventoryInputController>();

        SerializedObject serializedManager = new SerializedObject(inventoryManager);
        SerializedProperty startingItemsProperty = serializedManager.FindProperty("startingItems");
        startingItemsProperty.arraySize = 4;

        SetStartingItem(startingItemsProperty.GetArrayElementAtIndex(0), "training-pass", "Training Pass", "First room test item.", 1, new Color(0.73f, 0.52f, 0.96f, 1f));
        SetStartingItem(startingItemsProperty.GetArrayElementAtIndex(1), "small-potion", "Small Potion", "Placeholder healing item.", 3, new Color(0.95f, 0.34f, 0.44f, 1f));
        SetStartingItem(startingItemsProperty.GetArrayElementAtIndex(2), "practice-blade", "Practice Blade", "Placeholder weapon item.", 1, new Color(0.40f, 0.70f, 1f, 1f));
        SetStartingItem(startingItemsProperty.GetArrayElementAtIndex(3), "cloth-fragment", "Cloth Fragment", "Simple material placeholder.", 5, new Color(0.96f, 0.80f, 0.45f, 1f));

        serializedManager.ApplyModifiedPropertiesWithoutUndo();
        return inventoryManager;
    }

    private static InventoryPanel CreateInventoryCanvas(InventoryManager inventoryManager)
    {
        GameObject canvasObject = new GameObject(InventoryCanvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;

        CanvasScaler canvasScaler = canvasObject.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
        canvasScaler.matchWidthOrHeight = 0.5f;

        Transform canvasTransform = canvasObject.transform;
        CreateHintText(canvasTransform);
        return CreateInventoryPanel(canvasTransform, inventoryManager);
    }

    private static InventoryPanel CreateInventoryPanel(Transform canvasTransform, InventoryManager inventoryManager)
    {
        GameObject panelObject = CreateUiObject(InventoryPanelName, canvasTransform);
        Image panelImage = panelObject.AddComponent<Image>();
        ApplyUiSprite(panelImage, PanelSpritePath, new Color(0.92f, 0.90f, 0.80f, 0.98f));

        CanvasGroup canvasGroup = panelObject.AddComponent<CanvasGroup>();
        InventoryPanel inventoryPanel = panelObject.AddComponent<InventoryPanel>();

        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1f, 1f);
        panelRect.anchorMax = new Vector2(1f, 1f);
        panelRect.pivot = new Vector2(1f, 1f);
        panelRect.sizeDelta = new Vector2(650f, 360f);
        panelRect.anchoredPosition = new Vector2(-28f, -28f);

        Text emptyMessageText = CreatePanelText("EmptyMessage", panelObject.transform, "No items", 20, TextAnchor.MiddleCenter);
        RectTransform emptyMessageRect = emptyMessageText.GetComponent<RectTransform>();
        emptyMessageRect.anchorMin = new Vector2(0f, 0f);
        emptyMessageRect.anchorMax = new Vector2(1f, 1f);
        emptyMessageRect.offsetMin = new Vector2(20f, 20f);
        emptyMessageRect.offsetMax = new Vector2(-20f, -56f);

        CreatePanelText("Title", panelObject.transform, "Backpack", 28, TextAnchor.MiddleLeft);
        InventorySlotView[] slotViews = CreateSlots(panelObject.transform);

        AssignPanelReferences(inventoryPanel, canvasGroup, slotViews, emptyMessageText);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        return inventoryPanel;
    }

    private static InventorySlotView[] CreateSlots(Transform panelTransform)
    {
        GameObject gridObject = CreateUiObject("SlotGrid", panelTransform);
        RectTransform gridRect = gridObject.GetComponent<RectTransform>();
        gridRect.anchorMin = new Vector2(0f, 0f);
        gridRect.anchorMax = new Vector2(1f, 1f);
        gridRect.offsetMin = new Vector2(22f, 24f);
        gridRect.offsetMax = new Vector2(-252f, -74f);

        GridLayoutGroup gridLayout = gridObject.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(178f, 58f);
        gridLayout.spacing = new Vector2(12f, 12f);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 2;

        InventorySlotView[] slotViews = new InventorySlotView[SlotCount];

        for (int slotIndex = 0; slotIndex < SlotCount; slotIndex++)
            slotViews[slotIndex] = CreateSlotView(gridObject.transform, slotIndex);

        return slotViews;
    }

    private static InventorySlotView CreateSlotView(Transform gridTransform, int slotIndex)
    {
        GameObject slotObject = CreateUiObject($"Slot_{slotIndex + 1:00}", gridTransform);
        Image slotBackgroundImage = slotObject.AddComponent<Image>();
        ApplyUiSprite(slotBackgroundImage, SlotSpritePath, new Color(0.92f, 0.86f, 0.70f, 0.94f));

        InventorySlotView slotView = slotObject.AddComponent<InventorySlotView>();

        Image iconImage = CreateSlotIcon(slotObject.transform);
        Text quantityText = CreateSlotText("Quantity", slotObject.transform, string.Empty, 16, TextAnchor.LowerRight, new Vector2(120f, 4f), new Vector2(-10f, -8f));

        AssignSlotReferences(slotView, slotBackgroundImage, iconImage, quantityText);
        return slotView;
    }

    private static Image CreateSlotIcon(Transform slotTransform)
    {
        GameObject iconObject = CreateUiObject("Icon", slotTransform);
        Image iconImage = iconObject.AddComponent<Image>();
        iconImage.color = Color.white;
        iconImage.raycastTarget = false;

        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0f, 0.5f);
        iconRect.anchorMax = new Vector2(0f, 0.5f);
        iconRect.pivot = new Vector2(0f, 0.5f);
        iconRect.sizeDelta = new Vector2(34f, 34f);
        iconRect.anchoredPosition = new Vector2(12f, 0f);

        return iconImage;
    }

    private static Text CreatePanelText(string objectName, Transform parent, string text, int fontSize, TextAnchor alignment)
    {
        Text textComponent = CreateTextObject(objectName, parent, text, fontSize, alignment);
        textComponent.color = new Color(0.20f, 0.12f, 0.08f, 1f);
        RectTransform textRect = textComponent.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.pivot = new Vector2(0.5f, 1f);
        textRect.sizeDelta = new Vector2(0f, 48f);
        textRect.anchoredPosition = new Vector2(0f, -8f);
        textRect.offsetMin = new Vector2(20f, textRect.offsetMin.y);
        textRect.offsetMax = new Vector2(-20f, textRect.offsetMax.y);
        return textComponent;
    }

    private static void ApplyUiSprite(Image image, string spritePath, Color tintColor)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        Require(sprite != null, $"Inventory UI sprite is missing: {spritePath}");

        image.sprite = sprite;
        image.type = Image.Type.Sliced;
        image.color = tintColor;
    }

    private static void SetTopTextRect(RectTransform rectTransform, float topOffset, float height)
    {
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.offsetMin = new Vector2(12f, -topOffset - height);
        rectTransform.offsetMax = new Vector2(-12f, -topOffset);
    }

    private static void SetStretchTextRect(RectTransform rectTransform, Vector2 offsetMin, Vector2 offsetMax)
    {
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.offsetMin = offsetMin;
        rectTransform.offsetMax = offsetMax;
    }

    private static Text CreateSlotText(string objectName, Transform parent, string text, int fontSize, TextAnchor alignment, Vector2 offsetMin, Vector2 offsetMax)
    {
        Text textComponent = CreateTextObject(objectName, parent, text, fontSize, alignment);
        textComponent.color = new Color(0.20f, 0.12f, 0.08f, 1f);
        RectTransform textRect = textComponent.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = offsetMin;
        textRect.offsetMax = offsetMax;
        return textComponent;
    }

    private static void CreateHintText(Transform canvasTransform)
    {
        Text hintText = CreateTextObject("InventoryHint", canvasTransform, "Press I to open backpack, Esc to close", 22, TextAnchor.MiddleLeft);
        RectTransform hintRect = hintText.GetComponent<RectTransform>();
        hintRect.anchorMin = new Vector2(0f, 0f);
        hintRect.anchorMax = new Vector2(0f, 0f);
        hintRect.pivot = new Vector2(0f, 0f);
        hintRect.sizeDelta = new Vector2(420f, 42f);
        hintRect.anchoredPosition = new Vector2(24f, 22f);
    }

    private static Text CreateTextObject(string objectName, Transform parent, string text, int fontSize, TextAnchor alignment)
    {
        GameObject textObject = CreateUiObject(objectName, parent);
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = fontSize;
        textComponent.alignment = alignment;
        textComponent.color = Color.white;
        textComponent.raycastTarget = false;
        return textComponent;
    }

    private static GameObject CreateUiObject(string objectName, Transform parent)
    {
        GameObject gameObject = new GameObject(objectName, typeof(RectTransform));
        gameObject.transform.SetParent(parent, false);
        return gameObject;
    }

    private static void AssignInputController(InventoryManager inventoryManager, InventoryPanel inventoryPanel)
    {
        InventoryInputController inputController = inventoryManager.GetComponent<InventoryInputController>();
        SerializedObject serializedInput = new SerializedObject(inputController);
        serializedInput.FindProperty("inventoryPanel").objectReferenceValue = inventoryPanel;
        serializedInput.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void AssignPanelReferences(InventoryPanel inventoryPanel, CanvasGroup canvasGroup, InventorySlotView[] slotViews, Text emptyMessageText)
    {
        SerializedObject serializedPanel = new SerializedObject(inventoryPanel);
        serializedPanel.FindProperty("canvasGroup").objectReferenceValue = canvasGroup;
        serializedPanel.FindProperty("emptyMessageText").objectReferenceValue = emptyMessageText;

        SerializedProperty slotViewsProperty = serializedPanel.FindProperty("slotViews");
        slotViewsProperty.arraySize = slotViews.Length;

        for (int slotIndex = 0; slotIndex < slotViews.Length; slotIndex++)
            slotViewsProperty.GetArrayElementAtIndex(slotIndex).objectReferenceValue = slotViews[slotIndex];

        serializedPanel.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void AssignSlotReferences(InventorySlotView slotView, Image slotBackgroundImage, Image iconImage, Text quantityText)
    {
        SerializedObject serializedSlot = new SerializedObject(slotView);
        serializedSlot.FindProperty("slotBackgroundImage").objectReferenceValue = slotBackgroundImage;
        serializedSlot.FindProperty("iconImage").objectReferenceValue = iconImage;
        serializedSlot.FindProperty("quantityText").objectReferenceValue = quantityText;
        serializedSlot.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetStartingItem(SerializedProperty itemProperty, string itemId, string displayName, string description, int quantity, Color itemColor)
    {
        itemProperty.FindPropertyRelative("itemId").stringValue = itemId;
        itemProperty.FindPropertyRelative("displayName").stringValue = displayName;
        itemProperty.FindPropertyRelative("description").stringValue = description;
        itemProperty.FindPropertyRelative("quantity").intValue = quantity;
        itemProperty.FindPropertyRelative("canStack").boolValue = true;
        itemProperty.FindPropertyRelative("itemColor").colorValue = itemColor;
    }

    private static void EnsureEventSystem()
    {
        if (UnityEngine.Object.FindObjectOfType<EventSystem>() != null)
            return;

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
    }

    private static void RemoveExistingObject(string objectName)
    {
        GameObject existingObject = GameObject.Find(objectName);

        if (existingObject != null)
            UnityEngine.Object.DestroyImmediate(existingObject);
    }

    private static void MarkSceneDirty()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(activeScene);
    }


    private static void ValidatePanelReferences(InventoryPanel inventoryPanel)
    {
        SerializedObject serializedPanel = new SerializedObject(inventoryPanel);
        Require(serializedPanel.FindProperty("canvasGroup").objectReferenceValue != null, "Inventory validation failed: panel canvasGroup is missing.");
        Require(serializedPanel.FindProperty("emptyMessageText").objectReferenceValue != null, "Inventory validation failed: panel emptyMessageText is missing.");
        Require(serializedPanel.FindProperty("slotViews").arraySize == SlotCount, "Inventory validation failed: panel slotViews count is wrong.");
    }

    private static void ValidateInputReference(InventoryInputController inputController)
    {
        SerializedObject serializedInput = new SerializedObject(inputController);
        Require(serializedInput.FindProperty("inventoryPanel").objectReferenceValue != null, "Inventory validation failed: input inventoryPanel is missing.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
