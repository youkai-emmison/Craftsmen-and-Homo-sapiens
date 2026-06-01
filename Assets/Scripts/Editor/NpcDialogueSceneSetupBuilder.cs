// Script purpose: Builds the small demo NPC dialogue setup in SampleScene from the Unity editor.
// Key generated objects:
// - OpeningGuideDialogue / MidRoomWarningDialogue: ScriptableObject dialogue data.
// - DialoguePanel: Bottom popup UI under InventoryCanvas.
// - NPC_ArchivistGuide / NPC_FieldTechnician: Demo NPC trigger objects with animated sprite frames.
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class NpcDialogueSceneSetupBuilder
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";
    private const string DialogueAssetFolder = "Assets/Dialogue";
    private const string OpeningDialoguePath = DialogueAssetFolder + "/OpeningGuideDialogue.asset";
    private const string MidDialoguePath = DialogueAssetFolder + "/MidRoomWarningDialogue.asset";
    private const string BlockoutSpritePath = "Assets/Art/Generated/Environment/blockout_square.png";
    private const string PanelSpritePath = "Assets/Art/Kenney/FantasyUIBorders/PNG/Default/Panel/panel-001.png";
    private const string ArchivistFramePrefix = "Assets/Art/Generated/NPC/archivist_guide/archivist_guide_idle_";
    private const string TechnicianFramePrefix = "Assets/Art/Generated/NPC/field_technician/field_technician_idle_";

    [MenuItem("Tools/Dialogue/Create Demo NPC Dialogue Setup")]
    public static void CreateDemoNpcDialogueSetup()
    {
        OpenDemoScene();
        EnsureDialogueAssetFolder();

        DialogueSequence openingDialogue = CreateOrUpdateSequence(
            OpeningDialoguePath,
            "Opening Guide",
            new[]
            {
                CreateLine("Archivist", "Hey, you're finally here. Welcome to the dungeon shift. Not fancy, but hey, it keeps things interesting."),
                CreateLine("Archivist", "A / D to move, Space to jump, J or Left Mouse to hit. Easy enough, yeah?"),
                CreateLine("Archivist", "B opens the backpack, N opens crafting. Go right when you're ready, and uh... don't hug every weird monster.")
            });

        DialogueSequence midRoomDialogue = CreateOrUpdateSequence(
            MidDialoguePath,
            "Mid Room Warning",
            new[]
            {
                CreateLine("Field Tech", "Yo, quick heads-up. The next room hits harder, so don't just mash buttons and pray."),
                CreateLine("Field Tech", "That blue skill-energy bar comes back on its own. Spend it when it matters, then chill for a sec."),
                CreateLine("Field Tech", "If a door is locked, clear the room first. Boss room is ahead, so uh... breathe before you swing.")
            });

        DialoguePanelController dialoguePanel = CreateOrUpdateDialoguePanel();
        CreateOrUpdateNpc("NPC_ArchivistGuide", new Vector3(-12.75f, -1.05f, 0f), Color.white, openingDialogue, dialoguePanel, LoadFrameSprites(ArchivistFramePrefix));
        CreateOrUpdateNpc("NPC_FieldTechnician", new Vector3(26.5f, -1.25f, 0f), Color.white, midRoomDialogue, dialoguePanel, LoadFrameSprites(TechnicianFramePrefix));

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("NpcDialogueSceneSetupBuilder: Demo NPC dialogue setup created. Press Play, move near an NPC, then press E.");
    }

    private static void OpenDemoScene()
    {
        if (SceneManager.GetActiveScene().path == ScenePath)
        {
            return;
        }

        EditorSceneManager.OpenScene(ScenePath);
    }

    private static void EnsureDialogueAssetFolder()
    {
        if (AssetDatabase.IsValidFolder(DialogueAssetFolder))
        {
            return;
        }

        AssetDatabase.CreateFolder("Assets", "Dialogue");
    }

    private static DialogueLine CreateLine(string speakerName, string content)
    {
        return new DialogueLine
        {
            speakerName = speakerName,
            content = content
        };
    }

    private static DialogueSequence CreateOrUpdateSequence(string assetPath, string sequenceName, DialogueLine[] lines)
    {
        DialogueSequence dialogueSequence = AssetDatabase.LoadAssetAtPath<DialogueSequence>(assetPath);
        if (dialogueSequence == null)
        {
            dialogueSequence = ScriptableObject.CreateInstance<DialogueSequence>();
            AssetDatabase.CreateAsset(dialogueSequence, assetPath);
        }

        dialogueSequence.sequenceName = sequenceName;
        dialogueSequence.lines = lines;
        EditorUtility.SetDirty(dialogueSequence);
        return dialogueSequence;
    }

    private static DialoguePanelController CreateOrUpdateDialoguePanel()
    {
        GameObject inventoryCanvas = GameObject.Find("InventoryCanvas");
        if (inventoryCanvas == null)
        {
            Debug.LogError("NpcDialogueSceneSetupBuilder: InventoryCanvas was not found in SampleScene.");
            return null;
        }

        GameObject panelObject = FindOrCreateChild(inventoryCanvas.transform, "DialoguePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CanvasGroup), typeof(DialoguePanelController), typeof(DialogueInputController));
        panelObject.transform.SetAsLastSibling();
        ConfigureDialoguePanelRect(panelObject.GetComponent<RectTransform>());

        Image panelImage = panelObject.GetComponent<Image>();
        panelImage.color = new Color(0.08f, 0.07f, 0.11f, 0.92f);
        panelImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(PanelSpritePath);
        panelImage.type = Image.Type.Sliced;

        TextMeshProUGUI speakerNameText = CreateOrUpdateUiText(panelObject.transform, "SpeakerNameText", new Vector2(40f, -28f), new Vector2(420f, 44f), 28f, TextAlignmentOptions.Left, new Color(1f, 0.88f, 0.58f));
        TextMeshProUGUI contentText = CreateOrUpdateUiText(panelObject.transform, "ContentText", new Vector2(40f, -78f), new Vector2(1120f, 92f), 24f, TextAlignmentOptions.TopLeft, Color.white);
        TextMeshProUGUI continueHintText = CreateOrUpdateUiText(panelObject.transform, "ContinueHintText", new Vector2(-40f, 24f), new Vector2(540f, 34f), 18f, TextAlignmentOptions.Right, new Color(0.78f, 0.78f, 0.84f));

        ConfigureTextRect(speakerNameText.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
        ConfigureTextRect(contentText.rectTransform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
        ConfigureTextRect(continueHintText.rectTransform, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f));

        DialoguePanelController panelController = panelObject.GetComponent<DialoguePanelController>();
        panelController.canvasGroup = panelObject.GetComponent<CanvasGroup>();
        panelController.speakerNameText = speakerNameText;
        panelController.contentText = contentText;
        panelController.continueHintText = continueHintText;
        panelController.continueHint = "E / Space / Enter: Next     Esc: Close";

        DialogueInputController inputController = panelObject.GetComponent<DialogueInputController>();
        inputController.dialoguePanel = panelController;

        panelController.Close();
        return panelController;
    }

    private static void ConfigureDialoguePanelRect(RectTransform panelRect)
    {
        panelRect.anchorMin = new Vector2(0.5f, 0f);
        panelRect.anchorMax = new Vector2(0.5f, 0f);
        panelRect.pivot = new Vector2(0.5f, 0f);
        panelRect.sizeDelta = new Vector2(1200f, 220f);
        panelRect.anchoredPosition = new Vector2(0f, 48f);
        panelRect.localScale = Vector3.one;
    }

    private static TextMeshProUGUI CreateOrUpdateUiText(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, float fontSize, TextAlignmentOptions alignment, Color color)
    {
        GameObject textObject = FindOrCreateChild(parent, name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = color;
        text.enableWordWrapping = true;
        text.rectTransform.anchoredPosition = anchoredPosition;
        text.rectTransform.sizeDelta = size;
        return text;
    }

    private static void ConfigureTextRect(RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
    {
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;
        rectTransform.localScale = Vector3.one;
    }

    private static void CreateOrUpdateNpc(string objectName, Vector3 position, Color color, DialogueSequence dialogueSequence, DialoguePanelController dialoguePanel, Sprite[] animationFrames)
    {
        GameObject npcObject = GameObject.Find(objectName);
        if (npcObject == null)
        {
            npcObject = new GameObject(objectName);
        }

        npcObject.transform.position = position;
        npcObject.transform.localScale = Vector3.one;

        SpriteRenderer spriteRenderer = GetOrAdd<SpriteRenderer>(npcObject);
        spriteRenderer.sprite = animationFrames != null && animationFrames.Length > 0 ? animationFrames[0] : AssetDatabase.LoadAssetAtPath<Sprite>(BlockoutSpritePath);
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = 70;

        SimpleNpcSpriteAnimator npcAnimator = GetOrAdd<SimpleNpcSpriteAnimator>(npcObject);
        npcAnimator.targetRenderer = spriteRenderer;
        npcAnimator.frames = animationFrames;
        npcAnimator.framesPerSecond = 6f;

        BoxCollider2D triggerCollider = GetOrAdd<BoxCollider2D>(npcObject);
        triggerCollider.isTrigger = true;
        triggerCollider.size = new Vector2(4f, 2.8f);
        triggerCollider.offset = Vector2.zero;

        GameObject promptObject = CreateOrUpdatePrompt(npcObject.transform);

        NpcDialogueTrigger dialogueTrigger = GetOrAdd<NpcDialogueTrigger>(npcObject);
        dialogueTrigger.dialogueSequence = dialogueSequence;
        dialogueTrigger.dialoguePanel = dialoguePanel;
        dialogueTrigger.interactPrompt = promptObject;
        dialogueTrigger.closeWhenPlayerLeaves = true;
    }

    private static Sprite[] LoadFrameSprites(string framePrefix)
    {
        Sprite[] frames = new Sprite[8];
        for (int frameIndex = 0; frameIndex < frames.Length; frameIndex++)
        {
            frames[frameIndex] = AssetDatabase.LoadAssetAtPath<Sprite>($"{framePrefix}{frameIndex:00}.png");
        }

        return frames;
    }

    private static GameObject CreateOrUpdatePrompt(Transform npcTransform)
    {
        GameObject promptObject = FindOrCreateChild(npcTransform, "InteractPrompt", typeof(RectTransform), typeof(TextMeshPro));
        promptObject.transform.localPosition = new Vector3(0f, 1.45f, 0f);
        promptObject.transform.localRotation = Quaternion.identity;
        promptObject.transform.localScale = Vector3.one;

        TextMeshPro promptText = promptObject.GetComponent<TextMeshPro>();
        promptText.text = "E Talk";
        promptText.fontSize = 2.6f;
        promptText.alignment = TextAlignmentOptions.Center;
        promptText.color = Color.white;

        MeshRenderer promptRenderer = promptObject.GetComponent<MeshRenderer>();
        if (promptRenderer != null)
        {
            promptRenderer.sortingOrder = 120;
        }

        promptObject.SetActive(false);
        return promptObject;
    }

    private static GameObject FindOrCreateChild(Transform parent, string name, params System.Type[] componentTypes)
    {
        Transform existingChild = parent.Find(name);
        if (existingChild != null)
        {
            return existingChild.gameObject;
        }

        GameObject childObject = new GameObject(name, componentTypes);
        childObject.transform.SetParent(parent, false);
        return childObject;
    }

    private static T GetOrAdd<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        return gameObject.AddComponent<T>();
    }
}
