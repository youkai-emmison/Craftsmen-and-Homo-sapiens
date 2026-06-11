# 最终提交自动自检说明

这个文件说明如何运行仓库里的最终提交自检脚本。它不会修改项目文件，只读取提交材料、Git 状态和关键文档，用来提醒哪些内容还没准备好。

## 运行命令

在项目根目录执行：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Run-FinalSubmissionAudit.ps1
```

如果当前目录不确定，先进入项目根目录：

```powershell
cd "C:\Users\qintian\Desktop\腾讯比赛\Craftsmen-and-Homo-sapiens-master"
```

## 它会检查什么

- 必备提交材料是否存在。
- Git 工作区是否还有未提交改动。
- Git 是否跟踪了 `Library/`、`Temp/`、`Logs/`、`UserSettings/`、`Build/` 等不该提交的 Unity 本地目录。
- 最终面对评委的材料里是否还有：
  - `TODO`
  - `待回填`
  - `localhost`
  - `file://`
  - `127.0.0.1`
  - `C:\Users...` 本地路径
- 是否存在 `submissions/artifact-build-manifest.json`，提醒不要把它放进最终上传包。

## 为什么现在可能会失败

在 WebGL 链接、Demo 视频链接、CodeBuddy 导出和团队信息没有回填之前，脚本会故意报失败。这不是脚本坏了，而是在告诉你“现在还不能最终提交”。

常见失败文件：

- `submissions/FINAL_SUBMISSION_INFO.md`
- `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
- `submissions/JUDGE_QUICK_START.md`
- `submissions/WEBGL_PAGE_COPY.md`
- `submissions/DEMO_VIDEO_UPLOAD_COPY.md`
- `submissions/00_README_FIRST_ZH.md`

这些文件里的占位符需要在最终提交前替换成真实链接和团队信息。

## 什么时候算通过

脚本显示：

```text
Result: READY ENOUGH FOR FINAL HUMAN REVIEW.
```

这表示提交材料没有明显占位符红旗，但仍然要人工确认：

- 每个外部链接都能在无痕窗口打开。
- PPT 和 PDF 不是旧版。
- WebGL 能加载并进入 Demo。
- Demo 视频权限不是私密。
- CodeBuddy 历史记录能打开。
- 提交问卷内容和 `LINKS_TO_FILL.md` 一致。

## 和人工清单的关系

脚本只做自动扫描，不能替代人工检查。最终仍然要按这些文件走一遍：

- `submissions/PLACEHOLDER_CLEANUP_CHECKLIST_ZH.md`
- `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md`
- `submissions/FINAL_DELIVERABLES_MANIFEST.md`
- `submissions/SUBMISSION_READINESS_AUDIT.md`
