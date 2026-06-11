# 外部动作负责人看板

这个文件用于最后提交前分配“仓库外必须完成”的任务。仓库内的文案、项目书、PPT 初稿、清单和模板已经准备好；真正提交前还需要 WebGL、Demo 视频、CodeBuddy 导出、团队信息和链接回填这些外部动作。

使用方式：

1. 开会或群聊时先打开本文件。
2. 给每一行填负责人。
3. 完成后把链接或文件路径写进 `submissions/LINKS_TO_FILL.md`。
4. 最后由提交负责人按 `submissions/SUBMISSION_DAY_RUNBOOK_ZH.md` 提交。

## 当前状态总览

| 外部动作 | 状态 | 建议负责人 | 产物 | 验收标准 | 回填位置 |
| --- | --- | --- | --- | --- | --- |
| WebGL 在线试玩 | Not Started | Unity 工程负责人 | WebGL 公开链接 | 无痕窗口可打开，不是 `localhost`，不需要登录 | `LINKS_TO_FILL.md`、`JUDGE_QUICK_START.md`、PPT、提交问卷 |
| Demo 视频 | Not Started | 演示 / 录屏负责人 | 公开视频链接 | 3 分钟左右，能看到 AI 叙事、战斗、成长、Boss 或胜利反馈 | `LINKS_TO_FILL.md`、PPT、WebGL 页面、提交问卷 |
| CodeBuddy 历史导出 | Not Started | AI 协作记录负责人 | 导出文件或公开链接 | 能看到项目名、Unity / C# / 提交材料相关协作记录 | `LINKS_TO_FILL.md`、提交问卷、上传包 |
| 团队真实信息 | Not Started | 队长 / 提交负责人 | 团队名、学校、成员、分工 | `TEAM_INFO_TEMPLATE.md` 中没有 `TODO` | PPT 最后一页、提交问卷、视频简介 |
| PPT 最后一页回填 | Not Started | 答辩负责人 | 更新后的 PPT / PDF | 团队信息、WebGL、视频、GitHub、CodeBuddy 信息一致 | PPT、PDF 预览、上传包 |
| 最终上传包 | Optional | 提交负责人 | zip 或网盘链接 | 包内有项目书、PPT、链接 txt、CodeBuddy 记录，无 Unity 缓存目录 | `LINKS_TO_FILL.md` 可选链接 |
| 社交媒体发布 | Optional | 宣传负责人 | 公开帖子或视频链接 | 文案不含错误链接，可带 `#CodeBuddy #腾讯云黑客松` | `LINKS_TO_FILL.md` 可选链接 |
| 最终提交问卷 | Not Started | 提交负责人 | 提交成功截图 | 表单中无 `TODO`、无本地路径、链接全部可访问 | 团队群保存截图 |

## 任务细化

### 1. WebGL 在线试玩

负责人要做：

1. 按 `submissions/WEBGL_UPLOAD_RUNBOOK.md` 构建 WebGL。
2. 上传到可公开访问的位置。
3. 用无痕窗口打开测试。
4. 至少验证一次移动、跳跃、攻击、敌人、房间推进和胜利反馈。
5. 把链接填入 `submissions/LINKS_TO_FILL.md`。

不要：

- 不要提交 `localhost`。
- 不要提交本地 `file://` 路径。
- 不要提交需要登录的私密链接。

### 2. Demo 视频

负责人要做：

1. 按 `submissions/DEMO_RECORDING_RUNBOOK.md` 录制。
2. 旁白参考 `docs/DEMO_VIDEO_SCRIPT.md`。
3. 上传时复制 `submissions/DEMO_VIDEO_UPLOAD_COPY.md`。
4. 用无痕窗口确认视频可播放。
5. 把链接填入 `submissions/LINKS_TO_FILL.md`。

视频必须出现：

- 赛题三和 AI 叙事说明。
- 玩家移动、跳跃、攻击。
- 敌人或 Boss 战斗。
- 成长反馈或房间推进。
- 胜利、Demo Complete 或最终目标反馈。

### 3. CodeBuddy 历史导出

负责人要做：

1. 按 `submissions/CODEBUDDY_SUBMISSION_CHECKLIST.md` 做最终检查。
2. 导出 CodeBuddy 历史记录。
3. 文件名建议：`CodeBuddy_History_Craftsmen_and_Homo_sapiens.zip`。
4. 放进最终上传包的 `04_CodeBuddy/`，或上传后把链接填入 `LINKS_TO_FILL.md`。

注意：

- 不要用 Codex 记录替代 CodeBuddy 记录。
- 如果平台只允许附件，就在提交问卷中写“CodeBuddy 历史记录见附件”。

### 4. 团队真实信息

负责人要做：

1. 打开 `submissions/TEAM_INFO_TEMPLATE.md`。
2. 填团队名称、学校 / 机构、联系人、联系方式。
3. 填每位成员姓名和分工。
4. 同步到 PPT 最后一页、提交问卷、视频简介或片尾。

验收：

- `TEAM_INFO_TEMPLATE.md` 不再有 `TODO`。
- PPT、问卷、视频简介中的团队信息一致。

### 5. PPT 最后一页回填

负责人要做：

1. 打开 `submissions/Craftsmen_Hackathon_Deck.pptx`。
2. 补团队信息。
3. 补 WebGL、Demo 视频、GitHub、CodeBuddy 说明。
4. 重新导出 PDF 预览版。
5. 确认 PPT 和 PDF 都能打开。

验收：

- PPT 链接和 `LINKS_TO_FILL.md` 一致。
- PDF 预览不是旧版。

### 6. 最终上传包

负责人要做：

1. 按 `submissions/UPLOAD_PACKAGE_README_ZH.md` 组织目录。
2. 把 `submissions/00_README_FIRST_ZH.md` 复制成 `00_README_先看我.md`。
3. 把 `submissions/package_templates/` 中的 txt 模板复制到对应目录。
4. 替换所有 `TODO`。
5. 不要放入 Unity 缓存目录。

不要放：

- `Library/`
- `Temp/`
- `Obj/`
- `Build/`
- `Builds/`
- `Logs/`
- `UserSettings/`

### 7. 最终提交问卷

负责人要做：

1. 先确认 `LINKS_TO_FILL.md` 已填完整。
2. 用 `submissions/SUBMISSION_PORTAL_FIELD_MAP_ZH.md` 对照平台字段。
3. 复制 `submissions/FORM_ANSWERS_COPYPASTE_ZH.md` 中的文案。
4. 上传或填写 WebGL、Demo 视频、PPT、项目书、CodeBuddy 记录。
5. 提交后截图保存成功页面。

验收：

- 表单里没有 `TODO`。
- 没有本地路径。
- 所有链接无痕窗口可打开。
- 成功页面已截图发给团队。

## 群里可复制分工消息

```text
最终提交还差这些外部动作，大家认领一下：

1. WebGL 在线试玩链接：
负责人：
产物：

2. Demo 视频链接：
负责人：
产物：

3. CodeBuddy 历史记录导出：
负责人：
产物：

4. 团队真实信息：
负责人：
产物：

5. PPT 最后一页回填：
负责人：
产物：

6. 最终提交问卷：
负责人：
产物：

所有链接先填 submissions/LINKS_TO_FILL.md，再同步到 PPT、JUDGE_QUICK_START、FINAL_SUBMISSION_INFO 和提交问卷。
```

## 最终交接确认

- [ ] 每个外部动作都有负责人。
- [ ] 每个负责人知道产物放在哪里。
- [ ] `LINKS_TO_FILL.md` 已经更新。
- [ ] PPT、问卷、视频简介里的链接一致。
- [ ] 提交成功页面已截图保存。
