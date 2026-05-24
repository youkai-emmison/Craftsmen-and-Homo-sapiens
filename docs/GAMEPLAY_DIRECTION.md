# Gameplay Direction

## One-Line Positioning

Craftsmen and Homo sapiens is currently a dark dungeon side-scrolling action Roguelite prototype focused on a short, readable demo route: move, jump, attack, defeat abnormal enemies, gain a visible power bump, and clear a final demo boss.

This is still a prototype, not a complete Roguelike.

## Current Demo Direction

The current visual and pacing direction is:

- Dark dungeon / abnormal monster / low-saturation side-scrolling Roguelite.
- Compact rooms that feel like a controlled facility or stage, not a random maze.
- Strong readable silhouettes, clear enemy states, and fast demo feedback.
- A compressed three-stage route: Early Room, Mid Room, Boss Room.

Project Moon works can be used only as a high-level mood reference: abnormal monsters, oppressive facility atmosphere, archival feeling, and staged combat pressure. Do not copy any Project Moon characters, setting terms, UI, icons, text, music, numbers, story, or monster designs.

## Core Demo Loop

1. Enter Early Room.
2. Defeat one or two weak enemies.
3. Pick up large demo experience.
4. Level up quickly and gain a clear attack damage increase.
5. Enter Mid Room.
6. Defeat two or three stronger enemies faster because of the upgrade.
7. Enter Boss Room.
8. Defeat one simple DemoBoss.
9. Print `Demo Complete / Boss Defeated`.

## Why This Is Not A Full Roguelike Yet

This stage is for a competition-friendly recording path, not a full game structure.

The demo deliberately avoids:

- Random maps.
- Full equipment systems.
- Skill trees.
- Backpack expansion.
- Save data.
- Formal boss systems.
- Complex UI.
- Long-term balance.

The goal is to prove that the basic action route and progression readability work before adding more systems.

## Temporary Third-Party Visuals

Imported Unity Asset Store monster assets can be used as local visual placeholders only:

- Bringer Of Death can be used as a DemoBoss visual placeholder.
- Enemy Galore 1 - Pixel Art can be used as Early Room or Mid Room enemy placeholders.

If the GitHub repository is public, raw Asset Store files should not be committed unless the team confirms that the repository is private and the license allows that workflow. Final art can replace these placeholders later with original assets.

## Current Feature Scope

Allowed in this demo stage:

- Player movement, jump, and melee attack.
- Attack cooldown.
- Basic enemy patrol, chase, and attack.
- PlayerHealth and EnemyHealth.
- Room clear and exit door unlock.
- Demo experience pickup and fast level-up.
- Three-stage greybox route.

Not allowed in this demo stage:

- Boss complexity beyond chase and cooldown attack.
- Random dungeon generation.
- Complete Tilemap pipeline.
- Complete equipment, backpack, shop, crafting, or skill tree.
- AI API integration.
- Copying any commercial or open-source game content.
