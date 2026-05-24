# Level Blockout

## Current Builder

Use this Unity Editor menu:

`Tools/Stage Blockout/Create Three Stage Dungeon Demo`

The builder creates one root object:

`StageBlockout`

It does not create Player, Camera, Enemy prefabs, Boss prefabs, Layer, or Tag. Those must be configured manually in the Unity Editor.

## Generated Structure

`StageBlockout` contains:

- `Background`
- `EarlyRoom`
- `MidRoom`
- `BossRoom`
- `Boundaries`
- `Markers`

## Early Room

Purpose:

- Give the player a safe start.
- Show one or two weak enemies.
- Let the player quickly collect enough experience to level up.

Generated objects:

- `EarlyRoom/Ground/EarlyGround`
- `EarlyRoom/Ground/EarlyStep`
- `EarlyRoom/PlayerSpawnPoint`
- `EarlyRoom/EnemySpawnPoint_Early_01`
- `EarlyRoom/EnemySpawnPoint_Early_02`
- `EarlyRoom/EarlyExitDoorPoint`

Recommended setup:

- Move Player to `PlayerSpawnPoint`.
- Place one weak BasicEnemy at `EnemySpawnPoint_Early_01`.
- Optionally place a second weak BasicEnemy at `EnemySpawnPoint_Early_02`.
- Place Early ExitDoor at `EarlyExitDoorPoint`.
- Early enemy health: 2.
- Early enemy experience drop: 3.

## Mid Room

Purpose:

- Show that the level-up makes the player stronger.
- Use two or three enemies with slightly higher health.
- Keep the room readable for a short recording.

Generated objects:

- `MidRoom/Ground/MidGround`
- `MidRoom/Platforms/MidPlatform_Left`
- `MidRoom/Platforms/MidPlatform_Right`
- `MidRoom/EnemySpawnPoint_Mid_01`
- `MidRoom/EnemySpawnPoint_Mid_02`
- `MidRoom/EnemySpawnPoint_Mid_03`
- `MidRoom/MidExitDoorPoint`

Recommended setup:

- Place two BasicEnemy objects at `EnemySpawnPoint_Mid_01` and `EnemySpawnPoint_Mid_02`.
- Use `EnemySpawnPoint_Mid_03` only if the scene remains easy to read.
- Mid enemy health: 4.
- Mid enemy experience drop: 4 or 5.

## Boss Room

Purpose:

- End the recording with one stronger enemy.
- Use a simple chase and cooldown attack.
- Do not build a formal boss system yet.

Generated objects:

- `BossRoom/Ground/BossGround`
- `BossRoom/Ground/BossLeftLedge`
- `BossRoom/Ground/BossRightLedge`
- `BossRoom/BossSpawnPoint`
- `BossRoom/BossExitDoorPoint`

Recommended setup:

- Use BasicEnemyController with higher values for the DemoBoss.
- Boss health: 10 to 15.
- Boss damage: 1 or 2.
- Boss chase speed should be slower than normal enemies.
- Boss attack cooldown should be long enough that the player can win.

## Layers And Tags

Required Layers:

- `Player`
- `Ground`
- `Hittable`

Required Tag:

- `Player`

Manual assignments:

- Player object: `Player` Tag and `Player` Layer.
- Ground, platforms, and boundaries: `Ground` Layer.
- Normal enemies and DemoBoss: `Hittable` Layer.
- Experience pickup: any layer that can trigger with Player.

## Greybox Acceptance

- The menu appears in Unity under `Tools/Stage Blockout`.
- Clicking it creates `StageBlockout`.
- Early, Mid, and Boss rooms are visible from left to right.
- Ground and platform objects have `BoxCollider2D`.
- The player can stand on ground and platforms after the Ground Layer is assigned.
- Each room has enemy spawn markers and an exit door marker.
- The player can move from Early Room to Mid Room to Boss Room.
- The route can be recorded without the player falling through gaps.
