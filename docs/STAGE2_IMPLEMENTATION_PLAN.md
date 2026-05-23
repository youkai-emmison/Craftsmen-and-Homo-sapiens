# Stage 2 Implementation Plan

## 第二阶段唯一小闭环

第二阶段只做“横版动漫训练房间”。

目标：

- Player 从俯视角移动改为横版移动。
- Player 可以左右移动、跳跃、落地。
- Camera 继续跟随 Player。
- Player 可以用一次基础近战攻击命中静态训练靶。
- 攻击有防连点。
- TrainingTarget 血条清楚可见。
- 背景和角色占位能看出日系动漫美少女方向。

这个阶段不做完整战斗系统，只验证横版动作地基。

## 需要保留的脚本

### `Assets/Scripts/Camera/CameraFollow.cs`

保留。
职责仍然清晰：在 `LateUpdate` 跟随指定 `target`。第二阶段只需要在 Inspector 中确认 `target` 指向 Player，offset 适合横版视角即可。

### `Assets/Scripts/Player/PlayerMovement.cs`

保留横版版本。
职责只包括：

- 横向移动。
- 跳跃。
- 使用 `Rigidbody2D` 处理物理移动。
- 不处理攻击、UI、敌人、装备或奖励。

## 建议新增脚本及职责

### `GroundChecker`

只负责判断 Player 是否站在地面上。

- 使用指定检测点。
- 检测 `Ground` Layer。
- 不自动猜 Layer。

### `PlayerAttackController`

只负责攻击输入和攻击节奏。

- 接收攻击按下事件。
- 检查是否可以攻击。
- 管理攻击冷却。
- 触发一次命中检测。
- 不处理移动。

### `MeleeHitDetector`

只负责近战命中检测。

- 使用指定攻击点和攻击范围。
- 检测 `Hittable` Layer。
- 找到实现 `IDamageable` 的对象。
- 不决定输入和冷却。

### `IDamageable`

只定义可受击对象接口。

### `TrainingTarget`

只负责静态训练靶受击反馈。

- 实现 `IDamageable`。
- 被命中后给出简单反馈。
- 不写敌人 AI。

### `TrainingTargetHealthBar`

只负责训练靶血条显示。

- 接收当前血量和最大血量。
- 调整填充条宽度。
- 不管理目标生命值。
- 不做完整 UI 系统。

## Unity Editor 手动配置项

### Player

- Layer 设置为 `Player`。
- 添加 `Rigidbody2D`。
- `Rigidbody2D Body Type` 使用 Dynamic。
- `Gravity Scale` 使用 3 到 5。
- 勾选 Freeze Rotation Z。
- 添加 `Collider2D`，建议先用 BoxCollider2D 或 CapsuleCollider2D。
- 挂载 `PlayerMovement`。
- 挂载 `GroundChecker`。
- 挂载 `PlayerAttackController`。
- 挂载 `MeleeHitDetector`。
- 外观子物体可以只放 SpriteRenderer，不影响碰撞体。

### Camera

- Main Camera 保持 `CameraFollow`。
- `target` 手动指定为 Player Transform。
- 不通过代码自动查找 Player。

### Ground / Platform

- Ground 对象 Layer 设置为 `Ground`。
- 添加 Collider2D。
- 当前训练房可以先用简单方块，不必上 Tilemap。
- 背景色块不要加 Collider2D。

### TrainingTarget

- Layer 设置为 `Hittable`。
- 添加 Collider2D。
- 挂载 `TrainingTarget`。
- 挂载 `TrainingTargetHealthBar`。
- 放在 Player 攻击范围可达的位置。

## Layer / Tag 规划

### Layer

- `Player`：玩家对象。
- `Ground`：地面和平台。
- `Hittable`：可被玩家攻击命中的训练靶和后续敌人。
- `Enemy`：后续敌人对象，第二阶段可以先不使用。

### Tag

- `Player`：Player 对象可设置。
- `MainCamera`：Main Camera 保持 Unity 默认。

不要依赖代码自动查找 Tag。Tag 主要用于 Editor 中辨认和后续明确需求。

## Update / FixedUpdate / LateUpdate 分工

### Update

放输入和普通计时：

- 读取左右输入。
- 读取跳跃按下。
- 读取攻击按下。
- 更新攻击冷却计时或攻击状态计时。

### FixedUpdate

放物理相关逻辑：

- Rigidbody2D 横向移动。
- Rigidbody2D 跳跃。
- 地面检测。
- 与物理碰撞相关的命中检测。

### LateUpdate

放相机跟随：

- CameraFollow 根据 Player 最终位置移动相机。

## 攻击防连点方案

第二阶段只需要最简单的攻击节奏控制。

规则：

- 只响应攻击键“按下瞬间”，不响应长按每帧触发。
- 攻击开始后进入冷却。
- 冷却结束前再次按攻击键，不触发新攻击。
- 每次攻击只进行一次命中检测。
- 命中检测只对 `Hittable` Layer 生效。

不做连招、不做蓄力、不做取消、不做复杂动画状态机。

## 保持代码简洁，不写兜底代码

第二阶段脚本配置必须清晰。

- 需要的组件在 Inspector 中手动挂好。
- 需要的 Transform、LayerMask、攻击点等引用在 Inspector 中明确设置。
- 缺少配置时，给出清晰错误提示。
- 不自动创建 Player、Ground、Target 或攻击点。
- 不自动查找一堆对象。
- 不用默认猜测掩盖配置错误。

这样能让 Unity 初学者更快知道哪里没配对。

## 验收标准

打开 `Assets/Scenes/MainScene.unity` 并点击 Play。

必须满足：

- Player 出现在训练房左侧或中间。
- A/D 或方向键左右可以移动。
- 跳跃键可以让 Player 起跳。
- Player 会受重力影响落回 Ground。
- Player 不会通过输入直接上下漂移。
- Player 能站在 Ground 或 Platform 上。
- Camera 跟随 Player。
- TrainingTarget 放在攻击范围内时，按攻击键能触发一次命中反馈。
- 快速连按攻击键不会无间隔重复触发。
- TrainingTarget 血条能清楚减少。
- 背景不是纯白空场景。
- Console 没有编译错误和 NullReferenceException。
- 代码没有把攻击、敌人、UI、奖励塞进 `PlayerMovement`。

## 暂时不做的功能

- 不做 Boss。
- 不做完整 Roguelite 随机关卡。
- 不做完整装备系统。
- 不做机械系统。
- 不做技能树。
- 不接 AI API。
- 不做复杂敌人。
- 不做正式 UI。
- 不做背包。
- 不做掉落表。
- 不做存档。
- 不导入第三方插件。
- 不复制任何参考仓库代码或素材。
