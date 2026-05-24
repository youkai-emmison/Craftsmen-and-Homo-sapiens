# Imported Enemy Asset Usage

## Purpose

This guide explains how to use locally imported enemy art without changing gameplay scripts or committing raw Asset Store assets to a public repository.

## Bringer Of Death

Best use:

- DemoBoss visual placeholder.
- Late-room elite enemy visual placeholder.
- Dark dungeon atmosphere test.

Do not use it as:

- Final original boss design.
- Story identity.
- Copied lore, name, UI, or move set.

## Enemy Galore 1 - Pixel Art

Best use:

- Early Room weak enemies.
- Mid Room stronger enemies.
- Quick comparison of readable monster silhouettes.

Do not mix too many styles in the same room. Pick one or two enemy sprites that look readable beside the player and stick with them for the demo.

## Recommended Object Structure

Enemy root object:

- `Rigidbody2D`
- `Collider2D`
- `BasicEnemyController`
- `EnemyAttackController`
- `EnemyHealth`
- `EnemyDropController` optional

Child object:

- `EnemyVisual`
- SpriteRenderer or Animator from the imported art package.

Keep gameplay collider setup on the root. Do not let the visual package change movement, combat, or room clear logic.

## Sorting And Scale

Recommended setup:

- Player sorting order higher than background.
- Enemy visual sorting order similar to Player.
- Boss visual may be larger, but the collider should stay fair.
- Keep attack range and detection range visible with Gizmos during tuning.

## Repository Rule

If the repository is public, do not commit raw Unity Asset Store files. Use local imports for testing and record the source in `docs/THIRD_PARTY_ASSETS.md`.

If the repository becomes private and the team decides to commit selected assets, record:

- Asset URL.
- Publisher.
- License status.
- Imported date.
- Used scene or prefab.
