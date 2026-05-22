// Script purpose: Adds an Editor menu that generates a three-section side-scrolling greybox map.
// Key variables:
// - StageBlockoutName: Root object rebuilt by the tool.
// - BlockoutSpritePath: Project sprite used for simple greybox blocks.
// - BackdropSpritePath: Project backdrop sprite used for the room background.
// - blockoutSprite: Cached sprite shared by generated blocks.
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageBlockoutBuilder
{
    // Root name used to find and rebuild only the generated greybox.
    private const string StageBlockoutName = "StageBlockout";

    // Project square sprite path used by SpriteRenderer placeholders.
    private const string BlockoutSpritePath = "Assets/Art/Generated/Environment/blockout_square.png";

    // Project backdrop path used to make the blockout less empty.
    private const string BackdropSpritePath = "Assets/Art/Generated/Environment/anime_battle_room_backdrop.png";

    // Cached sprite assigned to every generated block.
    private static Sprite blockoutSprite;

    // Cached backdrop sprite assigned to the large room background.
    private static Sprite backdropSprite;

    [MenuItem("Tools/Stage Blockout/Create Anime Roguelite Blockout")]
    public static void CreateAnimeRogueliteBlockout()
    {
        if (!ConfirmReplaceExistingBlockout())
        {
            return;
        }

        blockoutSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BlockoutSpritePath);
        backdropSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BackdropSpritePath);

        if (blockoutSprite == null)
        {
            Debug.LogError($"StageBlockoutBuilder: Blockout sprite was not found at {BlockoutSpritePath}. Make sure it is imported as Sprite.");
            return;
        }

        if (backdropSprite == null)
        {
            Debug.LogError($"StageBlockoutBuilder: Backdrop sprite was not found at {BackdropSpritePath}. Make sure it is imported as Sprite.");
            return;
        }

        GameObject root = CreateRoot(StageBlockoutName);
        BuildBackground(root.transform);
        BuildGround(root.transform);
        BuildPlatforms(root.transform);
        BuildBoundaries(root.transform);
        BuildDecorations(root.transform);
        BuildMarkers(root.transform);
        LogManualSetupSteps();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private static bool ConfirmReplaceExistingBlockout()
    {
        GameObject existingBlockout = GameObject.Find(StageBlockoutName);

        if (existingBlockout == null)
        {
            return true;
        }

        bool shouldReplace = EditorUtility.DisplayDialog(
            "Rebuild Stage Blockout",
            "A StageBlockout object already exists. Rebuild it now? This only removes the old StageBlockout root and its children.",
            "Rebuild",
            "Cancel");

        if (!shouldReplace)
        {
            return false;
        }

        // Only rebuild the generated greybox root; gameplay objects stay untouched.
        Undo.DestroyObjectImmediate(existingBlockout);
        return true;
    }

    private static GameObject CreateRoot(string objectName)
    {
        GameObject root = new GameObject(objectName);
        Undo.RegisterCreatedObjectUndo(root, $"Create {objectName}");
        return root;
    }

    private static Transform CreateGroup(Transform parent, string groupName)
    {
        GameObject group = CreateRoot(groupName);
        group.transform.SetParent(parent);
        return group.transform;
    }

    private static void BuildBackground(Transform root)
    {
        Transform background = CreateGroup(root, "Background");

        CreateSpriteObject(background, "AnimeRoomBackdrop", backdropSprite, new Vector2(4f, 0.55f), new Vector2(5.9f, 1.55f), Color.white, -36, false);
        CreateSpriteBlock(background, "BackdropPanel", new Vector2(4f, 0.6f), new Vector2(60f, 8f), new Color(1f, 0.9f, 0.97f, 0.38f), -35, false);
        CreateSpriteBlock(background, "WindowPanel", new Vector2(4f, 1.2f), new Vector2(18f, 2.2f), new Color(0.82f, 0.76f, 1f, 0.62f), -29, false);
        CreateSpriteBlock(background, "FloorGlow", new Vector2(4f, -3.15f), new Vector2(58f, 1.1f), new Color(1f, 0.68f, 0.86f, 0.65f), -28, false);
    }

    private static void BuildGround(Transform root)
    {
        Transform ground = CreateGroup(root, "Ground");

        CreateLabeledGround(ground, "MainGround", new Vector2(4f, -3f), new Vector2(58f, 0.45f), new Color(0.45f, 0.35f, 0.58f), "Full safety floor for the blockout route.");
        CreateLabeledGround(ground, "SpawnGround", new Vector2(-14f, -2.75f), new Vector2(12f, 0.35f), new Color(0.92f, 0.82f, 0.93f), "Safe movement and camera follow test area.");
        CreateLabeledGround(ground, "TrainingGround", new Vector2(1f, -2.75f), new Vector2(18f, 0.35f), new Color(0.9f, 0.78f, 0.98f), "Jump and TrainingTarget attack practice area.");
        CreateLabeledGround(ground, "BattleGround", new Vector2(19f, -2.75f), new Vector2(18f, 0.35f), new Color(0.82f, 0.72f, 0.93f), "BasicEnemy and ExitDoor test area.");
    }

    private static void BuildPlatforms(Transform root)
    {
        Transform platforms = CreateGroup(root, "Platforms");

        CreateLabeledPlatform(platforms, "TrainingPlatformSmall", new Vector2(-2f, -1.4f), new Vector2(4f, 0.35f), new Color(0.98f, 0.78f, 0.92f), "Small jump target for the training area.");
        CreateLabeledPlatform(platforms, "BattlePlatformSmall", new Vector2(15f, -1.25f), new Vector2(3.5f, 0.35f), new Color(0.78f, 0.8f, 1f), "Small step for reading the battle room space.");
    }

    private static void BuildDecorations(Transform root)
    {
        Transform decorations = CreateGroup(root, "Decorations");

        CreateSpriteBlock(decorations, "SpawnAreaSoftPanel", new Vector2(-14f, -0.25f), new Vector2(10f, 3.5f), new Color(1f, 0.84f, 0.94f, 0.28f), -27, false);
        CreateSpriteBlock(decorations, "TrainingAreaSoftPanel", new Vector2(1f, -0.2f), new Vector2(15f, 3.6f), new Color(0.86f, 0.82f, 1f, 0.32f), -27, false);
        CreateSpriteBlock(decorations, "BattleAreaSoftPanel", new Vector2(19f, -0.2f), new Vector2(16f, 3.6f), new Color(1f, 0.78f, 0.88f, 0.26f), -27, false);
        CreateSpriteBlock(decorations, "FloorStripe", new Vector2(4f, -2.55f), new Vector2(56f, 0.08f), new Color(1f, 0.96f, 0.86f, 0.88f), -1, false);
        CreateSpriteBlock(decorations, "TrainingRibbon", new Vector2(-2f, -1.13f), new Vector2(4.3f, 0.08f), new Color(1f, 0.96f, 0.86f, 0.95f), -1, false);
        CreateSpriteBlock(decorations, "BattleRibbon", new Vector2(15f, -0.98f), new Vector2(3.8f, 0.08f), new Color(1f, 0.96f, 0.86f, 0.95f), -1, false);
    }

    private static void BuildBoundaries(Transform root)
    {
        Transform boundaries = CreateGroup(root, "Boundaries");

        CreateBoundary(boundaries, "LeftWall", new Vector2(-23f, 0.3f), new Vector2(0.5f, 8f), "Left boundary");
        CreateBoundary(boundaries, "RightWall", new Vector2(31f, 0.3f), new Vector2(0.5f, 8f), "Right boundary");
        CreateBoundary(boundaries, "CeilingLimit", new Vector2(4f, 4.5f), new Vector2(56f, 0.35f), "Ceiling limit");
    }

    private static void BuildMarkers(Transform root)
    {
        Transform markers = CreateGroup(root, "Markers");

        CreateMarker(markers, "PlayerSpawnPoint", LevelBlockoutMarker.MarkerType.PlayerSpawn, new Vector2(-18f, -1f), "Move Player here before pressing Play.");
        CreateMarker(markers, "TrainingTargetPoint", LevelBlockoutMarker.MarkerType.TrainingTarget, new Vector2(0f, -1f), "Place TrainingTarget here for attack range practice.");
        CreateMarker(markers, "BasicEnemyPoint", LevelBlockoutMarker.MarkerType.BasicEnemy, new Vector2(17f, -1f), "Place BasicEnemy here and set patrol points nearby.");
        CreateMarker(markers, "ExitDoorPoint", LevelBlockoutMarker.MarkerType.ExitDoor, new Vector2(28f, -1f), "Place ExitDoor here for room clear testing.");
        CreateMarker(markers, "CameraStartPoint", LevelBlockoutMarker.MarkerType.CameraStart, new Vector2(-18f, 0f), "Optional starting reference for Main Camera.");
    }

    private static void CreateLabeledGround(Transform parent, string objectName, Vector2 position, Vector2 size, Color color, string gameplayPurpose)
    {
        GameObject ground = CreateSpriteBlock(parent, objectName, position, size, color, -3, true);
        AddPlatformLabel(ground, objectName, gameplayPurpose);
    }

    private static void CreateLabeledPlatform(Transform parent, string objectName, Vector2 position, Vector2 size, Color color, string gameplayPurpose)
    {
        GameObject platform = CreateSpriteBlock(parent, objectName, position, size, color, -2, true);
        AddPlatformLabel(platform, objectName, gameplayPurpose);
    }

    private static void CreateBoundary(Transform parent, string objectName, Vector2 position, Vector2 size, string boundaryName)
    {
        GameObject boundary = CreateSpriteBlock(parent, objectName, position, size, new Color(0.22f, 0.16f, 0.3f), -1, true);
        RoomBoundary roomBoundary = boundary.AddComponent<RoomBoundary>();
        roomBoundary.boundaryName = boundaryName;
        roomBoundary.blocksPlayer = true;
    }

    private static GameObject CreateSpriteBlock(Transform parent, string objectName, Vector2 position, Vector2 size, Color color, int sortingOrder, bool addCollider)
    {
        return CreateSpriteObject(parent, objectName, blockoutSprite, position, size, color, sortingOrder, addCollider);
    }

    private static GameObject CreateSpriteObject(Transform parent, string objectName, Sprite sprite, Vector2 position, Vector2 size, Color color, int sortingOrder, bool addCollider)
    {
        GameObject block = CreateRoot(objectName);
        block.transform.SetParent(parent);
        block.transform.position = position;
        block.transform.localScale = new Vector3(size.x, size.y, 1f);

        SpriteRenderer spriteRenderer = block.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = sortingOrder;

        if (addCollider)
        {
            block.AddComponent<BoxCollider2D>();
        }

        return block;
    }

    private static void AddPlatformLabel(GameObject targetObject, string platformName, string gameplayPurpose)
    {
        SimplePlatformLabel platformLabel = targetObject.AddComponent<SimplePlatformLabel>();
        platformLabel.platformName = platformName;
        platformLabel.gameplayPurpose = gameplayPurpose;
    }

    private static void CreateMarker(Transform parent, string objectName, LevelBlockoutMarker.MarkerType markerType, Vector2 position, string note)
    {
        GameObject marker = CreateSpriteBlock(parent, objectName, position, new Vector2(0.45f, 0.45f), GetMarkerColor(markerType), 4, false);
        LevelBlockoutMarker levelBlockoutMarker = marker.AddComponent<LevelBlockoutMarker>();
        levelBlockoutMarker.markerType = markerType;
        levelBlockoutMarker.note = note;
    }

    private static Color GetMarkerColor(LevelBlockoutMarker.MarkerType markerType)
    {
        switch (markerType)
        {
            case LevelBlockoutMarker.MarkerType.PlayerSpawn:
                return Color.cyan;
            case LevelBlockoutMarker.MarkerType.TrainingTarget:
                return Color.magenta;
            case LevelBlockoutMarker.MarkerType.BasicEnemy:
                return new Color(0.45f, 0.2f, 0.7f);
            case LevelBlockoutMarker.MarkerType.ExitDoor:
                return Color.yellow;
            case LevelBlockoutMarker.MarkerType.CameraStart:
                return Color.white;
            default:
                return Color.gray;
        }
    }

    private static void LogManualSetupSteps()
    {
        Debug.Log("StageBlockoutBuilder: StageBlockout created. Set generated Ground, Platforms, and Boundaries objects to the Ground layer manually.");
        Debug.Log("StageBlockoutBuilder: Move Player to Markers/PlayerSpawnPoint and keep Player Tag = Player.");
        Debug.Log("StageBlockoutBuilder: Move TrainingTarget, BasicEnemy, and ExitDoor to their marker points. Do not duplicate existing gameplay objects.");
        Debug.Log("StageBlockoutBuilder: If BasicEnemy exists, set patrolLeftPoint near X 14 and patrolRightPoint near X 20, then wire RoomClearController and ExitDoorController manually.");
    }
}
