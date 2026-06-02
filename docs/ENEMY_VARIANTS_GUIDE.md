# Enemy Variants Guide

This branch adds two small enemy prefabs based on the existing `Enemy_FoxSprit` setup. They are meant for quick demo variety, not a full enemy system.

## Current Enemy Prefabs

- `Assets/Art/Prefabs/EnemyPrefabs/Enemy_FoxSprit.prefab`
  - Existing ground enemy.
  - Good baseline for normal chase and attack behavior.

- `Assets/Art/Prefabs/EnemyPrefabs/Enemy_EmberBatFamiliar.prefab`
  - Flying enemy.
  - Uses `Assets/Art/Generated/Enemies/ember_bat_familiar_girl_spritesheet.png`.
  - Has zero gravity and can chase the player vertically.
  - Good for platforms, gaps, and making the player look upward.

- `Assets/Art/Prefabs/EnemyPrefabs/Enemy_FlowerSlimeNymph.prefab`
  - Ground enemy with self-heal.
  - Uses `Assets/Art/Generated/Enemies/flower_slime_nymph_spritesheet.png`.
  - Restores health every few seconds while alive.
  - Good for teaching the player to finish a target quickly.

## Script Responsibilities

- `Enemy_EmberBatFamiliar`
  - Only changes movement behavior for the flying enemy.
  - Keeps the base attack, health, hurt, death, and animation trigger logic from `Enemy`.

- `Enemy_FlowerSlimeNymph`
  - Only adds self-healing behavior.
  - Keeps the base patrol, chase, attack, health, hurt, death, and animation trigger logic from `Enemy`.

- `Enemy`
  - Now raises its death event before being destroyed.
  - This lets room clear and drop systems react correctly when any `Enemy` subclass dies.

## Unity Setup

1. Open `Assets/Scenes/SampleScene.unity`.
2. Drag one of the prefabs from `Assets/Art/Prefabs/EnemyPrefabs/` into the scene.
3. Keep the enemy Layer consistent with existing enemies.
4. Make sure the enemy can reach or detect the Player:
   - `detectionRange`
   - `attackRange`
   - `playerLayer`
   - `groundLayer` for ground enemies
5. For room clear logic, add the new enemy Entity/Enemy reference to the correct room controller if that controller needs an enemy list.

## Suggested Demo Use

- Early room:
  - 1 `Enemy_FoxSprit`
  - 1 `Enemy_FlowerSlimeNymph`

- Mid room:
  - 1 `Enemy_FoxSprit`
  - 1 `Enemy_EmberBatFamiliar`
  - 1 `Enemy_FlowerSlimeNymph`

- Boss approach:
  - 1 `Enemy_EmberBatFamiliar` above a platform to force vertical awareness.

## What This Does Not Add

- No new boss system.
- No random spawning.
- No drop table.
- No new UI.
- No skill tree changes.
- No scene rewrite.

## Test Checklist

- Unity Console has no red C# compile errors.
- Fox spirit still moves, attacks, and dies.
- Ember bat familiar floats instead of falling.
- Ember bat familiar can chase the Player vertically.
- Flower slime nymph heals itself over time.
- All three enemy prefabs use their own visual spritesheets.
- Killing an enemy still triggers room clear or drop systems that subscribe to enemy death.
