// Script purpose: Builds a dark three-stage dungeon greybox in the open scene.
// Key variables:
// - StageBlockoutName: Root object rebuilt by this Editor tool.
// - BlockSpriteAssetPath: Visible project sprite used by SpriteRenderer blocks.
// - Tile sprite paths: Project-owned Art/Tiles sprites used to make the greybox look like a dungeon.
// - Room positions: Fixed demo route positions for Early, Mid, and Boss rooms.
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageBlockoutBuilder
{
    private const string StageBlockoutName = "StageBlockout";
    private const string BlockSpriteAssetPath = "Assets/Art/Generated/Environment/blockout_square.png";
    private const string BuiltInBlockSpritePath = "UI/Skin/UISprite.psd";
    private const string WallTileSpritePath = "Assets/Art/Tiles/wall_1.png";
    private const string PlatformTileSpritePath = "Assets/Art/Tiles/platform_1.png";

    private static Sprite blockSprite;
    private static Sprite wallTileSprite;
    private static Sprite platformTileSprite;

    [MenuItem("Tools/Stage Blockout/Create Three Stage Dungeon Demo")]
    public static void CreateThreeStageDungeonDemo()
    {
        if (!ConfirmRebuild())
        {
            return;
        }

        RemoveExistingStageBlockout();

        GameObject root = CreateGroup(StageBlockoutName, null);
        Transform rootTransform = root.transform;

        CreateBackground(rootTransform);
        CreateEarlyRoom(rootTransform);
        CreateMidRoom(rootTransform);
        CreateBossRoom(rootTransform);
        CreateBoundaries(rootTransform);
        CreateSharedMarkers(rootTransform);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("StageBlockoutBuilder: Three-stage dungeon demo created. Manually place Player, enemies, exits, and room controllers on the marker points.");
        Debug.Log("StageBlockoutBuilder: Add Player, Ground, and Hittable layers manually. Set generated ground, platforms, and boundaries to Ground.");
    }

    public static void RebuildThreeStageDungeonDemoForAutomation()
    {
        RemoveExistingStageBlockout();

        GameObject root = CreateGroup(StageBlockoutName, null);
        Transform rootTransform = root.transform;

        CreateBackground(rootTransform);
        CreateEarlyRoom(rootTransform);
        CreateMidRoom(rootTransform);
        CreateBossRoom(rootTransform);
        CreateBoundaries(rootTransform);
        CreateSharedMarkers(rootTransform);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("StageBlockoutBuilder: Three-stage dungeon demo rebuilt by scene setup automation.");
    }

    private static bool ConfirmRebuild()
    {
        GameObject existingStageBlockout = GameObject.Find(StageBlockoutName);

        if (existingStageBlockout == null)
        {
            return true;
        }

        return EditorUtility.DisplayDialog(
            "Rebuild StageBlockout",
            "A StageBlockout object already exists. Rebuild only StageBlockout and keep Player, Camera, enemies, doors, and other scene objects?",
            "Rebuild StageBlockout",
            "Cancel");
    }

    private static void RemoveExistingStageBlockout()
    {
        GameObject existingStageBlockout = GameObject.Find(StageBlockoutName);

        if (existingStageBlockout != null)
        {
            Object.DestroyImmediate(existingStageBlockout);
        }
    }

    private static void CreateBackground(Transform rootTransform)
    {
        Transform background = CreateGroup("Background", rootTransform).transform;
        CreateBlock("BackWall", background, new Vector2(18f, 1.2f), new Vector2(112f, 14f), new Color(0.07f, 0.075f, 0.09f, 1f), false, -50);
        CreateBlock("DistantUpperShadow", background, new Vector2(18f, 3.6f), new Vector2(112f, 2.8f), new Color(0.035f, 0.038f, 0.05f, 1f), false, -49);
        CreateBlock("DungeonWallPanel", background, new Vector2(18f, 0.7f), new Vector2(96f, 9.5f), new Color(0.12f, 0.12f, 0.13f, 1f), false, -48);
        CreateBlock("SoftDepthPanel", background, new Vector2(18f, 1.2f), new Vector2(78f, 6.0f), new Color(0.16f, 0.16f, 0.18f, 0.55f), false, -47);
        CreateBlock("SubtleWarningLine", background, new Vector2(18f, -2.58f), new Vector2(104f, 0.12f), new Color(0.30f, 0.03f, 0.05f, 0.55f), false, -46);
        CreateBlock("BossRoomBackPlate", background, new Vector2(50f, 1.1f), new Vector2(27f, 8.0f), new Color(0.055f, 0.045f, 0.065f, 0.85f), false, -45);
        CreateBackgroundAccent("EarlyWallAccent", background, new Vector2(-18f, 1.4f), new Vector2(18f, 0.12f));
        CreateBackgroundAccent("MidWallAccent", background, new Vector2(12f, 1.0f), new Vector2(20f, 0.12f));
        CreateBackgroundAccent("BossWallAccent", background, new Vector2(44f, 0.8f), new Vector2(22f, 0.12f));
    }

    private static void CreateEarlyRoom(Transform rootTransform)
    {
        Transform room = CreateGroup("EarlyRoom", rootTransform).transform;
        Transform ground = CreateGroup("Ground", room).transform;

        CreateGroundBlock("EarlyGround", ground, new Vector2(-18f, -3f), new Vector2(26f, 1.4f), "Early safe route and first weak enemy space.");
        CreateGroundBlock("EarlyStep", ground, new Vector2(-8f, -1.7f), new Vector2(5f, 0.8f), "Small jump check before the mid room.");

        CreateMarker("PlayerSpawnPoint", room, new Vector2(-28f, -1.6f), LevelBlockoutMarker.MarkerType.PlayerSpawn, "Move Player here before Play.");
        CreateMarker("EnemySpawnPoint_Early_01", room, new Vector2(-13f, -1.6f), LevelBlockoutMarker.MarkerType.EarlyEnemy, "Place one weak BasicEnemy here.");
        CreateMarker("EnemySpawnPoint_Early_02", room, new Vector2(-8f, -0.5f), LevelBlockoutMarker.MarkerType.EarlyEnemy, "Optional second weak BasicEnemy.");
        CreateMarker("EarlyExitDoorPoint", room, new Vector2(-3f, -1.6f), LevelBlockoutMarker.MarkerType.EarlyExitDoor, "Place Early ExitDoor here.");
    }

    private static void CreateMidRoom(Transform rootTransform)
    {
        Transform room = CreateGroup("MidRoom", rootTransform).transform;
        Transform ground = CreateGroup("Ground", room).transform;
        Transform platforms = CreateGroup("Platforms", room).transform;

        CreateGroundBlock("MidGround", ground, new Vector2(12f, -3f), new Vector2(26f, 1.4f), "Mid room floor for upgraded combat.");
        CreateGroundBlock("MidPlatform_Left", platforms, new Vector2(5f, -0.6f), new Vector2(7f, 0.7f), "Optional enemy or jump platform.");
        CreateGroundBlock("MidPlatform_Right", platforms, new Vector2(18f, 0.4f), new Vector2(8f, 0.7f), "Higher route for spacing checks.");

        CreateMarker("EnemySpawnPoint_Mid_01", room, new Vector2(6f, -1.6f), LevelBlockoutMarker.MarkerType.MidEnemy, "Place stronger BasicEnemy here.");
        CreateMarker("EnemySpawnPoint_Mid_02", room, new Vector2(13f, -1.6f), LevelBlockoutMarker.MarkerType.MidEnemy, "Place stronger BasicEnemy here.");
        CreateMarker("EnemySpawnPoint_Mid_03", room, new Vector2(20f, 1.2f), LevelBlockoutMarker.MarkerType.MidEnemy, "Optional platform enemy.");
        CreateMarker("MidExitDoorPoint", room, new Vector2(25f, -1.6f), LevelBlockoutMarker.MarkerType.MidExitDoor, "Place Mid ExitDoor here.");
    }

    private static void CreateBossRoom(Transform rootTransform)
    {
        Transform room = CreateGroup("BossRoom", rootTransform).transform;
        Transform ground = CreateGroup("Ground", room).transform;

        CreateGroundBlock("BossGround", ground, new Vector2(44f, -3f), new Vector2(32f, 1.4f), "Long floor for the demo boss chase.");
        CreateGroundBlock("BossLeftLedge", ground, new Vector2(35f, -0.8f), new Vector2(6f, 0.7f), "Short ledge for camera framing.");
        CreateGroundBlock("BossRightLedge", ground, new Vector2(52f, -0.8f), new Vector2(6f, 0.7f), "Short ledge for camera framing.");

        CreateMarker("BossSpawnPoint", room, new Vector2(45f, -1.6f), LevelBlockoutMarker.MarkerType.DemoBoss, "Use BasicEnemyController with higher health, or Bringer Of Death visual as child.");
        CreateMarker("BossExitDoorPoint", room, new Vector2(58f, -1.6f), LevelBlockoutMarker.MarkerType.BossExitDoor, "Place final ExitDoor here.");
    }

    private static void CreateBoundaries(Transform rootTransform)
    {
        Transform boundaries = CreateGroup("Boundaries", rootTransform).transform;
        CreateBoundary("DungeonLeftWall", boundaries, new Vector2(-32f, 0f), new Vector2(1f, 11f), "Left demo route wall.");
        CreateBoundary("DungeonRightWall", boundaries, new Vector2(61f, 0f), new Vector2(1f, 11f), "Right demo route wall.");
        CreateBoundary("DungeonCeilingLimit", boundaries, new Vector2(14.5f, 4.3f), new Vector2(94f, 1f), "Prevents camera and jump tests from leaving the room vertically.");
        CreateBlock("EarlyMidDivider", boundaries, new Vector2(-1f, 0f), new Vector2(0.20f, 8.5f), new Color(0.10f, 0.04f, 0.05f, 0.55f), false, -20);
        CreateBlock("MidBossDivider", boundaries, new Vector2(29f, 0f), new Vector2(0.20f, 8.5f), new Color(0.10f, 0.04f, 0.05f, 0.55f), false, -20);
    }

    private static void CreateSharedMarkers(Transform rootTransform)
    {
        Transform markers = CreateGroup("Markers", rootTransform).transform;
        CreateMarker("CameraStartPoint", markers, new Vector2(-22f, 0f), LevelBlockoutMarker.MarkerType.CameraStart, "Place Main Camera near this point and keep CameraFollow target on Player.");
        CreateMarker("DemoStageControllerPoint", markers, new Vector2(0f, 2.9f), LevelBlockoutMarker.MarkerType.RoomController, "Create DemoStageController and three RoomClearControllers in the scene.");
    }

    private static void CreateGroundBlock(string objectName, Transform parent, Vector2 position, Vector2 size, string purpose)
    {
        GameObject block = CreateBlock(objectName, parent, position, size, new Color(0.08f, 0.075f, 0.075f, 1f), true, -8);
        CreateGroundTileVisuals(block.transform, size);

        SimplePlatformLabel platformLabel = block.AddComponent<SimplePlatformLabel>();
        platformLabel.platformName = objectName;
        platformLabel.gameplayPurpose = purpose;
    }

    private static void CreateBoundary(string objectName, Transform parent, Vector2 position, Vector2 size, string boundaryName)
    {
        GameObject boundary = CreateBlock(objectName, parent, position, size, new Color(0.05f, 0.04f, 0.06f, 1f), true, -4);
        RoomBoundary roomBoundary = boundary.AddComponent<RoomBoundary>();
        roomBoundary.boundaryName = boundaryName;
        roomBoundary.blocksPlayer = true;
    }

    private static void CreateBackgroundAccent(string objectName, Transform parent, Vector2 position, Vector2 size)
    {
        CreateBlock(objectName, parent, position, size, new Color(0.34f, 0.29f, 0.22f, 0.45f), false, -44);
    }

    private static void CreateGroundTileVisuals(Transform groundTransform, Vector2 size)
    {
        Transform visualRoot = CreateGroup("TileVisuals", groundTransform).transform;
        Sprite wallSprite = GetWallTileSprite();
        Sprite platformSprite = GetPlatformTileSprite();

        if (wallSprite != null)
        {
            CreateTiledVisual("WallFill", visualRoot, wallSprite, new Vector2(0f, -0.12f), new Vector2(size.x, Mathf.Max(0.25f, size.y - 0.22f)), new Color(0.54f, 0.52f, 0.46f, 1f), 4);
        }

        if (platformSprite != null)
        {
            CreateTiledVisual("TopEdge", visualRoot, platformSprite, new Vector2(0f, size.y * 0.5f - 0.25f), new Vector2(size.x, 0.5f), new Color(0.96f, 0.89f, 0.73f, 1f), 12);
        }
    }

    private static void CreateTiledVisual(string objectName, Transform parent, Sprite sprite, Vector2 localPosition, Vector2 size, Color color, int sortingOrder)
    {
        GameObject visual = new GameObject(objectName);
        visual.transform.SetParent(parent, false);
        visual.transform.localPosition = localPosition;

        SpriteRenderer spriteRenderer = visual.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.size = size;
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    private static GameObject CreateBlock(string objectName, Transform parent, Vector2 position, Vector2 size, Color color, bool addCollider, int sortingOrder)
    {
        GameObject block = new GameObject(objectName);
        block.transform.SetParent(parent, false);
        block.transform.position = position;

        SpriteRenderer spriteRenderer = block.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetBlockSprite();
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        spriteRenderer.size = size;

        if (addCollider)
        {
            BoxCollider2D boxCollider = block.AddComponent<BoxCollider2D>();
            boxCollider.size = size;
        }

        return block;
    }

    private static void CreateMarker(string objectName, Transform parent, Vector2 position, LevelBlockoutMarker.MarkerType markerType, string note)
    {
        GameObject marker = CreateGroup(objectName, parent);
        marker.transform.position = position;

        LevelBlockoutMarker levelBlockoutMarker = marker.AddComponent<LevelBlockoutMarker>();
        levelBlockoutMarker.markerType = markerType;
        levelBlockoutMarker.note = note;
    }

    private static GameObject CreateGroup(string objectName, Transform parent)
    {
        GameObject group = new GameObject(objectName);

        if (parent != null)
        {
            group.transform.SetParent(parent, false);
        }

        return group;
    }

    private static Sprite GetBlockSprite()
    {
        if (blockSprite != null)
        {
            return blockSprite;
        }

        ConfigureBlockSpriteImporter();
        blockSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BlockSpriteAssetPath);

        if (blockSprite != null)
        {
            return blockSprite;
        }

        blockSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(BuiltInBlockSpritePath);

        if (blockSprite == null)
        {
            Debug.LogError("StageBlockoutBuilder: Built-in UI sprite was not found. Blocks will need a Sprite assigned manually.");
        }

        return blockSprite;
    }

    private static Sprite GetWallTileSprite()
    {
        if (wallTileSprite == null)
        {
            ConfigureTileSpriteImporter(WallTileSpritePath);
            wallTileSprite = AssetDatabase.LoadAssetAtPath<Sprite>(WallTileSpritePath);
        }

        return wallTileSprite;
    }

    private static Sprite GetPlatformTileSprite()
    {
        if (platformTileSprite == null)
        {
            ConfigureTileSpriteImporter(PlatformTileSpritePath);
            platformTileSprite = AssetDatabase.LoadAssetAtPath<Sprite>(PlatformTileSpritePath);
        }

        return platformTileSprite;
    }

    private static void ConfigureTileSpriteImporter(string spritePath)
    {
        TextureImporter importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;

        if (importer == null)
        {
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = 32;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;

        TextureImporterSettings importerSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(importerSettings);
        importerSettings.spriteMeshType = SpriteMeshType.FullRect;
        importer.SetTextureSettings(importerSettings);

        importer.SaveAndReimport();
    }

    private static void ConfigureBlockSpriteImporter()
    {
        TextureImporter importer = AssetImporter.GetAtPath(BlockSpriteAssetPath) as TextureImporter;

        if (importer == null)
        {
            return;
        }

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = 8;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;

        TextureImporterSettings importerSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(importerSettings);
        importerSettings.spriteMeshType = SpriteMeshType.FullRect;
        importer.SetTextureSettings(importerSettings);

        importer.SaveAndReimport();
    }

    private static Color GetMarkerColor(LevelBlockoutMarker.MarkerType markerType)
    {
        if (markerType == LevelBlockoutMarker.MarkerType.PlayerSpawn)
        {
            return new Color(0.18f, 0.72f, 0.32f, 1f);
        }

        if (markerType == LevelBlockoutMarker.MarkerType.DemoBoss)
        {
            return new Color(0.85f, 0.08f, 0.08f, 1f);
        }

        if (markerType == LevelBlockoutMarker.MarkerType.EarlyExitDoor
            || markerType == LevelBlockoutMarker.MarkerType.MidExitDoor
            || markerType == LevelBlockoutMarker.MarkerType.BossExitDoor)
        {
            return new Color(0.95f, 0.72f, 0.12f, 1f);
        }

        return new Color(0.62f, 0.18f, 0.85f, 1f);
    }
}
