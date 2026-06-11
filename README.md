# 能工智人 / Craftsmen and Homo sapiens

## 项目定位

《能工智人 / Craftsmen and Homo sapiens》是一个面向 **AI CAN DO IT | 腾讯云黑客松游戏开发挑战赛** 的 Unity 2D 横版动作地牢 Demo。项目选择方向为 **赛题三：叙事类游戏 / AI 重塑叙事体验**。

一句话介绍：

> 玩家进入一座由 AI 档案记录重构的地下工坊，在横版战斗中击败异常敌人、获取成长、挑战 Demo Boss，并通过房间叙事日志逐步理解“工匠”与“智人”分裂的真相。

## 当前 Demo 流程

当前原型围绕 3 到 5 分钟演示设计：

1. Start / 开场说明
2. Early Room：熟悉移动、跳跃、攻击
3. 击败小怪，获取经验或成长反馈
4. Mid Room：面对更强敌人，展示数值成长
5. Boss Room：击败 Demo Boss
6. Victory / Demo Complete

## 操作方式

- 移动：`A / D` 或方向键
- 跳跃：`Space`
- 攻击：`J` 或鼠标左键
- 背包：`I`
- 装置部署：`5`
- NPC / 对话交互：`E`

具体按键以 Unity 场景内当前配置为准。

## AI 创作说明

本项目的 AI 使用重点不是实时接入大模型 API，而是把 AI 作为创作过程和叙事包装的一部分：

- AI 辅助生成世界观、房间档案、Boss 设定和结局文本。
- AI 辅助生成角色、敌人、道具、UI、地图风格等占位美术方向。
- AI / CodeBuddy 辅助完成 Unity C# 脚本、场景搭建说明、文档和调试流程。

详细说明见：

- `docs/AI_CREATION_LOG.md`
- `docs/PROJECT_PROPOSAL_ZH.md`
- `docs/HACKATHON_SUBMISSION_CHECKLIST.md`

## 如何在 Unity 中运行

1. 使用 Unity `2022.3.53f1 / 2022.3.53f1c1` 打开项目。
2. 打开 `Assets/Scenes/SampleScene.unity`。
3. 点击 Play 测试当前 Demo。
4. 如果后续生成正式提交场景，可切换到 `Assets/Scenes/HackathonDemo.unity`。

## WebGL 构建与部署

当前提交材料建议 WebGL 输出路径为：

`Build/WebGL`

部署建议：

- 腾讯云 Cloud Studio 静态站点
- GitHub Pages
- 其他可托管 Unity WebGL 静态文件的平台

详细步骤见：

`docs/WEBGL_DEPLOYMENT.md`

## 黑客松提交包

最终提交材料集中放在：

`submissions/README.md`

已准备好的材料包括：

- `submissions/Craftsmen_Hackathon_Deck.pptx`
  - 10 页作品介绍 PPT 初稿。
- `submissions/Craftsmen_Hackathon_Deck_Preview.pdf`
  - PPT 的 10 页 PDF 预览版。
- `submissions/FINAL_SUBMISSION_INFO.md`
  - 最终提交问卷用的信息总表。
- `submissions/FINAL_DELIVERABLES_MANIFEST.md`
  - 最终交付包清单，集中说明哪些文件能直接上传、哪些还缺外部链接。
- `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md`
  - 最终提交当天执行手册，按顺序处理 WebGL、视频、CodeBuddy、PPT 和问卷回填。
- `submissions/EXTERNAL_ACTION_OWNER_BOARD_ZH.md`
  - 外部动作负责人看板，用于认领 WebGL、视频、CodeBuddy、团队信息和最终提交任务。
- `submissions/SUBMISSION_RISK_REGISTER_ZH.md`
  - 最终提交风险登记表，集中列出 WebGL、视频、CodeBuddy、链接和素材等风险兜底。
- `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md`
  - 最终占位符清理清单，避免把 `TODO`、本地路径或 `localhost` 填进正式材料。
- `submissions/FINAL_SUBMISSION_AUTOCHECK_ZH.md`
  - 最终提交自动自检说明，可按说明运行 `submissions/Run-FinalSubmissionAudit.ps1` 扫描明显红旗。
- `submissions/UPLOAD_PACKAGE_README_ZH.md`
  - 最终上传包说明，用于准备附件包、网盘包或老师审核包。
- `submissions/FINAL_UPLOAD_PACKAGE_BUILDER_ZH.md`
  - 最终上传包组装说明，可按说明运行 `submissions/Build-FinalUploadPackage.ps1` 生成推荐目录结构。
- `submissions/00_README_FIRST_ZH.md`
  - 可直接放进最终附件包根目录的“先看我”说明文件。
- `submissions/package_templates/`
  - 最终上传包可用的 WebGL、Demo 视频、PPT、CodeBuddy、GitHub 链接模板。
- `submissions/LINK_BACKFILL_TOOL_ZH.md`
  - 最终链接统一回填工具说明，可在外部链接齐后批量更新常用文本材料。
- `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
  - 最终提交问卷复制稿。
- `submissions/SUBMISSION_PORTAL_FIELD_MAP_ZH.md`
  - 提交平台字段映射表，说明每个字段从哪个材料复制、如何验证。
- `submissions/PROJECT_BOOK_FINAL_ZH.md`
  - 最终项目书提交版。
- `submissions/PROJECT_BOOK_FINAL_ZH.docx`
  - 最终项目书 Word 版，可直接上传或转 PDF。
- `submissions/PROJECT_BOOK_FINAL_ZH.pdf`
  - 最终项目书 PDF 版，可直接上传或转发预览。
- `submissions/JUDGE_QUICK_START.md`
  - 评委快速打开指南。
- `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md`
  - 评委单页摘要，方便快速理解项目、AI 用法和评审看点。
- `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf`
  - 评委单页摘要 PDF 版，可直接转发或上传。
- `submissions/SCORING_EVIDENCE_MAP_ZH.md`
  - 评分项证据映射表，把主题契合度、AI 工具使用和游戏品质对应到文件与 Demo 画面。
- `submissions/ROADSHOW_QA_CHEATSHEET_ZH.md`
  - 路演答辩速查卡，适合现场快速回答评委常见问题。
- `submissions/SUBMISSION_READINESS_AUDIT.md`
  - 最终提交前的就绪审计表。
- `submissions/WEBGL_UPLOAD_RUNBOOK.md`
  - WebGL 在线试玩链接执行清单。
- `submissions/DEMO_RECORDING_RUNBOOK.md`
  - Demo 录屏执行清单。
- `submissions/DEMO_VIDEO_UPLOAD_COPY.md`
  - Demo 视频上传标题、简介、标签和置顶评论文案。
- `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md`
  - CodeBuddy 历史记录导出检查清单。
- `submissions/TEAM_INFO_TEMPLATE.md`
  - 团队成员信息模板。
- `submissions/TEAM_HANDOFF_MESSAGE_ZH.md`
  - 最终提交团队交接消息模板，可直接发给队友或老师。

仍需在最终提交前外部补齐：

- 在线试玩链接。
- Demo 视频链接。
- CodeBuddy 历史对话导出文件或链接。
- 团队成员姓名、学校和分工。
- PPT 最后一页中的真实链接与团队信息。

## 重要说明

本仓库包含第三方素材记录。第三方资源来源、用途和提交策略见：

`docs/THIRD_PARTY_ASSETS.md`

比赛提交时请确认素材许可证、在线试玩链接、视频链接和 CodeBuddy 历史记录均已准备完成。
