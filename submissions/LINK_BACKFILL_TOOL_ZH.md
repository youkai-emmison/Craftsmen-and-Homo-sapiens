# 最终链接统一回填工具说明

这个工具用于最后一天拿到 WebGL、Demo 视频、PPT、CodeBuddy 等外部链接后，一次性回填到常用提交材料中，减少手动复制漏改。

## 相关文件

- `submissions/link_backfill_values.example.json`
  - 链接填写模板，可以提交到仓库。
- `submissions/link_backfill_values.local.json`
  - 你本地实际填写的链接文件，已被 `.gitignore` 忽略，不会提交。
- `submissions/Apply-LinkBackfill.ps1`
  - 回填脚本。

## 使用步骤

1. 复制模板：

```powershell
Copy-Item submissions\link_backfill_values.example.json submissions\link_backfill_values.local.json
```

2. 打开 `submissions/link_backfill_values.local.json`，把 `TODO` 替换成真实链接。

必填字段：

- `webglUrl`
- `demoVideoUrl`
- `pptFileUrl`
- `codeBuddyUrl`
- `githubUrl`

可选字段：

- `projectBookUrl`
- `judgeBriefUrl`
- `socialMediaUrl`

如果这些可选字段留空，脚本会自动写入兜底说明：

- `projectBookUrl`：指向仓库里的 `submissions/PROJECT_BOOK_FINAL_ZH.pdf`。
- `judgeBriefUrl`：指向仓库里的 `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.pdf`。
- `socialMediaUrl`：写成 `N/A (optional, not published)`。

这样最终材料里不会留下“待回填”，也不会把可选项误当成必填项。

3. 先试跑：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Apply-LinkBackfill.ps1 -DryRun
```

4. 确认输出的将修改文件合理后，正式执行：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Apply-LinkBackfill.ps1
```

5. 运行最终自检：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Run-FinalSubmissionAudit.ps1
```

## 会更新哪些文件

脚本会尝试更新这些文本材料：

- `submissions/LINKS_TO_FILL.md`
- `submissions/FINAL_SUBMISSION_INFO.md`
- `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
- `submissions/JUDGE_QUICK_START.md`
- `submissions/WEBGL_PAGE_COPY.md`
- `submissions/DEMO_VIDEO_UPLOAD_COPY.md`
- `submissions/00_README_FIRST_ZH.md`
- `submissions/package_templates/WebGL_Link.txt`
- `submissions/package_templates/Demo_Video_Link.txt`
- `submissions/package_templates/PPT_File_Link.txt`
- `submissions/package_templates/CodeBuddy_History_Link.txt`
- `submissions/package_templates/Social_Media_Link_Optional.txt`

## 不会自动更新的内容

脚本不会改二进制文件和外部平台内容。下面这些仍要人工处理：

- `submissions/Craftsmen_Hackathon_Deck.pptx`
- `submissions/Craftsmen_Hackathon_Deck_Preview.pdf`
- 提交问卷网页
- 视频平台简介
- WebGL 作品页
- 最终上传包里已经复制出去的旧文件

如果已经生成过上传包，回填链接后建议重新运行：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Build-FinalUploadPackage.ps1
```

## 注意

- 不要把 `link_backfill_values.local.json` 提交到仓库。
- 不要填 `localhost`、`file://` 或 `C:\Users\...` 本地路径。
- 如果脚本拒绝执行，先检查 JSON 中是否仍有 `TODO`。
- 回填后必须跑 `Run-FinalSubmissionAudit.ps1`，再人工用无痕窗口打开每个外部链接。
