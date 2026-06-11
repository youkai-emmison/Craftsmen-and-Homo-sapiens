# 最终上传包说明

这个文件用于准备腾讯云黑客松最终附件包、网盘包或老师审核包。它说明“压缩包里应该放什么、不要放什么、每个文件有什么用”。

如果提交平台只要求填写链接，不要求上传压缩包，也可以把本文件作为备用包说明。

## 推荐压缩包名称

`Craftsmen_and_Homo_sapiens_TencentCloudHackathon_Submission.zip`

如果需要中文名，可以用：

`能工智人_腾讯云黑客松提交包.zip`

## 推荐目录结构

```text
Craftsmen_and_Homo_sapiens_Submission/
  00_README_先看我.md
  01_ProjectBook/
    PROJECT_BOOK_FINAL_ZH.pdf
    PROJECT_BOOK_FINAL_ZH.docx
  02_Presentation/
    Craftsmen_Hackathon_Deck.pptx
    Craftsmen_Hackathon_Deck_Preview.pdf
  03_Demo/
    Demo_Video_Link.txt
    WebGL_Link.txt
    JUDGE_QUICK_START.md
  04_CodeBuddy/
    CodeBuddy_History_Craftsmen_and_Homo_sapiens.zip
    CODEBUDDY_SUBMISSION_CHECKLIST.md
  05_Source/
    GitHub_Link.txt
  06_BackupDocs/
    FINAL_SUBMISSION_INFO.md
    FORM_ANSWERS_COPYPASTE_ZH.md
    SCORING_EVIDENCE_MAP_ZH.md
    ROADSHOW_QA_CHEATSHEET_ZH.md
```

## 00_README_先看我.md

仓库中已经准备好可直接使用的版本：

`submissions/00_README_FIRST_ZH.md`

打包时把它复制到最终压缩包根目录，并重命名为：

`00_README_先看我.md`

## 必放文件

| 文件 | 来源 | 用途 |
| --- | --- | --- |
| `PROJECT_BOOK_FINAL_ZH.pdf` | `submissions/PROJECT_BOOK_FINAL_ZH.pdf` | 项目书 PDF，优先给评委或老师看 |
| `PROJECT_BOOK_FINAL_ZH.docx` | `submissions/PROJECT_BOOK_FINAL_ZH.docx` | 可编辑项目书 |
| `Craftsmen_Hackathon_Deck.pptx` | `submissions/Craftsmen_Hackathon_Deck.pptx` | 作品介绍 PPT |
| `Craftsmen_Hackathon_Deck_Preview.pdf` | `submissions/Craftsmen_Hackathon_Deck_Preview.pdf` | PPT PDF 预览 |
| `JUDGE_QUICK_START.md` | `submissions/JUDGE_QUICK_START.md` | 评委快速打开指南 |
| `FINAL_SUBMISSION_INFO.md` | `submissions/FINAL_SUBMISSION_INFO.md` | 最终提交信息总表 |
| `FORM_ANSWERS_COPYPASTE_ZH.md` | `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` | 问卷复制稿 |
| `SCORING_EVIDENCE_MAP_ZH.md` | `submissions/SCORING_EVIDENCE_MAP_ZH.md` | 评分项证据映射 |
| `ROADSHOW_QA_CHEATSHEET_ZH.md` | `submissions/ROADSHOW_QA_CHEATSHEET_ZH.md` | 路演答辩速查卡 |

## 需要手动生成的链接文件

最终上传包里建议放多个小文本文件。仓库已经准备好模板目录：

`submissions/package_templates/`

打包时复制这些文件，并把每个文件里的 `TODO` 替换为真实链接。

### 推荐复制关系

| 模板文件 | 推荐放入最终上传包的位置 |
| --- | --- |
| `submissions/package_templates/WebGL_Link.txt` | `03_Demo/WebGL_Link.txt` |
| `submissions/package_templates/Demo_Video_Link.txt` | `03_Demo/Demo_Video_Link.txt` |
| `submissions/package_templates/GitHub_Link.txt` | `05_Source/GitHub_Link.txt` |
| `submissions/package_templates/PPT_File_Link.txt` | `06_BackupDocs/PPT_File_Link.txt` |
| `submissions/package_templates/CodeBuddy_History_Link.txt` | `04_CodeBuddy/CodeBuddy_History_Link.txt` |
| `submissions/package_templates/Social_Media_Link_Optional.txt` | `06_BackupDocs/Social_Media_Link_Optional.txt` |

### WebGL_Link.txt

```text
WebGL 在线试玩链接：
TODO

验证方式：
无痕窗口可打开，不需要登录，不是 localhost。
```

### Demo_Video_Link.txt

```text
Demo 视频链接：
TODO

说明：
视频展示移动、战斗、AI 叙事、成长反馈、Boss 或最终胜利。
```

### GitHub_Link.txt

```text
GitHub 仓库：
https://github.com/youkai-emmison/Craftsmen-and-Homo-sapiens/tree/master
```

## CodeBuddy 历史记录

如果平台允许上传附件，建议放：

```text
CodeBuddy_History_Craftsmen_and_Homo_sapiens.zip
```

如果 CodeBuddy 只能导出网页或截图，也可以把导出的 HTML、PDF、截图文件夹统一放进：

```text
04_CodeBuddy/
```

注意：

- 不要用 Codex 记录替代 CodeBuddy 记录。
- CodeBuddy 记录中最好能看到项目名、Unity C#、提交材料、调试或脚本修改过程。

## 不要放进上传包

不要把 Unity 自动生成或本地缓存目录放进最终上传包：

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `UserSettings/`
- `.vs/`
- `.idea/`
- `.vscode/`
- 本地未确认授权的 Asset Store 原始素材包

也不要上传：

- 只有本机能打开的本地路径。
- 需要登录权限但没有开放访问的网盘链接。
- 旧版本 PPT 或旧版本视频。
- 含 `localhost` 的试玩链接。

## 第三方素材提醒

如果上传包里包含第三方素材原文件，需要先确认授权和仓库/提交平台规则。

当前素材说明以 `docs/THIRD_PARTY_ASSETS.md` 为准。不要把第三方 Asset Store 素材说成全部原创。

## 最终压缩前检查

- [ ] `WebGL_Link.txt` 已填真实链接。
- [ ] `Demo_Video_Link.txt` 已填真实链接。
- [ ] PPT 最后一页的链接和文本文件一致。
- [ ] 项目书 PDF 能打开。
- [ ] CodeBuddy 历史记录已放入 `04_CodeBuddy/`。
- [ ] 压缩包里没有 `Library/`、`Temp/`、`UserSettings/`。
- [ ] 压缩包名称能看出项目名和比赛名。
- [ ] 用另一个目录解压一次，确认文件都能打开。
