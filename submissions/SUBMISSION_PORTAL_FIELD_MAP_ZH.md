# 提交平台字段映射表

这个文件用于最终填写腾讯云黑客松提交平台、学校初筛表或作品页。它把常见字段对应到现有材料，避免临时翻很多文档。

使用方式：

1. 先把 `submissions/LINKS_TO_FILL.md` 中的外部链接补齐。
2. 再打开本文件，按字段逐项复制。
3. 如果平台字段名称不同，就找含义最接近的一行。
4. 提交前用“验证标准”列做最后检查。

## 基础信息字段

| 平台字段 | 推荐填写内容 | 复制来源 | 验证标准 |
| --- | --- | --- | --- |
| 作品名称 | 能工智人 / Craftsmen and Homo sapiens | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 1 节 | 中英文名称一致 |
| 参赛赛题 | 赛题三：叙事类游戏 / AI 重塑叙事体验 | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 2 节 | 不要误选其他赛题 |
| 作品一句话介绍 | 《能工智人》是一款 Unity 2D 横版动作地牢 Demo，玩家进入由 AI 档案重构的地下工坊，在战斗、成长和 Boss 挑战中逐步理解“工匠”与“智人”分裂的真相。 | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 3 节 | 句子不宜过长，能突出 AI 叙事 |
| 项目类型 | Unity 2D 横版动作地牢 Demo / WebGL 游戏原型 | `submissions/FINAL_SUBMISSION_INFO.md` 基本信息 | 与 PPT 和项目书说法一致 |
| 推荐演示时长 | 3 分钟以内 | `submissions/FINAL_SUBMISSION_INFO.md` 基本信息 | 视频时长最好接近 3 分钟 |

## 作品介绍字段

| 平台字段 | 推荐填写内容 | 复制来源 | 验证标准 |
| --- | --- | --- | --- |
| 作品简介，短版 | 复制“作品简介，短版” | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 4 节 | 适合 200 到 300 字以内字段 |
| 作品简介，长版 | 复制“作品简介，长版” | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 5 节 | 适合项目详情、作品说明等长文本字段 |
| 项目亮点 | 复制“项目亮点”项目符号 | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 6 节 | 至少包含 AI、完整闭环、CodeBuddy 协作 |
| 当前完成度 | 复制“当前完成度” | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 10 节 | 不要承诺完整商业游戏 |
| 作品网页介绍 | 复制“作品网页 / WebGL 页面简介” | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 12 节，或 `submissions/WEBGL_PAGE_COPY.md` | 适合 WebGL 页面、作品页、视频简介 |

## AI 使用字段

| 平台字段 | 推荐填写内容 | 复制来源 | 验证标准 |
| --- | --- | --- | --- |
| AI 使用说明，短版 | 复制“AI 使用说明，短版” | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 7 节 | 明确 AI 用于叙事、美术辅助、编程协作 |
| AI 使用说明，长版 | 复制“AI 使用说明，长版” | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 8 节 | 适合需要详细说明 AI 工具使用的字段 |
| CodeBuddy 使用证明 | 说明 CodeBuddy / AI 编程助手辅助 Unity C# 脚本、调试、文档和提交材料整理，历史记录见附件或链接 | `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md`、`docs/CODEBUDDY_EXPORT_GUIDE.md` | 必须附上 CodeBuddy 导出文件或链接 |
| 为什么没接实时 AI API | 黑客松 Demo 优先稳定，采用 AI 预生成叙事内容 + 游戏内呈现，避免网络、延迟、成本和不可控输出风险 | `submissions/ROADSHOW_QA_CHEATSHEET_ZH.md` 第 3 节 | 不要说已经实现实时 AI NPC |
| AI 叙事体现 | NPC 对话、房间档案、敌人与 Boss 设定、胜利文本体现 AI 叙事 | `docs/AI_CREATION_LOG.md`、`submissions/SCORING_EVIDENCE_MAP_ZH.md` | 视频中最好展示一次对话或文本 |

## 技术实现字段

| 平台字段 | 推荐填写内容 | 复制来源 | 验证标准 |
| --- | --- | --- | --- |
| 技术栈 | Unity 2022.3 LTS、C#、WebGL | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 9 节 | Unity 版本与项目一致 |
| 核心模块 | Player、Combat、Enemy、Rooms、Inventory / Crafting、Dialogue、UI | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 第 9 节 | 不要把未完成系统写成完整商业级系统 |
| Demo 流程 | Early Room -> Mid Room -> Boss Room -> Victory | `docs/DEMO_STAGE_FLOW.md`、`submissions/SCORING_EVIDENCE_MAP_ZH.md` | 视频必须能看到完整路线或清楚说明 |
| 操作说明 | A/D 移动、Space 跳跃、J 或鼠标左键攻击，其他键位按当前 Demo 页面说明 | `submissions/JUDGE_QUICK_START.md`、`submissions/WEBGL_PAGE_COPY.md` | WebGL 页面和视频简介里的键位一致 |

## 链接字段

所有链接先填到 `submissions/LINKS_TO_FILL.md`，再复制到平台。

| 平台字段 | 链接来源 | 验证标准 |
| --- | --- | --- |
| WebGL 在线试玩链接 | `submissions/LINKS_TO_FILL.md` | 无痕窗口可打开，不是 `localhost` |
| Demo 视频链接 | `submissions/LINKS_TO_FILL.md` | 公开可播放，最好 3 分钟左右 |
| GitHub 仓库链接 | `https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master` | 指向 `master`，不是旧分支 |
| PPT 文件链接 | `submissions/LINKS_TO_FILL.md` | 文件能打开，不是旧版 |
| CodeBuddy 历史记录链接 | `submissions/LINKS_TO_FILL.md` | 能看到项目名和 CodeBuddy 参与记录 |
| 社交媒体发布链接 | `submissions/LINKS_TO_FILL.md` 可选项 | 公开可访问，文案不含错误链接 |

## 附件字段

| 平台字段 | 推荐附件 | 来源 |
| --- | --- | --- |
| 项目书 | `PROJECT_BOOK_FINAL_ZH.pdf` | `submissions/PROJECT_BOOK_FINAL_ZH.pdf` |
| 项目书可编辑版 | `PROJECT_BOOK_FINAL_ZH.docx` | `submissions/PROJECT_BOOK_FINAL_ZH.docx` |
| 作品介绍 PPT | `Craftsmen_Hackathon_Deck.pptx` | `submissions/Craftsmen_Hackathon_Deck.pptx` |
| PPT 预览 | `Craftsmen_Hackathon_Deck_Preview.pdf` | `submissions/Craftsmen_Hackathon_Deck_Preview.pdf` |
| CodeBuddy 历史记录 | `CodeBuddy_History_Craftsmen_and_Homo_sapiens.zip` 或平台导出文件 | 按 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 导出 |
| 备用说明包 | 按 `submissions/UPLOAD_PACKAGE_README_ZH.md` 组织 | 可选 |

## 团队信息字段

| 平台字段 | 来源 | 验证标准 |
| --- | --- | --- |
| 团队名称 | `submissions/TEAM_INFO_TEMPLATE.md` | 替换 `TODO` |
| 学校 / 单位 | `submissions/TEAM_INFO_TEMPLATE.md` | 与报名信息一致 |
| 成员姓名 | `submissions/TEAM_INFO_TEMPLATE.md` | 不要漏人 |
| 成员分工 | `submissions/TEAM_INFO_TEMPLATE.md` | 与 PPT 最后一页一致 |
| 联系方式 | 平台要求或团队真实信息 | 不要填错邮箱 / 手机 |

## 最终粘贴前检查

- [ ] 平台里的作品名称和 PPT、项目书一致。
- [ ] 平台里的赛题是“赛题三：叙事类游戏 / AI 重塑叙事体验”。
- [ ] WebGL 链接、视频链接、PPT 链接、CodeBuddy 链接都已回填。
- [ ] 没有把 `TODO`、`localhost`、本地文件路径粘进平台。
- [ ] AI 使用说明没有声称已接入实时 AI API。
- [ ] 第三方素材没有被误称为全部原创。
- [ ] 提交前截图保存最终表单页面或提交成功页面。
