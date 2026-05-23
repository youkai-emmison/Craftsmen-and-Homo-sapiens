# 能工智人 / Craftsmen and Homo sapiens - Agent Guidelines

本项目是 Unity 2D 项目。任何 AI 或开发者接手时，先遵守本文件，再看具体任务。

## 当前阶段

- 当前目标只是第一个可运行测试：验证 Unity 能编译 C#、玩家能移动、相机能跟随。
- 不要在当前阶段实现完整游戏系统。
- 不要提前添加敌人、背包、Boss、AI、装备、机械系统等内容，除非任务明确要求。

## 文件与资源约束

- 默认只在任务需要的最小范围内修改文件。
- 不引入第三方插件。
- 脚本放在 `Assets/Scripts/` 下，并按职责拆分子目录。
- 修改 `ProjectSettings`、`Packages`、`.unity`、`.prefab` 前必须确认任务允许。
- 如果没有统一命名空间，脚本暂时不使用 namespace，方便 Unity 初学者挂组件；如果后续确定统一命名空间，使用 `NengongZhiren`。

## 架构规则

- 不允许在一个脚本里写过多功能。脚本职责要单一。
- 角色的基本移动、输入等基础操作可以在角色脚本中；其他能力应通过接口或独立组件扩展，例如技能系统。
- 数据和 UI 分开管理。数据变化后，由 `UIManager` 或对应 UI 控制层响应并更新界面。
- 不允许频繁创建和销毁对象；大量复用对象必须考虑对象池。
- 物理逻辑放在 `FixedUpdate` 中，输入读取等非物理逻辑放在 `Update` 中。
- 做好 Layer 和 Tag 管理。新增需要碰撞、筛选、查找的对象时，先确认 Layer/Tag 方案。
- 交互、按钮、攻击、技能等输入要防止连点或重复触发。
- 敌人行为使用有限状态机组织，不把复杂敌人逻辑堆在一个方法里。

## 代码风格

- 命名必须清晰，例如 `playerSpeed`、`enemyHealth`。不要使用意义不明的缩写；确实需要缩写时，必须加注释说明。
- 函数保持短小，一个函数只做一件事。
- 优先使用 Unity 2D 组件和 API；不要在 2D 逻辑里混入 3D 组件。
- 组件缺失时给出清晰 Console 提示，避免空引用异常。
- 不要写兜底代码。不要用静默 fallback、自动猜测、默认创建对象等方式掩盖错误配置；除非任务明确要求容错，否则应让问题清晰暴露。

## 当前可运行测试配置

- `Assets/Scripts/Player/PlayerMovement.cs`：玩家 2D 移动脚本，使用 `Rigidbody2D`，在 `Update` 读取 WASD/方向键，在 `FixedUpdate` 移动。
- `Assets/Scripts/Camera/CameraFollow.cs`：相机跟随脚本，在 `LateUpdate` 平滑跟随目标。
- `Assets/Scenes/MainScene.unity` 当前包含一个 `Player` 测试对象和配置好的 `Main Camera` 跟随。
