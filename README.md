# 能工智人：遗忘工坊

**Craftsmen and Homo Sapiens: The Forgotten Forge**

赛道：**叙事类游戏 / Narrative Games**

一句话介绍：

> AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。

## Project Overview

《能工智人：遗忘工坊》是一款 Unity 2D 横版动作 / 叙事地牢 Demo。玩家扮演进入地下工坊遗迹的探索者，在房间推进、近战战斗、经验成长、Boss 战和结局文本中逐步读取“工坊记忆日志”，拼合工匠文明与智人文明冲突的真相。

本项目当前目标是比赛提交原型，不追求完整商业游戏系统。优先保证：

- 3 到 5 分钟可演示流程
- AI 叙事主题明确
- WebGL 可部署准备
- 海报、PPT、源码包和提交文案完整

## Controls

- Move: `A / D` or arrow keys
- Jump: `Space`
- Attack: `J` or left mouse button
- Backpack: `I`
- Device / slot usage: current scene configuration
- NPC / Dialogue: `E`

具体按键以 Unity 场景中的当前配置为准。

## Gameplay Loop

```text
Opening narrative
→ room exploration
→ melee combat
→ EXP / growth
→ boss fight
→ ending memory log
```

Demo 展示重点不是系统数量，而是短流程内完整表达“探索、战斗、成长、读档案、通关”的叙事体验。

## AI-Generated Content

AI 辅助生成：

- 世界观与剧情
- 工匠文明 / 智人文明设定
- 房间记忆日志
- Boss 背景
- 结局文本
- 提交海报和展示材料
- Unity 脚本、部署脚本和文档整理

详细说明见：

```text
docs/AI_CREATION_LOG.md
```

## Run In Unity

1. 使用 Unity `2022.3.53f1` 或 `2022.3.53f1c1` 打开项目。
2. 打开场景：

```text
Assets/Scenes/SampleScene.unity
```

3. 点击 Play 测试当前 Demo。

## WebGL Build

WebGL 输出目录：

```text
Build/WebGL
```

Unity 编辑器菜单：

```text
Tools > Hackathon > Build WebGL
Tools > Hackathon > Prepare Deploy Folder
```

部署准备脚本：

```powershell
powershell -ExecutionPolicy Bypass -File tools/prepare_webgl_site.ps1
```

详细说明：

```text
docs/WEBGL_DEPLOYMENT.md
docs/DEPLOYMENT_OPTIONS.md
docs/FINAL_MANUAL_STEPS.md
```

## Submission Materials

本次提交物料集中在：

```text
Submission/
docs/
tools/
submissions/
```

核心文件：

- `Submission/poster_1920x1080.png`
- `Submission/poster_source.svg`
- `Submission/project_deck.pptx`
- `Submission/project_deck.pdf`
- `docs/SUBMISSION_FORM_COPY.md`
- `docs/HACKATHON_SUBMISSION_CHECKLIST.md`
- `docs/DEMO_RECORDING_GUIDE.md`

源码包可本地生成：

```powershell
powershell -ExecutionPolicy Bypass -File tools/package_submission.ps1
```

生成目标：

```text
Submission/Craftsmen-and-Homo-sapiens_Source.zip
```

`.zip` 默认不提交到 Git，适合最终人工上传。

## Deployment Status

当前仓库只准备部署代码、脚本和文档，**没有实际部署**。

最终提交前请人工回填：

- Playable Demo Link: `待回填`
- Demo Video Link: `待回填`
- CodeBuddy History: `待回填`
- Team / School / Captain: `待回填`

## Third-Party Assets

如果项目使用 Unity Asset Store 或其他第三方资源，请先确认授权和仓库公开策略。记录见：

```text
docs/THIRD_PARTY_ASSETS.md
```
