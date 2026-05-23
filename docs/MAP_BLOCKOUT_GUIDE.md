# Map Blockout Guide

## 为什么先做地图灰盒

当前最大风险不是系统不够多，而是场景还不像一个可以演示的横版关卡。先用灰盒地图把移动、跳跃、攻击距离、相机跟随、敌人位置和出口推进跑通，后续替换 Tilemap 和正式美术时才不会反复推翻基础空间。

本阶段只做一个从左到右推进的可演示关卡，不做 Boss、随机地图、完整 UI、装备、技能树、机械系统或背包。

## 三段式关卡结构

### 出生区 Spawn Area

范围建议：X = -20 到 -8。

目标：
- Player 出生后能稳定落到地面。
- 玩家可以测试左右移动、跳跃和 CameraFollow。
- 场景第一眼不再是空白测试场。

对象：
- `PlayerSpawnPoint`
- `SpawnGround`
- `BackdropPanel`
- `CameraStartPoint`

### 训练区 Training Area

范围建议：X = -8 到 10。

目标：
- 测试跳跃到小平台。
- 测试 J 键或鼠标左键攻击。
- 测试 TrainingTarget 受击和血条可读性。

对象：
- `TrainingGround`
- `TrainingPlatformSmall`
- `TrainingTargetPoint`
- `TrainingTarget`

### 第一战斗区 Battle Area

范围建议：X = 10 到 28，出口点在 X = 28 到 30 附近。

目标：
- 放置一个 BasicEnemy 的占位点。
- 给敌人预留左右巡逻空间。
- 放置 ExitDoor 和房间清理出口。

对象：
- `BattleGround`
- `BattlePlatformSmall`
- `BasicEnemyPoint`
- `ExitDoorPoint`
- `LeftWall`
- `RightWall`

## 运行 StageBlockoutBuilder

1. 打开 `Assets/Scenes/MainScene.unity`。
2. 在 Unity 顶部菜单点击 `Tools/Stage Blockout/Create Anime Roguelite Blockout`。
3. 如果场景里已经有 `StageBlockout`，选择 `Rebuild` 会只删除旧的 `StageBlockout` 根物体和它的子物体。
4. 确认 Hierarchy 中出现 `StageBlockout`。
5. 展开后应看到 `Background`、`Ground`、`Platforms`、`Boundaries`、`Markers` 五组对象。

生成器不会创建 Player、Main Camera、Layer 或 Tag，也不会自动修改已有玩法对象。它只负责搭好地图灰盒和摆放参考点。

## 移动 Player 到出生点

1. 在 Hierarchy 里选中 `StageBlockout/Markers/PlayerSpawnPoint`。
2. 记下它的位置，默认是 `X = -18, Y = -1, Z = 0`。
3. 选中场景里的 `Player`，把 Transform Position 改到相同位置。
4. 确认 `Player` 的 Tag 是 `Player`，Layer 是 `Player`。
5. 确认 `Player` 仍然有 `Rigidbody2D`、`Collider2D`、`PlayerMovement`、`GroundChecker`、`PlayerAttackController`、`MeleeHitDetector`。

## 放置 TrainingTarget

1. 找到 `StageBlockout/Markers/TrainingTargetPoint`。
2. 把现有 `TrainingTarget` 移动到该点附近。
3. 确认 `TrainingTarget` 在 `Hittable` Layer。
4. 确认它有 Collider2D，并且能被玩家攻击范围覆盖。

## 放置 BasicEnemy

1. 找到 `StageBlockout/Markers/BasicEnemyPoint`。
2. 把现有 `BasicEnemy` 移动到该点附近。
3. 确认 `BasicEnemy` 在 `Hittable` Layer。
4. 如果敌人脚本支持巡逻，创建或移动两个空物体作为巡逻点：
   - `EnemyPatrolLeftPoint` 放在 X 约 14。
   - `EnemyPatrolRightPoint` 放在 X 约 20。
5. 在 `BasicEnemyController` 上手动拖入：
   - `playerTarget` = `Player`
   - `patrolLeftPoint` = `EnemyPatrolLeftPoint`
   - `patrolRightPoint` = `EnemyPatrolRightPoint`

## 放置 ExitDoor

1. 找到 `StageBlockout/Markers/ExitDoorPoint`。
2. 把现有 `ExitDoor` 移动到该点附近。
3. 确认 `ExitDoor` 的 Collider2D 勾选 `Is Trigger`。
4. 如果使用 `RoomClearController`，把 `ExitDoorController` 拖到 `exitDoor` 字段。

## Layer / Tag 设置

需要手动准备这些 Layer：
- `Player`
- `Ground`
- `Hittable`

需要手动确认这些 Tag：
- `Player`
- `MainCamera` 保持默认即可。

生成器创建的 `Ground`、`Platforms`、`Boundaries` 子物体需要手动设为 `Ground` Layer。不要让代码自动创建 Layer 或 Tag。

## 测试流程

1. 点 Play。
2. 确认 Player 从出生点落在地面上，不会从左侧或右侧轻易跑出地图。
3. 用 A/D 或方向键左右移动，确认 CameraFollow 跟随。
4. 在训练区跳上 `TrainingPlatformSmall`。
5. 攻击 `TrainingTarget`，确认它受击并显示血量变化。
6. 进入战斗区，确认 BasicEnemy 的巡逻、追击和攻击配置没有红色报错。
7. 击败 BasicEnemy 后，确认 ExitDoor 解锁。
8. 触碰解锁后的 ExitDoor，Console 应输出 `Room Cleared / Next Room Unlocked`。

## 常见问题检查

Player 掉下去：
- 检查 Player 是否在 `PlayerSpawnPoint` 附近。
- 检查 `MainGround`、`SpawnGround`、`TrainingGround`、`BattleGround` 是否都有 `BoxCollider2D`。
- 检查这些地面对象是否设置为 `Ground` Layer。
- 检查 `GroundChecker.groundLayer` 是否包含 `Ground`。

跳不上平台：
- 检查 `jumpForce` 是否太低。
- 检查平台 Collider2D 是否存在。
- 检查平台是否在 `Ground` Layer。
- 检查 Player 的 `Rigidbody2D` 是否为 Dynamic，Gravity Scale 建议 3 到 5。

打不到靶子：
- 检查 `attackPoint` 是否在 Player 前方。
- 检查 `attackRange` 是否覆盖 TrainingTarget。
- 检查 TrainingTarget 是否在 `Hittable` Layer。
- 检查 `MeleeHitDetector.hittableLayer` 是否包含 `Hittable`。

## 为什么现在不做 Tilemap

Tilemap 很适合正式关卡，但现在更重要的是验证横版空间和玩法距离。灰盒方块更快、更直观，哪里太宽、哪里太窄、哪里会掉下去都能马上调整。

等训练区、第一战斗区、奖励区三个手工房间都稳定后，再把灰盒替换为 Tilemap 会更稳。

## 后续引入 Tilemap 的时机

可以在以下条件满足后引入 Tilemap：
- Player 移动、跳跃、攻击手感稳定。
- TrainingTarget 和 BasicEnemy 的测试位置稳定。
- RoomClearController 和 ExitDoorController 已经完成最小闭环。
- 至少有三种手工房间的灰盒尺寸已经被验证。
