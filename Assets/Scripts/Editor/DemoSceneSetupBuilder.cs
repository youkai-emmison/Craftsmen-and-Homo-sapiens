// Script purpose: Rebuilds SampleScene into a playable three-stage demo scene.
// Key variables:
// - ScenePath: Scene file saved by this Editor setup tool.
// - ExperiencePickupPrefabPath: Local prefab used by enemy demo drops.
// - Layer names: Existing Unity layers assigned to generated gameplay objects.
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DemoSceneSetupBuilder
{
    private const string ScenePath = "Assets/Scenes/SampleScene.unity";
    private const string ExperiencePickupPrefabPath = "Assets/Prefabs/Demo/ExperiencePickup.prefab";
    private const string BlockSpriteAssetPath = "Assets/Art/Tiles/wall_1.png";
    private const string MihoPrefabPath = "Assets/FW_MIHO/prefabs/Miho.prefab";
    private const string ImportedBackdropPath = "Assets/Free 2D Cartoon Parallax Background/FullBG/3_Graveyard.png";
    private const string FallbackEnemySpriteAssetPath = "Assets/Art/Enemy/Little Evil/basic_enemy_placeholder.png";

    private static readonly string[] BasicEnemySpriteCandidatePaths =
    {
        "Assets/Enemy Galore 1 - Pixel Art/Sprites/Rat/Rat_Idle.png",
        "Assets/Enemy Galore 1 - Pixel Art/Sprites/Spiked Slime/Slime_Spiked_Idle.png",
        "Assets/Enemy Galore 1 - Pixel Art/Sprites/Crab/Crab_Idle.png"
    };

    private static readonly string[] BossSpriteCandidatePaths =
    {
        "Assets/Bringer Of Death/Sprite Sheet/Bringer-of-Death-SpritSheet.png",
        "Assets/Enemy Galore 1 - Pixel Art/Sprites/Reinforced Golem/Golem_Armor_Idle.png",
        "Assets/Enemy Galore 1 - Pixel Art/Sprites/Golem/Golem_IdleA.png"
    };

    private static Sprite blockSprite;
    private static Sprite enemySprite;
    private static Sprite bossSprite;

    [MenuItem("Tools/Demo Scene/Build Playable Three Stage Demo")]
    public static void BuildPlayableThreeStageDemo()
    {
        OpenSampleScene();
        CleanGeneratedObjects();
        StageBlockoutBuilder.RebuildThreeStageDungeonDemoForAutomation();
        AssignGeneratedStageLayers();
        CreateImportedBackdrop();

        GameObject experiencePickupPrefab = CreateExperiencePickupPrefab();
        Player player = CreatePlayer();
        CameraFollow cameraFollow = CreateMainCamera(player.transform);
        CreatePlayerManager(player);

        EnemyHealth earlyEnemy = CreateEnemy("EarlyEnemy_01", new Vector2(-13f, -1.45f), 2, 1, 3, 2.1f, 3.0f, player, experiencePickupPrefab);
        EnemyHealth midEnemyOne = CreateEnemy("MidEnemy_01", new Vector2(7f, -1.45f), 4, 1, 4, 2.4f, 3.2f, player, experiencePickupPrefab);
        EnemyHealth midEnemyTwo = CreateEnemy("MidEnemy_02", new Vector2(15f, -1.45f), 4, 1, 4, 2.4f, 3.2f, player, experiencePickupPrefab);
        EnemyHealth bossEnemy = CreateEnemy("DemoBoss", new Vector2(45f, -1.35f), 12, 2, 5, 1.7f, 2.5f, player, experiencePickupPrefab);
        bossEnemy.transform.localScale = new Vector3(1.35f, 1.35f, 1f);

        ExitDoorController earlyDoor = CreateExitDoor("EarlyExitDoor", new Vector2(-3f, -1.35f));
        ExitDoorController midDoor = CreateExitDoor("MidExitDoor", new Vector2(25f, -1.35f));
        ExitDoorController bossDoor = CreateExitDoor("BossExitDoor", new Vector2(58f, -1.35f));

        RoomClearController earlyRoom = CreateRoomController("EarlyRoomController", new[] { earlyEnemy }, earlyDoor);
        RoomClearController midRoom = CreateRoomController("MidRoomController", new[] { midEnemyOne, midEnemyTwo }, midDoor);
        RoomClearController bossRoom = CreateRoomController("BossRoomController", new[] { bossEnemy }, bossDoor);
        CreateDemoStageController(earlyRoom, midRoom, bossRoom, earlyDoor, midDoor, bossDoor);

        EditorUtility.SetDirty(cameraFollow);
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        Debug.Log("DemoSceneSetupBuilder: SampleScene is now a playable Early -> Mid -> Boss demo.");
    }

    public static void ValidatePlayableThreeStageDemo()
    {
        OpenSampleScene();

        RequireSingleObject("Player");
        RequireSingleObject("Main Camera");
        RequireSingleObject("PlayerManager");
        RequireSingleObject("EarlyEnemy_01");
        RequireSingleObject("MidEnemy_01");
        RequireSingleObject("MidEnemy_02");
        RequireSingleObject("DemoBoss");
        RequireSingleObject("EarlyExitDoor");
        RequireSingleObject("MidExitDoor");
        RequireSingleObject("BossExitDoor");
        RequireSingleObject("DemoStageController");

        RequireComponent<Player>("Player");
        RequireComponent<PlayerHealth>("Player");
        RequireComponent<PlayerDemoStats>("Player");
        RequireComponent<PlayerAttackController>("Player");
        RequireComponent<MeleeHitDetector>("Player");
        RequireComponent<PlayerAttackFacingController>("Player");

        RequireEnemySetup("EarlyEnemy_01");
        RequireEnemySetup("MidEnemy_01");
        RequireEnemySetup("MidEnemy_02");
        RequireEnemySetup("DemoBoss");

        Debug.Log("DemoSceneSetupBuilder: Playable demo scene validation passed.");
    }

    private static void OpenSampleScene()
    {
        EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
    }

    private static void CleanGeneratedObjects()
    {
        RemoveObject("StageBlockout");
        RemoveObject("DemoRuntime");
        RemoveObject("Player");
        RemoveObject("Main Camera");
        RemoveObject("Directional Light");
        RemoveObject("PlayerManager");
        RemoveObject("EarlyEnemy_01");
        RemoveObject("MidEnemy_01");
        RemoveObject("MidEnemy_02");
        RemoveObject("DemoBoss");
        RemoveObject("EarlyEnemy_01_PatrolLeftPoint");
        RemoveObject("EarlyEnemy_01_PatrolRightPoint");
        RemoveObject("MidEnemy_01_PatrolLeftPoint");
        RemoveObject("MidEnemy_01_PatrolRightPoint");
        RemoveObject("MidEnemy_02_PatrolLeftPoint");
        RemoveObject("MidEnemy_02_PatrolRightPoint");
        RemoveObject("DemoBoss_PatrolLeftPoint");
        RemoveObject("DemoBoss_PatrolRightPoint");
        RemoveObject("EarlyExitDoor");
        RemoveObject("MidExitDoor");
        RemoveObject("BossExitDoor");
        RemoveObject("EarlyRoomController");
        RemoveObject("MidRoomController");
        RemoveObject("BossRoomController");
        RemoveObject("DemoStageController");
        RemoveObject("InventoryCanvas");
        RemoveObject("InventorySystem");
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

    private static void AssignGeneratedStageLayers()
    {
        GameObject stageBlockout = GameObject.Find("StageBlockout");

        if (stageBlockout == null)
        {
            throw new InvalidOperationException("DemoSceneSetupBuilder: StageBlockout was not generated.");
        }

        int groundLayer = RequireLayer("Ground");

        foreach (Collider2D collider in stageBlockout.GetComponentsInChildren<Collider2D>())
        {
            collider.gameObject.layer = groundLayer;
        }
    }

    private static void CreateImportedBackdrop()
    {
        Sprite backdropSprite = LoadFirstSprite(ImportedBackdropPath);

        if (backdropSprite == null)
        {
            Debug.LogWarning("DemoSceneSetupBuilder: Imported parallax backdrop was not found. Keeping generated blockout background.");
            return;
        }

        GameObject backdropObject = new GameObject("ImportedGraveyardBackdrop");
        backdropObject.transform.position = new Vector3(18f, 0.1f, 0f);

        SpriteRenderer backdropRenderer = backdropObject.AddComponent<SpriteRenderer>();
        backdropRenderer.sprite = backdropSprite;
        backdropRenderer.sortingOrder = -45;
        backdropRenderer.color = new Color(0.72f, 0.76f, 0.86f, 1f);

        Vector2 spriteSize = backdropSprite.bounds.size;
        backdropObject.transform.localScale = new Vector3(112f / spriteSize.x, 13f / spriteSize.y, 1f);
    }

    private static GameObject CreateExperiencePickupPrefab()
    {
        EnsureFolder("Assets/Prefabs");
        EnsureFolder("Assets/Prefabs/Demo");

        GameObject pickupObject = new GameObject("ExperiencePickup");
        pickupObject.layer = 0;

        SpriteRenderer spriteRenderer = pickupObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetBlockSprite();
        spriteRenderer.color = new Color(0.85f, 0.12f, 0.18f, 1f);
        spriteRenderer.sortingOrder = 30;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        spriteRenderer.size = new Vector2(0.35f, 0.35f);

        CircleCollider2D circleCollider = pickupObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = 0.25f;

        ExperiencePickup experiencePickup = pickupObject.AddComponent<ExperiencePickup>();
        experiencePickup.experienceAmount = 3;

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(pickupObject, ExperiencePickupPrefabPath);
        UnityEngine.Object.DestroyImmediate(pickupObject);
        return prefab;
    }

    private static Player CreatePlayer()
    {
        GameObject playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.layer = RequireLayer("Player");
        playerObject.transform.position = new Vector3(-28f, -1.45f, 0f);

        Rigidbody2D rigidbody = playerObject.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 3f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CapsuleCollider2D capsuleCollider = playerObject.AddComponent<CapsuleCollider2D>();
        capsuleCollider.direction = CapsuleDirection2D.Vertical;
        capsuleCollider.size = new Vector2(0.75f, 1.25f);
        capsuleCollider.offset = new Vector2(0f, -0.02f);

        Transform visualRootTransform;
        SpriteRenderer bodyRenderer = CreatePlayerVisual(playerObject.transform, out visualRootTransform);
        Player player = playerObject.AddComponent<Player>();
        AssignSerializedReference(player, "spriteRenderer", bodyRenderer);
        AssignLayerMask(player, "groundLayer", RequireLayer("Ground"));
        AssignFloat(player, "groundCheckDistance", 0.45f);

        PlayerHealth playerHealth = playerObject.AddComponent<PlayerHealth>();
        playerHealth.maxHealth = 5;

        PlayerDemoStats playerDemoStats = playerObject.AddComponent<PlayerDemoStats>();
        playerDemoStats.experienceToLevel = 3;

        Transform attackPoint = CreateAttackPoint(playerObject.transform);
        MeleeHitDetector meleeHitDetector = playerObject.AddComponent<MeleeHitDetector>();
        meleeHitDetector.attackPoint = attackPoint;
        meleeHitDetector.attackRange = 1.1f;
        meleeHitDetector.hittableLayer = 1 << RequireLayer("Hittable");
        meleeHitDetector.damageAmount = 1;
        meleeHitDetector.playerDemoStats = playerDemoStats;

        PlayerAttackFacingController attackFacingController = playerObject.AddComponent<PlayerAttackFacingController>();
        attackFacingController.attackPoint = attackPoint;
        attackFacingController.visualRoot = visualRootTransform;
        attackFacingController.horizontalOffset = 0.9f;

        AttackSlashVisual attackSlashVisual = CreateAttackSlash(playerObject.transform);
        PlayerAttackController playerAttackController = playerObject.AddComponent<PlayerAttackController>();
        playerAttackController.attackCooldown = 0.35f;
        playerAttackController.meleeHitDetector = meleeHitDetector;
        playerAttackController.attackFacingController = attackFacingController;
        playerAttackController.attackSlashVisual = attackSlashVisual;

        CreateHealthBar("PlayerHealthBar", playerObject.transform, new Vector3(0f, 1.0f, 0f), playerHealth, null);
        return player;
    }

    private static SpriteRenderer CreatePlayerVisual(Transform parent, out Transform visualRootTransform)
    {
        SpriteRenderer importedRenderer = CreateImportedMihoVisual(parent, out visualRootTransform);

        if (importedRenderer != null)
        {
            return importedRenderer;
        }

        GameObject visualRoot = new GameObject("PlayerVisual");
        visualRoot.transform.SetParent(parent, false);
        visualRootTransform = visualRoot.transform;

        SpriteRenderer legs = CreateVisualBlock("Legs", visualRoot.transform, new Vector3(0f, -0.38f, 0f), new Vector2(0.44f, 0.36f), new Color(0.08f, 0.08f, 0.12f, 1f), 24);
        SpriteRenderer body = CreateVisualBlock("Coat", visualRoot.transform, new Vector3(0f, -0.02f, 0f), new Vector2(0.58f, 0.56f), new Color(0.16f, 0.18f, 0.32f, 1f), 25);
        CreateVisualBlock("Head", visualRoot.transform, new Vector3(0f, 0.47f, 0f), new Vector2(0.50f, 0.42f), new Color(0.92f, 0.86f, 0.78f, 1f), 26);
        CreateVisualBlock("Hair", visualRoot.transform, new Vector3(-0.03f, 0.62f, 0f), new Vector2(0.58f, 0.28f), new Color(0.10f, 0.07f, 0.13f, 1f), 27);
        CreateVisualBlock("EyeLeft", visualRoot.transform, new Vector3(-0.12f, 0.47f, 0f), new Vector2(0.07f, 0.09f), new Color(0.85f, 0.1f, 0.14f, 1f), 28);
        CreateVisualBlock("EyeRight", visualRoot.transform, new Vector3(0.12f, 0.47f, 0f), new Vector2(0.07f, 0.09f), new Color(0.85f, 0.1f, 0.14f, 1f), 28);
        return body != null ? body : legs;
    }

    private static SpriteRenderer CreateImportedMihoVisual(Transform parent, out Transform visualRootTransform)
    {
        visualRootTransform = null;
        GameObject mihoPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(MihoPrefabPath);

        if (mihoPrefab == null)
        {
            return null;
        }

        GameObject visualRoot = PrefabUtility.InstantiatePrefab(mihoPrefab) as GameObject;

        if (visualRoot == null)
        {
            return null;
        }

        visualRoot.name = "PlayerVisual_MihoImported";
        visualRoot.transform.SetParent(parent, false);
        visualRoot.transform.localPosition = new Vector3(0f, -0.67f, 0f);
        visualRoot.transform.localRotation = Quaternion.identity;
        visualRoot.transform.localScale = new Vector3(0.52f, 0.52f, 1f);
        visualRootTransform = visualRoot.transform;

        RemoveImportedGameplayComponents(visualRoot);
        SetImportedVisualSorting(visualRoot, 35);

        SpriteRenderer renderer = visualRoot.GetComponentsInChildren<SpriteRenderer>(true).FirstOrDefault();

        if (renderer == null)
        {
            Debug.LogWarning("DemoSceneSetupBuilder: Miho prefab was imported but no SpriteRenderer was found.");
            return null;
        }

        return CreateFacingProxyRenderer(parent);
    }

    private static SpriteRenderer CreateFacingProxyRenderer(Transform parent)
    {
        SpriteRenderer proxyRenderer = CreateVisualBlock(
            "FacingProxyRenderer",
            parent,
            Vector3.zero,
            new Vector2(0.1f, 0.1f),
            new Color(1f, 1f, 1f, 0f),
            -100);

        return proxyRenderer;
    }

    private static Transform CreateAttackPoint(Transform parent)
    {
        GameObject attackPoint = new GameObject("attackPoint");
        attackPoint.transform.SetParent(parent, false);
        attackPoint.transform.localPosition = new Vector3(0.9f, 0f, 0f);
        return attackPoint.transform;
    }

    private static AttackSlashVisual CreateAttackSlash(Transform playerTransform)
    {
        GameObject slashObject = new GameObject("AttackSlashVisual");
        slashObject.transform.SetParent(playerTransform, false);
        slashObject.transform.localRotation = Quaternion.Euler(0f, 0f, -24f);

        SpriteRenderer slashRenderer = slashObject.AddComponent<SpriteRenderer>();
        slashRenderer.sprite = GetBlockSprite();
        slashRenderer.color = new Color(1f, 0.14f, 0.1f, 0.9f);
        slashRenderer.drawMode = SpriteDrawMode.Sliced;
        slashRenderer.size = new Vector2(1.25f, 0.18f);
        slashRenderer.sortingOrder = 40;

        AttackSlashVisual slashVisual = slashObject.AddComponent<AttackSlashVisual>();
        slashVisual.slashRenderer = slashRenderer;
        slashVisual.showDuration = 0.12f;
        return slashVisual;
    }

    private static CameraFollow CreateMainCamera(Transform target)
    {
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(-24f, 0f, -10f);

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.05f, 0.06f, 0.09f, 1f);
        camera.orthographic = true;
        camera.orthographicSize = 5.5f;

        CameraFollow cameraFollow = cameraObject.AddComponent<CameraFollow>();
        AssignSerializedReference(cameraFollow, "target", target);
        return cameraFollow;
    }

    private static void CreatePlayerManager(Player player)
    {
        GameObject managerObject = new GameObject("PlayerManager");
        PlayerManager playerManager = managerObject.AddComponent<PlayerManager>();
        AssignSerializedReference(playerManager, "player", player);
    }

    private static EnemyHealth CreateEnemy(string objectName, Vector2 position, int maxHealth, int damageAmount, int experienceAmount, float patrolSpeed, float chaseSpeed, Player player, GameObject experiencePickupPrefab)
    {
        GameObject enemyObject = new GameObject(objectName);
        enemyObject.layer = RequireLayer("Hittable");
        enemyObject.transform.position = new Vector3(position.x, position.y, 0f);

        Rigidbody2D rigidbody = enemyObject.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 3f;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        CapsuleCollider2D collider = enemyObject.AddComponent<CapsuleCollider2D>();
        collider.direction = CapsuleDirection2D.Vertical;
        collider.size = new Vector2(0.9f, 1.0f);

        CreateEnemyVisual(enemyObject.transform, objectName == "DemoBoss");

        EnemyHealth enemyHealth = enemyObject.AddComponent<EnemyHealth>();
        enemyHealth.maxHealth = maxHealth;
        enemyHealth.disableOnDefeat = true;

        EnemyAttackController enemyAttackController = enemyObject.AddComponent<EnemyAttackController>();
        enemyAttackController.damageAmount = damageAmount;
        enemyAttackController.attackCooldown = objectName == "DemoBoss" ? 1.4f : 1f;

        Transform patrolLeftPoint = CreatePatrolPoint($"{objectName}_PatrolLeftPoint", enemyObject.transform.parent, position + Vector2.left * 2f);
        Transform patrolRightPoint = CreatePatrolPoint($"{objectName}_PatrolRightPoint", enemyObject.transform.parent, position + Vector2.right * 2f);

        BasicEnemyController controller = enemyObject.AddComponent<BasicEnemyController>();
        controller.playerTarget = player.transform;
        controller.playerHealth = player.GetComponent<PlayerHealth>();
        controller.patrolLeftPoint = patrolLeftPoint;
        controller.patrolRightPoint = patrolRightPoint;
        controller.enemyAttackController = enemyAttackController;
        controller.enemyHealth = enemyHealth;
        controller.patrolSpeed = patrolSpeed;
        controller.chaseSpeed = chaseSpeed;
        controller.detectionRange = objectName == "DemoBoss" ? 9f : 5.5f;
        controller.attackRange = objectName == "DemoBoss" ? 1.25f : 0.95f;

        EnemyDropController dropController = enemyObject.AddComponent<EnemyDropController>();
        dropController.enemyHealth = enemyHealth;
        dropController.experiencePickupPrefab = experiencePickupPrefab;
        dropController.dropCount = 1;
        dropController.experienceAmount = experienceAmount;
        dropController.dropOffset = new Vector3(0f, 0.65f, 0f);

        CreateHealthBar($"{objectName}_HealthBar", enemyObject.transform, new Vector3(0f, 0.9f, 0f), null, enemyHealth);
        return enemyHealth;
    }

    private static void CreateEnemyVisual(Transform parent, bool isBoss)
    {
        GameObject visual = new GameObject("EnemyVisual");
        visual.transform.SetParent(parent, false);
        visual.transform.localScale = isBoss ? new Vector3(1.25f, 1.25f, 1f) : Vector3.one;

        SpriteRenderer renderer = visual.AddComponent<SpriteRenderer>();
        renderer.sprite = GetEnemySprite(isBoss);
        renderer.color = Color.white;
        renderer.sortingOrder = isBoss ? 23 : 22;

        if (renderer.sprite == null)
        {
            renderer.sprite = GetBlockSprite();
            renderer.drawMode = SpriteDrawMode.Sliced;
            renderer.size = isBoss ? new Vector2(1.2f, 1.4f) : new Vector2(0.8f, 0.8f);
            renderer.color = isBoss ? new Color(0.55f, 0.04f, 0.08f, 1f) : new Color(0.48f, 0.18f, 0.72f, 1f);
        }
    }

    private static Transform CreatePatrolPoint(string objectName, Transform parent, Vector2 position)
    {
        GameObject patrolPoint = new GameObject(objectName);
        patrolPoint.transform.SetParent(parent, false);
        patrolPoint.transform.position = new Vector3(position.x, position.y, 0f);
        return patrolPoint.transform;
    }

    private static ExitDoorController CreateExitDoor(string objectName, Vector2 position)
    {
        GameObject doorObject = new GameObject(objectName);
        doorObject.transform.position = new Vector3(position.x, position.y, 0f);

        SpriteRenderer renderer = doorObject.AddComponent<SpriteRenderer>();
        renderer.sprite = GetBlockSprite();
        renderer.drawMode = SpriteDrawMode.Sliced;
        renderer.size = new Vector2(1.0f, 2.2f);
        renderer.color = new Color(0.22f, 0.02f, 0.04f, 1f);
        renderer.sortingOrder = 18;

        BoxCollider2D collider = doorObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1.0f, 2.2f);

        ExitDoorController exitDoor = doorObject.AddComponent<ExitDoorController>();
        exitDoor.IsUnlocked = false;
        exitDoor.doorSpriteRenderer = renderer;
        exitDoor.lockedColor = new Color(0.22f, 0.02f, 0.04f, 1f);
        exitDoor.unlockedColor = new Color(0.75f, 0.10f, 0.08f, 1f);
        return exitDoor;
    }

    private static RoomClearController CreateRoomController(string objectName, EnemyHealth[] enemies, ExitDoorController exitDoor)
    {
        GameObject controllerObject = new GameObject(objectName);
        RoomClearController roomClearController = controllerObject.AddComponent<RoomClearController>();
        roomClearController.enemiesInRoom = enemies;
        roomClearController.exitDoor = exitDoor;
        return roomClearController;
    }

    private static void CreateDemoStageController(RoomClearController earlyRoom, RoomClearController midRoom, RoomClearController bossRoom, ExitDoorController earlyDoor, ExitDoorController midDoor, ExitDoorController bossDoor)
    {
        GameObject controllerObject = new GameObject("DemoStageController");
        DemoStageController demoStageController = controllerObject.AddComponent<DemoStageController>();
        demoStageController.currentStage = DemoStageType.Early;
        demoStageController.earlyRoom = earlyRoom;
        demoStageController.midRoom = midRoom;
        demoStageController.bossRoom = bossRoom;
        demoStageController.earlyExitDoor = earlyDoor;
        demoStageController.midExitDoor = midDoor;
        demoStageController.bossExitDoor = bossDoor;
    }

    private static SimpleHealthBar CreateHealthBar(string objectName, Transform parent, Vector3 localPosition, PlayerHealth playerHealth, EnemyHealth enemyHealth)
    {
        GameObject barRoot = new GameObject(objectName);
        barRoot.transform.SetParent(parent, false);
        barRoot.transform.localPosition = localPosition;

        CreateVisualBlock("Background", barRoot.transform, Vector3.zero, new Vector2(0.9f, 0.12f), new Color(0.03f, 0.02f, 0.03f, 1f), 45);
        SpriteRenderer fillRenderer = CreateVisualBlock("Fill", barRoot.transform, Vector3.zero, new Vector2(0.82f, 0.07f), new Color(0.86f, 0.06f, 0.12f, 1f), 46);

        SimpleHealthBar healthBar = barRoot.AddComponent<SimpleHealthBar>();
        healthBar.fillTransform = fillRenderer.transform;
        healthBar.playerHealth = playerHealth;
        healthBar.enemyHealth = enemyHealth;
        return healthBar;
    }

    private static SpriteRenderer CreateVisualBlock(string objectName, Transform parent, Vector3 localPosition, Vector2 size, Color color, int sortingOrder)
    {
        GameObject block = new GameObject(objectName);
        block.transform.SetParent(parent, false);
        block.transform.localPosition = localPosition;

        SpriteRenderer renderer = block.AddComponent<SpriteRenderer>();
        renderer.sprite = GetBlockSprite();
        renderer.drawMode = SpriteDrawMode.Sliced;
        renderer.size = size;
        renderer.color = color;
        renderer.sortingOrder = sortingOrder;
        return renderer;
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
            blockSprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        }

        return blockSprite;
    }

    private static Sprite GetEnemySprite(bool isBoss)
    {
        if (isBoss)
        {
            if (bossSprite == null)
            {
                bossSprite = LoadFirstExistingSprite(BossSpriteCandidatePaths);
            }

            return bossSprite;
        }

        if (enemySprite == null)
        {
            enemySprite = LoadFirstExistingSprite(BasicEnemySpriteCandidatePaths);

            if (enemySprite == null)
            {
                enemySprite = LoadFirstSprite(FallbackEnemySpriteAssetPath);
            }
        }

        return enemySprite;
    }

    private static Sprite LoadFirstExistingSprite(string[] assetPaths)
    {
        foreach (string assetPath in assetPaths)
        {
            Sprite sprite = LoadFirstSprite(assetPath);

            if (sprite != null)
            {
                return sprite;
            }
        }

        return null;
    }

    private static Sprite LoadFirstSprite(string assetPath)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

        if (sprite != null)
        {
            return sprite;
        }

        return AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>().FirstOrDefault();
    }

    private static void RemoveImportedGameplayComponents(GameObject visualRoot)
    {
        foreach (Rigidbody2D rigidbody in visualRoot.GetComponentsInChildren<Rigidbody2D>(true))
        {
            UnityEngine.Object.DestroyImmediate(rigidbody);
        }

        foreach (Collider2D collider in visualRoot.GetComponentsInChildren<Collider2D>(true))
        {
            UnityEngine.Object.DestroyImmediate(collider);
        }

        foreach (MonoBehaviour behaviour in visualRoot.GetComponentsInChildren<MonoBehaviour>(true))
        {
            UnityEngine.Object.DestroyImmediate(behaviour);
        }
    }

    private static void SetImportedVisualSorting(GameObject visualRoot, int baseSortingOrder)
    {
        SpriteRenderer[] renderers = visualRoot.GetComponentsInChildren<SpriteRenderer>(true);

        for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
        {
            renderers[rendererIndex].sortingOrder = baseSortingOrder + rendererIndex;
            renderers[rendererIndex].color = Color.white;
        }
    }

    private static void AssignSerializedReference(UnityEngine.Object target, string propertyName, UnityEngine.Object reference)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.FindProperty(propertyName).objectReferenceValue = reference;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void AssignLayerMask(UnityEngine.Object target, string propertyName, int layer)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.FindProperty(propertyName).intValue = 1 << layer;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void AssignFloat(UnityEngine.Object target, string propertyName, float value)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.FindProperty(propertyName).floatValue = value;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private static int RequireLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);

        if (layer < 0)
        {
            throw new InvalidOperationException($"DemoSceneSetupBuilder: Required layer '{layerName}' is missing.");
        }

        return layer;
    }

    private static void EnsureFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            return;
        }

        string parentFolder = Path.GetDirectoryName(folderPath)?.Replace("\\", "/");
        string folderName = Path.GetFileName(folderPath);

        if (string.IsNullOrEmpty(parentFolder) || string.IsNullOrEmpty(folderName))
        {
            throw new InvalidOperationException($"DemoSceneSetupBuilder: Invalid folder path '{folderPath}'.");
        }

        AssetDatabase.CreateFolder(parentFolder, folderName);
    }

    private static void RequireSingleObject(string objectName)
    {
        int matchCount = 0;

        foreach (GameObject rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            matchCount += CountObjectsNamed(rootObject.transform, objectName);
        }

        if (matchCount != 1)
        {
            throw new InvalidOperationException($"DemoSceneSetupBuilder: Expected one '{objectName}', found {matchCount}.");
        }
    }

    private static int CountObjectsNamed(Transform transformToCheck, string objectName)
    {
        int count = transformToCheck.name == objectName ? 1 : 0;

        for (int childIndex = 0; childIndex < transformToCheck.childCount; childIndex++)
        {
            count += CountObjectsNamed(transformToCheck.GetChild(childIndex), objectName);
        }

        return count;
    }

    private static T RequireComponent<T>(string objectName) where T : Component
    {
        GameObject gameObject = GameObject.Find(objectName);

        if (gameObject == null)
        {
            throw new InvalidOperationException($"DemoSceneSetupBuilder: '{objectName}' is missing.");
        }

        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            throw new InvalidOperationException($"DemoSceneSetupBuilder: '{objectName}' is missing {typeof(T).Name}.");
        }

        return component;
    }

    private static void RequireEnemySetup(string objectName)
    {
        RequireComponent<Rigidbody2D>(objectName);
        RequireComponent<Collider2D>(objectName);
        RequireComponent<BasicEnemyController>(objectName);
        RequireComponent<EnemyAttackController>(objectName);
        RequireComponent<EnemyHealth>(objectName);
        RequireComponent<EnemyDropController>(objectName);
    }
}
