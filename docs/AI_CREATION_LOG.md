# AI Creation Log

本文档用于比赛提交时说明 AI 在《能工智人：糖芯工坊》中的使用方式。

## Project

- 中文名：能工智人：糖芯工坊
- English: Craftsmen and Homo Sapiens: The Candy Forge
- Track: 叙事类游戏 / Narrative Games

## AI Usage Summary

本项目没有在运行时接入实时 AI API。AI 主要作为创作和开发辅助工具，用于生成、整理和包装叙事内容，并辅助完成 Unity 原型、提交材料和展示视觉。

## AI-Generated / AI-Assisted Content

### Worldbuilding

AI 辅助生成“糖果异世界 + 女仆工程师 + 合成科技 + Boss 回家”的轻喜剧叙事方向。

核心设定：

```text
糖芯王国是一座由“糖芯炉”驱动的异世界。这里的甜点不是食物，而是能源、武器、魔法和机械零件。主角洛辰原本是现实世界的理工男大学生，在实验事故后穿越到这里，被系统错误绑定为“见习女仆工程师”。为了回到现实世界，他必须学习技能、收集糖果材料、合成装备与道具，并击败污染糖芯炉的糖蚀巫师。
```

### Candy Forge Logs

AI 辅助生成糖芯工坊日志，用于把剧情嵌入 NPC 对话、房间推进和 Demo 展示流程。

示例：

```text
糖芯日志 01：
未知人类个体坠入糖芯工坊。检测到异常知识结构：工程学、动画数据库、熬夜经验。系统职业绑定失败，已临时分配为“见习女仆工程师”。

糖芯日志 04：
CandyStick、Moonthread、Scrapguard……这些看起来像零食的材料，经过工坊装置处理后，可以变成真正能打 Boss 的装备。

糖芯日志 06：
糖蚀巫师守在传送门前。他不是最终答案，而是阻止你回家的最后一道 Bug。
```

游戏中对应位置：

- NPC 对话
- 糖芯工坊日志
- Boss 背景文本
- 结局 / Demo Complete 文本
- Demo 展示讲解文案

### Character / Enemy / Boss Lore

AI 辅助生成：

- 主角：洛辰 / 见习女仆工程师，现实世界理工男大学生。
- NPC：糖芯档案员，负责引导操作和解释糖芯工坊日志。
- 普通敌人：被污染糖芯能量影响后失控的暴走甜点精灵。
- Demo Boss：糖蚀巫师，污染糖芯炉并阻止主角回家。

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

### Real Screenshot Cleanup

团队提供了 6 张项目实际运行截图，Codex 辅助清理录屏遮挡并用于海报和 PPT：

```text
Submission/raw_screenshots/
Submission/clean_screenshots/
tools/clean_submission_screenshots.py
```

清理内容包括：

- 裁掉视频字幕区域。
- 遮掉 NVIDIA 录屏提示。
- 裁剪背包、合成、技能树等 UI 局部，让 PPT 更清楚。
- 为 Boss 战生成海报和封面用的 16:9 主视觉裁切。

这些处理只去除非游戏内容，不伪造不存在的 UI、角色、关卡或胜利画面。

### Poster and PPT

新版海报和 PPT 使用项目真实截图与糖芯工坊世界观重做：

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
- Demo Link、Video Link 等外部链接仍需人工回填。

### Development Assistance

AI / Codex 辅助完成：

- Unity C# 脚本拆分、注释与调试思路。
- NPC 对话、背包、制作、技力、战斗流程相关文档。
- WebGL 构建菜单和静态部署准备脚本。
- 海报、PPT、提交文案、录屏指南和最终检查清单。

## Human Review

AI 生成或整理的内容由团队人工筛选、修改和接入 Unity 项目。最终设计、素材授权确认、部署和提交由团队成员人工负责。
