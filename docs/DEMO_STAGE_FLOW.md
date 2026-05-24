# Demo Stage Flow

## Goal

The next playable demo should be a compressed three-stage route:

`Early Room -> Mid Room -> Boss Room`

The player should understand the loop in one recording: fight, collect experience, level up, hit harder, clear a stronger room, then defeat a simple DemoBoss.

## Early Room

Purpose:

- Teach the current combat loop.
- Give one quick upgrade.
- Keep the first room low-pressure.

Suggested setup:

- 1 to 2 weak enemies.
- Enemy health: 2.
- Enemy damage: 1.
- Each enemy drops one experience pickup.
- Experience pickup value: 3.
- Player starts with `experienceToLevel = 3`, so one pickup can level up.

## Mid Room

Purpose:

- Show that the level-up matters.
- Let the player defeat stronger enemies more quickly after gaining damage.

Suggested setup:

- 2 to 3 enemies.
- Enemy health: 4.
- Enemy damage: 1.
- Experience pickup value: 4 or 5.
- Use wider spacing than Early Room.

## Boss Room

Purpose:

- End the demo route with a clear final target.
- Avoid building a real boss system too early.

Suggested setup:

- 1 DemoBoss.
- Health: 10 to 15.
- Damage: 1 or 2.
- Chase speed slightly slower than normal enemies.
- Attack cooldown slightly longer than normal enemies.
- On defeat, print `Demo Complete / Boss Defeated`.

## Why The Numbers Are Large

The demo numbers are intentionally exaggerated. A competition recording needs viewers to see growth in seconds, not after a long run. These are not final balance values.

## Recording Route

1. Start at Early Room.
2. Defeat the first weak enemy.
3. Pick up experience and level up.
4. Move through Early ExitDoor.
5. Clear Mid Room faster because attack damage is higher.
6. Move through Mid ExitDoor.
7. Defeat DemoBoss.
8. Show Console message: `Demo Complete / Boss Defeated`.

## Not Doing Yet

- Random map generation.
- Full equipment system.
- Skill tree.
- Backpack systems.
- Save data.
- Formal boss skills.
- Formal UI.

## Acceptance

- Player can gain experience.
- Player can level up.
- Melee damage increases after level-up.
- Enemies can drop pickups after defeat.
- Early, Mid, and Boss rooms can each unlock an exit.
- Boss defeat completes the demo route.
