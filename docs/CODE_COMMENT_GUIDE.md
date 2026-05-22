# Code Comment Guide

## 目标

为了方便协作，每个 C# 脚本都要让接手的人快速看懂三件事：
- 这个脚本负责什么。
- Inspector 里需要配置哪些变量。
- 关键运行时变量为什么存在。

注释要短，服务协作，不写成教程，也不重复解释一眼能看懂的语法。

## 脚本开头注释

每个脚本文件开头，在 `using` 前添加统一说明：

```csharp
// Script purpose: Controls side-scrolling player movement and jumping.
// Key Inspector variables:
// - moveSpeed: Horizontal movement speed.
// - jumpForce: Upward jump velocity.
// - groundChecker: Ground check component that decides whether jumping is allowed.
```

要求：
- `Script purpose` 只写一个职责。
- `Key Inspector variables` 只列需要 Unity Inspector 配置或重点理解的变量。
- 不写未来大系统，不把不存在的功能写进去。

## 变量注释

公开字段和关键私有字段上方添加一句短注释：

```csharp
// Horizontal speed applied from left/right input.
public float moveSpeed = 5f;

// Cached horizontal input read in Update and applied in FixedUpdate.
private float horizontalInput;
```

要求：
- public 字段必须解释用途，因为 Unity 初学者会在 Inspector 里看到它。
- private 字段只在它承担状态、缓存、冷却、引用、防刷屏日志等职责时注释。
- 注释解释“用途”，不要复述类型，例如不要写“这是一个 float”。
- 缩写变量必须解释含义；优先不用缩写。

## 注释风格

推荐：
- 短句。
- 明确对象，例如 Player、BasicEnemy、ExitDoor。
- 说明为什么放在 Inspector 配置。
- 说明 Update / FixedUpdate / LateUpdate 的关键分工。

避免：
- 大段废话。
- 重复代码本身已经表达清楚的内容。
- 在注释里承诺未实现的功能。
- 用注释掩盖不清晰的命名。
- 为兜底逻辑找理由。缺少引用时仍应暴露清楚错误，不自动创建对象。

## 当前已执行范围

本规则已经应用到 `Assets/Scripts/` 下当前所有 C# 脚本：
- Player
- Camera
- Combat
- Test
- Enemies
- Rooms
- Visual
- Level
- Editor

后续新增脚本也按这个格式执行。
