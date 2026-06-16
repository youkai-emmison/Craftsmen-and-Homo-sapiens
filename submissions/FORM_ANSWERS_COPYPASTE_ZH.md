# 腾讯云黑客松提交问卷复制稿

这个文件按常见提交问卷字段整理。填表时可以直接复制对应段落，再把外部链接从 `submissions/LINKS_TO_FILL.md` 回填进来。

## 1. 作品名称

能工智人 / Craftsmen and Homo sapiens

## 2. 参赛赛题

赛题三：叙事类游戏 / AI 重塑叙事体验

## 3. 一句话介绍

《能工智人》是一款 Unity 2D 横版动作地牢 Demo，玩家进入由 AI 档案重构的地下工坊，在战斗、成长和 Boss 挑战中逐步理解“工匠”与“智人”分裂的真相。

## 4. 作品简介，短版

《能工智人》是一个面向腾讯云黑客松赛题三制作的 Unity 2D 横版动作地牢 Demo。玩家从地下工坊入口出发，依次经历 Early Room、Mid Room 和 Boss Room，在移动、跳跃、攻击、成长和清房过程中阅读 AI 档案式叙事，最终击败 Demo Boss，完成一条 3 分钟左右的可演示闭环。

## 5. 作品简介，长版

《能工智人 / Craftsmen and Homo sapiens》面向 AI CAN DO IT | 腾讯云黑客松游戏开发挑战赛制作，参赛方向为“叙事类游戏 / AI 重塑叙事体验”。项目以 Unity 2D 横版动作 Roguelite 原型为基础，将 AI 生成的世界观、房间档案、角色设定和结局文本嵌入游戏流程。

玩家进入一座由 AI 档案重构的地下工坊，依次经过 Early Room、Mid Room 和 Boss Room。过程中玩家会完成移动、跳跃、近战攻击、击败异常敌人、获得成长反馈、解锁出口和挑战 Demo Boss 等操作。游戏中的叙事内容以 NPC 对话、房间提示和档案记录的形式出现，让关卡不只是战斗空间，也是逐步揭示“工匠”与“智人”分裂真相的叙事片段。

当前版本不是完整商业游戏，而是一个 3 到 5 分钟可试玩、可录屏、可说明 AI 创作价值的黑客松 Demo。

## 6. 项目亮点

- AI 参与世界观、房间档案、NPC 台词、敌人和 Boss 设定生成。
- 短流程内包含开始、成长、清房、Boss 和胜利反馈。
- 横版动作、背包、制作、技力、机械装置和对话系统形成基础 Demo 支撑。
- 用 CodeBuddy / AI 编程助手辅助 Unity C# 脚本、文档、PPT 和提交材料整理。
- 提交材料包含项目书、PPT、视频脚本、WebGL 部署清单、CodeBuddy 导出清单和评委快速打开指南。

## 7. AI 使用说明，短版

本项目使用 AI 辅助完成三类工作：第一，生成地下工坊世界观、房间档案、NPC 台词、敌人与 Boss 设定；第二，规划主角、怪物、道具、地图和 UI 的占位视觉方向；第三，使用 CodeBuddy / AI 编程助手辅助 Unity C# 脚本开发、调试、文档整理和提交材料准备。最终提交将附带 CodeBuddy 历史记录，证明 AI 在开发过程中的参与。

## 8. AI 使用说明，长版

本项目将 AI 用作叙事创作、视觉辅助和编程协作工具。

叙事方面，AI 辅助生成地下工坊世界观、“工匠”与“智人”的核心设定、房间档案、NPC 对话、敌人与 Boss 说明以及胜利后的总结文本。这些内容被整理进游戏内提示、项目书、PPT 和 Demo 视频脚本中。

视觉方面，AI 辅助规划主角、怪物、道具、装备、材料、地图和 UI 的占位方向，用于快速统一黑客松 Demo 的展示风格。第三方资源和 AI 生成资源会在素材记录文档中注明来源和用途。

开发方面，CodeBuddy / AI 编程助手参与 Unity C# 脚本开发、调试、架构拆分、文档整理、PPT 大纲、视频脚本和提交清单准备。项目不会把 AI 参与伪装成人工完成，最终会提交 CodeBuddy 历史对话导出文件作为佐证。

## 9. 技术实现说明

- 游戏引擎：Unity 2022.3 LTS
- 主要语言：C#
- 目标平台：WebGL 浏览器
- 核心模块：
  - Player：移动、跳跃、攻击、朝向、血量、技力。
  - Combat：伤害接口、近战命中、攻击反馈、装置炮台。
  - Enemy：敌人状态机、敌人血量、攻击、Boss 占位。
  - Rooms：房间清理、出口门、三段式 Demo 流程。
  - Inventory / Crafting：背包、物品、配方、装备和制作面板。
  - Dialogue：NPC 对话触发、对话序列和弹出式对话框。
  - UI：血条、技力条、背包、制作、角色信息和提示面板。

## 10. 当前完成度

当前仓库已经具备可演示原型基础：

- Player 横版移动、跳跃、攻击。
- 敌人与 Boss 占位战斗。
- 房间推进和出口解锁。
- 背包、制作、技力、机械炮台等辅助系统。
- NPC 对话与 AI 档案叙事文档。
- 项目书、PPT 初稿、PDF 预览、视频脚本、WebGL 部署说明、CodeBuddy 导出清单。

最终提交前仍需要外部补齐 WebGL 在线试玩链接、Demo 视频链接、CodeBuddy 历史记录导出文件和团队真实信息。

## 11. 可复制链接区

填表前请先在 `submissions/LINKS_TO_FILL.md` 中补齐以下链接。

- WebGL 在线试玩链接：https://craftsmen-and-homo-sapiens.onrender.com
- Demo 视频链接：待回填
- GitHub 仓库链接：https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master
- PPT 文件链接：待回填
- CodeBuddy 历史记录链接：待回填
- 社交媒体发布链接：可选，待回填

## 12. 作品网页 / WebGL 页面简介

欢迎试玩《能工智人 / Craftsmen and Homo sapiens》。这是一个 Unity 2D 横版动作地牢 Demo，玩家将在 AI 档案重构的地下工坊中完成移动、跳跃、战斗、成长、清房和 Boss 挑战。项目使用 AI 辅助生成世界观、房间档案、角色设定和提交材料，并使用 CodeBuddy / AI 编程助手辅助 Unity 原型开发。

推荐先按 `A / D` 移动、`Space` 跳跃、`J` 或鼠标左键攻击，跟随 NPC 和房间提示从 Early Room 推进到 Boss Room。

## 13. Demo 视频简介

本视频展示《能工智人》的完整黑客松 Demo 流程：玩家从地下工坊入口出发，完成基础移动和攻击，击败 Early Room 和 Mid Room 中的异常敌人，获得成长反馈，最后进入 Boss Room 并击败 Demo Boss。视频同时说明 AI 如何参与世界观、房间档案、角色设定和开发协作。

## 14. GitHub 仓库说明

仓库包含 Unity 项目、C# 脚本、文档、提交材料、PPT 初稿、WebGL 部署说明和 CodeBuddy 提交检查清单。第三方素材和 AI 生成素材的使用情况记录在 `docs/THIRD_PARTY_ASSETS.md` 中，最终提交材料集中在 `submissions/` 目录。

## 15. 提交前提醒

- 不要把本地路径当作在线链接提交。
- WebGL 在线试玩链接：https://craftsmen-and-homo-sapiens.onrender.com
- Demo 视频链接必须公开可访问。
- CodeBuddy 历史记录需要能证明 AI 编程助手参与开发。
- PPT 最后一页、提交问卷、作品页和 `FINAL_SUBMISSION_INFO.md` 中的链接应保持一致。
