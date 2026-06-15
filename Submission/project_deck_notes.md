# Project Deck Notes V2

新版 PPT 先依据 `Submission/layout_plan_v2.md` 规划，再用 `@oai/artifact-tool` 生成可编辑 PPTX。

## Slide Layouts

| # | Title | Layout | Real Assets | Purpose |
| - | - | - | - | - |
| 1 | 能工智人：遗忘工坊 | 大主视觉封面 | hero_stage.png | 第一眼说明这是可爱工坊风的叙事动作游戏。 |
| 2 | 游戏是什么 | 左图右文 | player_lineup.png | 用主角动作展示和三个关键词解释游戏类型。 |
| 3 | 核心玩法循环 | 横向路线图 | gameplay_loop_route.png | 把房间推进、战斗、成长和结局画成路线。 |
| 4 | 角色与怪物 | 阵容卡牌墙 | character_cards.png, enemy_lineup.png | 展示主角、NPC、普通怪、精英怪和 Boss 占位。 |
| 5 | AI 叙事如何进入游戏 | 日志 UI 展示 | memory_log_mock.png | 说明 AI 生成内容如何变成工坊记忆日志。 |
| 6 | Demo 录屏路线 | 时间轴 + 截图占位 | demo_timeline.png | 给同学录视频和后续截图回填使用。 |
| 7 | 技术与部署结构 | 模块架构图 | tech_architecture.png | 展示 Player、Combat、Enemy、UI、Dialogue、WebGL 的关系。 |
| 8 | 亮点与提交状态 | 左右清单看板 | submission_status_board.png | 诚实说明已完成与待完成事项。 |

## QA Notes

- 每页采用不同布局：封面、左图右文、路线图、卡牌墙、日志 UI、时间轴、架构图、提交看板。
- 所有游戏视觉来自仓库内真实素材或由脚本从真实素材整理出的展示图。
- `demo_timeline.png` 和海报中的截图框仍明确标注“等待 Unity 截图回填”。
- 本次没有伪造 WebGL 部署、Demo 视频、Unity 截图或 CodeBuddy 历史。
- PPTX 已导出到 `Submission/project_deck.pptx`，PNG 预览导出到 `Submission/project_deck_assets/`。
