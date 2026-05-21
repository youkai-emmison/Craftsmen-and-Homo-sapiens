# 能工智人 / Nengong Zhiren

> 腾讯云黑客松·游戏开发挑战赛 2026 赛道 3「叙事剧情游戏」候选项目。  
> 核心定位：**手绘机械漫画风 Roguelike 叙事游戏**。  
> 当前目标：先做出一个稳定、可演示、可答辩的 Unity 2D MVP，而不是一口气做完整商业游戏。

---

## 0. 给 Codex 的一句话任务

请在本仓库中实现一款 Unity 2D 单机 Roguelike Demo，项目名为《能工智人》。玩家在地下工坊中探索、战斗、收集装备与材料，并在「武器装备流」和「机械智斗流」之间选择成长路线。UI 采用手绘漫画/机械蓝图风。AI 不是简单聊天，而是作为「漫画导演」和「工坊助手」生成关卡事件、Boss 台词、机械推荐和结算战报。

优先做 **能跑通的一关 Demo**：主菜单 → 地牢关卡 → 击杀怪物/开宝箱/拾取材料 → 部署机械装置 → Boss 登场 → 击败 Boss → 漫画战报结算。

---

## 1. 项目背景

### 1.1 原始策划要点

项目原案：

- 项目名：能工 OR 智人
- 类型：Roguelike，2D 单机游戏
- 核心流程：击杀怪物、开宝箱、完成隐藏任务，获取装备和材料，提升实力，最终击败 Boss
- 通关条件：击败最终层 Boss
- 两条战斗流派：
  - 装备武器流：玩家穿戴武器、防具、饰品，提升自身战斗力
  - 机械智斗流：玩家制作机械设备，破坏地形或部署装置击杀怪物
- 系统设想：角色等级、天赋、技能树、装备词条、金币、装备背包、材料背包
- 原美术方向：像素地牢，后续调整为「手绘机械漫画风 UI + 简洁 2D 战斗」

### 1.2 当前设计升级

为了匹配赛道 3「叙事剧情游戏」，本项目不只做普通 Roguelike，而是把每局冒险包装成一本动态生成的「地下工坊漫画手账」。

AI 的定位：

1. **AI 漫画导演**：根据玩家行为生成关卡事件、Boss 登场台词、结算战报。
2. **AI 工坊助手**：根据玩家已有材料、敌人类型、当前关卡状态，推荐机械装置。
3. **AI Boss 适配器**：根据玩家更偏向装备流还是机械流，调整 Boss 台词和部分策略参数。

MVP 中 AI 可以先用本地假数据/模板模拟，后续再接真实大模型 API。

---

## 2. 技术路线

### 2.1 推荐技术栈

- 游戏引擎：Unity 5 LTS / Unity 2D
- 语言：C#
- 游戏类型：2D Top-down Roguelike
- 数据：ScriptableObject + JSON 配置
- AI 服务：先 Mock，后续可接 FastAPI + OpenAI-compatible API / 腾讯云模型
- UI 风格：手绘漫画边框、纸张贴片、机械蓝图、漫画气泡
- 版本管理：Git + GitHub
- AI 编程：Codex / Cursor / Claude Code 均可，优先让 Codex 小步提交

### 2.2 为什么选 Unity 而不是 Web

当前项目有角色移动、碰撞、敌人行为、地图、掉落、背包、技能/装备/机械部署等游戏机制。Unity 比 React/Phaser 更适合快速做 2D 动作 Demo。Web 方案适合剧情模拟/卡牌/对话类项目，但你们这个案子已经是 Roguelike + 战斗 + 机械部署，Unity 风险更低。

### 2.3 没安装 Unity 时也能做什么

在安装 Unity 前，Codex 仍然可以先完成：

- 生成项目 README、AGENTS.md、任务拆解文档
- 创建 Unity 项目目录结构：`Assets/Scripts`、`Assets/Resources/Data`、`Assets/ArtPlaceholders`
- 编写纯 C# 游戏逻辑脚本
- 编写 JSON 配置：装备、怪物、机械设备、关卡事件
- 编写 Mock AI 响应数据
- 编写伪代码、类图、接口文档

但以下事情必须等 Unity Editor 安装后验证：

- Scene 场景是否能正确打开
- 组件是否正确挂载
- Prefab 是否正常实例化
- Tilemap、Collider、Animator、Canvas 是否配置正确
- Play Mode 是否无报错
- WebGL/Windows Build 是否能打包

---

## 3. MVP 范围

### 3.1 只做一关，但必须完整

MVP 不做完整多层地牢。先做一个可玩的「地下工坊第一层」。

必须包含：

1. 主菜单
2. 玩家角色移动
3. 基础攻击
4. 3 种普通怪物
5. 1 个 Boss
6. 2 种武器
7. 3 种材料
8. 3 种机械装置
9. 宝箱/掉落
10. 简单背包
11. 机械合成/部署
12. Boss 登场漫画 UI
13. 通关结算漫画战报
14. AI Mock 系统

### 3.2 暂时不做的内容

这些功能先不要做，避免项目爆炸：

- 完整技能树
- 大量装备词条
- 多角色
- 联机
- 大地图随机生成
- 完整剧情分支树
- 全实时 AI 控制怪物
- 复杂物理破坏地形
- 商城和复杂 NPC 系统
- 真实美术全量资产

---

## 4. 游戏核心循环

```text
开始游戏
  ↓
进入地下工坊第一层
  ↓
击杀怪物 / 躲避怪物 / 搜索宝箱
  ↓
获得金币、装备、材料
  ↓
选择路线：强化自身装备 or 制作机械装置
  ↓
部署机械装置辅助战斗
  ↓
触发 AI 关卡事件 / Boss 漫画登场
  ↓
Boss 根据玩家路线说出不同台词和使用不同策略
  ↓
击败 Boss
  ↓
AI 生成漫画战报结算
  ↓
展示本局风格标签：能工 / 智人 / 混合流
```

---

## 5. 玩家体验目标

### 5.1 爽点

- 前期：打怪、开箱、拿材料，快速建立成长反馈。
- 中期：玩家开始选择路线，是亲自冲进怪群，还是放机械装置控场。
- 后期：Boss 根据玩家打法做出回应，让玩家感觉「系统看懂了我的玩法」。
- 结算：AI 用漫画战报总结本局，让比赛评委一眼看出叙事创新。

### 5.2 关键路演演示效果

演示时必须出现这些画面：

1. 手绘漫画风主菜单
2. 玩家打怪并拾取材料
3. 打开机械蓝图界面，制作机械装置
4. 部署机械装置击杀怪物
5. Boss 漫画分镜登场，并说出和玩家行为相关的台词
6. 通关后生成漫画战报

---

## 6. 项目目录结构

Codex 应按以下结构组织项目：

```text
NengongZhiren/
├── README.md
├── AGENTS.md
├── LICENSE
├── NOTICE.md
├── docs/
│   ├── GDD.md
│   ├── TECH_ARCHITECTURE.md
│   ├── AI_DESIGN.md
│   ├── TASK_BOARD.md
│   └── OPEN_SOURCE_AUDIT.md
├── Assets/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── DungeonDemo.unity
│   │   └── ResultComic.unity
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── GameState.cs
│   │   │   ├── RunStats.cs
│   │   │   └── ServiceLocator.cs
│   │   ├── Player/
│   │   │   ├── PlayerController.cs
│   │   │   ├── PlayerCombat.cs
│   │   │   ├── PlayerStats.cs
│   │   │   └── PlayerInventory.cs
│   │   ├── Enemy/
│   │   │   ├── EnemyBase.cs
│   │   │   ├── EnemyChaser.cs
│   │   │   ├── EnemyShooter.cs
│   │   │   ├── EnemyTank.cs
│   │   │   └── BossController.cs
│   │   ├── Items/
│   │   │   ├── ItemData.cs
│   │   │   ├── EquipmentData.cs
│   │   │   ├── MaterialData.cs
│   │   │   ├── LootDrop.cs
│   │   │   └── Chest.cs
│   │   ├── Machines/
│   │   │   ├── MachineData.cs
│   │   │   ├── MachineBase.cs
│   │   │   ├── TurretMachine.cs
│   │   │   ├── SpikeTrapMachine.cs
│   │   │   ├── SteamMineMachine.cs
│   │   │   └── MachineCraftingSystem.cs
│   │   ├── AI/
│   │   │   ├── IAiNarrativeService.cs
│   │   │   ├── MockAiNarrativeService.cs
│   │   │   ├── AiPromptBuilder.cs
│   │   │   ├── AiResponseModels.cs
│   │   │   └── AiSafetyGuard.cs
│   │   ├── UI/
│   │   │   ├── MainMenuUI.cs
│   │   │   ├── HudUI.cs
│   │   │   ├── InventoryUI.cs
│   │   │   ├── CraftingBlueprintUI.cs
│   │   │   ├── BossComicIntroUI.cs
│   │   │   └── ResultComicUI.cs
│   │   └── Utils/
│   │       ├── ObjectPool.cs
│   │       ├── Timer.cs
│   │       └── WeightedRandom.cs
│   ├── Resources/
│   │   ├── Data/
│   │   │   ├── enemies.json
│   │   │   ├── equipment.json
│   │   │   ├── materials.json
│   │   │   ├── machines.json
│   │   │   └── narrative_events.json
│   │   └── MockAI/
│   │       ├── boss_lines.json
│   │       ├── workshop_advice.json
│   │       └── comic_reports.json
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Enemies/
│   │   ├── Machines/
│   │   ├── Items/
│   │   └── UI/
│   ├── Art/
│   │   ├── PlaceholderSprites/
│   │   ├── ComicUI/
│   │   └── BlueprintUI/
│   └── Audio/
│       ├── SFX/
│       └── Music/
└── ProjectSettings/
```

---

## 7. 场景设计

### 7.1 MainMenu.unity

目的：让评委第一眼看到手绘漫画风。

元素：

- 游戏标题：《能工智人》
- 漫画封面背景：地下工坊门、齿轮、纸张纹理
- 按钮：开始游戏、机械图鉴、设置、退出
- 可用临时素材，不要求精美，但要风格统一

验收标准：

- 点击「开始游戏」进入 DungeonDemo
- UI 不使用默认蓝色按钮样式
- 标题和按钮具有漫画纸张/手绘边框风格

### 7.2 DungeonDemo.unity

目的：核心玩法演示。

内容：

- 一个封闭地牢房间
- 玩家出生点
- 3 种怪物生成点
- 2 个宝箱
- 1 个材料掉落区域
- 1 个 Boss 触发区域
- 1 个机械合成/部署教程提示

验收标准：

- 玩家可以移动、攻击、受伤、死亡
- 怪物会追击或攻击玩家
- 击杀怪物会掉落金币/材料
- 玩家可打开背包
- 玩家可制作并部署至少 1 种机械
- Boss 能登场、战斗、死亡
- Boss 死亡后进入 ResultComic

### 7.3 ResultComic.unity

目的：展示 AI 叙事创新。

内容：

- 三格漫画式结算页
- 显示玩家路线：装备流 / 机械流 / 混合流
- 显示本局统计：击杀数、开箱数、机械使用数、受伤次数、通关时间
- 显示 AI 战报文本
- 显示「再来一局」和「返回主菜单」

验收标准：

- 至少展示 3 段 AI Mock 文本
- 根据玩家行为改变战报内容
- 战报不是固定单句文本

---

## 8. 输入控制

默认键位：

| 操作 | 键位 |
|---|---|
| 移动 | WASD / 方向键 |
| 普通攻击 | 鼠标左键 / J |
| 交互/开箱 | E |
| 打开背包 | I |
| 打开机械蓝图 | B |
| 部署机械 | 鼠标右键 / K |
| 暂停 | Esc |

---

## 9. 角色与战斗系统

### 9.1 玩家属性

```csharp
public class PlayerStats
{
    public int Level;
    public float MaxHp;
    public float CurrentHp;
    public float Attack;
    public float MoveSpeed;
    public float Defense;
    public int Gold;
    public int KillCount;
    public int MachinePlacedCount;
    public int ChestOpenedCount;
}
```

MVP 默认数值：

| 属性 | 初始值 |
|---|---:|
| 生命值 | 100 |
| 攻击力 | 12 |
| 移速 | 5 |
| 防御 | 0 |
| 金币 | 0 |

注意：不要照搬《死亡细胞》或任何商业游戏数值。所有数值先用简单原创占位值，后续通过测试调整。

### 9.2 武器装备流

MVP 只做 2 个武器：

| 武器 | 效果 |
|---|---|
| 工匠短刃 | 攻击速度快，伤害中等 |
| 重锤扳手 | 攻击速度慢，伤害高，击退强 |

### 9.3 机械智斗流

MVP 只做 3 种机械：

| 机械 | 材料 | 效果 |
|---|---|---|
| 自动弩塔 | 齿轮 x2 + 木片 x1 | 自动射击最近敌人 |
| 弹簧地刺 | 齿轮 x1 + 铁片 x2 | 敌人踩中后受伤并减速 |
| 蒸汽地雷 | 晶核 x1 + 铁片 x1 | 延迟爆炸，范围伤害 |

### 9.4 流派判断

结算时根据统计判断玩家风格：

```text
如果 MachinePlacedCount >= 3 且机器击杀数 >= 玩家击杀数 * 0.5 → 智人流
如果 MachinePlacedCount <= 1 且玩家近战击杀数较多 → 能工流
否则 → 混合流
```

---

## 10. 敌人与 Boss

### 10.1 普通怪物

| 怪物 | 行为 | 作用 |
|---|---|---|
| 鼠形矿工 | 追击玩家，近战攻击 | 基础敌人 |
| 废铁投手 | 保持距离，投掷废铁 | 逼玩家移动 |
| 铁皮守卫 | 血量高，速度慢 | 适合被机械克制 |

### 10.2 Boss：失控工坊核心

Boss 设定：地下工坊的旧 AI 机械核心，能够读取玩家前几分钟的战斗方式。

Boss 根据玩家路线切换台词：

```text
能工流：
“你把自己锻造成了武器。那我就看看，肉身能不能敲碎钢铁。”

智人流：
“你躲在机械背后。很好，我会先拆掉你的工具。”

混合流：
“半个工匠，半个谋士。你的矛盾，正好是你的弱点。”
```

Boss MVP 行为：

- 阶段 1：追击 + 普通攻击
- 阶段 2：召唤小怪 + 发射范围冲击波
- 如果玩家机械使用较多，Boss 优先攻击机械
- 如果玩家装备流明显，Boss 增加冲刺攻击频率

---

## 11. AI 叙事系统

### 11.1 AI 服务接口

先写接口，MVP 使用 Mock 实现。

```csharp
public interface IAiNarrativeService
{
    BossIntro GenerateBossIntro(RunStats stats);
    WorkshopAdvice GenerateWorkshopAdvice(RunStats stats, List<MaterialStack> materials, List<EnemyInfo> enemies);
    ComicReport GenerateComicReport(RunStats stats);
}
```

### 11.2 MockAI 规则

不要一开始就接真实 API。先用规则和随机模板模拟 AI：

```text
输入：RunStats
- killCount
- machinePlacedCount
- machineKillCount
- chestOpenedCount
- damageTaken
- bossDefeated
- playerStyle

输出：
- bossTitle
- bossLine
- workshopAdvice
- comicPanel1
- comicPanel2
- comicPanel3
- finalComment
```

### 11.3 后续真实 AI 接入

后续接入真实 AI 时，必须：

- 限制输出 JSON，不让模型自由输出长文本
- 使用状态机约束剧情阶段
- 加安全过滤，不生成不适合展示的内容
- 失败时回退到 Mock 模板
- 不让 AI 控制所有实时战斗逻辑，避免不稳定

示例 Prompt：

```text
你是《能工智人》的漫画叙事导演。请根据玩家本局行为生成 Boss 登场台词和三格漫画结算文案。

世界观：地下工坊、机械遗迹、手绘漫画风、轻微黑色幽默。

玩家统计：
- 击杀数：{killCount}
- 使用机械次数：{machinePlacedCount}
- 机械击杀数：{machineKillCount}
- 开启宝箱数：{chestOpenedCount}
- 承受伤害：{damageTaken}
- 玩家风格：{playerStyle}

要求：
1. 输出必须是 JSON。
2. 不要超过 120 个中文字符。
3. 不要引入游戏中不存在的角色。
4. 不要改变 Boss 是否死亡、玩家是否通关等事实。
5. 文案要像漫画分镜旁白。

输出格式：
{
  "bossLine": "...",
  "panel1": "...",
  "panel2": "...",
  "panel3": "...",
  "finalComment": "..."
}
```

---

## 12. UI 风格规范

关键词：

- 手绘漫画
- 地下工坊
- 机械蓝图
- 泛黄纸张
- 黑色描边
- 胶带贴纸
- 漫画气泡
- 拟声词
- 结算分镜

### 12.1 主菜单 UI

像一本漫画封面：

```text
《能工智人》
副标题：用蛮力闯过去，还是用机械拆掉这个世界？
按钮：开始冒险 / 机械图鉴 / 设置 / 退出
```

### 12.2 战斗 HUD

- 左上角：血量条，做成撕裂纸条/墨水条
- 右上角：当前区域，做成漫画角标
- 下方：武器、机械快捷栏，做成手绘卡片
- 中间：Boss 登场时出现漫画大分镜

### 12.3 背包 UI

- 左页：装备背包
- 右页：材料背包
- 装备像手绘卡片
- 材料像被胶带贴在纸上

### 12.4 机械蓝图 UI

- 左侧：材料
- 中间：蓝图草稿
- 右侧：可制作机械
- 底部：AI 工坊助手建议

---

## 13. 数据配置示例

### 13.1 machines.json

```json
[
  {
    "id": "turret_basic",
    "name": "自动弩塔",
    "cost": [
      { "materialId": "gear", "count": 2 },
      { "materialId": "wood", "count": 1 }
    ],
    "damage": 8,
    "cooldown": 1.2,
    "range": 5.0,
    "description": "自动攻击最近的敌人。"
  },
  {
    "id": "spike_trap",
    "name": "弹簧地刺",
    "cost": [
      { "materialId": "gear", "count": 1 },
      { "materialId": "iron", "count": 2 }
    ],
    "damage": 18,
    "cooldown": 4.0,
    "range": 1.2,
    "description": "敌人踩中后受伤并减速。"
  },
  {
    "id": "steam_mine",
    "name": "蒸汽地雷",
    "cost": [
      { "materialId": "core", "count": 1 },
      { "materialId": "iron", "count": 1 }
    ],
    "damage": 30,
    "cooldown": 0,
    "range": 2.2,
    "description": "延迟爆炸，造成范围伤害。"
  }
]
```

### 13.2 boss_lines.json

```json
{
  "craftsman": [
    "你把自己锻造成了武器。那我就看看，肉身能不能敲碎钢铁。",
    "没有蓝图，只有蛮力？很好，工坊最喜欢这样的实验品。"
  ],
  "thinker": [
    "你躲在机械背后。很好，我会先拆掉你的工具。",
    "你的装置很聪明，但它们也会背叛你。"
  ],
  "hybrid": [
    "半个工匠，半个谋士。你的矛盾，正好是你的弱点。",
    "你什么都想要，工坊会让你付出双倍代价。"
  ]
}
```

---

## 14. Codex 开工方式

### 14.1 第一次给 Codex 的 Prompt

把下面这段复制给 Codex：

```text
你现在接手 Unity 2D 项目《能工智人》。请先阅读 README.md 和 AGENTS.md，不要急着实现所有功能。

第一阶段目标：搭建可维护的 Unity 2D Roguelike MVP 代码骨架。

请完成：
1. 按 README.md 创建 Assets/Scripts 目录结构。
2. 编写核心 C# 类：GameManager、GameState、RunStats、PlayerStats、PlayerInventory。
3. 编写 AI 叙事接口 IAiNarrativeService 和 MockAiNarrativeService。
4. 编写 MachineData、MachineBase、TurretMachine、SpikeTrapMachine、SteamMineMachine 的基础结构。
5. 编写敌人基类 EnemyBase 和 3 个简单敌人类。
6. 不要导入第三方插件。
7. 不要使用商业游戏素材或数值。
8. 每个脚本要能被 Unity 编译，命名空间统一为 NengongZhiren。
9. 完成后输出你新增/修改的文件列表，并说明下一步需要在 Unity Editor 里如何挂载组件。

暂时不要做复杂场景，不要写联网 AI，不要做完整技能树。
```

### 14.2 第二次给 Codex 的 Prompt

```text
现在请实现最小可玩的 DungeonDemo 场景逻辑。

请完成：
1. PlayerController：支持 WASD 移动。
2. PlayerCombat：支持近战攻击和攻击冷却。
3. EnemyBase：支持生命值、受伤、死亡。
4. EnemyChaser：追击玩家并近战攻击。
5. LootDrop：敌人死亡时掉落材料/金币。
6. Chest：玩家按 E 开箱，获得材料。
7. HudUI：显示血量、金币、材料数量。

要求：
- 代码尽量简单，可读性优先。
- 所有 public 字段加 Tooltip，方便 Unity Inspector 调试。
- 如果找不到引用，用 SerializeField 暴露给 Inspector。
- 不要依赖外部资源，Prefab 可以后续由人工在 Unity 中创建。
```

### 14.3 第三次给 Codex 的 Prompt

```text
现在请实现机械蓝图系统。

请完成：
1. MachineCraftingSystem：根据材料判断能否制作机械。
2. PlayerInventory：支持添加/消耗材料。
3. TurretMachine：自动寻找范围内最近敌人并发射简单 projectile 或直接造成伤害。
4. SpikeTrapMachine：敌人触发后造成伤害和短暂减速。
5. SteamMineMachine：部署后延迟爆炸，造成范围伤害。
6. CraftingBlueprintUI：显示三种机械、材料需求、是否可制作。
7. RunStats：记录部署机械次数、机械击杀数。

要求：
- 机械装置是本项目核心卖点，代码要模块化。
- 后续能够把 AI 工坊助手建议接到 CraftingBlueprintUI 上。
```

### 14.4 第四次给 Codex 的 Prompt

```text
现在请实现 AI Mock 叙事与漫画 UI。

请完成：
1. MockAiNarrativeService：根据 RunStats 返回 BossIntro、WorkshopAdvice、ComicReport。
2. BossComicIntroUI：Boss 出现前显示漫画分镜和台词。
3. ResultComicUI：通关后显示三格漫画战报。
4. RunStats：统计击杀数、开箱数、机械部署数、机械击杀数、受伤次数、通关时间。
5. 根据统计判断玩家风格：能工流、智人流、混合流。

要求：
- 不接真实 API。
- 文案要短，适合 UI 展示。
- UI 可以先用 TextMeshPro + Image 占位。
```

### 14.5 第五次给 Codex 的 Prompt

```text
现在请做代码审查和修复。

请检查：
1. 是否有空引用风险。
2. 是否有 Update 中低效查找对象的问题。
3. 是否有命名混乱或职责过大的类。
4. 是否有无法在 Unity 中编译的 C# 语法。
5. 是否有未使用的 using。
6. 是否有不应该 hardcode 的配置。
7. 是否所有 MVP 功能都有可解释的验收方式。

请做最小必要修改，不要重构成复杂架构。
```

---

## 15. 开源借鉴策略

可以借鉴开源项目，但必须遵守：

1. **优先学习结构和思路，不整段复制代码。**
2. 如果复制或改写了实质性代码，必须确认 License。
3. 如果是 MIT License，通常需要保留原版权声明和许可证文本。
4. 无 License 的 GitHub 仓库默认不能随便复制、修改、再发布。
5. 比赛提交材料中应写明：参考了哪些开源项目、用了哪些素材、许可证是什么。
6. 不要使用商业游戏素材、商业游戏数值、未授权音乐、未授权字体。

### 15.1 推荐参考项目

| 项目 | 用途 | 借鉴点 | 注意 |
|---|---|---|---|
| Unity Learn 2D Roguelike | 官方教程 | Unity 2D Roguelike 基础流程、任务组织、像素美术工作流 | 学习用，别直接照搬成品 |
| intrepion/unity-tutorial-2d-roguelike | 教程实现 | 基础 2D Roguelike 项目结构 | MIT License，仍需记录来源 |
| Chizaruu/Unity-RL-Tutorial | 教程实现 | 初学者 Roguelike 实现思路 | MIT License，需记录来源 |
| KonstantinTomashevich/unity-2d-roguelike | 原型项目 | A*、fog of war、utility-based AI | WIP，不要直接套全项目 |
| Pedro2712/ShadowyKeep | 完整 Roguelike 参考 | 程序化关卡、永久死亡、背包、战斗 | MIT License，学习结构即可 |

在 `docs/OPEN_SOURCE_AUDIT.md` 中记录：

```text
项目名：
链接：
License：
参考内容：
是否复制代码：否 / 是，文件路径：
是否复制素材：否 / 是，文件路径：
处理方式：仅学习 / 改写 / 引用
备注：
```

---

## 16. 任务拆解

### 16.1 版本 A：7 天 MVP

目标：能跑、能演示、有核心差异化。

#### Day 1：环境与项目骨架

产出：

- Unity 项目创建完成
- GitHub 仓库创建完成
- README.md、AGENTS.md、TASK_BOARD.md 完成
- 核心目录结构完成
- `GameManager`、`RunStats`、`PlayerStats` 初版完成

验收：

- Unity 打开无报错
- Git 有第一次提交
- Codex 能读懂项目结构

不够时间砍：暂时不做 UI，只做代码骨架。

有余力增强：做 MainMenu 占位场景。

#### Day 2：玩家与基础战斗

产出：

- 玩家移动
- 玩家近战攻击
- 玩家受伤/死亡
- 摄像机跟随
- 基础 HUD

验收：

- 玩家能移动、攻击、扣血
- Play Mode 不报错

不够时间砍：攻击动画先不做。

有余力增强：加入击退效果。

#### Day 3：敌人与掉落

产出：

- 3 种敌人
- 敌人生成点
- 敌人死亡掉落材料/金币
- 宝箱交互

验收：

- 至少能击杀 5 个怪
- 背包材料数量会变化

不够时间砍：远程怪先砍掉，只保留追击怪。

有余力增强：敌人死亡特效。

#### Day 4：机械系统

产出：

- 3 种材料
- 3 种机械
- 机械制作逻辑
- 机械部署逻辑
- 机械击杀统计

验收：

- 玩家能用材料做出至少 1 种机械
- 机械能对敌人造成伤害

不够时间砍：只做自动弩塔。

有余力增强：蓝图 UI。

#### Day 5：Boss 与关卡闭环

产出：

- Boss 登场触发
- Boss 基础行为
- Boss 死亡通关
- 进入结算页

验收：

- 从开始到击败 Boss 能完整跑通一次

不够时间砍：Boss 只做一种攻击。

有余力增强：Boss 根据流派换台词。

#### Day 6：AI Mock 叙事 + 漫画 UI

产出：

- MockAiNarrativeService
- Boss 漫画登场 UI
- 结算漫画战报 UI
- 玩家风格判断

验收：

- 机械流和装备流结算文案不同
- Boss 登场台词会变

不够时间砍：结算只做文字，不做三格 UI。

有余力增强：三格漫画排版。

#### Day 7：打磨、录屏、提交材料初版

产出：

- 可运行 Build
- 1 分钟演示路线
- 项目截图
- README 完整
- 已知问题列表

验收：

- 新电脑拉代码后能打开项目
- 演示路线不会卡死
- 录屏能看出 AI 叙事和机械玩法

不够时间砍：技能树、图鉴、设置全部砍。

有余力增强：音效和过场动效。

---

### 16.2 版本 B：14 天路演版

目标：有完整可讲的玩法、有 PPT、有演示视频。

第 8-9 天：UI 风格统一

- 主菜单漫画化
- HUD 漫画化
- 背包卡片化
- 蓝图界面漫画化
- Boss 登场分镜优化

第 10 天：关卡节奏优化

- 怪物波次
- 宝箱位置
- 材料掉落概率
- Boss 触发条件
- 通关时间控制在 3-5 分钟

第 11 天：AI 真实接口预留

- 完成 `IAiNarrativeService` 真实 API 实现的接口层
- 真实 API 可选，不影响 Demo
- 加回退策略：API 失败 → Mock 文案

第 12 天：答辩材料

- 3 页技术架构图
- 3 页玩法说明
- 2 页 AI 机制说明
- 1 页开源与版权说明
- 1 页未来规划

第 13 天：演示视频

- 录制 90 秒视频
- 包含主菜单、战斗、机械、Boss、战报
- 准备旁白稿

第 14 天：路演彩排

- 3 分钟路演稿
- 评委 Q&A
- Bug 修复
- 最终 Build

---

### 16.3 版本 C：30 天冲奖版

目标：让评委觉得不是普通小游戏，而是 AI 原生叙事游戏。

第 15-18 天：AI 叙事增强

- 接入真实模型 API
- JSON 输出约束
- Prompt 版本管理
- 安全过滤
- API 失败兜底

第 19-21 天：玩家记忆系统

- 记录玩家习惯：近战偏好、机械偏好、开箱偏好、受伤频率
- Boss 根据玩家行为调整台词和少量策略
- 结算给出玩家风格标签

第 22-24 天：内容扩展

- 增加第二个小关卡
- 增加 2 件装备
- 增加 1 种机械
- 增加 1 个隐藏事件

第 25-27 天：美术与音效

- 统一手绘 UI
- 添加漫画气泡、拟声词
- 添加基础音效
- 添加演示用主视觉图

第 28-30 天：最终提交包

- 稳定 Build
- GitHub README
- PPT
- 视频
- 路演稿
- Q&A
- 开源许可证审计
- 项目复盘

---

## 17. 验收清单

### 17.1 代码验收

- [ ] Unity 打开无编译错误
- [ ] 所有脚本命名清晰
- [ ] 无商业游戏数值/素材
- [ ] 无未记录第三方代码
- [ ] 所有核心功能可在 Play Mode 演示
- [ ] Mock AI 与真实 AI 接口分离
- [ ] API 失败不会导致游戏卡死

### 17.2 游戏验收

- [ ] 玩家能移动、攻击、受伤、死亡
- [ ] 敌人能追击、攻击、死亡
- [ ] 宝箱能打开
- [ ] 材料能拾取
- [ ] 机械能制作和部署
- [ ] Boss 能登场和死亡
- [ ] 能通关并进入结算
- [ ] 结算内容会根据玩家行为变化

### 17.3 比赛展示验收

- [ ] 评委 30 秒内能看懂玩法
- [ ] 评委能看到 AI 的核心作用
- [ ] 画面有明显手绘漫画风
- [ ] 演示不依赖网络也能跑
- [ ] README 能说明项目价值
- [ ] PPT 能解释 AI 不是装饰
- [ ] 开源引用清楚

---

## 18. 风险与降级方案

| 风险 | 表现 | 降级方案 |
|---|---|---|
| Unity 不熟 | 场景搭建慢 | 只做一个房间，用方块占位 |
| 机械系统太复杂 | 部署、寻敌、伤害都出 Bug | 只保留自动弩塔 |
| AI API 不稳定 | 演示时无响应 | 全程使用 Mock AI，答辩说明接口已预留 |
| UI 来不及 | 默认 UI 太丑 | 先做纸张背景 + 黑色描边 + 气泡文本 |
| Boss 太难做 | 行为复杂 | Boss 只做追击 + 冲击波 |
| 数值不会调 | 体验混乱 | 目标通关时间 3-5 分钟，按演示路线调数值 |
| 开源版权不清 | 答辩风险 | 只参考 MIT/官方教程，全部记录到 NOTICE.md |

---

## 19. 提交物建议

最终至少准备：

- Unity 项目源码
- 可运行 Build
- README.md
- AGENTS.md
- NOTICE.md
- docs/GDD.md
- docs/AI_DESIGN.md
- docs/OPEN_SOURCE_AUDIT.md
- 项目 PPT
- 90 秒演示视频
- 3 分钟路演稿
- 评委 Q&A

---

## 20. 仓库提交规范

建议提交信息：

```text
feat: add player movement and combat
feat: add enemy base and chaser enemy
feat: add material inventory and chest interaction
feat: add machine crafting system
feat: add mock AI narrative service
feat: add boss comic intro UI
fix: handle null reference in HUD binding
docs: add open source audit notes
```

每次让 Codex 做任务前：

```bash
git status
git add .
git commit -m "checkpoint: before codex task"
```

每次 Codex 完成后：

```bash
git diff
git status
```

确认无误再提交。

---

## 21. 当前第一步

如果你还没安装 Unity，先做：

1. 创建 GitHub 仓库：`nengong-zhiren`
2. 放入 `README.md`、`AGENTS.md`
3. 安装 Unity Hub
4. 用 Unity Hub 安装 Unity 6 LTS
5. 创建 2D Core 项目
6. 把 Codex 打开在项目根目录
7. 先让 Codex 做「代码骨架」，不要让它直接做完整游戏

第一天最小目标：

```text
仓库能打开，Unity 能打开，Codex 能读懂任务，Assets/Scripts 结构搭好，核心类能编译。
```

