# 最终提交团队交接消息模板

这个文件用于把当前提交材料状态发给队友、老师或负责最终上传的人。可以直接复制到 QQ / 微信 / 飞书群里。

## 群里短消息版

```text
我把腾讯云黑客松提交材料都整理到 master 分支了，提交材料集中在 submissions/ 目录。

最重要先看这几个：
1. submissions/FINAL_DELIVERABLES_MANIFEST.md：最终交付包总清单
2. submissions/LINKS_TO_FILL.md：WebGL、视频、PPT、CodeBuddy 链接回填表
3. submissions/FORM_ANSWERS_COPYPASTE_ZH.md：提交问卷复制稿
4. submissions/PROJECT_BOOK_FINAL_ZH.pdf：最终项目书 PDF
5. submissions/Craftsmen_Hackathon_Deck.pptx：作品介绍 PPT
6. submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf：评委一页摘要

现在仓库内文案、PPT、项目书、评委摘要都已准备。
还缺外部动作：
- WebGL 在线试玩链接
- Demo 视频链接
- CodeBuddy 历史记录导出
- 团队真实信息
- PPT 最后一页补真实链接和团队信息

拿到外部链接后，先填 submissions/LINKS_TO_FILL.md，再同步到 FINAL_SUBMISSION_INFO.md、PPT 和提交问卷。
```

## 给最终上传负责人的详细版

```text
当前 master 已准备好腾讯云黑客松提交包。

请按这个顺序处理：
1. 打开 submissions/FINAL_DELIVERABLES_MANIFEST.md，确认最小提交包。
2. 用 Unity 构建并上传 WebGL，拿到在线试玩链接。
3. 按 submissions/DEMO_RECORDING_RUNBOOK.md 录 Demo 视频。
4. 上传视频时复制 submissions/DEMO_VIDEO_UPLOAD_COPY.md 的标题、简介和标签。
5. 导出 CodeBuddy 历史记录。
6. 把 WebGL / 视频 / PPT / CodeBuddy 链接统一填进 submissions/LINKS_TO_FILL.md。
7. 再把链接同步到：
   - submissions/FINAL_SUBMISSION_INFO.md
   - submissions/FORM_ANSWERS_COPYPASTE_ZH.md
   - submissions/JUDGE_QUICK_START.md
   - PPT 最后一页
   - 最终提交问卷
8. 按 submissions/SUBMISSION_READINESS_AUDIT.md 做最后检查。

可直接上传或转发的文件：
- submissions/PROJECT_BOOK_FINAL_ZH.pdf
- submissions/PROJECT_BOOK_FINAL_ZH.docx
- submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf
- submissions/Craftsmen_Hackathon_Deck.pptx
- submissions/Craftsmen_Hackathon_Deck_Preview.pdf

注意：
- 不要把 localhost 当 WebGL 链接。
- 视频链接要公开可访问。
- CodeBuddy 记录要能打开。
- GitHub 链接使用 master：
  https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master
```

## 推荐团队分工

| 事项 | 建议负责人 | 产物 | 检查方式 |
| --- | --- | --- | --- |
| WebGL 构建上传 | Unity 工程负责人 | 在线试玩链接 | 无痕浏览器打开，不需要登录 |
| Demo 视频录制上传 | 演示负责人 | 视频链接 | 3 分钟左右，能看到 AI 叙事、成长、Boss 和胜利 |
| CodeBuddy 导出 | AI 协作记录负责人 | 导出文件或链接 | 文件能打开，能看到项目开发过程 |
| PPT 最后一页更新 | 答辩负责人 | 更新后的 PPT | 团队信息、试玩链接、视频链接一致 |
| 团队信息填写 | 队长 / 提交负责人 | 团队信息、分工 | `TEAM_INFO_TEMPLATE.md` 中无 TODO |
| 最终提交问卷 | 提交负责人 | 提交成功页面或截图 | 所有链接与 `LINKS_TO_FILL.md` 一致 |

## 给老师 / 指导者的简短说明

```text
老师您好，我们的腾讯云黑客松作品《能工智人 / Craftsmen and Homo sapiens》提交材料已经整理好。

项目是 Unity 2D 横版动作地牢 Demo，参赛方向为赛题三“叙事类游戏 / AI 重塑叙事体验”。AI 主要参与世界观、房间档案、NPC 台词、敌人与 Boss 设定，以及 CodeBuddy 辅助开发和提交材料整理。

可先查看：
- 项目书 PDF：submissions/PROJECT_BOOK_FINAL_ZH.pdf
- 评委单页摘要：submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf
- 作品介绍 PPT：submissions/Craftsmen_Hackathon_Deck.pptx

最终还需要补 WebGL 在线试玩链接、Demo 视频链接和 CodeBuddy 历史记录导出。
```

## 最后一轮群提醒

```text
最后提交前大家再确认：
- WebGL 链接能打开
- Demo 视频能公开访问
- CodeBuddy 历史记录已导出
- PPT 最后一页链接正确
- 团队成员信息已补齐
- FINAL_SUBMISSION_INFO.md 没有必填 TODO
- 提交问卷里的链接和 LINKS_TO_FILL.md 一致
```
