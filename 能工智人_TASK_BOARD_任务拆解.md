# TASK_BOARD.md - 《能工智人》任务拆解

## 当前阶段判断

先不要追求完整 Roguelike。当前目标是 7 天内做出一个完整闭环：

主菜单 → 一间地牢 → 打怪/开箱/拿材料 → 做机械 → Boss 登场 → 击败 Boss → AI 漫画战报。

---

## Sprint 0：准备

- [ ] 安装 Unity Hub
- [ ] 安装 Unity 6 LTS
- [ ] 创建 2D Core 项目
- [ ] 创建 GitHub 仓库
- [ ] 添加 README.md
- [ ] 添加 AGENTS.md
- [ ] 添加 `.gitignore`
- [ ] 第一次 git commit

验收：Unity 项目能打开，仓库能提交。

---

## Sprint 1：代码骨架

- [ ] `GameManager.cs`
- [ ] `GameState.cs`
- [ ] `RunStats.cs`
- [ ] `PlayerStats.cs`
- [ ] `PlayerInventory.cs`
- [ ] `IAiNarrativeService.cs`
- [ ] `MockAiNarrativeService.cs`
- [ ] `MachineData.cs`
- [ ] `EnemyBase.cs`

验收：Unity 编译无错误。

Codex Prompt：

```text
请按 README 和 AGENTS 生成第一阶段代码骨架，只写核心类和接口，保证 Unity 可编译，不要做复杂场景。
```

---

## Sprint 2：玩家战斗

- [ ] WASD 移动
- [ ] 摄像机跟随
- [ ] 普通攻击
- [ ] 受伤/死亡
- [ ] HUD 显示血量

验收：玩家能在场景中移动并攻击假目标。

---

## Sprint 3：敌人和掉落

- [ ] 追击怪
- [ ] 远程怪
- [ ] 坦克怪
- [ ] 怪物生成点
- [ ] 死亡掉落材料
- [ ] 宝箱交互

验收：打死怪物后背包材料增加。

---

## Sprint 4：机械系统

- [ ] 材料数据
- [ ] 机械数据
- [ ] 自动弩塔
- [ ] 弹簧地刺
- [ ] 蒸汽地雷
- [ ] 机械制作 UI
- [ ] 机械部署逻辑
- [ ] 机械击杀统计

验收：至少一种机械能打死敌人。

---

## Sprint 5：Boss 闭环

- [ ] Boss 触发区域
- [ ] Boss 漫画登场 UI
- [ ] Boss 基础行为
- [ ] Boss 根据流派换台词
- [ ] Boss 死亡进入结算

验收：从开局到击败 Boss 能跑通。

---

## Sprint 6：AI Mock 叙事

- [ ] 工坊助手建议
- [ ] Boss 台词生成
- [ ] 三格漫画战报
- [ ] 玩家风格判断
- [ ] API 失败兜底设计文档

验收：不同玩法触发不同结算文案。

---

## Sprint 7：路演打磨

- [ ] 主菜单漫画风
- [ ] HUD 漫画风
- [ ] 蓝图 UI
- [ ] 战报 UI
- [ ] 录屏路线
- [ ] 90 秒演示视频
- [ ] 3 分钟路演稿
- [ ] 开源许可证审计

验收：项目能给评委演示，不需要解释半天才能看懂。

---

## 今日必须完成

1. 安装 Unity Hub 和 Unity 6 LTS。
2. 建 GitHub 仓库。
3. 把 README.md 和 AGENTS.md 放进仓库。
4. 让 Codex 只做代码骨架，不要做完整游戏。
5. Unity 打开后确认无编译错误。

---

## 今日不要做

- 不要找素材包堆美术。
- 不要做技能树。
- 不要接真实 AI API。
- 不要写完整剧情。
- 不要随机生成大地图。
- 不要复制别人的完整项目。

