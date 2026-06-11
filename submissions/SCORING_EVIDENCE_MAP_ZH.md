# 评分项证据映射表

这个文件用于提交问卷、路演答辩和 Demo 视频录制前快速对齐评分标准。它不替代项目书，而是把“评委要看什么”和“我们用什么证明”放在一张表里。

## 总体定位

- 赛事：AI CAN DO IT | 腾讯云黑客松游戏开发挑战赛
- 赛题：赛题三，叙事类游戏 / AI 重塑叙事体验
- 项目：能工智人 / Craftsmen and Homo sapiens
- 核心证明路径：AI 叙事设定进入 Unity 横版地牢 Demo，玩家在移动、战斗、成长、清房和 Boss 收束中逐步理解地下工坊故事。

## 1. 主题契合度：叙事类游戏 / AI 重塑叙事体验

| 评审可能关注 | 本项目回答 | 证据文件 | Demo 或视频中应展示 |
| --- | --- | --- | --- |
| 是否明确选择赛题三 | 项目明确以 AI 叙事包装横版动作流程 | `README.md`、`submissions/FINAL_SUBMISSION_INFO.md`、`submissions/PROJECT_BOOK_FINAL_ZH.md` | 视频开头 10 秒说出“赛题三：叙事类游戏 / AI 重塑叙事体验” |
| 叙事是否真的进入游戏流程 | 房间推进、NPC 对话、Boss 设定和胜利文本共同构成地下工坊档案 | `docs/GAMEPLAY_DIRECTION.md`、`docs/DEMO_STAGE_FLOW.md`、`docs/NPC_DIALOGUE_GUIDE.md` | 展示 NPC 对话、房间推进和最终胜利反馈 |
| 是否只是普通动作游戏 | 战斗不是独立玩法，而是读取异常档案和推进故事的方式 | `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md`、`submissions/ROADSHOW_QA_CHEATSHEET_ZH.md` | 旁白说明“每个房间也是 AI 档案片段” |
| 项目身份是否原创 | 世界观、玩法包装、提交文案和原型流程是原创项目 | `docs/THIRD_PARTY_ASSETS.md`、`docs/AI_CREATION_LOG.md` | 说明第三方素材仅作临时占位，核心设定和流程为原创 |

## 2. AI 工具使用情况

| 评审可能关注 | 本项目回答 | 证据文件 | Demo 或提交中应展示 |
| --- | --- | --- | --- |
| AI 用在什么地方 | AI 用于叙事创作、美术辅助、代码协作和提交材料整理 | `docs/AI_CREATION_LOG.md`、`submissions/FINAL_SUBMISSION_INFO.md` | 视频或答辩中用 20 秒说明三类 AI 用法 |
| 是否有 CodeBuddy 记录 | 需要导出 CodeBuddy 历史对话作为提交证据 | `docs/CODEBUDDY_EXPORT_GUIDE.md`、`submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` | 提交附件或链接中提供 CodeBuddy 导出 |
| 为什么没有实时 AI API | 黑客松 Demo 优先稳定，采用 AI 预生成叙事内容 + 游戏内呈现 | `docs/ROADSHOW_QA.md`、`submissions/ROADSHOW_QA_CHEATSHEET_ZH.md` | 答辩时说明避免网络、延迟、成本和不可控输出 |
| AI 美术是否合规 | 记录 AI 生成素材和第三方素材来源，避免把第三方资源说成原创 | `docs/THIRD_PARTY_ASSETS.md`、`docs/ART_PLACEHOLDER_GUIDE.md` | 如被问素材来源，按记录说明哪些是占位、哪些需替换 |
| AI 是否影响开发效率 | AI 协助脚本、文档、PPT、视频脚本和提交材料整理 | `docs/AI_CREATION_LOG.md`、`submissions/SUBMISSION_DAY_RUNBOOK_ZH.md` | CodeBuddy 导出中应能看到开发和整理过程 |

## 3. 游戏品质

| 评审可能关注 | 本项目回答 | 证据文件 | Demo 或视频中应展示 |
| --- | --- | --- | --- |
| 是否能玩 | Unity 2D Demo 有移动、跳跃、攻击、敌人、清房、Boss 和胜利反馈 | `docs/DEMO_STAGE_FLOW.md`、`submissions/DEMO_RECORDING_RUNBOOK.md` | 从出生点移动到 Boss 房，完整走一遍 |
| 目标是否清楚 | 三段式 Demo：Early Room、Mid Room、Boss Room | `docs/DEMO_STAGE_FLOW.md`、`submissions/JUDGE_QUICK_START.md` | 录屏中说明“前期打小怪 -> 中期变强 -> Boss 收束” |
| 操作是否容易理解 | 键位和推荐路线集中写在评委快速打开指南 | `submissions/JUDGE_QUICK_START.md`、`submissions/WEBGL_PAGE_COPY.md` | WebGL 页面或视频简介写明操作键位 |
| 是否有成长反馈 | 玩家通过战斗获得成长，升级后攻击更明显 | `docs/DEMO_STAGE_FLOW.md`、`docs/SKILL_ENERGY_GUIDE.md`、`docs/CRAFTING_CONTENT_GUIDE.md` | 展示打怪、掉落或成长后击败更强敌人 |
| 是否有完整收束 | Boss 或最终目标被击败后出现胜利反馈 | `docs/BOSS_ROOM_PLACEHOLDER_GUIDE.md`、`submissions/DEMO_RECORDING_RUNBOOK.md` | 视频结尾必须出现 Boss / Victory / Demo Complete |
| UI 是否足够演示 | 血量、技力、背包/制作、对话框等面板能解释当前目标 | `docs/SKILL_ENERGY_GUIDE.md`、`docs/NPC_DIALOGUE_GUIDE.md`、`docs/CRAFTING_CONTENT_GUIDE.md` | 不需要展示所有系统，但至少展示 UI 存在且不遮挡核心流程 |

## 4. 提交材料证据链

| 材料 | 作用 | 当前文件 |
| --- | --- | --- |
| 项目书 | 证明项目定位、技术路线和 AI 创作说明 | `submissions/PROJECT_BOOK_FINAL_ZH.pdf`、`submissions/PROJECT_BOOK_FINAL_ZH.docx` |
| PPT | 评委快速理解项目结构和亮点 | `submissions/Craftsmen_Hackathon_Deck.pptx`、`submissions/Craftsmen_Hackathon_Deck_Preview.pdf` |
| Demo 视频 | 证明游戏能跑、流程能看懂 | `docs/DEMO_VIDEO_SCRIPT.md`、`submissions/DEMO_RECORDING_RUNBOOK.md` |
| WebGL 链接 | 证明可在线试玩 | `submissions/WEBGL_UPLOAD_RUNBOOK.md`、`submissions/LINKS_TO_FILL.md` |
| CodeBuddy 导出 | 证明 AI 编程助手参与 | `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` |
| 评委快速材料 | 降低评委理解成本 | `submissions/JUDGE_QUICK_START.md`、`submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf` |

## 5. Demo 视频必说句

如果视频时间很短，至少说清这几句：

1. “我们选择的是赛题三：叙事类游戏 / AI 重塑叙事体验。”
2. “AI 帮我们生成世界观、房间档案、NPC 台词、敌人和 Boss 设定，并参与代码和提交材料协作。”
3. “当前 Demo 用三段式流程压缩展示：前期打小怪、中期成长变强、后期 Boss 挑战。”
4. “我们没有接实时 AI API，是为了保证 WebGL Demo 稳定；AI 内容以预生成叙事和游戏内呈现为主。”
5. “完整版本后续可以扩展更多 AI 档案、NPC 分支、机械装置和动态剧情。”

## 6. 最容易丢分的地方

- 只展示打怪，不说明 AI 叙事如何进入玩法。
- 提交 CodeBuddy 历史记录时找不到项目相关上下文。
- WebGL 链接打不开，且没有 Demo 视频兜底。
- PPT、问卷、视频简介里的链接不一致。
- 把第三方素材或 Asset Store 资源说成全部原创。
- 答辩时承诺已经实现实时 AI NPC、完整随机地图或完整装备系统。

## 7. 最后一轮自检

- [ ] 视频开头说明赛题三和 AI 叙事。
- [ ] 视频中至少出现一次 NPC、档案、Boss 文本或胜利文本。
- [ ] 视频中能看到玩家操作和战斗闭环。
- [ ] CodeBuddy 导出已准备。
- [ ] 项目书、PPT、问卷复制稿都提到 AI 使用方式。
- [ ] 第三方素材说明没有夸大或误称原创。
- [ ] 所有外部链接已写入 `submissions/LINKS_TO_FILL.md`。
