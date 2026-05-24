# Player Visual Replacement

## Purpose

The previous Player visual used imported MIHO prefab parts directly. That made the Player hierarchy large and hard to read, and the visual Animator caused Console warnings when the root Player script pushed parameters that did not exist on the visual controller.

This pass replaces the Player display with a compact generated maid-adventurer sprite sheet for the current dungeon demo.

## Asset Used

Generated original placeholder asset:

- `Assets/Art/Generated/Characters/maid_heroine_spritesheet.png`
- `Assets/Art/Generated/Characters/Animations/Maid_Idle.anim`
- `Assets/Art/Generated/Characters/Animations/Maid_Walk.anim`
- `Assets/Art/Generated/Characters/Animations/Maid_Attack.anim`
- `Assets/Art/Generated/Characters/Animations/Maid_Hurt.anim`
- `Assets/Art/Generated/Characters/MaidPlayerAnimator.controller`

The sprite sheet is an original pixel-art placeholder made for this project. It is not an Asset Store resource and does not copy the provided reference image or any commercial character.

## Third-Party Asset Policy

Local Asset Store imports are kept under:

- `Assets/Art/ThirdParty/`

This folder is ignored while the repository is public. Teammates who need the old local visual packages should import the same Asset Store packages themselves.

## Player Hierarchy

Recommended structure in `Assets/Scenes/SampleScene.unity`:

- `Player`
- `MaidVisual`
- `attackPoint`
- `AttackSlashVisual`
- `PlayerHealthBar`

The Player root keeps gameplay components such as movement, health, attack, Rigidbody2D, and Collider2D. `MaidVisual` is visual-only.

## Animator Setup

`MaidVisual` has:

- `SpriteRenderer`
- `Animator`
- `MaidVisualAnimatorDriver`

The Animator Controller only uses:

- Bool: `IsMoving`
- Trigger: `Attack`
- Trigger: `Hurt`

It does not use `IsWallSliding`.

## IsWallSliding Warning Fix

The warning came from the root `Player` script calling `anim.SetBool("IsWallSliding", isWallSliding)` after it automatically found a child visual Animator. The root gameplay script now clears its Animator reference in `Awake`, so imported or generated child visual Animators are not driven by root movement parameters.

The visual animation is handled by `MaidVisualAnimatorDriver` instead.

## Sorting And Scale

Current generated setup:

- `MaidVisual` local scale: around `1.3, 1.3, 1`.
- `MaidVisual` local position: around `0, -0.63, 0`.
- Maid SpriteRenderer order: `35`.
- Attack slash order: `40`.
- Health bar order: `45+`.

Keep the visual child aligned with the root Collider2D feet. Do not resize the root collider just to fit the picture unless gameplay collision really needs it.

## Attack Point

Current attack point setup:

- Right side: `x = 0.9`.
- Left side: `x = -0.9`.
- `PlayerAttackFacingController` moves the point based on the last horizontal input.
- `AttackSlashVisual` uses the same facing direction.

## Validation

1. Open `Assets/Scenes/SampleScene.unity`.
2. Press Play.
3. Move left and right: MaidVisual should flip cleanly.
4. Jump: visual should follow Player root.
5. Press `J` or left mouse: slash appears in front of the character.
6. Attack left and right: hit detection follows `attackPoint`.
7. Check PlayerHealthBar remains above the character.
8. Confirm Console no longer prints `Parameter 'IsWallSliding' does not exist`.
