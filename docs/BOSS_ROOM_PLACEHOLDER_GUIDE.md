# Boss Room Placeholder Guide

## Goal

The Boss Room is a demo ending, not a formal boss system. It should prove that the player can enter a final room, fight a stronger enemy, defeat it, and complete the recording route.

## Recommended Boss Setup

Use the existing simple enemy components:

- `Rigidbody2D`
- `Collider2D`
- `BasicEnemyController`
- `EnemyAttackController`
- `EnemyHealth`
- `EnemyDropController` optional

Recommended values:

- `EnemyHealth.maxHealth`: 10 to 15.
- `EnemyAttackController.damageAmount`: 1 or 2.
- `EnemyAttackController.attackCooldown`: 1.2 to 1.8.
- `BasicEnemyController.chaseSpeed`: slightly slower than normal enemies.
- `BasicEnemyController.attackRange`: around 1.0 to 1.4.
- `BasicEnemyController.detectionRange`: large enough to start the fight after the player enters the room.

## Bringer Of Death Placeholder

If Bringer Of Death is imported locally:

1. Keep the Boss root object as the gameplay object.
2. Put `Rigidbody2D`, `Collider2D`, `BasicEnemyController`, `EnemyAttackController`, and `EnemyHealth` on the root.
3. Add a child object named `BossVisual`.
4. Put the Bringer Of Death SpriteRenderer or Animator on `BossVisual`.
5. Adjust `BossVisual` scale and sorting order.
6. Do not resize the gameplay Collider just because the sprite is large.

## Boss Room Controller

Create a `RoomClearController` for Boss Room:

- `enemiesInRoom`: one element, the Boss `EnemyHealth`.
- `exitDoor`: the final `ExitDoorController`.

Create or use a `DemoStageController`:

- Assign `bossRoom`.
- Assign `bossExitDoor`.
- When the boss dies, Console should print `Demo Complete / Boss Defeated`.

## Why No Formal Boss System Yet

Formal bosses need unique attacks, animation events, camera framing, UI, tuning, and readability work. This demo only needs a final stronger enemy so the route can be shown from start to finish.

## Acceptance

- Boss chases the player.
- Boss attacks only on cooldown.
- Player can damage the Boss.
- Boss defeat triggers `EnemyHealth.OnEnemyDefeated`.
- Boss Room exit unlocks.
- Console prints `Demo Complete / Boss Defeated`.
