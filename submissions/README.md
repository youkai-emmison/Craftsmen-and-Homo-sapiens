# 腾讯云黑客松提交成品目录

这个目录放最终可以直接提交或转交给队友的成品材料。

## 已生成

- `submissions/Craftsmen_Hackathon_Deck.pptx`
  - 10 页作品介绍 PPT 初稿。
  - 内容覆盖项目定位、赛题三契合点、AI 创作说明、三段式 Demo、技术结构、提交链路。
  - 生成日期：2026-06-11。

- `submissions/Craftsmen_Hackathon_Deck_Preview.pdf`
  - 10 页作品介绍 PDF 预览版。
  - 内容来自同一版 PPT 预览图，适合快速上传、预览或转发给队友。

- `submissions/FINAL_SUBMISSION_INFO.md`
  - 最终提交问卷用的信息总表。
  - 把作品简介、AI 使用说明、材料路径和待补链接集中到一个文件里。

- `submissions/FINAL_DELIVERABLES_MANIFEST.md`
  - 最终交付包清单。
  - 集中说明哪些文件能直接上传、哪些只是辅助材料、哪些还缺外部链接。

- `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md`
  - 最终提交当天执行手册。
  - 按顺序处理 WebGL、Demo 视频、CodeBuddy 历史记录、PPT 更新和提交问卷回填。

- `submissions/EXTERNAL_ACTION_OWNER_BOARD_ZH.md`
  - 外部动作负责人看板。
  - 用于团队认领 WebGL、Demo 视频、CodeBuddy 导出、团队信息、PPT 回填和最终提交问卷。

- `submissions/SUBMISSION_RISK_REGISTER_ZH.md`
  - 最终提交风险登记表。
  - 集中列出 WebGL 黑屏、视频权限、CodeBuddy 记录、链接不一致、素材授权等风险与兜底动作。

- `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md`
  - 最终占位符清理清单。
  - 区分哪些 `TODO` 是仓库模板可以保留，哪些必须在提交问卷、PPT、评委入口和上传包中替换。

- `submissions/FINAL_SUBMISSION_AUTOCHECK_ZH.md`
  - 最终提交自动自检说明。
  - 说明如何运行 `Run-FinalSubmissionAudit.ps1`，自动检查必备文件、占位符、本地路径和 Git 风险项。

- `submissions/Run-FinalSubmissionAudit.ps1`
  - 最终提交自检脚本。
  - 不修改文件，只扫描提交材料状态；外部链接未回填时会故意报失败，提醒还不能最终提交。

- `submissions/UPLOAD_PACKAGE_README_ZH.md`
  - 最终上传包说明。
  - 用于准备附件包、网盘包或老师审核包，明确哪些文件要放、哪些 Unity 本地目录不要放。

- `submissions/FINAL_UPLOAD_PACKAGE_BUILDER_ZH.md`
  - 最终上传包组装脚本说明。
  - 说明如何运行 `Build-FinalUploadPackage.ps1`，自动按推荐目录结构复制项目书、PPT、链接模板和备用材料。

- `submissions/Build-FinalUploadPackage.ps1`
  - 最终上传包组装脚本。
  - 每次生成带时间戳的新目录，不删除旧包、不覆盖已有文件；生成目录已被 `.gitignore` 忽略。

- `submissions/00_README_FIRST_ZH.md`
  - 最终附件包根目录“先看我”说明文件。
  - 打包时可复制成 `00_README_先看我.md` 放在压缩包根目录。

- `submissions/package_templates/`
  - 最终上传包链接模板。
  - 包含 WebGL、Demo 视频、PPT、CodeBuddy、GitHub 和社交媒体链接说明模板。

- `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
  - 最终提交问卷复制稿。
  - 按作品名称、简介、AI 使用说明、技术实现、链接区等常见字段整理。

- `submissions/SUBMISSION_PORTAL_FIELD_MAP_ZH.md`
  - 提交平台字段映射表。
  - 说明每个常见平台字段应从哪个文件复制、哪些链接要回填、如何验证。

- `submissions/PROJECT_BOOK_FINAL_ZH.md`
  - 最终项目书提交版。
  - 比 `docs/PROJECT_PROPOSAL_ZH.md` 更适合直接交给评委、报名表或老师审核。

- `submissions/PROJECT_BOOK_FINAL_ZH.docx`
  - 最终项目书 Word 版。
  - 可直接作为附件上传，或手动另存为 PDF。

- `submissions/PROJECT_BOOK_FINAL_ZH.pdf`
  - 最终项目书 PDF 版。
  - 可直接上传、转发或给评委预览。

- `submissions/LINKS_TO_FILL.md`
  - 最终链接回填表。
  - 用于集中填写 WebGL、Demo 视频、PPT、CodeBuddy、社交媒体等外部链接。

- `submissions/JUDGE_QUICK_START.md`
  - 评委快速打开指南。
  - 集中说明试玩链接、操作键位、推荐路线、评审看点和打不开时的备用材料。

- `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md`
  - 评委单页摘要。
  - 用一页说明项目定位、AI 用法、演示流程和评审看点。

- `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf`
  - 评委单页摘要 PDF 版。
  - 适合直接转发、上传或给老师快速预览。

- `submissions/SCORING_EVIDENCE_MAP_ZH.md`
  - 评分项证据映射表。
  - 把主题契合度、AI 工具使用和游戏品质对应到证据文件与 Demo 画面。

- `submissions/ROADSHOW_PITCH_SCRIPT.md`
  - 路演口播稿。
  - 包含 30 秒版、60 秒版、Demo 视频开头版和答辩收尾版。

- `submissions/ROADSHOW_QA_CHEATSHEET_ZH.md`
  - 路演答辩速查卡。
  - 把常见评委问题压缩成短答案，适合现场排练和临时补位。

- `submissions/TEAM_INFO_TEMPLATE.md`
  - 团队成员信息模板。
  - 用于补报名表、PPT 最后一页、视频片尾和作品页。

- `submissions/TEAM_HANDOFF_MESSAGE_ZH.md`
  - 最终提交团队交接消息模板。
  - 可直接复制到 QQ / 微信 / 飞书，告诉队友还缺哪些外部动作。

- `submissions/WEBGL_UPLOAD_RUNBOOK.md`
  - WebGL 在线试玩链接执行清单。
  - 用于从 Unity 构建 WebGL、上传静态网页、检查公开试玩链接。

- `submissions/WEBGL_PAGE_COPY.md`
  - WebGL 作品页可复制文案。
  - 用于填写试玩页面标题、简介、操作说明、标签和备用链接。

- `submissions/DEMO_RECORDING_RUNBOOK.md`
  - Demo 录屏时逐项检查的执行清单。
  - 用来控制 3 分钟视频节奏，避免漏掉 AI 叙事、成长、Boss 和胜利。

- `submissions/DEMO_VIDEO_UPLOAD_COPY.md`
  - Demo 视频上传文案。
  - 包含视频标题、简介、标签、置顶评论和上传后检查项。

- `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md`
  - CodeBuddy 历史记录导出前的检查清单。
  - 包含可直接复制给 CodeBuddy 的最终检查提示词。

- `submissions/SUBMISSION_READINESS_AUDIT.md`
  - 最终提交前的就绪审计表。
  - 用 Ready / Needs External Action 标出哪些材料已存在、哪些还必须外部生成。

## 还需要手动补齐

- WebGL 在线试玩链接。
- Demo 视频链接。
- CodeBuddy 历史对话导出文件。
- 团队成员姓名、学校、分工等最终信息。
- 如果 PPT 中需要真实截图，可以在 Unity 最终可演示版本稳定后替换部分素材页。

## 使用建议

1. 先打开 `Craftsmen_Hackathon_Deck.pptx` 检查文字和版式。
2. 需要快速预览或上传时，可以使用 `Craftsmen_Hackathon_Deck_Preview.pdf`。
3. 最终提交前先看 `FINAL_DELIVERABLES_MANIFEST.md`，确认最小提交包是否齐。
4. 最后一天按 `SUBMISSION_DAY_RUNBOOK_ZH.md` 的顺序执行，不要分散回填链接。
5. 用 `EXTERNAL_ACTION_OWNER_BOARD_ZH.md` 给 WebGL、视频、CodeBuddy、团队信息和提交问卷分配负责人。
6. 用 `SUBMISSION_RISK_REGISTER_ZH.md` 检查高优先级风险和兜底动作。
7. 用 `PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md` 检查哪些占位符可以保留、哪些必须清空。
8. 用 `FINAL_SUBMISSION_AUTOCHECK_ZH.md` 里的命令跑一次自动自检。
9. 如果平台要求附件或网盘包，按 `UPLOAD_PACKAGE_README_ZH.md` 组织文件。
10. 用 `FINAL_UPLOAD_PACKAGE_BUILDER_ZH.md` 里的命令生成最终上传包目录。
11. 打包时把生成目录里的链接模板替换成真实链接。
12. 填报名表时优先使用 `FORM_ANSWERS_COPYPASTE_ZH.md`。
13. 如果平台字段很多，用 `SUBMISSION_PORTAL_FIELD_MAP_ZH.md` 对照字段逐项复制。
14. 如需正式项目书，优先使用 `PROJECT_BOOK_FINAL_ZH.pdf`；需要可编辑版本时使用 `PROJECT_BOOK_FINAL_ZH.docx`，需要复制内容时再看 `PROJECT_BOOK_FINAL_ZH.md`。
15. 按 `TEAM_INFO_TEMPLATE.md` 补齐团队成员和分工。
16. 如果要交接给队友，直接复制 `TEAM_HANDOFF_MESSAGE_ZH.md` 的群消息模板。
17. 把团队成员、试玩链接、视频链接补进最后一页或备注里。
18. 按 `WEBGL_UPLOAD_RUNBOOK.md` 构建、上传并检查 WebGL 在线试玩链接。
19. 上传试玩页时复制 `WEBGL_PAGE_COPY.md` 的页面文案。
20. 按 `DEMO_RECORDING_RUNBOOK.md` 录制 Demo 视频。
21. 上传视频时复制 `DEMO_VIDEO_UPLOAD_COPY.md` 的标题、简介和标签。
22. 按 `CODEBUDDY_SUBMISSION_CHECKLIST.md` 导出 CodeBuddy 历史记录。
23. 先把所有外部链接填进 `LINKS_TO_FILL.md`。
24. 用 `ROADSHOW_PITCH_SCRIPT.md` 排练 30 秒和 60 秒介绍。
25. 用 `ROADSHOW_QA_CHEATSHEET_ZH.md` 准备常见评委问题。
26. 用 `SCORING_EVIDENCE_MAP_ZH.md` 检查视频、PPT 和答辩是否覆盖评分点。
27. 把 `JUDGE_QUICK_START.md` 中的 WebGL、视频、团队信息占位替换掉。
28. 打开 `SUBMISSION_READINESS_AUDIT.md` 确认所有必交项状态。
29. 优先按 `FINAL_SUBMISSION_INFO.md` 填提交表，再参考 `docs/SUBMISSION_FORM_DRAFT.md` 补充长文案。
