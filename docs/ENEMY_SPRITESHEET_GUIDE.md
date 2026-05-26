# Enemy Spritesheet Guide

## Purpose

Enemy animation art should keep every action for one enemy in one spritesheet. This avoids mismatched sprite sizes when Unity switches animation clips.

## Current Generated Enemy Sheets

All current generated enemy sheets are original placeholder art for the demo:

- `Assets/Art/Generated/Enemies/crystal_fox_spirit_girl_spritesheet.png`
- `Assets/Art/Generated/Enemies/ember_bat_familiar_girl_spritesheet.png`
- `Assets/Art/Generated/Enemies/flower_slime_nymph_spritesheet.png`
- `Assets/Art/Generated/Enemies/cream_bun_monster_spritesheet.png`

Each sheet uses:

- Sheet size: `1024 x 1024`
- Grid: `4 columns x 4 rows`
- Frame size: `256 x 256`
- Pixels Per Unit: `64`
- Sprite Mode: `Multiple`
- Mesh Type: `Full Rect`
- Filter Mode: `Point`
- Background: transparent PNG

## Frame Order

Rows are always ordered from top to bottom:

1. `walk`
2. `attack`
3. `hurt`
4. `death`

Columns are frame order from left to right:

1. frame `00`
2. frame `01`
3. frame `02`
4. frame `03`

Example frame names:

- `crystal_fox_spirit_walk_00`
- `crystal_fox_spirit_attack_00`
- `crystal_fox_spirit_hurt_00`
- `crystal_fox_spirit_death_00`

## Unity Import Rule

These generated spritesheets must stay compatible with Unity 2022.3.53f1 / 2022.3.53f1c1:

- TextureImporter `serializedVersion` should be `12`.
- Do not commit Unity 6-only TextureImporter fields.
- Keep every frame in the same grid size.
- Use `Full Rect` for enemies unless there is a strong reason to use tight meshes.

## Animation Setup

For each enemy, create four animation clips:

- `<enemy>_Walk`
- `<enemy>_Attack`
- `<enemy>_Hurt`
- `<enemy>_Death`

Keep the enemy root object responsible for gameplay scripts:

- `Rigidbody2D`
- `Collider2D`
- `BasicEnemyController`
- `EnemyHealth`
- `EnemyAttackController`

Put the sprites and Animator on an `EnemyVisual` child object. Do not move gameplay scripts onto the visual child.

## Design Direction

Current enemy visual direction:

- Cute but hostile.
- Chibi anime monster-girl or creature-inspired silhouette.
- Elemental or creature archetypes are okay as inspiration.
- Do not copy Pokemon designs, names, silhouettes, colors, or UI.
- Do not use adult, sexualized, gory, or copyrighted character designs.
