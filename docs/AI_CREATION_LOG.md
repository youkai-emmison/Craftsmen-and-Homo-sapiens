# AI Creation Log

本文档用于比赛提交时说明 AI 在《能工智人：遗忘工坊》中的使用方式。

## Project

- 中文名：能工智人：遗忘工坊
- English: Craftsmen and Homo Sapiens: The Forgotten Forge
- Track: 叙事类游戏 / Narrative Games

## AI Usage Summary

本项目没有在运行时接入实时 AI API。AI 主要作为创作和开发辅助工具，用于生成、整理和包装叙事内容，并辅助完成 Unity 原型、提交材料和展示视觉。

## AI-Generated / AI-Assisted Content

### Worldbuilding

AI 辅助生成地下工坊、工匠文明、智人文明、遗忘档案、异常构造体等世界观方向。

核心设定：

```text
地下工坊曾试图把工匠的技艺和智人的判断力融合进“智核”。实验失败后，工坊记忆被封存。玩家进入遗迹，在记忆日志中读取真相。
```

### Room Memory Logs

AI 辅助生成房间记忆日志，用于把剧情嵌入战斗和探索流程。

示例：

```text
炉心记录：工匠们把判断力交给机器，把记忆交给人类。协议中断后，房间仍在重复旧命令。
```

游戏中对应位置：

- NPC 对话
- 房间提示
- Boss 背景文本
- 结局 / Demo Complete 文本
- Demo 录屏讲解文案

### Character / Enemy / Boss Lore

AI 辅助生成：

- 玩家身份：进入遗忘工坊的探索者。
- NPC：档案员与技术员，负责引导记忆日志和玩法提示。
- 普通敌人：工坊异常生物、失控构造体、精英怪视觉占位。
- Demo Boss：核心哨兵或最终异常体。

这些设定服务于原创叙事氛围，不复制任何商业作品角色、剧情、UI、音乐或数值。

### Game Visual Asset Presentation

Codex 脚本整理项目内真实素材，并生成提交展示图：

```text
Submission/visual_assets/player_lineup.png
Submission/visual_assets/enemy_lineup.png
Submission/visual_assets/npc_lineup.png
Submission/visual_assets/asset_contact_sheet.png
Submission/visual_assets/character_cards.png
Submission/visual_assets/hero_stage.png
Submission/visual_assets/gameplay_loop_route.png
Submission/visual_assets/memory_log_mock.png
Submission/visual_assets/demo_timeline.png
Submission/visual_assets/tech_architecture.png
Submission/visual_assets/submission_status_board.png
```

这些展示图来自仓库内已有游戏素材，包括：

- `Assets/Art/Generated/Characters/`
- `Assets/Art/Generated/Enemies/`
- `Assets/Art/Generated/NPC/`
- `Assets/Art/Generated/Items/`
- `Assets/Art/Generated/Devices/`
- `Assets/Art/Tiles/`

### Poster and PPT

新版海报和 PPT 使用项目真实素材重做：

```text
Submission/poster_1920x1080.png
Submission/poster_source.svg
Submission/poster_source.html
Submission/project_deck.pptx
Submission/project_deck.pdf
```

注意：

- 没有使用其他游戏截图。
- 没有伪造 Unity 实机截图。
- 没有声称已经部署或已经录制最终视频。
- 缺失的实机截图位置会明确标注为“等待 Unity 截图回填”。

### Development Assistance

AI / Codex 辅助完成：

- Unity C# 脚本拆分、注释与调试思路。
- NPC 对话、背包、制作、技力、战斗流程相关文档。
- WebGL 构建菜单和静态部署准备脚本。
- 海报、PPT、提交文案、录屏指南和最终检查清单。

## CodeBuddy Export Placeholder

最终提交前需要补入 CodeBuddy / AI 对话历史导出链接或文件。

```text
待回填：CodeBuddy 历史导出链接或文件名
```

## Human Review

AI 生成或整理的内容由团队人工筛选、修改和接入 Unity 项目。最终设计、素材授权确认、部署和提交由团队成员人工负责。
