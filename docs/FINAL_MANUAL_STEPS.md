# Final Manual Steps

Codex 已准备提交材料、构建脚本和部署说明，但没有实际部署。最终提交前请人工完成以下步骤。

## 1. Unity WebGL 构建

1. 用 Unity 2022.3.53f1 / 2022.3.53f1c1 打开项目。
2. 打开 `Assets/Scenes/SampleScene.unity`。
3. 确认 Console 没有红色编译错误。
4. 打开 `File > Build Settings...`，确认 WebGL 平台可用。
5. 确认 Demo 场景已加入 Scenes In Build。
6. 点击 Unity 菜单：

```text
Tools > Hackathon > Build WebGL
```

输出目录应为：

```text
Build/WebGL
```

## 2. 整理静态站点目录

Unity 构建完成后，执行：

```text
Tools > Hackathon > Prepare Deploy Folder
```

或在 PowerShell 中执行：

```powershell
powershell -ExecutionPolicy Bypass -File tools/prepare_webgl_site.ps1
```

确认生成：

```text
Submission/WebGLSite/index.html
```

## 3. 手动部署

任选一个平台部署：

- Render Static Site
- Cloudflare Pages
- GitHub Pages

部署后把公开链接复制到：

- `docs/SUBMISSION_FORM_COPY.md`
- `submissions/link_backfill_values.local.json`
- 最终报名表

## 4. 录制 Demo 视频

按 `docs/DEMO_RECORDING_GUIDE.md` 录制 3 到 5 分钟视频。

必须展示：

- 标题或开场画面
- NPC / 记忆日志叙事
- 移动、跳跃、攻击
- 成长反馈
- Boss 战
- 结局 / Demo Complete

## 5. 回填外部链接

准备好以下链接：

- WebGL 在线试玩链接
- Demo 视频链接
- PPT / PDF 链接
- CodeBuddy 历史导出链接或文件
- GitHub 仓库链接

如果要批量回填已有材料，可参考：

```text
submissions/LINK_BACKFILL_TOOL_ZH.md
```

## 6. 打包源码

需要源码包时执行：

```powershell
powershell -ExecutionPolicy Bypass -File tools/package_submission.ps1
```

生成目标：

```text
Submission/Craftsmen-and-Homo-sapiens_Source.zip
```

注意：`.zip` 默认被 `.gitignore` 忽略，适合本地生成后手动上传，不建议长期提交到仓库。
