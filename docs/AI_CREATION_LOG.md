# AI 创作说明

## 目的

本文件用于比赛提交时说明：项目中哪些内容由 AI 辅助生成、AI 如何参与创作、这些内容如何体现在游戏 Demo 和提交材料中。

## AI 使用总览

本项目主要使用 AI 完成以下工作：

- 世界观与剧情包装。
- 房间叙事日志。
- 角色、敌人与 Boss 设定。
- 部分占位图像和 sprite sheet 设计提示词。
- Unity C# 脚本实现、调试思路和文档整理。
- 黑客松项目书、PPT 大纲、视频脚本和提交清单整理。

## 世界观生成内容

核心设定：

> 地下工坊曾试图把工匠的技艺和人类的判断力融合进“智核”。实验失败后，工匠失去自我，智人依赖机械延续生存。玩家进入被封存的工坊，在 AI 档案的引导下回收记忆碎片，击败异常敌人，重建事故真相。

AI 生成方向：

- 废弃地下工坊。
- 工匠与智人的分裂。
- 异常敌人来自错误训练的工艺模型。
- Boss 是失控的档案守门人或智核守卫。

## 房间叙事文本

### Opening / Start

档案记录：

> Archive boot sequence restored. Visitor identity unknown. The workshop still remembers the hands that built it, but not the people they belonged to.

中文含义：

> 档案系统重新启动。访客身份未知。工坊仍记得创造它的双手，却忘记了那些双手原本属于谁。

### Early Room

档案记录：

> Training chamber unlocked. Minor constructs are still following obsolete orders. Break them, collect the remaining signal, and prove that your body can still learn.

中文含义：

> 训练室已解锁。低级构造体仍在执行过期命令。击败它们，回收残留信号，证明你的身体仍能学习。

### Mid Room

档案记录：

> The second chamber stores failed correction data. Each enemy here was once a repair routine. Now they only know how to reject the living.

中文含义：

> 第二房间保存着失败的修正数据。这里的每个敌人都曾是修复程序，如今却只会排斥活物。

### Boss Room

档案记录：

> Core sentinel awake. It was built to protect human knowledge from machines. It has forgotten which side the player belongs to.

中文含义：

> 核心哨兵苏醒。它原本用于保护人类知识不被机械吞没，却已经忘记玩家究竟属于哪一边。

### Victory

档案记录：

> The sentinel falls. The archive does not declare you human or machine. It records a third answer: a maker who remembers, and a survivor who can build.

中文含义：

> 哨兵倒下。档案没有判定你是人类还是机械，而是记录了第三种答案：一个仍能记忆的制造者，一个仍能建造的幸存者。

## 角色设定

### Player

- 身份：被唤醒的探索者。
- 功能：能移动、跳跃、攻击，并通过战斗获得成长。
- 叙事意义：玩家是工匠与智人之间的“第三答案”。

### 普通敌人

- 来源：过期训练构造体、失败修复程序、异常档案碎片。
- 功能：用于展示基础战斗和成长反馈。
- 叙事意义：敌人不是单纯怪物，而是工坊失控记忆的一部分。

### Demo Boss

- 身份：核心哨兵 / 档案守门人。
- 功能：作为 3 到 5 分钟 Demo 的最终目标。
- 叙事意义：检验玩家是否能重建工坊真相。

## AI 视觉辅助

当前项目中存在多类 AI 或工具辅助的占位视觉：

- 女仆主角 sprite sheet。
- 怪物帧序列。
- 道具与装备图标。
- 地图与 UI 风格参考。

第三方资源和生成资源需要分别记录。第三方素材详见：

`docs/THIRD_PARTY_ASSETS.md`

## AI 编程辅助

AI / CodeBuddy 可用于：

- 生成 Unity 脚本草案。
- 拆分脚本职责。
- 解释报错并提出修复方案。
- 生成提交文档。
- 整理 WebGL 部署步骤。

比赛提交前应补充：

- CodeBuddy 历史对话导出文件。
- 关键 prompt 或对话摘要。
- 哪些模块由 AI 辅助生成，哪些由人工调整。

## 可放入 PPT 的总结

本项目将 AI 的作用拆成三层：

1. AI 帮助开发：辅助生成 C# 脚本、文档和调试方案。
2. AI 帮助创作：生成世界观、角色档案、房间日志和结局文本。
3. AI 帮助表达：把原本普通的横版动作 Demo 包装成一段“读取地下工坊 AI 档案”的叙事体验。
