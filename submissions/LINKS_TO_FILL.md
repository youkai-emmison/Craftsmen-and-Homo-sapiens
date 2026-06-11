# 最终链接回填表

这个文件用于最后一天把所有外部链接集中回填。拿到链接后，先填这里，再同步到 `FINAL_SUBMISSION_INFO.md`、PPT、作品页和提交问卷。

## 必填链接

| 链接项 | 当前状态 | 链接 | 生成方式 | 必须同步到哪里 | 验证标准 |
| --- | --- | --- | --- | --- | --- |
| WebGL 在线试玩 | TODO | TODO | 按 `WEBGL_UPLOAD_RUNBOOK.md` 构建并上传 | `FINAL_SUBMISSION_INFO.md`、`JUDGE_QUICK_START.md`、PPT 最后一页、提交问卷、Demo 视频简介 | 无痕窗口可打开，不是 localhost，不需要登录 |
| Demo 视频 | TODO | TODO | 按 `DEMO_RECORDING_RUNBOOK.md` 录制并上传 | `FINAL_SUBMISSION_INFO.md`、`JUDGE_QUICK_START.md`、PPT、提交问卷、WebGL 页面 | 可公开播放，3 分钟左右，能看到 AI 叙事和完整闭环 |
| PPT 文件 | TODO | TODO | 上传 `Craftsmen_Hackathon_Deck.pptx` 或 PDF 预览版 | `FINAL_SUBMISSION_INFO.md`、提交问卷 | 链接可访问，文件不是空白或旧版 |
| CodeBuddy 历史记录 | TODO | TODO | 按 `CODEBUDDY_SUBMISSION_CHECKLIST.md` 导出并上传 | `FINAL_SUBMISSION_INFO.md`、提交问卷 | 文件能打开，能看到项目名和 CodeBuddy 参与记录 |
| GitHub 仓库 | Ready | https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master | 已确认远端仓库 | `FINAL_SUBMISSION_INFO.md`、提交问卷、WebGL 页面 | 链接可打开，指向最终 `master` |

## 可选链接

| 链接项 | 当前状态 | 链接 | 用途 | 验证标准 |
| --- | --- | --- | --- | --- |
| 社交媒体发布 | Optional | TODO | 加分项，建议带 `#CodeBuddy #腾讯云黑客松` | 公开可访问，文案不含错误链接 |
| 备用网盘提交包 | Optional | TODO | 备份 PPT、视频、CodeBuddy 导出 | 链接不需要登录或已设置可访问权限 |

## 回填顺序

1. 先填 WebGL 在线试玩链接。
2. 再填 Demo 视频链接。
3. 上传 PPT / PDF 后填 PPT 文件链接。
4. 导出 CodeBuddy 历史记录后填 CodeBuddy 链接。
5. 如有社交媒体发布，再填可选链接。
6. 最后统一检查所有 `TODO` 是否清空。

## 需要同步修改的文件

- `submissions/FINAL_SUBMISSION_INFO.md`
- `submissions/JUDGE_QUICK_START.md`
- `submissions/WEBGL_PAGE_COPY.md`
- `submissions/WEBGL_UPLOAD_RUNBOOK.md`
- `docs/SUBMISSION_FORM_DRAFT.md`
- `docs/HACKATHON_SUBMISSION_CHECKLIST.md`
- `submissions/Craftsmen_Hackathon_Deck.pptx`
- `submissions/Craftsmen_Hackathon_Deck_Preview.pdf`，如果重新导出

## 最终提交前检查

- [ ] 没有把 `localhost` 当作在线试玩链接。
- [ ] 没有把本地文件路径当作 PPT / 视频链接。
- [ ] 所有必填链接都可以在无痕窗口打开。
- [ ] WebGL、视频、PPT、CodeBuddy 链接都已经写进提交问卷。
- [ ] PPT 最后一页和提交问卷中的链接一致。
- [ ] `FINAL_SUBMISSION_INFO.md` 中不再有必填 `TODO`。
