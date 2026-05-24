// Script purpose: Builds the bright adventure demo map with Kenney CC0 food-platformer tiles.
// Key variables:
// - KenneyTileRoot: Imported Kenney tile folder used by this scene builder.
// - Map root: Scene object that keeps Ground and Background map objects together.
// - Tile unit: One Kenney tile is imported as one Unity world unit.
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class KenneyAdventureMapBuilder
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";
    private const string MapRootName = "Map";
    private const string StageBlockoutName = "StageBlockout";
    private const string KenneyTileRoot = "Assets/Art/Kenney/PixelPlatformerFoodExpansion/Tiles";
    private const string BlockSpriteAssetPath = "Assets/Art/Generated/Environment/blockout_square.png";
    private const string BuiltInBlockSpritePath = "UI/Skin/UISprite.psd";
    private const int TilePixelsPerUnit = 16;

    private static Sprite blockSprite;

    [MenuItem("Tools/Map/Build Kenney Adventure Map")]
    public static void BuildKenneyAdventureMap()
    {
        EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        ConfigureKenneyTileImporters();

        RemoveObject(MapRootName);
        RemoveObject(StageBlockoutName);

        GameObject mapRoot = new GameObject(MapRootName);
        Transform background = CreateGroup("Background", mapRoot.transform).transform;
        Transform ground = CreateGroup("Ground", mapRoot.transform).transform;

        BuildBackground(background);
        BuildGround(ground);
        TuneCameraForBrightMap();

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        Debug.Log("KenneyAdventureMapBuilder: SampleScene map rebuilt with Kenney adventure tiles.");
    }

    private static void BuildBackground(Transform parent)
    {
        CreateColoredBlock("SkyGradientBase", parent, new Vector2(14f, 1.5f), new Vector2(100f, 16f), new Color(0.50f, 0.78f, 0.95f, 1f), -70);
        CreateColoredBlock("SoftCloudBand", parent, new Vector2(12f, 2.7f), new Vector2(92f, 2.1f), new Color(0.88f, 0.95f, 1f, 0.75f), -69);
        CreateColoredBlock("DistantGreenHill", parent, new Vector2(5f, -1.0f), new Vector2(44f, 3.2f), new Color(0.43f, 0.75f, 0.36f, 0.85f), -68);
        CreateColoredBlock("DistantYellowHill", parent, new Vector2(38f, -1.2f), new Vector2(48f, 2.6f), new Color(0.84f, 0.73f, 0.35f, 0.72f), -68);
        CreateColoredBlock("TreeCanopyLeft", parent, new Vector2(-18f, 3.6f), new Vector2(28f, 1.5f), new Color(0.20f, 0.54f, 0.28f, 0.8f), -67);
        CreateColoredBlock("TreeCanopyRight", parent, new Vector2(32f, 3.7f), new Vector2(36f, 1.3f), new Color(0.18f, 0.48f, 0.24f, 0.72f), -67);

        AddDecoration(parent, "LollipopTreeLeft", "tile_0008.png", new Vector2(-25f, -0.6f), new Vector2(2.2f, 2.2f), -55);
        AddDecoration(parent, "LollipopTreeMid", "tile_0009.png", new Vector2(3f, 0.1f), new Vector2(2.0f, 2.0f), -55);
        AddDecoration(parent, "CupCakeHouse", "tile_0109.png", new Vector2(27f, -0.5f), new Vector2(1.6f, 1.6f), -54);
        AddDecoration(parent, "FloatingBurger", "tile_0093.png", new Vector2(17f, 1.6f), new Vector2(1.4f, 1.4f), -54);
        AddDecoration(parent, "FloatingCandy", "tile_0060.png", new Vector2(45f, 1.2f), new Vector2(1.5f, 1.5f), -54);
    }

    private static void BuildGround(Transform parent)
    {
        Transform colliders = CreateGroup("GroundColliders", parent).transform;
        Transform terrain = CreateGroup("TerrainTiles", parent).transform;
        Transform platforms = CreateGroup("PlatformTiles", parent).transform;
        Transform decorations = CreateGroup("Decorations", parent).transform;

        CreateGroundCollider(colliders, "MainGroundCollider", new Vector2(14f, -3.0f), new Vector2(92f, 2f));
        CreateGroundCollider(colliders, "LeftBoundaryCollider", new Vector2(-32.5f, 0f), new Vector2(1f, 12f));
        CreateGroundCollider(colliders, "RightBoundaryCollider", new Vector2(60.5f, 0f), new Vector2(1f, 12f));
        CreateGroundCollider(colliders, "CeilingBoundaryCollider", new Vector2(14f, 5.2f), new Vector2(94f, 1f));

        BuildTerrainStrip(terrain, -32, 60, -2.5f);
        BuildPlatform(platforms, -25, -17, 0);
        BuildPlatform(platforms, -8, 2, -0.2f);
        BuildPlatform(platforms, 8, 18, 0.6f);
        BuildPlatform(platforms, 31, 39, 0.0f);
        BuildPlatform(platforms, 47, 56, 0.7f);

        CreatePlatformCollider(colliders, "PlatformCollider_LeftTraining", new Vector2(-21f, 0f), new Vector2(9f, 0.45f));
        CreatePlatformCollider(colliders, "PlatformCollider_EarlyExit", new Vector2(-3f, -0.2f), new Vector2(11f, 0.45f));
        CreatePlatformCollider(colliders, "PlatformCollider_MidRoom", new Vector2(13f, 0.6f), new Vector2(11f, 0.45f));
        CreatePlatformCollider(colliders, "PlatformCollider_BossApproach", new Vector2(35f, 0f), new Vector2(9f, 0.45f));
        CreatePlatformCollider(colliders, "PlatformCollider_BossRoom", new Vector2(51.5f, 0.7f), new Vector2(10f, 0.45f));

        AddDecoration(decorations, "SushiMarker_Early", "tile_0104.png", new Vector2(-7f, -1.35f), Vector2.one, 18);
        AddDecoration(decorations, "CandyPillar_Mid", "tile_0059.png", new Vector2(22f, -1.2f), new Vector2(1.3f, 1.3f), 18);
        AddDecoration(decorations, "CakeReward_Boss", "tile_0110.png", new Vector2(56f, -1.25f), new Vector2(1.25f, 1.25f), 18);
        AddDecoration(decorations, "CheeseHazardVisual_A", "tile_0105.png", new Vector2(5f, -1.25f), Vector2.one, 18);
        AddDecoration(decorations, "CheeseHazardVisual_B", "tile_0106.png", new Vector2(40f, -1.25f), Vector2.one, 18);
    }

    private static void BuildTerrainStrip(Transform parent, int startX, int endX, float topY)
    {
        for (int x = startX; x <= endX; x++)
        {
            string topTileName = x == startX ? "tile_0000.png" : x == endX ? "tile_0003.png" : x % 4 == 0 ? "tile_0002.png" : "tile_0001.png";
            AddTile(parent, $"MainGround_Top_{x}", topTileName, new Vector2(x, topY), 0);

            for (int fillRow = 1; fillRow <= 2; fillRow++)
            {
                string fillTileName = (x + fillRow) % 3 == 0 ? "tile_0034.png" : "tile_0033.png";
                AddTile(parent, $"MainGround_Fill_{x}_{fillRow}", fillTileName, new Vector2(x, topY - fillRow), -1);
            }
        }
    }

    private static void BuildPlatform(Transform parent, int startX, int endX, float y)
    {
        for (int x = startX; x <= endX; x++)
        {
            string tileName = x == startX ? "tile_0016.png" : x == endX ? "tile_0019.png" : x % 3 == 0 ? "tile_0018.png" : "tile_0017.png";
            AddTile(parent, $"Platform_{startX}_{endX}_{x}", tileName, new Vector2(x, y), 8);
        }
    }

    private static void AddTile(Transform parent, string objectName, string tileFileName, Vector2 position, int sortingOrder)
    {
        Sprite tileSprite = LoadTileSprite(tileFileName);

        GameObject tileObject = new GameObject(objectName);
        tileObject.transform.SetParent(parent, false);
        tileObject.transform.position = new Vector3(position.x, position.y, 0f);

        SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = tileSprite;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    private static void AddDecoration(Transform parent, string objectName, string tileFileName, Vector2 position, Vector2 scale, int sortingOrder)
    {
        Sprite tileSprite = LoadTileSprite(tileFileName);

        GameObject decoration = new GameObject(objectName);
        decoration.transform.SetParent(parent, false);
        decoration.transform.position = new Vector3(position.x, position.y, 0f);
        decoration.transform.localScale = new Vector3(scale.x, scale.y, 1f);

        SpriteRenderer spriteRenderer = decoration.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = tileSprite;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    private static void CreateGroundCollider(Transform parent, string objectName, Vector2 position, Vector2 size)
    {
        GameObject colliderObject = new GameObject(objectName);
        colliderObject.transform.SetParent(parent, false);
        colliderObject.transform.position = new Vector3(position.x, position.y, 0f);
        colliderObject.layer = RequireLayer("Ground");

        BoxCollider2D boxCollider = colliderObject.AddComponent<BoxCollider2D>();
        boxCollider.size = size;
    }

    private static void CreatePlatformCollider(Transform parent, string objectName, Vector2 tileCenterPosition, Vector2 size)
    {
        Vector2 colliderPosition = new Vector2(tileCenterPosition.x, tileCenterPosition.y + 0.275f);
        CreateGroundCollider(parent, objectName, colliderPosition, size);
    }

    private static void CreateColoredBlock(string objectName, Transform parent, Vector2 position, Vector2 size, Color color, int sortingOrder)
    {
        GameObject block = new GameObject(objectName);
        block.transform.SetParent(parent, false);
        block.transform.position = new Vector3(position.x, position.y, 0f);
        block.transform.localScale = new Vector3(size.x, size.y, 1f);

        SpriteRenderer spriteRenderer = block.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetBlockSprite();
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    private static Sprite LoadTileSprite(string tileFileName)
    {
        string assetPath = $"{KenneyTileRoot}/{tileFileName}";
        Sprite tileSprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

        if (tileSprite == null)
        {
            throw new InvalidOperationException($"KenneyAdventureMapBuilder: missing tile sprite {assetPath}");
        }

        return tileSprite;
    }

    private static void ConfigureKenneyTileImporters()
    {
        string[] tileGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { KenneyTileRoot });

        foreach (string tileGuid in tileGuids)
        {
            string tilePath = AssetDatabase.GUIDToAssetPath(tileGuid);
            TextureImporter importer = AssetImporter.GetAtPath(tilePath) as TextureImporter;

            if (importer == null)
            {
                continue;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = TilePixelsPerUnit;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }
    }

    private static Sprite GetBlockSprite()
    {
        if (blockSprite != null)
        {
            return blockSprite;
        }

        blockSprite = AssetDatabase.LoadAssetAtPath<Sprite>(BlockSpriteAssetPath);

        if (blockSprite == null)
        {
            blockSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(BuiltInBlockSpritePath);
        }

        if (blockSprite == null)
        {
            throw new InvalidOperationException("KenneyAdventureMapBuilder: block sprite was not found.");
        }

        return blockSprite;
    }

    private static GameObject CreateGroup(string objectName, Transform parent)
    {
        GameObject groupObject = new GameObject(objectName);
        groupObject.transform.SetParent(parent, false);
        return groupObject;
    }

    private static void RemoveObject(string objectName)
    {
        GameObject existingObject = GameObject.Find(objectName);

        while (existingObject != null)
        {
            UnityEngine.Object.DestroyImmediate(existingObject);
            existingObject = GameObject.Find(objectName);
        }
    }

    private static int RequireLayer(string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);

        if (layerIndex < 0)
        {
            throw new InvalidOperationException($"KenneyAdventureMapBuilder: Layer '{layerName}' is missing.");
        }

        return layerIndex;
    }

    private static void TuneCameraForBrightMap()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            return;
        }

        mainCamera.backgroundColor = new Color(0.50f, 0.78f, 0.95f, 1f);
        mainCamera.orthographicSize = 5.2f;
        EditorUtility.SetDirty(mainCamera);
    }
}
