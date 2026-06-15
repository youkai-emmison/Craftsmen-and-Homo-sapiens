# Screenshot Capture Guide

This guide explains how to capture real Unity screenshots for the poster, PPT, and demo recording materials.

## Why Screenshots Matter

The rebuilt poster and PPT now use real project sprites, enemies, NPCs, item icons, and tiles. The only remaining visual gap is real in-game screenshots. Do not fake screenshots in submission materials. Capture them from Unity after the scene is playable.

## Output Folder

Screenshots are saved to:

```text
Submission/screenshots/
```

Suggested filenames:

```text
01_title_or_spawn.png
02_intro_memory_log.png
03_early_room_combat.png
04_level_up_or_growth.png
05_mid_room_enemy.png
06_boss_room.png
07_victory_screen.png
```

The editor tool automatically uses the first missing filename in that order. If all names already exist, it creates a timestamped file.

## Unity Steps

1. Open the Unity project.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Enter Play Mode.
4. Move the player to the moment you want to capture.
5. Click:

```text
Tools > Hackathon > Capture Submission Screenshots
```

6. Wait a moment for Unity to write the PNG.
7. Click:

```text
Tools > Hackathon > Open Screenshot Folder
```

8. Confirm the screenshot appeared under `Submission/screenshots/`.

## Shots To Capture

- `01_title_or_spawn.png`: player at spawn or first room.
- `02_intro_memory_log.png`: NPC dialogue or memory log UI.
- `03_early_room_combat.png`: player attacking an early enemy.
- `04_level_up_or_growth.png`: experience, growth, crafting, or skill-energy feedback.
- `05_mid_room_enemy.png`: mid-room enemy group or stronger enemy.
- `06_boss_room.png`: Boss or final abnormal enemy room.
- `07_victory_screen.png`: victory, Demo Complete, or ending text.

## After Capturing

Run:

```powershell
python tools/generate_submission_visuals.py
node tools/generate_project_deck.cjs
```

The poster and PPT will still show the real project assets. The poster currently labels missing screenshot frames as placeholders, so update those frames only after real captures exist.

## Rules

- Do not use screenshots from other games.
- Do not pretend placeholder boxes are real gameplay.
- Do not claim WebGL deployment or demo recording is complete until those steps are actually done.
- Keep UI readable: if text is too small, zoom the Game View or adjust the camera before capturing.
