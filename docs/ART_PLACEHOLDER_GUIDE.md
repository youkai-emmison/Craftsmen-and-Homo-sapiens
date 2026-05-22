# Art Placeholder Guide

## StageBlockout 灰盒视觉规则

当前地图灰盒优先服务玩法验证，不追求最终美术。`StageBlockoutBuilder` 使用简单方块和柔和配色，让场景先像一个横版训练房/战斗房，而不是空白测试场。

当前 `StageBlockoutBuilder` 会把 `anime_battle_room_backdrop.png` 用作大背景，并额外生成 `Decorations` 色块来区分出生区、训练区和战斗区。攻击反馈使用 `hit_slash_placeholder.png`，Player 血条使用简单色块占位。

推荐配色：
- `BackdropPanel` 使用浅粉或奶白色。
- `WindowPanel` 使用浅紫或浅蓝色。
- `FloorGlow` 使用柔和粉色，降低纯白背景的空感。
- `Ground` 使用偏深的紫灰色，保证 Player 和敌人站位清楚。
- `Platforms` 使用比地面稍亮的紫色，方便区分可跳平台。
- `Boundaries` 使用深紫色，让左右边界一眼可见。
- `Markers` 使用高对比小色块，只作为 Editor 配置参考。

当前不需要为灰盒地图生成复杂图片。只要三段式空间清楚、地板不会过短、靶子和敌人的摆放点明确，就满足本阶段美术目标。

禁止事项：
- 不使用成人化、性感化或擦边角色表现。
- 不模仿任何商业动漫角色。
- 不复制网上图片、商业游戏素材、UI 或关卡。
- 不因为美术替换去改动碰撞尺寸和基础玩法脚本。

## 当前阶段占位原则

当前美术方向是日系动漫美少女风格。第三阶段仍然使用简单原创 2D 占位资源，不因为正式立绘、复杂动画或 AI 美术生成卡住玩法验证。

目标是让对象一眼可分辨：

- Player 能看出来是主角。
- Ground 和 Platform 能看出来可站立。
- TrainingTarget 能看出来可攻击。
- BasicEnemy 能看出来是危险对象。
- ExitDoor 能看出来是否锁定。
- HealthBar 必须足够清楚。

## 当前占位图清单

当前已准备的原创占位 PNG：

- `Assets/Art/Generated/Characters/heroine_placeholder.png`
- `Assets/Art/Generated/Enemies/basic_enemy_placeholder.png`
- `Assets/Art/Generated/Environment/anime_battle_room_backdrop.png`
- `Assets/Art/Generated/Environment/blockout_square.png`
- `Assets/Art/Generated/Environment/exit_door_locked.png`
- `Assets/Art/Generated/Environment/exit_door_unlocked.png`
- `Assets/Art/Generated/Effects/hit_slash_placeholder.png`

这些资源用于测试美术方向，不是最终正式素材。

## 通用视觉规则

- 使用柔和明亮的二次元配色，例如粉色、淡紫、浅蓝、暖白和深紫描边。
- 重要对象保持高对比，不要让白色背景吞掉白色物体。
- 主角、训练靶、敌人、地面、门、背景必须分层清楚。
- 占位图形可以简单，但颜色和轮廓要服务测试。
- 不复制任何商业动漫角色、游戏 UI、素材或截图。
- 不做成人内容，不做性感姿势，不做裸露，不做擦边表达。
- 不使用版权不明素材，不联网下载素材。

## Player 占位规则

- Player 可以用简单原创女主角占位图或场景内形状拼装。
- 推荐结构：头发、脸、眼睛、身体、服装色块分开，方便后续替换。
- 角色主色建议使用粉色或浅色系，头发使用深紫、深棕或其他高对比颜色。
- 角色碰撞体仍然保持简单，不被外观子物体影响。

验收：玩家移动和跳跃时，测试者能立刻看出哪个对象是 Player，并且能感受到“动漫女主角方向”。

## BasicEnemy 占位规则

- BasicEnemy 使用暗紫、黑紫或冷色系，与 Player 的暖色/粉色区分。
- 当前阶段推荐可爱的小影子怪、小史莱姆或小木偶怪。
- 不要恐怖血腥，不要复杂细节。
- 敌人必须使用有限状态机，不把行为塞进 Player 脚本。

验收：玩家能立刻判断它是危险对象，并能看清它的移动范围。

## Environment 占位规则

- 背景使用清爽训练房或室内战斗房。
- 推荐粉紫、浅蓝、奶白色调。
- 可以有简单窗户、地板、光斑或装饰。
- 背景不加 Collider2D，不参与物理。
- 背景不能喧宾夺主，不能遮住 Player、Enemy、Door。

## ExitDoor 占位规则

- 锁定门使用偏暗色。
- 解锁门使用更明亮的粉色、黄色或高亮标记。
- 当前阶段可以只用颜色切换表达锁定/解锁。
- Door 的 Collider2D 必须是 Trigger。

验收：玩家能知道门是否解锁。

## Hit Slash 占位规则

- 斩击特效使用粉白或紫白色。
- 当前阶段资源先准备，可以暂时不挂进场景。
- 后续挂载时不要写复杂动画系统。

## HealthBar 占位规则

- 血条要比背景和目标更醒目。
- 推荐深色外框、亮粉或亮红填充。
- 不要太细，当前测试阶段宁可大一点。
- 血条位置放在目标头顶，不能被角色或地面挡住。

验收：攻击目标后，测试者能立刻看出血量变化。

## 后续正式美术替换原则

正式美术替换时遵守：

- 先替换 Player、BasicEnemy、ExitDoor、HealthBar、Ground 这些最高频对象。
- 每替换一个对象，都要确认碰撞体尺寸没有被外观误导。
- 角色和 UI 必须原创，不参考或复刻任何商业角色。
- 不使用成人内容或版权不明素材。
- 不为了美术替换破坏已有脚本职责。

美术替换顺序建议：

1. Player 占位图。
2. BasicEnemy 占位图。
3. ExitDoor。
4. HealthBar。
5. Ground 和 Platform。
6. TrainingTarget。
7. Hit Slash。

## 禁止事项

- 禁止成人化。
- 禁止模仿商业角色。
- 禁止复制网上素材。
- 禁止使用版权不明素材。
- 禁止把美术替换和复杂系统开发绑在一起。
- 禁止现在为了美术做完整动画控制器或完整 UI 系统。
