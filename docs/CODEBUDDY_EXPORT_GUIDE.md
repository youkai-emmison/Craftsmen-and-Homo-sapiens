# CodeBuddy 历史对话导出指南

## 为什么需要导出

腾讯云黑客松要求参赛作品开发过程使用 CodeBuddy，并在提交时导出 CodeBuddy 历史对话作为评审依据之一。

本项目可以用 Codex、人工开发和其他工具辅助，但最终提交材料里必须准备 CodeBuddy 侧的使用记录。建议在提交前用 CodeBuddy 对项目做最后一轮检查、修复和总结，然后导出对话记录。

## 建议在 CodeBuddy 中执行的最后一轮任务

把下面这段提示词复制给 CodeBuddy：

```text
你现在接手 Unity 2D 项目《能工智人 / Craftsmen and Homo sapiens》。

当前目标是腾讯云黑客松提交前最终检查。请只做检查、轻量修复建议和提交材料补全，不要大改玩法系统。

请完成：
1. 检查 README.md、docs/PROJECT_PROPOSAL_ZH.md、docs/AI_CREATION_LOG.md、docs/HACKATHON_SUBMISSION_CHECKLIST.md 是否能说明项目符合“赛题三：叙事类游戏 / AI 重塑叙事体验”。
2. 检查 Unity 项目是否能打开，当前 Demo 场景是否能 Play。
3. 检查是否有明显 C# 编译错误。
4. 检查 WebGL 构建步骤是否清楚。
5. 检查是否还缺在线试玩链接、Demo 视频链接、PPT 和 CodeBuddy 历史导出。
6. 输出一份最终提交前问题清单。

不要新增复杂系统，不要改 ProjectSettings，不要导入第三方插件。
```

## 导出内容建议

导出的历史对话中最好能体现：

- 使用 CodeBuddy 检查 Unity 项目。
- 使用 CodeBuddy 生成或完善 C# 脚本。
- 使用 CodeBuddy 协助调试报错。
- 使用 CodeBuddy 整理 WebGL 构建步骤。
- 使用 CodeBuddy 整理项目书、PPT、视频脚本或 AI 创作说明。

## 导出文件命名建议

建议文件名：

`CodeBuddy_History_Craftsmen_and_Homo_sapiens.pdf`

或：

`CodeBuddy_History_Craftsmen_and_Homo_sapiens.md`

如果平台只支持截图或网页导出，可以统一打包为：

`CodeBuddy_History_Craftsmen_and_Homo_sapiens.zip`

## 提交前检查

- [ ] CodeBuddy 历史记录已导出。
- [ ] 文件能打开。
- [ ] 文件中能看到项目名。
- [ ] 文件中能看到 CodeBuddy 对代码、文档或调试的实际参与。
- [ ] 文件已经和 WebGL 链接、视频、PPT 一起放入最终提交包。

## 可放进 PPT 的说法

> 我们使用 CodeBuddy 参与 Unity 原型开发和提交材料整理，包括 C# 脚本生成、报错检查、WebGL 构建流程梳理、AI 叙事文本整合和项目文档完善。提交时附带 CodeBuddy 历史对话记录，作为 AI 辅助开发过程证明。
