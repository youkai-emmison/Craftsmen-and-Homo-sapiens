# Stage 3 Implementation Plan

## 第三阶段唯一小闭环

Player 进入第一个简单战斗房：

1. BasicEnemy 在房间内简单巡逻。
2. Player 靠近后 BasicEnemy 进入追击。
3. BasicEnemy 靠近 Player 后按冷却造成伤害。
4. Player 可以攻击 BasicEnemy。
5. BasicEnemy 血量归零后禁用。
6. RoomClearController 判定房间清理完成。
7. ExitDoor 从锁定变为解锁。
8. Player 接触解锁门后 Console 输出 `Room Cleared / Next Room Unlocked`。

## 新增脚本列表与职责

### `Assets/Scripts/Player/PlayerHealth.cs`

只负责 Player 生命值。

- 初始化当前生命值。
- 接收伤害。
- 输出当前血量。
- 血量归零时输出 `Player defeated`。
- 不做死亡动画、重生、正式 UI。

### `Assets/Scripts/Enemies/EnemyState.cs`

只定义 BasicEnemy 的极简状态：

- Idle
- Patrol
- Chase
- Attack
- Dead

### `Assets/Scripts/Enemies/BasicEnemyController.cs`

只负责 BasicEnemy 的有限状态机和移动行为。

- Patrol：在左右巡逻点之间移动。
- Chase：玩家进入 detectionRange 后追击。
- Attack：玩家进入 attackRange 后停止移动，并交给 EnemyAttackController。
- Dead：停止移动。
- 不做复杂寻路、跳跃敌人、飞行敌人。

### `Assets/Scripts/Enemies/EnemyAttackController.cs`

只负责敌人攻击节奏。

- 使用 attackCooldown 防止每帧扣血。
- 冷却结束后调用 PlayerHealth.TakeDamage。
- 不负责移动、状态机、UI。

### `Assets/Scripts/Enemies/EnemyHealth.cs`

只负责敌人生命值、受击、死亡通知。

- 实现 `IDamageable`。
- 死亡时触发 `OnEnemyDefeated`。
- 死亡后 SetActive(false)。
- 不掉落物品，不写对象池，不写 Boss 逻辑。

### `Assets/Scripts/Rooms/RoomClearController.cs`

只负责判断房间敌人是否全部清理，并解锁出口。

- 手动配置 enemiesInRoom。
- 手动配置 exitDoor。
- 不自动搜索敌人。
- 不切换场景。

### `Assets/Scripts/Rooms/ExitDoorController.cs`

只负责出口门锁定/解锁和接触反馈。

- 锁定时接触 Player 输出 `Door is locked`。
- 解锁时接触 Player 输出 `Room Cleared / Next Room Unlocked`。
- 不加载下一关，不写正式 UI。

### `Assets/Scripts/Visual/SimpleHitFlash.cs`

提供简单受击闪烁反馈。

- 手动指定 targetRenderer。
- PlayFlash 时短暂切换颜色。
- 不写复杂动画系统。

### `Assets/Scripts/Visual/RuntimeBillboard.cs`

可选脚本，让世界空间提示物跟随摄像机朝向。

- 当前阶段不是必须挂载。
- 不写复杂 UI 系统。

## Unity Editor 手动配置

### Player

- Tag = `Player`。
- Layer = `Player`。
- 保持 `Rigidbody2D`、`Collider2D`、`PlayerMovement`、`GroundChecker`、`PlayerAttackController`、`MeleeHitDetector`。
- 新增 `PlayerHealth`。
- `PlayerHealth.maxHealth` 建议先设为 5。
- 如果使用 `SimpleHitFlash`，手动把 Player 的 SpriteRenderer 拖到 `targetRenderer`。

### BasicEnemy

- 创建 `BasicEnemy` GameObject。
- Layer = `Hittable`。
- 添加 `Rigidbody2D`，Body Type 使用 Dynamic，Gravity Scale 可用 3 到 5，Freeze Rotation Z 勾选。
- 添加 `Collider2D`，不要设为 Trigger。
- 添加 `BasicEnemyController`。
- 添加 `EnemyAttackController`。
- 添加 `EnemyHealth`。
- 可选添加 `SimpleHitFlash`。
- `playerTarget` 指向 Player Transform。
- `playerHealth` 指向 PlayerHealth。
- `patrolLeftPoint` 和 `patrolRightPoint` 指向两个空物体。
- `enemyAttackController` 指向同物体上的 EnemyAttackController。
- `enemyHealth` 指向同物体上的 EnemyHealth。
- BasicEnemy 必须在 `MeleeHitDetector.hittableLayer` 包含的 Layer 上。

### ExitDoor

- 创建 `ExitDoor` GameObject。
- 添加 SpriteRenderer。
- 添加 Collider2D，并勾选 Is Trigger。
- 添加 `ExitDoorController`。
- `doorSpriteRenderer` 指向门的 SpriteRenderer。
- `lockedColor` 使用偏暗色。
- `unlockedColor` 使用明亮色。
- 摆放在房间右侧。

### RoomClearController

- 创建 `RoomController` GameObject。
- 添加 `RoomClearController`。
- `enemiesInRoom` 填入 BasicEnemy 上的 EnemyHealth。
- `exitDoor` 填入 ExitDoorController。

### Ground

- 保持 Layer = `Ground`。
- 使用简单方块或平台即可。
- 不上 Tilemap 也可以。

### Camera

- 保持 `CameraFollow`。
- target 指向 Player。

## Layer / Tag 要求

Layer：

- `Player`
- `Ground`
- `Hittable`

Tag：

- `Player`
- `MainCamera` 保持默认

代码不会自动创建 Layer 或 Tag。缺少时需要在 Unity Editor 手动添加。

## BasicEnemy FSM 说明

BasicEnemy 使用极简 FSM：

- `Patrol`：默认状态，在两个巡逻点之间左右移动。
- `Chase`：Player 进入 detectionRange 后追击。
- `Attack`：Player 进入 attackRange 后停止横向移动并尝试攻击。
- `Dead`：敌人死亡后停止移动，不再追击，不再攻击。

当前不做复杂寻路、不做跳跃敌人、不做飞行敌人。

## PlayerHealth / EnemyHealth 关系

- `PlayerHealth` 只接收敌人伤害。
- `EnemyHealth` 实现 `IDamageable`，所以现有 `MeleeHitDetector` 可以直接命中 BasicEnemy。
- `EnemyAttackController` 不查找 Player，只接收 BasicEnemyController 手动传入的 PlayerHealth。
- `EnemyHealth` 死亡后通知 RoomClearController。

## RoomClearController / ExitDoorController 关系

- RoomClearController 订阅 enemiesInRoom 里每个 EnemyHealth 的 OnEnemyDefeated。
- 所有敌人 IsDead 后调用 exitDoor.UnlockDoor()。
- ExitDoorController 解锁后改变颜色并输出 Console 提示。
- Player 接触解锁门后只输出推进反馈，不切换场景。

## 轻量美术占位说明

已准备小尺寸原创 PNG：

- `Assets/Art/Generated/Characters/heroine_placeholder.png`
- `Assets/Art/Generated/Enemies/basic_enemy_placeholder.png`
- `Assets/Art/Generated/Environment/anime_battle_room_backdrop.png`
- `Assets/Art/Generated/Environment/exit_door_locked.png`
- `Assets/Art/Generated/Environment/exit_door_unlocked.png`
- `Assets/Art/Generated/Effects/hit_slash_placeholder.png`

Unity 导入建议：

- Texture Type: Sprite。
- Sprite Mode: Single。
- Pixels Per Unit: 100。
- Filter Mode: Bilinear。
- Compression: Low Quality 或 Normal Quality 都可以。

这些资源只是占位，后续可以替换为原创正式素材。

## 验收标准

代码验收：

- Unity Console 没有红色编译错误。
- Player 原有横版移动和跳跃仍然可用。
- Player 原有攻击训练靶仍然可用。
- BasicEnemy 可以巡逻。
- Player 进入检测范围后 BasicEnemy 可以追击。
- BasicEnemy 进入攻击范围后可以按冷却伤害 Player。
- Player 可以攻击 BasicEnemy。
- BasicEnemy 血量归零后禁用。
- RoomClearController 能识别敌人被清理。
- ExitDoor 能从锁定变为解锁。
- Player 接触解锁门后输出 `Room Cleared / Next Room Unlocked`。
- 狂按攻击键不会无冷却连续触发。
- 敌人攻击不会每帧疯狂扣血。

美术验收：

- `Assets/Art/Generated/` 下有清晰命名的原创占位图。
- 画风是清爽二次元，不是灰方块。
- 不出现成人内容。
- 不出现任何商业角色或疑似复刻角色。
- 图片文件不过大。

## 暂时不做的功能

- 不做 Boss。
- 不做随机地图。
- 不做完整装备系统。
- 不做机械系统。
- 不做技能树。
- 不做背包。
- 不做掉落表。
- 不接 AI API。
- 不做对象池。
- 不导入第三方插件。
- 不做复杂 UI 系统。
- 不做场景切换。
