# Clean Submission Screenshots

These images are cleaned from the six gameplay screenshots provided by the team.
Cleaning only removes recording overlays, subtitles, or irrelevant margins; it does not fabricate gameplay UI.

## Files

| Clean File | Raw Source | Cleanup | Best Use |
| --- | --- | --- | --- |
| `01_move_jump_attack_clean.png` | `01_move_jump_attack_raw.png` | Cropped bottom video subtitle and covered the NVIDIA recording prompt. | PPT Slide 2, Demo timeline |
| `02_npc_dialogue_clean.png` | `02_npc_dialogue_raw.png` | Cropped bottom video subtitle while preserving the NPC dialogue box. | PPT Slide 3, poster narrative panel |
| `03_backpack_clean.png` | `03_backpack_raw.png` | Cropped to the backpack, equipment, stat and character panels. | PPT Slide 4, system-growth proof |
| `04_crafting_clean.png` | `04_crafting_raw.png` | Cropped away the top subtitle and focused on materials, item preview and Craft button. | PPT Slide 5, crafting system proof |
| `05_skilltree_clean.png` | `05_skilltree_raw.png` | Cropped away the bottom subtitle and kept the skill tree and skill detail panel. | PPT Slide 5, skill growth proof |
| `06_boss_combat_clean.png` | `06_boss_combat_raw.png` | Kept the full Boss combat frame as the clean gameplay climax shot. | PPT Slide 7, boss proof |
| `06_boss_combat_hero_crop.png` | `06_boss_combat_raw.png` | 16:9 hero crop for large visual placement. | Poster background, PPT cover |

## Manual Check

- Confirm no video subtitles remain in the final poster and deck.
- Confirm the cropped UI still shows the required backpack, crafting and skill-tree information.
- If the final demo recording changes, replace the raw files and rerun `python tools/clean_submission_screenshots.py`.