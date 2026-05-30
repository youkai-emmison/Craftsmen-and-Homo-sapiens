# Skill Energy Guide

## 当前目标

技力点是玩家释放技能用的轻量资源。当前只做最小闭环：

- 玩家默认技力上限为 5 点。
- 技力会按固定时间自然恢复。
- 未来装备可以增加技力上限，但本阶段不做装备联动。
- 技能释放系统暂时不做，只保留 `TrySpendSkillEnergy(int cost)` 给后续技能调用。
- 场景里先用玩家头顶蓝色小条显示当前技力。

## 脚本职责

`PlayerSkillEnergy`

- 只负责玩家技力数据。
- 维护当前技力、基础上限、装备加成上限和恢复计时。
- 提供 `TrySpendSkillEnergy`、`RestoreSkillEnergy`、`SetBonusMaxSkillEnergy`。
- 不控制 UI，不创建技能，不处理装备。

`SimpleSkillEnergyBar`

- 只负责把 `PlayerSkillEnergy` 显示成一条蓝色世界空间进度条。
- 读取数据并缩放 `fillTransform`。
- 不修改玩家数据，不处理技能逻辑。

`CharacterInfoPanel`

- 在背包角色信息里显示 `SP: 当前值 / 最大值`。
- 只读数据，不控制技力恢复。

## Unity 配置

`SampleScene` 中 Player 已添加：

- `PlayerSkillEnergy`
- `PlayerSkillEnergyBar`
- `PlayerSkillEnergyBar/Background`
- `PlayerSkillEnergyBar/Fill`

推荐参数：

- `baseMaxSkillEnergy = 5`
- `bonusMaxSkillEnergy = 0`
- `recoverSecondsPerPoint = 2`

## 测试方法

1. 打开 `Assets/Scenes/SampleScene.unity`。
2. 点 Play。
3. Player 头顶应该有红色生命条和蓝色技力条。
4. 选中 Player，在 Inspector 里找到 `PlayerSkillEnergy`。
5. 运行时把 `currentSkillEnergy` 临时改低，例如 2。
6. 等待几秒，蓝条应按 `recoverSecondsPerPoint` 逐点恢复。
7. 打开背包角色信息，确认能看到 `SP: 当前值 / 最大值`。

## 暂时不做

- 不做技能树。
- 不做正式技能释放。
- 不做装备增加技力上限的实际联动。
- 不做复杂 HUD 系统。
- 不做存档。
