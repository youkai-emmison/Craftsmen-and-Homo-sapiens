# 最终占位符清理清单

这个文件用于最终提交前清理 `TODO`、`待回填`、本地路径和临时链接。目标不是把仓库里的模板占位符全部删掉，而是确保提交问卷、最终上传包、PPT 和评委入口里没有会让评委困惑的占位内容。

## 一句话原则

源码仓库可以保留模板文件里的占位符，最终提交给平台或评委的材料不能保留占位符。

## 必须回填的外部内容

| 内容 | 最终来源 | 回填到哪里 | 验收标准 |
| --- | --- | --- | --- |
| WebGL 在线试玩链接 | WebGL 部署平台 | `LINKS_TO_FILL.md`、`FINAL_SUBMISSION_INFO.md`、`JUDGE_QUICK_START.md`、PPT、提交问卷 | 无痕窗口可打开，不是 `localhost`，不需要登录 |
| Demo 视频链接 | 视频平台或网盘 | `LINKS_TO_FILL.md`、`FORM_ANSWERS_COPYPASTE_ZH.md`、`WEBGL_PAGE_COPY.md`、提交问卷 | 可公开播放，权限不是私密 |
| CodeBuddy 历史记录 | CodeBuddy 导出文件或链接 | `LINKS_TO_FILL.md`、`FINAL_SUBMISSION_INFO.md`、提交问卷 | 能看到项目名、Unity 开发和 AI 协作过程 |
| PPT 文件链接 | 平台附件或网盘 | `LINKS_TO_FILL.md`、提交问卷 | 文件能打开，不是旧版或空白 |
| 团队真实信息 | 队长或提交负责人 | `TEAM_INFO_TEMPLATE.md`、PPT、视频简介、提交问卷 | 团队名、学校、成员和分工没有空项 |
| 社交媒体链接 | 可选加分项 | `LINKS_TO_FILL.md`、提交问卷可选字段 | 公开可访问，内容不含错误链接 |

## 仓库中允许保留占位符的模板

这些文件本来就是给最后回填用的工作表或模板，可以在仓库中保留 `TODO`：

- `submissions/LINKS_TO_FILL.md`
- `submissions/TEAM_INFO_TEMPLATE.md`
- `submissions/package_templates/*.txt`
- `submissions/package_templates/README_PACKAGE_TEMPLATES_ZH.md`
- `submissions/EXTERNAL_ACTION_OWNER_BOARD_ZH.md`
- `submissions/SUBMISSION_RISK_REGISTER_ZH.md`
- `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md`
- `submissions/SUBMISSION_PORTAL_FIELD_MAP_ZH.md`
- `submissions/UPLOAD_PACKAGE_README_ZH.md`
- `submissions/WEBGL_UPLOAD_RUNBOOK.md`

保留它们的原因是让队友知道还缺什么，不代表可以直接把这些占位内容粘进提交平台。

## 最终提交前必须清空占位符的材料

这些材料更接近最终提交内容，真正提交前应替换所有必填占位符：

- `submissions/FINAL_SUBMISSION_INFO.md`
- `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`
- `submissions/JUDGE_QUICK_START.md`
- `submissions/JUDGE_ONE_PAGE_BRIEF_ZH.md`
- `submissions/WEBGL_PAGE_COPY.md`
- `submissions/DEMO_VIDEO_UPLOAD_COPY.md`
- `submissions/00_README_FIRST_ZH.md`
- `submissions/Craftsmen_Hackathon_Deck.pptx`
- `submissions/Craftsmen_Hackathon_Deck_Preview.pdf`

如果时间来不及，至少保证提交问卷和评委第一眼看到的入口没有 `TODO`、`待回填`、本地路径或 `localhost`。

## 不要放进最终上传包的内容

这些内容可以存在于开发仓库或本机，但不要作为最终附件直接交给评委：

- `Library/`
- `Temp/`
- `Logs/`
- `UserSettings/`
- `Build/` 中未整理的临时构建目录
- `outputs/` 中的本地生成缓存
- `submissions/artifact-build-manifest.json`
- Unity Asset Store 原始素材包，除非授权和仓库权限已确认
- 含 `C:\Users\...`、`file://`、`localhost` 的临时文件或链接说明

`submissions/artifact-build-manifest.json` 是 PPT 生成工具留下的构建元数据，里面可能包含本机路径。它不属于评委需要看的材料。

## 最终搜索命令

在 PowerShell 中可以用下面的命令做最后检查：

```powershell
rg -n "TODO|待回填|待补|PLACEHOLDER|localhost|file://|127\.0\.0\.1|C:\\Users" submissions docs README.md
```

也可以运行仓库提供的自动自检脚本：

```powershell
powershell -ExecutionPolicy Bypass -File submissions\Run-FinalSubmissionAudit.ps1
```

脚本说明见 `submissions/FINAL_SUBMISSION_AUTOCHECK_ZH.md`。

看到结果后按下面规则处理：

- 出现在模板文件里：确认它确实只是模板，不要复制到最终表单。
- 出现在提交问卷复制稿里：必须替换。
- 出现在 WebGL、Demo、PPT、CodeBuddy 链接位置：必须替换。
- 出现在 `artifact-build-manifest.json`：不要放入最终上传包。
- 出现在文档说明句里，例如“不要提交 localhost”：可以保留。

## 手工检查顺序

1. 打开 `submissions/LINKS_TO_FILL.md`，把所有外部链接先集中填完。
2. 打开 `submissions/FINAL_SUBMISSION_INFO.md`，替换 WebGL、视频、PPT、CodeBuddy 和团队信息。
3. 打开 `submissions/FORM_ANSWERS_COPYPASTE_ZH.md`，替换链接区。
4. 打开 `submissions/JUDGE_QUICK_START.md`，替换评委入口链接。
5. 打开 `submissions/WEBGL_PAGE_COPY.md` 和 `submissions/DEMO_VIDEO_UPLOAD_COPY.md`，替换对外展示链接。
6. 打开 PPT，补最后一页团队信息和真实链接。
7. 如果生成压缩包，按 `submissions/UPLOAD_PACKAGE_README_ZH.md` 组织文件，不放本地缓存。
8. 最后把提交问卷里的每个链接复制到无痕窗口打开一次。

## 最终验收

- [ ] 提交问卷没有 `TODO`、`待回填`、本地路径、`localhost`。
- [ ] WebGL 链接、Demo 视频链接、GitHub 链接、PPT 链接、CodeBuddy 链接都能打开。
- [ ] PPT 最后一页没有空白团队信息。
- [ ] 评委快速指南中的链接和提交问卷一致。
- [ ] 上传包里没有 Unity 自动生成目录。
- [ ] 上传包里没有 `artifact-build-manifest.json`。
- [ ] 第三方素材说明和实际提交策略一致。
