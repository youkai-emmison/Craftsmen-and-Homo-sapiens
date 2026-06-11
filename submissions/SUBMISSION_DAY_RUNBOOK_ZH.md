# 最终提交当天执行手册

这个文件用于最后一天实际提交腾讯云黑客松作品。目标是把“构建、录屏、上传、回填、提交”按顺序做完，避免材料已经准备好但最后链接填错、权限没开或 PPT 还是旧版本。

## 一句话原则

先产出外部链接，再统一回填，最后再提交问卷。不要一边上传一边分散修改多个文件。

## 0. 提交前准备

- 确认仓库在 `master` 分支。
- 确认 GitHub 远端已经 push 到最新。
- 确认 Unity 项目可以打开，Demo 场景可以 Play。
- 打开 `submissions/LINKS_TO_FILL.md`，把它当成所有链接的总表。
- 打开 `submissions/FINAL_DELIVERABLES_MANIFEST.md`，确认最小提交包里哪些还缺外部动作。

## 1. 先生成 WebGL 在线试玩链接

推荐先做 WebGL，因为这是最容易受 Unity 设置、浏览器加载和资源路径影响的部分。

执行：

1. 按 `submissions/WEBGL_UPLOAD_RUNBOOK.md` 构建 WebGL。
2. 上传到可公开访问的位置。
3. 用无痕窗口打开链接。
4. 确认不是 `localhost`，不需要登录，不是本地文件路径。
5. 至少跑一遍：移动、跳跃、攻击、打怪、开门、Boss 或胜利反馈。
6. 把链接填入 `submissions/LINKS_TO_FILL.md`。

失败时：

- 如果 WebGL 黑屏，先录本地 Unity Demo 视频，不要卡死在 WebGL。
- 如果资源加载慢，把 Demo 视频和 GitHub 链接作为备用材料放进 `JUDGE_QUICK_START.md`。
- 如果 WebGL 完全来不及，提交材料中必须说明“可运行源码 + Demo 视频”为主要验证方式。

## 2. 再录制 Demo 视频

视频要优先证明作品能玩、AI 叙事能看见、流程能走完。

执行：

1. 按 `submissions/DEMO_RECORDING_RUNBOOK.md` 走录制路线。
2. 用 `docs/DEMO_VIDEO_SCRIPT.md` 控制旁白。
3. 视频开头 10 秒说清：作品名、赛题三、AI 叙事游戏原型。
4. 中段展示：移动、跳跃、攻击、成长、NPC 或档案文本。
5. 结尾展示：Boss / 最终目标 / Victory 或 Demo Complete。
6. 按 `submissions/DEMO_VIDEO_UPLOAD_COPY.md` 填标题、简介、标签。
7. 上传后用无痕窗口确认可播放。
8. 把视频链接填入 `submissions/LINKS_TO_FILL.md`。

不要：

- 不要录太长，3 分钟左右更稳。
- 不要只录战斗，必须说清 AI 用在叙事、美术辅助和编程协作。
- 不要把视频权限设成私密。

## 3. 导出 CodeBuddy 历史记录

CodeBuddy 记录是证明 AI 编程助手参与的重要材料。

执行：

1. 按 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 做最后检查。
2. 导出或整理 CodeBuddy 历史对话。
3. 确认记录里能看到项目名、Unity 开发、脚本修改、调试或提交材料协作。
4. 上传到提交平台允许的位置，或保存成可提交附件。
5. 把链接或文件说明填入 `submissions/LINKS_TO_FILL.md`。

注意：

- 不要用 Codex 记录冒充 CodeBuddy 记录。
- 如果只能上传文件，就在问卷里说明“CodeBuddy 历史记录见附件”。

## 4. 回填所有链接

当 WebGL、Demo 视频、PPT 文件、CodeBuddy 记录都有结果后，再集中回填。

必须同步：

- `submissions/LINKS_TO_FILL.md`
- `submissions/FINAL_SUBMISSION_INFO.md`
- `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
- `submissions/JUDGE_QUICK_START.md`
- `submissions/WEBGL_PAGE_COPY.md`
- `docs/SUBMISSION_FORM_DRAFT.md`
- `docs/HACKATHON_SUBMISSION_CHECKLIST.md`
- PPT 最后一页或备注页

检查：

- 所有 `TODO` 都有明确处理。
- GitHub 链接指向 `master`。
- WebGL、视频、PPT、CodeBuddy 链接在无痕窗口能打开。
- PPT 和问卷里的链接一致。

## 5. 更新 PPT 和附件

执行：

1. 打开 `submissions/Craftsmen_Hackathon_Deck.pptx`。
2. 补团队信息、WebGL 链接、Demo 视频链接和 CodeBuddy 说明。
3. 如页面有旧截图，优先换成最新 Demo 截图。
4. 重新导出 PDF 预览版。
5. 本地打开 PPT 和 PDF，确认不是空白、乱码或旧版本。
6. 如果平台要求附件或网盘包，按 `submissions/UPLOAD_PACKAGE_README_ZH.md` 组织最终上传包。

如果时间不够：

- 至少保证 PPT 最后一页有可打开链接。
- 使用 `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf` 作为评委快速理解项目的补充材料。

## 6. 最后 30 分钟检查

- [ ] WebGL 链接可公开打开。
- [ ] Demo 视频可公开播放。
- [ ] CodeBuddy 历史记录已导出或上传。
- [ ] GitHub 仓库链接可打开，指向 `master`。
- [ ] PPT 或 PDF 是最新版。
- [ ] 项目书 PDF 可打开。
- [ ] 团队成员信息已填真实内容。
- [ ] 提交问卷中没有 `TODO`、本地路径或 `localhost`。
- [ ] 已按 `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md` 扫过最终材料和上传包。
- [ ] `submissions/FINAL_SUBMISSION_INFO.md` 与提交问卷内容一致。
- [ ] `submissions/JUDGE_QUICK_START.md` 中的链接和问卷一致。

## 7. 提交后立刻做的事

提交问卷后不要马上关闭所有窗口。

1. 截图保存提交成功页面。
2. 把最终提交链接、视频链接、WebGL 链接发给队友。
3. 在团队群里说明：已经提交 / 还差什么 / 有没有备用材料。
4. 如果平台允许修改，记录最后修改截止时间。

## 紧急兜底话术

如果某个外部链接临时失败，可以在提交说明或答辩中这样说：

> 当前版本已经完成 Unity 可运行 Demo 和核心闭环。若在线试玩链接受平台构建或托管影响，请优先查看 Demo 视频、GitHub 仓库和项目书中的流程说明。项目核心包括 AI 叙事包装、横版战斗、成长反馈、房间推进和 Boss 收束。

## 最推荐打开顺序

1. `submissions/LINKS_TO_FILL.md`
2. `submissions/FINAL_DELIVERABLES_MANIFEST.md`
3. `submissions/WEBGL_UPLOAD_RUNBOOK.md`
4. `submissions/DEMO_RECORDING_RUNBOOK.md`
5. `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md`
6. `submissions/FINAL_SUBMISSION_INFO.md`
7. `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
