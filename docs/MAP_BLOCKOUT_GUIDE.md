# Map Blockout Guide

## Why Build The Map First

The project already has several isolated systems. The demo now needs a stable route that can be played and recorded without rebuilding the scene by hand every time.

The current map target is a three-stage dark dungeon route:

`Early Room -> Mid Room -> Boss Room`

## Run The Builder

Open the gameplay scene, then click:

`Tools/Stage Blockout/Create Three Stage Dungeon Demo`

Unity should create:

- `StageBlockout/Background`
- `StageBlockout/EarlyRoom`
- `StageBlockout/MidRoom`
- `StageBlockout/BossRoom`
- `StageBlockout/Boundaries`
- `StageBlockout/Markers`

The builder only creates greybox objects and placement markers. It does not create Player, Camera, enemies, doors, layers, or tags.

## Manual Setup

1. Set ground, platforms, and boundaries to `Ground` Layer.
2. Move Player to `EarlyRoom/PlayerSpawnPoint`.
3. Keep Player Tag as `Player`.
4. Add `PlayerDemoStats` to Player.
5. Assign `PlayerDemoStats` to `MeleeHitDetector.playerDemoStats`.
6. Place one or two BasicEnemy objects at Early Room enemy markers.
7. Place two or three BasicEnemy objects at Mid Room enemy markers.
8. Place one stronger BasicEnemy at `BossRoom/BossSpawnPoint`.
9. Place ExitDoor objects at the three exit door markers.
10. Create three RoomClearController objects and one DemoStageController.

## Experience Pickup Prefab

Create a simple pickup prefab:

- SpriteRenderer or simple visual child.
- Collider2D with `Is Trigger` checked.
- `ExperiencePickup` component.
- `experienceAmount = 3` for Early Room.

Assign this prefab to each enemy's `EnemyDropController.experiencePickupPrefab`.

## Enemy Setup

Each demo enemy root should have:

- `Rigidbody2D`
- `Collider2D`
- `BasicEnemyController`
- `EnemyAttackController`
- `EnemyHealth`
- `EnemyDropController`

Use Inspector references:

- `playerTarget`: Player transform.
- `playerHealth`: PlayerHealth on Player.
- `enemyAttackController`: same enemy root.
- `enemyHealth`: same enemy root.
- `patrolLeftPoint` and `patrolRightPoint`: manually created empty objects.

## Boss Setup

The Boss is still a simple demo enemy:

- Use `BasicEnemyController`.
- Increase `EnemyHealth.maxHealth`.
- Slow the chase speed if needed.
- Use Bringer Of Death only as a visual child if it is imported locally.
- Do not build a formal boss system yet.

## Test Route

1. Press Play.
2. Move right through Early Room.
3. Defeat one weak enemy.
4. Pick up experience and check Console for `Level Up`.
5. Enter Mid Room and defeat stronger enemies faster.
6. Enter Boss Room.
7. Defeat DemoBoss.
8. Confirm Console prints `Demo Complete / Boss Defeated`.

## If Something Breaks

Player falls through ground:

- Check ground object has `BoxCollider2D`.
- Check it is on `Ground` Layer.
- Check Player Rigidbody2D and Collider2D.

Player cannot level up:

- Check enemy has `EnemyDropController`.
- Check `experiencePickupPrefab` is assigned.
- Check pickup has Trigger Collider2D.
- Check Player has `PlayerDemoStats`.

Player cannot damage enemies:

- Check enemy is on `Hittable` Layer.
- Check `MeleeHitDetector.hittableLayer` includes `Hittable`.
- Check enemy has `EnemyHealth`.

Door does not unlock:

- Check RoomClearController has all enemy `EnemyHealth` references.
- Check ExitDoorController is assigned.
- Check enemies become `IsDead = true` when defeated.
