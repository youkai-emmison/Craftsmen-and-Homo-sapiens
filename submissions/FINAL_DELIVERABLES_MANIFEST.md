# 腾讯云黑客松最终交付包清单

这个文件用于最后提交前确认“哪些材料已经能上传、哪些还只是辅助文档、哪些仍缺外部链接”。如果时间很紧，优先按本文件的“最小提交包”处理。

## 最小提交包

| 材料 | 当前文件或链接 | 状态 | 提交用途 | 最后动作 |
| --- | --- | --- | --- | --- |
| 在线试玩链接 | https://craftsmen-and-homo-sapiens.onrender.com | Ready | 作品网页链接 / 试玩入口 | 按 `WEBGL_UPLOAD_RUNBOOK.md` 构建上传，并填入 `LINKS_TO_FILL.md` |
| Demo 视频 | 视频链接待回填 | Needs External Action | 展示核心玩法和 AI 使用 | 按 `DEMO_RECORDING_RUNBOOK.md` 录制上传，并填入 `LINKS_TO_FILL.md` |
| CodeBuddy 历史记录 | 导出文件或链接待回填 | Needs External Action | 证明 AI 编程助手参与 | 按 `CODEBUDDY_SUBMISSION_CHECKLIST.md` 导出 |
| GitHub 仓库 | https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master | Ready | 代码仓库链接 | 最终确认仓库可访问 |
| 作品介绍 PPT | `submissions/Craftsmen_Hackathon_Deck.pptx` | Ready, Needs Link Backfill | PPT 附件或路演材料 | 补团队信息、WebGL 链接、视频链接 |
| PPT PDF 预览 | `submissions/Craftsmen_Hackathon_Deck_Preview.pdf` | Ready, Needs Link Backfill | 快速预览或备用上传 | 如 PPT 改动较大，重新导出 |
| 最终项目书 PDF | `submissions/PROJECT_BOOK_FINAL_ZH.pdf` | Ready | 项目书附件 / 老师审核 / 评委预览 | 本地打开检查版式 |
| 最终项目书 Word | `submissions/PROJECT_BOOK_FINAL_ZH.docx` | Ready | 可编辑项目书附件 | 如需改团队信息，可编辑此版 |
| 最终提交信息表 | `submissions/FINAL_SUBMISSION_INFO.md` | Ready, Needs Link Backfill | 填问卷总控 | 替换所有 `TODO` |
| 问卷复制稿 | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` | Ready, Needs Link Backfill | 复制到提交问卷 | 回填外部链接后再复制 |

## 辅助提交材料

| 材料 | 文件 | 用途 |
| --- | --- | --- |
| 最终链接回填表 | `submissions/LINKS_TO_FILL.md` | 集中填写 WebGL、视频、PPT、CodeBuddy、社交媒体链接 |
| 最终提交当天执行手册 | `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md` | 按顺序处理 WebGL、视频、CodeBuddy、PPT 和问卷回填 |
| 外部动作负责人看板 | `submissions/EXTERNAL_ACTION_OWNER_BOARD_ZH.md` | 分配 WebGL、视频、CodeBuddy、团队信息、PPT 和最终提交负责人 |
| 最终提交风险登记表 | `submissions/SUBMISSION_RISK_REGISTER_ZH.md` | 集中列出 WebGL、视频、CodeBuddy、链接、PPT 和素材授权等风险兜底 |
| 最终占位符清理清单 | `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md` | 区分哪些模板占位符可以留在仓库，哪些必须在提交材料中替换 |
| 最终提交自动自检 | `submissions/FINAL_SUBMISSION_AUTOCHECK_ZH.md`、`submissions/Run-FinalSubmissionAudit.ps1` | 自动扫描必备文件、占位符、本地路径和 Git 风险项 |
| 最终上传包说明 | `submissions/UPLOAD_PACKAGE_README_ZH.md` | 准备附件包、网盘包或老师审核包 |
| 最终上传包组装脚本 | `submissions/FINAL_UPLOAD_PACKAGE_BUILDER_ZH.md`、`submissions/Build-FinalUploadPackage.ps1` | 按推荐目录结构复制提交材料，生成带时间戳的上传包目录 |
| 上传包先看我 | `submissions/00_README_FIRST_ZH.md` | 可复制到最终压缩包根目录，指导评委查看材料 |
| 上传包链接模板 | `submissions/package_templates/` | 最终压缩包中 WebGL、视频、PPT、CodeBuddy、GitHub 链接 txt 模板 |
| 提交平台字段映射表 | `submissions/SUBMISSION_PORTAL_FIELD_MAP_ZH.md` | 按平台字段定位可复制文案和验证标准 |
| 链接统一回填工具 | `submissions/LINK_BACKFILL_TOOL_ZH.md`、`submissions/Apply-LinkBackfill.ps1` | 外部链接齐后批量回填常用 Markdown 和 txt 链接模板 |
| 评委快速打开指南 | `submissions/JUDGE_QUICK_START.md` | 和作品链接一起给评委，说明怎么玩、看什么 |
| 评委单页摘要 | `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md` | 用一页说明项目定位、AI 用法、演示流程和评审看点 |
| 评委单页摘要 PDF | `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf` | 可直接转发、上传或给老师快速预览 |
| 评分项证据映射表 | `submissions/SCORING_EVIDENCE_MAP_ZH.md` | 对齐主题契合度、AI 工具使用和游戏品质评分点 |
| WebGL 上传清单 | `submissions/WEBGL_UPLOAD_RUNBOOK.md` | 构建、上传、检查在线试玩链接 |
| WebGL 页面文案 | `submissions/WEBGL_PAGE_COPY.md` | 粘贴到 itch.io、GitHub Pages、静态站点或作品页 |
| Demo 录屏清单 | `submissions/DEMO_RECORDING_RUNBOOK.md` | 录视频时防止漏掉 AI 叙事、成长、Boss 和胜利 |
| Demo 视频上传文案 | `submissions/DEMO_VIDEO_UPLOAD_COPY.md` | 上传视频时复制标题、简介、标签和置顶评论 |
| CodeBuddy 检查清单 | `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` | 导出 CodeBuddy 历史前做最终检查 |
| 团队信息模板 | `submissions/TEAM_INFO_TEMPLATE.md` | 填成员、学校、分工、联系方式 |
| 团队交接消息 | `submissions/TEAM_HANDOFF_MESSAGE_ZH.md` | 可直接发给队友或老师，说明材料状态和剩余动作 |
| 路演口播稿 | `submissions/ROADSHOW_PITCH_SCRIPT.md` | 答辩、视频开头或现场展示时使用 |
| 路演答辩速查卡 | `submissions/ROADSHOW_QA_CHEATSHEET_ZH.md` | 现场快速回答评委常见问题 |
| 提交就绪审计 | `submissions/SUBMISSION_READINESS_AUDIT.md` | 最终提交前逐项确认 Ready / Needs External Action |

## 推荐上传顺序

1. 先确认 Unity Demo 能 Play，避免录屏和 WebGL 链接出现基础问题。
2. 构建并上传 WebGL，拿到在线试玩链接。
3. 录制 Demo 视频，并按 `DEMO_VIDEO_UPLOAD_COPY.md` 填写视频标题、简介和标签。
4. 导出 CodeBuddy 历史记录，上传或整理成可访问文件。
5. 打开 `LINKS_TO_FILL.md`，集中回填 WebGL、视频、PPT、CodeBuddy 链接。
6. 如需减少手动漏改，按 `LINK_BACKFILL_TOOL_ZH.md` 批量回填常用文本材料。
7. 打开 `FINAL_SUBMISSION_INFO.md` 和 `FORM_ANSWERS_COPYPASTE_ZH.md`，检查所有外部链接占位是否替换。
8. 按 `FINAL_SUBMISSION_AUTOCHECK_ZH.md` 跑一次自动自检，确认最终面对评委的材料没有占位符红旗。
9. 如需附件包，按 `FINAL_UPLOAD_PACKAGE_BUILDER_ZH.md` 生成上传包目录，并替换其中链接模板。
10. 打开 PPT，补团队信息、试玩链接、视频链接。
11. 最后按提交问卷要求上传 PPT、项目书 PDF、视频链接、试玩链接和 CodeBuddy 记录。

## 最后 10 分钟检查

- [ ] WebGL 链接不是 `localhost`，无痕浏览器可打开。
- [ ] Demo 视频公开可访问，能在 3 分钟左右讲清楚核心流程。
- [ ] GitHub 链接指向 `master`，不是旧分支。
- [ ] PPT 最后一页有团队信息、试玩链接、视频链接。
- [ ] 项目书 PDF 能打开，文件不是空白。
- [ ] CodeBuddy 历史记录已导出并能访问。
- [ ] `FINAL_SUBMISSION_INFO.md` 中没有必填 `TODO`。
- [ ] 提交问卷中的链接和 `LINKS_TO_FILL.md` 一致。
- [ ] 已按 `PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md` 扫过 `TODO`、`待回填`、本地路径和 `localhost`。
- [ ] 已运行 `Run-FinalSubmissionAudit.ps1`，并理解所有失败或警告。

## 当前还缺的外部动作

- WebGL 在线试玩链接。
- Demo 视频链接。
- CodeBuddy 历史导出文件或链接。
- 团队成员真实姓名、学校、分工。
- PPT 最后一页真实链接和团队信息。
