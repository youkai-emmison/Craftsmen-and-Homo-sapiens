# 腾讯云黑客松提交就绪审计

审计日期：2026-06-11

这个文件用于在最终提交前逐项确认材料是否真的存在。状态分为：

- `Ready`：仓库中已有可直接使用的文件或链接。
- `Needs External Action`：需要在 Unity、CodeBuddy、视频平台或部署平台完成外部动作。
- `Optional`：加分项或非必交项。

## 必交材料审计

| 项目 | 状态 | 当前证据 | 下一步 |
| --- | --- | --- | --- |
| 作品网页链接 | Needs External Action | `docs/WEBGL_DEPLOYMENT.md`、`submissions/WEBGL_UPLOAD_RUNBOOK.md` 和 `submissions/WEBGL_PAGE_COPY.md` 已准备 | 构建 WebGL，上传到可公开访问的平台，把链接填入 `submissions/FINAL_SUBMISSION_INFO.md` |
| 游戏 Demo 视频 | Needs External Action | `docs/DEMO_VIDEO_SCRIPT.md` 和 `submissions/DEMO_RECORDING_RUNBOOK.md` 已准备 | 录制 3 分钟以内视频，上传后填写视频链接 |
| Demo 视频上传文案 | Ready | `submissions/DEMO_VIDEO_UPLOAD_COPY.md` 已准备 | 上传视频时复制标题、简介、标签和置顶评论 |
| 作品介绍 PPT | Ready | `submissions/Craftsmen_Hackathon_Deck.pptx` 和 `submissions/Craftsmen_Hackathon_Deck_Preview.pdf` 已生成 | 提交前补团队信息、试玩链接、视频链接 |
| CodeBuddy 历史对话 | Needs External Action | `docs/CODEBUDDY_EXPORT_GUIDE.md` 和 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 已准备 | 用 CodeBuddy 做最终检查并导出历史记录 |
| GitHub 仓库链接 | Ready | `https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master` 已写入提交信息表 | 最终确认仓库可访问 |
| 最终交付包清单 | Ready | `submissions/FINAL_DELIVERABLES_MANIFEST.md` 已准备 | 最后提交前按最小提交包逐项核对 |
| 报名表文案 | Ready | `docs/SUBMISSION_FORM_DRAFT.md`、`submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 和 `submissions/FINAL_SUBMISSION_INFO.md` 已准备 | 填表时复制，并替换所有外部链接占位 |
| 最终项目书 | Ready | `submissions/PROJECT_BOOK_FINAL_ZH.md`、`submissions/PROJECT_BOOK_FINAL_ZH.docx` 和 `submissions/PROJECT_BOOK_FINAL_ZH.pdf` 已准备 | PDF 可直接上传或预览 |
| 评委单页摘要 | Ready | `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md` 和 `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf` 已准备 | 可和试玩链接一起发给评委或老师 |
| 团队交接消息 | Ready | `submissions/TEAM_HANDOFF_MESSAGE_ZH.md` 已准备 | 可直接发给队友或老师，说明材料状态和剩余动作 |
| 最终链接回填表 | Ready | `submissions/LINKS_TO_FILL.md` 已准备 | 拿到外部链接后先填入此表，再同步到各提交材料 |
| 最终提交当天执行手册 | Ready | `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md` 已准备 | 提交当天按顺序处理 WebGL、视频、CodeBuddy、PPT 和问卷回填 |
| 外部动作负责人看板 | Ready | `submissions/EXTERNAL_ACTION_OWNER_BOARD_ZH.md` 已准备 | 最终分工时认领外部动作和产物 |
| 最终提交风险登记表 | Ready | `submissions/SUBMISSION_RISK_REGISTER_ZH.md` 已准备 | 最后一天按高优先级风险逐项兜底 |
| 最终占位符清理清单 | Ready | `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md` 已准备 | 最终提交前清理 `TODO`、`待回填`、本地路径和 `localhost` |
| 最终提交自动自检 | Ready | `submissions/FINAL_SUBMISSION_AUTOCHECK_ZH.md` 和 `submissions/Run-FinalSubmissionAudit.ps1` 已准备 | 最终链接回填后运行一次，检查明显红旗 |
| 最终上传包说明 | Ready | `submissions/UPLOAD_PACKAGE_README_ZH.md` 已准备 | 如果平台要求附件包或网盘包，按此文件组织 |
| 最终上传包组装脚本 | Ready | `submissions/FINAL_UPLOAD_PACKAGE_BUILDER_ZH.md` 和 `submissions/Build-FinalUploadPackage.ps1` 已准备 | 最终链接回填后生成上传包目录 |
| 上传包先看我 | Ready | `submissions/00_README_FIRST_ZH.md` 已准备 | 可复制到最终压缩包根目录 |
| 上传包链接模板 | Ready | `submissions/package_templates/` 已准备 | 最终打包时复制并替换 `TODO` |
| 提交平台字段映射表 | Ready | `submissions/SUBMISSION_PORTAL_FIELD_MAP_ZH.md` 已准备 | 最终填问卷时按字段定位可复制文案 |
| 评委快速打开指南 | Ready | `submissions/JUDGE_QUICK_START.md` 已准备 | 填入 WebGL 和 Demo 视频链接 |
| 评分项证据映射表 | Ready | `submissions/SCORING_EVIDENCE_MAP_ZH.md` 已准备 | 视频、PPT 和答辩前检查评分点是否覆盖 |
| 路演口播稿 | Ready | `submissions/ROADSHOW_PITCH_SCRIPT.md` 已准备 | 答辩或视频录制前按真实链接微调 |
| 路演答辩速查卡 | Ready | `submissions/ROADSHOW_QA_CHEATSHEET_ZH.md` 已准备 | 现场答辩前快速排练 |
| 团队成员信息 | Needs External Action | `submissions/TEAM_INFO_TEMPLATE.md` 已准备 | 填入真实团队名称、学校、成员和分工 |

## 评分点材料审计

| 评分点 | 状态 | 当前证据 | 说明 |
| --- | --- | --- | --- |
| 主题契合度：赛题三 | Ready | `README.md`、`docs/PROJECT_PROPOSAL_ZH.md`、`submissions/Craftsmen_Hackathon_Deck.pptx` | 已明确“叙事类游戏 / AI 重塑叙事体验” |
| AI 使用说明 | Ready | `docs/AI_CREATION_LOG.md`、`submissions/FINAL_SUBMISSION_INFO.md` | 已说明 AI 用于叙事、美术辅助、编程协作 |
| 游戏品质说明 | Ready | `docs/DEMO_STAGE_FLOW.md`、`docs/DEMO_VIDEO_SCRIPT.md` | 已有三段式 Demo 路线与视频脚本 |
| 真实可玩验证 | Needs External Action | `docs/WEBGL_DEPLOYMENT.md`、`submissions/WEBGL_UPLOAD_RUNBOOK.md` 和 `submissions/WEBGL_PAGE_COPY.md` | 仍需要 WebGL 在线链接作为最终证据 |
| CodeBuddy 使用证明 | Needs External Action | `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` | 仍需要实际导出的 CodeBuddy 历史文件 |

## 当前最短提交路径

1. 按 `submissions/WEBGL_UPLOAD_RUNBOOK.md` 在 Unity 中构建 WebGL。
2. 上传 WebGL 到可公开访问的平台，拿到在线试玩链接。
3. 按 `submissions/DEMO_RECORDING_RUNBOOK.md` 录制并上传 Demo 视频。
4. 按 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 用 CodeBuddy 做最终检查并导出历史记录。
5. 打开 `submissions/Craftsmen_Hackathon_Deck.pptx`，补团队信息、试玩链接和视频链接。
6. 打开 `submissions/JUDGE_QUICK_START.md`，补 WebGL、视频和团队信息。
7. 打开 `submissions/TEAM_INFO_TEMPLATE.md`，补真实团队成员和分工。
8. 打开 `submissions/LINKS_TO_FILL.md`，集中填写所有外部链接。
9. 打开 `submissions/FINAL_SUBMISSION_INFO.md`，替换所有 `TODO`。
10. 打开 `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md`，确认最终材料没有占位符和本地路径。
11. 按 `submissions/FINAL_SUBMISSION_AUTOCHECK_ZH.md` 运行自动自检脚本。
12. 按 `docs/SUBMISSION_FORM_DRAFT.md` 和最终链接填写比赛提交问卷。

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
