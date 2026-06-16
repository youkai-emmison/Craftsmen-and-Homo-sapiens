# Final Manual Steps

Codex 已准备提交材料、构建脚本和部署说明，但没有实际部署。最终提交前请人工完成以下步骤。

## 1. Unity WebGL 构建

1. 用 Unity `2022.3.53f1` 或 `2022.3.53f1c1` 打开项目。
2. 打开 `Assets/Scenes/SampleScene.unity`。
3. 确认 Console 没有红色编译错误。
4. 打开 `File > Build Settings...`，确认 WebGL 平台可用。
5. 确认 Demo 场景已加入 `Scenes In Build`。
6. 建议 WebGL Publishing Settings 使用 `Compression Format: Disabled`，或启用 `Decompression Fallback`。
7. 点击：

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
Submission/WebGLSite/Build/
Submission/WebGLSite/TemplateData/
```

## 3. Render 部署

Codex 只能准备仓库、脚本和文档。Render 账号授权和创建 Static Site 需要人工操作。

推荐流程见：

```text
docs/RENDER_DEPLOYMENT.md
```

Render 控制台核心配置：

```text
Repository: youkai-emmison/Craftsmen-and-Homo-sapiens
Branch: render-deploy
Root Directory: 留空
Build Command: bash tools/render_validate_static_site.sh
Publish Directory: Submission/WebGLSite
Environment Variables: SKIP_INSTALL_DEPS=true
```

Render playable demo link is ready: `https://craftsmen-and-homo-sapiens.onrender.com`.

## 4. 回填 WebGL 链接

部署成功后，把链接复制到：

- `docs/SUBMISSION_FORM_COPY.md`
- `README.md`
- PPT Demo Link has been filled.
- 比赛提交表单

## 5. 录制 Demo 视频

按 `docs/DEMO_RECORDING_GUIDE.md` 录制 3 到 5 分钟视频。

必须展示：

- 标题或开场画面
- NPC / 糖芯工坊日志叙事
- 移动、跳跃、攻击
- 背包、合成、技能树
- Boss 战
- Victory / 结局或回家装置修复

## 6. 回填外部链接

准备好以下链接：

- WebGL 在线试玩链接
- Demo 视频链接
- PPT / PDF 链接
- CodeBuddy 历史导出文件
- GitHub 仓库链接

## 7. 打包源码

需要源码包时执行：

```powershell
powershell -ExecutionPolicy Bypass -File tools/package_submission.ps1
```

生成目标：

```text
Submission/Craftsmen-and-Homo-sapiens_Source.zip
```

注意：`.zip` 默认被 `.gitignore` 忽略，适合本地生成后手动上传，不建议长期提交到仓库。
