# Demo 体验流程 / Demo Recording Guide

本文档给负责录屏的同学使用。表达上面向比赛展示，不写成内部调试备注。

## Recommended Length

3 到 5 分钟。

## Experience Flow

和新版 PPT 的“Demo 体验流程”页保持一致。当前已经有 6 张清理后的真实截图用于 PPT 和海报：

| Time | Shot | What To Show |
| --- | --- | --- |
| 0:00 | 穿越开场 / 移动 | 主角在糖果异世界中移动、跳跃、攻击，参考 `01_move_jump_attack_clean.png` |
| 0:30 | NPC 对话 / 日志 | 糖芯档案员对话和糖芯工坊日志，参考 `02_npc_dialogue_clean.png` |
| 1:30 | 背包成长 | 背包、装备说明、角色属性，参考 `03_backpack_clean.png` |
| 2:00 | 合成系统 | 材料、CandyStick、Craft 按钮，参考 `04_crafting_clean.png` |
| 2:30 | 技能树 | 技能图标、技能说明和成长反馈，参考 `05_skilltree_clean.png` |
| 3:30 | Boss 战 | 糖蚀巫师 / Boss 战斗高潮，参考 `06_boss_combat_clean.png` |
| 4:00 | Victory / 结局 | Demo Complete / Victory / 回家装置修复文本，最终录屏时补拍 |

一句话展示目标：

```text
3–5 分钟展示完整游戏闭环：穿越开场、NPC 对话、战斗、成长、合成、Boss 战与结局。
```

## Must-Capture 7 Shots

这些镜头也对应 `Submission/screenshots/` 的截图文件名：

1. `01_title_or_spawn.png`：出生点 / 糖果异世界开场画面。
2. `02_intro_memory_log.png`：NPC 对话或糖芯工坊日志。
3. `03_early_room_combat.png`：早期房间战斗。
4. `04_level_up_or_growth.png`：经验成长、制作、技力、背包或技能树反馈。
5. `05_mid_room_enemy.png`：中段敌人或更强怪物。
6. `06_boss_room.png`：Boss 房或糖蚀巫师战斗。
7. `07_victory_screen.png`：Demo Complete / Victory / 结局文本。

## Screenshot Tool

可在 Unity 里使用：

```text
Tools > Hackathon > Capture Submission Screenshots
```

截图会保存到：

```text
Submission/screenshots/
```

具体步骤见：

```text
docs/SCREENSHOT_CAPTURE_GUIDE.md
```

## Current Clean Screenshots

这些图片已经进入 `Submission/clean_screenshots/`，可直接用于海报和 PPT：

1. `01_move_jump_attack_clean.png`
2. `02_npc_dialogue_clean.png`
3. `03_backpack_clean.png`
4. `04_crafting_clean.png`
5. `05_skilltree_clean.png`
6. `06_boss_combat_clean.png`
7. `06_boss_combat_hero_crop.png`

清理脚本：

```text
tools/clean_submission_screenshots.py
```

清理只删除录屏提示、视频字幕和无关边缘，不伪造游戏 UI。

## Recording Checklist

- Unity Console 没有红色编译错误。
- Game View 使用 16:9。
- 对话文字不乱码。
- UI 不遮挡角色。
- 主角、敌人、NPC 和地形清楚可见。
- 如果录 WebGL 版本，确认链接不是 `localhost`。
- 如果录 Editor Play 版本，尽量不要露出 Inspector / Console。

## Do Not Record

- 不要录 Unity 编译等待过程。
- 不要录素材导入过程。
- 不要长时间停在 Inspector 或 Console。
- 不要把未回填的 `localhost` 链接当正式链接展示。
- 不要使用其他游戏截图或视频片段。
