# 最终上传包组装脚本说明

这个文件说明如何用脚本组装腾讯云黑客松最终上传包。脚本只复制已经准备好的提交材料，不会删除旧文件，也不会覆盖已有文件。

## 脚本位置

```text
submissions/Build-FinalUploadPackage.ps1
```

## 推荐先试跑

在项目根目录执行：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Build-FinalUploadPackage.ps1 -DryRun
```

`-DryRun` 只显示会复制哪些文件，不会生成目录。

## 正式生成上传包目录

确认试跑没有缺文件后执行：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Build-FinalUploadPackage.ps1
```

脚本会生成带时间戳的新目录，例如：

```text
submissions/final_upload_packages/Craftsmen_Submission_20260611_153000/
```

这个目录已通过 `.gitignore` 忽略，不会被提交到仓库。

## 生成后的目录结构

```text
Craftsmen_Submission_时间戳/
  00_README_FIRST.md
  NEXT_STEPS_BEFORE_UPLOAD.txt
  01_ProjectBook/
  02_Presentation/
  03_Demo/
  04_CodeBuddy/
  05_Source/
  06_BackupDocs/
```

其中：

- `01_ProjectBook/` 放项目书 PDF 和 Word。
- `02_Presentation/` 放 PPT 和 PDF 预览。
- `03_Demo/` 放 WebGL 链接模板、Demo 视频链接模板和评委快速指南。
- `04_CodeBuddy/` 放 CodeBuddy 链接模板和检查清单。
- `05_Source/` 放 GitHub 链接。
- `06_BackupDocs/` 放提交信息、问卷复制稿、评分证据、答辩速查卡和备用链接模板。

## 生成后必须手动处理

脚本会复制链接模板，但不会替你生成真实外部链接。最终上传前必须手动替换：

- `03_Demo/WebGL_Link.txt`
- `03_Demo/Demo_Video_Link.txt`
- `04_CodeBuddy/CodeBuddy_History_Link.txt`
- `06_BackupDocs/PPT_File_Link.txt`
- `06_BackupDocs/Social_Media_Link_Optional.txt`

如果有 CodeBuddy 导出的附件，把文件放进：

```text
04_CodeBuddy/
```

## 不要放进上传包

不要把这些目录或文件塞进脚本生成的上传包：

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `UserSettings/`
- `outputs/`
- `submissions/artifact-build-manifest.json`
- 未确认授权的第三方素材原始包

## 最终检查

生成上传包后，再运行：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Run-FinalSubmissionAudit.ps1
```

如果自检仍然提示 `TODO`、`待回填`、`localhost` 或本地路径，就不要提交。先回填真实链接，再打包或上传。
