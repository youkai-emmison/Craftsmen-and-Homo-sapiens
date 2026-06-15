# 能工智人：糖芯工坊

**Craftsmen and Homo Sapiens: The Candy Forge**

赛道：**叙事类游戏 / Narrative Games**

一句话介绍：

> 理工男穿越成异世界女仆工程师，用糖果材料搓科技，打败 Boss 找到回家的路。

## Project Overview

《能工智人：糖芯工坊》是一款 AI 叙事驱动的 Unity 2D 横版动作冒险 Demo。玩家扮演一名意外穿越到糖果异世界的理工男，被系统误绑定为“见习女仆工程师”。为了回到现实世界，玩家需要在糖芯工坊中与 NPC 对话、移动跳跃、近战战斗、收集怪物掉落材料、合成装备与道具、学习技能，并最终击败污染糖芯炉的 Boss。

本项目当前目标是比赛提交原型，不追求完整商业游戏系统。优先保证：

- 3 到 5 分钟可演示流程
- AI 叙事主题明确
- WebGL 可部署准备
- 海报、PPT、源码包和提交文案完整

## World Setting

糖芯王国是一座由“糖芯炉”驱动的异世界。这里的甜点不是食物，而是能源、武器、魔法和机械零件。

主角洛辰原本是现实世界的理工男大学生，在实验事故后穿越到糖芯王国，并被系统错误绑定为“见习女仆工程师”。他必须用工程知识、番剧储备和糖果材料合成科技装置，修复糖芯传送装置，击败糖蚀巫师，重新打开回家的传送门。

详细设定见：

```text
docs/WORLD_SETTING_CANDY_FORGE.md
```

## Controls

- Move: `A / D` or arrow keys
- Jump: `Space`
- Attack: `J` or left mouse button
- Backpack: `I`
- NPC / Dialogue: `E`

具体按键以 Unity 场景中的当前配置为准。

## Gameplay Loop

```text
穿越开场
-> NPC 对话 / 糖芯工坊日志
-> 房间探索
-> 近战战斗
-> 材料与经验成长
-> 背包 / 合成 / 技能树
-> Boss 战
-> 结局文本
```

Demo 展示重点不是系统数量，而是在短流程内完整表达“进入糖果异世界、读日志、战斗、成长、合成、挑战 Boss、修复回家装置”的叙事体验。

## AI-Generated Content

AI 辅助生成：

- 糖果异世界世界观
- 主角、NPC、怪物和 Boss 设定
- 糖芯工坊日志
- Boss 背景
- 结局文本
- 提交海报和展示材料整理
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

本次提交材料集中在：

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
- Team / School / Captain: `待回填`

## Third-Party Assets

如果项目使用 Unity Asset Store 或其他第三方资源，请先确认授权和仓库公开策略。记录见：

```text
docs/THIRD_PARTY_ASSETS.md
```
