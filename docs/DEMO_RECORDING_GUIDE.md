# Demo Recording Guide

本任务不录制最终 Demo 视频。此文档给负责录屏的同学使用。

## Recommended Length

3 到 5 分钟。

## Must-Capture 7 Shots

这些镜头也对应 `Submission/screenshots/` 的截图文件名：

1. `01_title_or_spawn.png`：出生点 / 开场画面。
2. `02_intro_memory_log.png`：NPC 对话或工坊记忆日志。
3. `03_early_room_combat.png`：早期房间战斗。
4. `04_level_up_or_growth.png`：经验成长、制作、技力或背包反馈。
5. `05_mid_room_enemy.png`：中段敌人或更强怪物。
6. `06_boss_room.png`：Boss 房或最终异常体。
7. `07_victory_screen.png`：Demo Complete / Victory / 结局文本。

## Recording Route

1. 标题 / 开场
   - 展示项目名：`能工智人：遗忘工坊`。
   - 简短说明赛道：叙事类游戏。
2. Intro / NPC 引导
   - 走近 NPC。
   - 按交互键打开对话框。
   - 展示“工坊记忆日志”的叙事感。
3. 基础操作
   - 左右移动。
   - 跳跃。
   - 近战攻击。
   - 如果要展示背包或制作面板，时间不要太长。
4. Combat / Room Clear
   - 击败普通敌人。
   - 展示血条、攻击反馈、房间推进或门解锁。
5. Progression
   - 展示经验、升级、制作、技力条或装置反馈。
   - 只需要证明“打怪 -> 变强/获得工具”这个小闭环。
6. Boss Fight
   - 进入 Boss 房。
   - 展示 Boss 攻击和玩家反击。
   - 击败 Boss。
7. Victory / Ending
   - 展示 Demo Complete 或结局文本。
   - 结尾停留 2 到 3 秒，方便剪辑。

## Screenshot Tool

可在 Unity 里使用：

```text
Tools > Hackathon > Capture Submission Screenshots
```

截图会保存到：

```text
Submission/screenshots/
```

具体步骤见：

```text
docs/SCREENSHOT_CAPTURE_GUIDE.md
```

## Recording Checklist

- Unity Console 没有红色错误。
- Game View 使用 16:9。
- 对话文字不乱码。
- UI 不遮挡角色。
- 主角、敌人、NPC 和地形清楚可见。
- 如果录 WebGL 版本，确认链接不是 `localhost`。
- 如果录 Editor Play 版本，尽量不要露出 Inspector / Console。

## Do Not Record

- 不要录 Unity 编译等待过程。
- 不要录素材导入过程。
- 不要长时间停在 Inspector 或 Console。
- 不要把未回填的 `localhost` 链接当正式链接展示。
- 不要使用其他游戏截图或视频片段。
