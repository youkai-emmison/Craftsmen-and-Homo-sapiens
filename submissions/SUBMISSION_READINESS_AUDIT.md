# 腾讯云黑客松提交就绪审计

审计日期：2026-06-11

这个文件用于在最终提交前逐项确认材料是否真的存在。状态分为：

- `Ready`：仓库中已有可直接使用的文件或链接。
- `Needs External Action`：需要在 Unity、CodeBuddy、视频平台或部署平台完成外部动作。
- `Optional`：加分项或非必交项。

## 必交材料审计

| 项目 | 状态 | 当前证据 | 下一步 |
| --- | --- | --- | --- |
| 作品网页链接 | Needs External Action | `docs/WEBGL_DEPLOYMENT.md` 已说明部署流程 | 构建 WebGL，上传到可公开访问的平台，把链接填入 `submissions/FINAL_SUBMISSION_INFO.md` |
| 游戏 Demo 视频 | Needs External Action | `docs/DEMO_VIDEO_SCRIPT.md` 和 `submissions/DEMO_RECORDING_RUNBOOK.md` 已准备 | 录制 3 分钟以内视频，上传后填写视频链接 |
| 作品介绍 PPT | Ready | `submissions/Craftsmen_Hackathon_Deck.pptx` 和 `submissions/Craftsmen_Hackathon_Deck_Preview.pdf` 已生成 | 提交前补团队信息、试玩链接、视频链接 |
| CodeBuddy 历史对话 | Needs External Action | `docs/CODEBUDDY_EXPORT_GUIDE.md` 和 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 已准备 | 用 CodeBuddy 做最终检查并导出历史记录 |
| GitHub 仓库链接 | Ready | `https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master` 已写入提交信息表 | 最终确认仓库可访问 |
| 报名表文案 | Ready | `docs/SUBMISSION_FORM_DRAFT.md` 和 `submissions/FINAL_SUBMISSION_INFO.md` 已准备 | 填表时复制，并替换所有 `TODO` |

## 评分点材料审计

| 评分点 | 状态 | 当前证据 | 说明 |
| --- | --- | --- | --- |
| 主题契合度：赛题三 | Ready | `README.md`、`docs/PROJECT_PROPOSAL_ZH.md`、`submissions/Craftsmen_Hackathon_Deck.pptx` | 已明确“叙事类游戏 / AI 重塑叙事体验” |
| AI 使用说明 | Ready | `docs/AI_CREATION_LOG.md`、`submissions/FINAL_SUBMISSION_INFO.md` | 已说明 AI 用于叙事、美术辅助、编程协作 |
| 游戏品质说明 | Ready | `docs/DEMO_STAGE_FLOW.md`、`docs/DEMO_VIDEO_SCRIPT.md` | 已有三段式 Demo 路线与视频脚本 |
| 真实可玩验证 | Needs External Action | `docs/WEBGL_DEPLOYMENT.md` | 仍需要 WebGL 在线链接作为最终证据 |
| CodeBuddy 使用证明 | Needs External Action | `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` | 仍需要实际导出的 CodeBuddy 历史文件 |

## 当前最短提交路径

1. 在 Unity 中构建 WebGL。
2. 上传 WebGL 到可公开访问的平台，拿到在线试玩链接。
3. 按 `submissions/DEMO_RECORDING_RUNBOOK.md` 录制并上传 Demo 视频。
4. 按 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 用 CodeBuddy 做最终检查并导出历史记录。
5. 打开 `submissions/Craftsmen_Hackathon_Deck.pptx`，补团队信息、试玩链接和视频链接。
6. 打开 `submissions/FINAL_SUBMISSION_INFO.md`，替换所有 `TODO`。
7. 按 `docs/SUBMISSION_FORM_DRAFT.md` 和最终链接填写比赛提交问卷。

## 不应提交或上传为公开仓库原始文件的内容

- Unity `Library/`、`Temp/`、`Logs/`、`UserSettings/`。
- Unity Asset Store 原始素材包，除非仓库权限和授权已确认。
- 本地构建缓存，例如 `outputs/`。
- 未检查授权的第三方压缩包、临时截图、缓存文件。

## 结论

当前仓库已经准备好了提交文案、PPT 初稿、PPT PDF 预览版、录屏脚本、CodeBuddy 导出清单和 WebGL 部署说明。最终提交尚未完成，因为缺少三个外部产物：

- WebGL 在线试玩链接。
- Demo 视频链接。
- CodeBuddy 历史对话导出文件或链接。
