# Level Blockout

## 当前灰盒实现：StageBlockoutBuilder

本阶段新增 Unity Editor 菜单 `Tools/Stage Blockout/Create Anime Roguelite Blockout`，用于在当前打开场景中生成一个可重复重建的三段式横版灰盒地图。

生成根物体：
- `StageBlockout`

生成分组：
- `StageBlockout/Background`
- `StageBlockout/Ground`
- `StageBlockout/Platforms`
- `StageBlockout/Boundaries`
- `StageBlockout/Markers`

生成器只创建灰盒地图和摆放标记点，不创建 Player、Main Camera、TrainingTarget、BasicEnemy、ExitDoor、Layer 或 Tag。

## StageBlockout 具体对象

Background：
- `BackdropPanel`：浅粉背景板。
- `WindowPanel`：浅紫窗户/装饰板。
- `FloorGlow`：地面附近的柔和色块，让场景不再是空白背景。

Ground：
- `MainGround`：贯穿整段关卡的安全主地面。
- `SpawnGround`：出生区地面。
- `TrainingGround`：训练区地面。
- `BattleGround`：第一战斗区地面。

Platforms：
- `TrainingPlatformSmall`：训练区小平台，用于测试跳跃。
- `BattlePlatformSmall`：战斗区小平台，用于让空间更像横版房间。

Boundaries：
- `LeftWall`：左边界，防止 Player 跑出地图。
- `RightWall`：右边界，防止 Player 跑出地图。
- `CeilingLimit`：顶部边界，防止测试时跳出可视区域。

Markers：
- `PlayerSpawnPoint`
- `TrainingTargetPoint`
- `BasicEnemyPoint`
- `ExitDoorPoint`
- `CameraStartPoint`

## 三段式尺寸建议

整体长度建议约 45 到 60 Unity units。

出生区：
- 范围：X = -20 到 -8。
- Player 出生点：`PlayerSpawnPoint`，默认 X = -18，Y = -1。
- 用途：移动、跳跃、相机跟随的安全测试。

训练区：
- 范围：X = -8 到 10。
- TrainingTarget 点位：`TrainingTargetPoint`，默认 X = 0，Y = -1。
- 用途：测试小平台、攻击距离、训练靶受击和血条。

第一战斗区：
- 范围：X = 10 到 28。
- BasicEnemy 点位：`BasicEnemyPoint`，默认 X = 17，Y = -1。
- ExitDoor 点位：`ExitDoorPoint`，默认 X = 28，Y = -1。
- 用途：测试敌人、房间清理和出口解锁。

地面高度：
- 主地面中心 Y = -3。
- Main Camera Orthographic Size 建议 5 到 6。

## 三段式地图验收标准

- Unity 顶部菜单能看到 `Tools/Stage Blockout/Create Anime Roguelite Blockout`。
- 点击菜单后，Hierarchy 中出现 `StageBlockout`。
- `StageBlockout` 下有 `Background`、`Ground`、`Platforms`、`Boundaries`、`Markers`。
- 地图从左到右能清楚看出出生区、训练区、第一战斗区。
- `Ground`、`Platforms`、`Boundaries` 中的可站立或阻挡对象都有 `BoxCollider2D`。
- 这些可站立对象手动设置到 `Ground` Layer 后，Player 不会从地板中掉下去。
- Player 放到 `PlayerSpawnPoint` 后，可以左右移动并跳到 `TrainingPlatformSmall`。
- TrainingTarget 放到 `TrainingTargetPoint` 后，可以被攻击命中。
- BasicEnemy 放到 `BasicEnemyPoint` 后，有足够空间设置左右巡逻点。
- ExitDoor 放到 `ExitDoorPoint` 后，能作为战斗区出口测试点。
- 左右边界能阻止 Player 轻易跑出测试关卡。

详细操作步骤见 `docs/MAP_BLOCKOUT_GUIDE.md`。

## 总原则

早期关卡只用方块、Tilemap 和简单原创 2D 图形占位。目标是验证移动路线、跳跃距离、攻击空间、敌人位置和房间推进节奏，不追求最终美术。

当前方向是日系动漫美少女横版动作 Roguelite，不规划机械教学房或机械系统。

## 第二阶段训练房间布局

目标：验证 Player 的横版移动、跳跃、相机跟随、基础攻击、防连点和训练靶血条。

建议布局：

- 左侧放 Player 出生点。
- 中间放一段平地。
- 右侧放一个静态 `TrainingTarget`。
- TrainingTarget 头顶放清楚可见的血条。
- 背景使用柔和动漫训练房占位色块。

对象：

- Player
- Main Camera
- Ground
- groundCheckPoint
- attackPoint
- TrainingTarget
- TrainingTargetHealthBar

验收标准：

- Player 只能横向移动，不再上下自由漂移。
- Player 能跳起并落回地面。
- 相机能跟随 Player。
- 攻击键能让 TrainingTarget 产生一次受击反馈。
- 狂按攻击键不会无冷却连续触发。
- 血条变化清楚可见。
- 场景能看出清爽二次元方向。

## 第三阶段第一战斗房布局

目标：第一次验证简单敌人、玩家生命值、房间清理和出口解锁。

建议布局：

- Player 从房间左侧进入。
- Ground 保持一段连续平地。
- BasicEnemy 放在中右侧，拥有左右巡逻点。
- ExitDoor 放在房间最右侧。
- RoomController 放在场景根节点，负责敌人清理判定。

训练房和第一战斗房的区别：

- 训练房只验证攻击静态目标。
- 第一战斗房验证敌人会移动、追击、攻击 Player。
- 第一战斗房需要 `PlayerHealth`、`EnemyHealth`、`RoomClearController` 和 `ExitDoorController`。

对象：

- Player
- PlayerHealth
- Ground
- BasicEnemy
- EnemyPatrolLeftPoint
- EnemyPatrolRightPoint
- EnemyHealth
- EnemyAttackController
- RoomController
- ExitDoor

房间清理条件：

- `enemiesInRoom` 里的所有 `EnemyHealth.IsDead` 都为 true。
- 清理后 `RoomClearController` 调用 `ExitDoorController.UnlockDoor()`。
- Player 接触解锁后的 ExitDoor，Console 输出 `Room Cleared / Next Room Unlocked`。

验收标准：

- BasicEnemy 在两个巡逻点之间移动。
- Player 进入 detectionRange 后 BasicEnemy 追击。
- BasicEnemy 进入 attackRange 后停止移动并按冷却造成伤害。
- Player 可以攻击 BasicEnemy。
- BasicEnemy 血量归零后禁用。
- ExitDoor 从锁定变为解锁。
- Player 接触解锁门后有 Console 反馈。
- 不出现敌人逻辑塞进 Player 脚本的情况。

## 第四阶段奖励房布局

目标：验证房间推进后的基础奖励选择，不做完整装备系统。

建议布局：

- 房间较小，无敌人。
- 中间放 1 到 2 个奖励占位物。
- 奖励可以先是“更快攻击冷却”或“更高伤害”的临时测试项。
- 奖励表现使用动漫风格图标占位，例如星星、心形、缎带色块。

对象：

- Player
- RewardPickup 占位
- RoomExit 占位

验收标准：

- 玩家知道这是安全奖励房。
- 玩家能选择或拾取一个奖励。
- 奖励逻辑不膨胀成完整装备系统。
- 奖励 UI 不做复杂界面，只保留必要反馈。

## 房间类型规划

### 出生房

目标：让玩家安全进入本轮测试。

对象：

- PlayerSpawn
- Main Camera
- Ground
- RoomExit

验收：

- Player 出生位置稳定。
- 不出现敌人。
- 玩家能移动到出口。

### 训练房

目标：验证基础动作和攻击。

对象：

- Ground
- Platform 可选
- TrainingTarget
- TrainingTargetHealthBar

验收：

- 移动、跳跃、攻击、防连点都能测。
- 血条清楚。

### 战斗房

目标：验证一个简单敌人和房间清理。

对象：

- BasicEnemy
- Ground / Platform
- RoomController
- ExitDoor

验收：

- 敌人行为清楚。
- Player 能击败敌人。
- 战斗后出口解锁。
- Player 接触出口有推进反馈。

### 奖励房

目标：提供一次轻量奖励反馈。

对象：

- RewardPickup
- RoomExit

验收：

- 玩家知道获得了奖励。
- 不做完整装备库。

### Boss 房

当前不做。

不做原因：

- Boss 需要完整动作、敌人、数值和反馈支持。
- 当前最重要的是训练房和第一战斗房。

## 暂时不做程序化生成

当前阶段禁止做完整 Roguelite 随机关卡。

原因：

- 手工房间更适合调试跳跃、攻击距离和敌人位置。
- 程序化生成会增加调试成本，让移动、攻击、敌人和奖励问题混在一起。
- 小团队先做 3 到 4 个稳定手工房间更可靠。

进入程序化生成前，至少应完成训练房、第一战斗房、奖励房三个手工房间。
