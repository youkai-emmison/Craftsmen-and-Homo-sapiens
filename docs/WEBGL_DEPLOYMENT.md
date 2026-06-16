# WebGL Deployment Guide

The Render playable demo link is now available: https://craftsmen-and-homo-sapiens.onrender.com.

## Project

- 中文名：能工智人：糖芯工坊
- English: Craftsmen and Homo Sapiens: The Candy Forge
- Track: 叙事类游戏 / Narrative Games
- Demo scene: `Assets/Scenes/SampleScene.unity`

## Build Output

Unity WebGL 原始构建输出：

```text
Build/WebGL
```

Render / 静态站点发布目录：

```text
Submission/WebGLSite
```

## Unity Editor Menu

项目提供两个编辑器菜单：

```text
Tools > Hackathon > Build WebGL
Tools > Hackathon > Prepare Deploy Folder
```

对应脚本：

```text
Assets/Scripts/Editor/WebGLBuildCommand.cs
Assets/Scripts/Editor/HackathonBuildMenu.cs
```

## Build Steps

1. 用 Unity `2022.3.53f1` 或 `2022.3.53f1c1` 打开项目。
2. 打开 `Assets/Scenes/SampleScene.unity`。
3. 确认 Console 没有红色编译错误。
4. 打开 `File > Build Settings...`，切换到 WebGL。
5. 确认 Demo 场景已经加入 `Scenes In Build`。
6. 建议在 WebGL Publishing Settings 中使用 `Compression Format: Disabled`，或启用 `Decompression Fallback`。
7. 点击 `Tools > Hackathon > Build WebGL`。
8. 等待 Unity 生成 `Build/WebGL`。
9. 点击 `Tools > Hackathon > Prepare Deploy Folder`，或运行整理脚本。

## Prepare Static Site Folder

Windows:

```powershell
powershell -ExecutionPolicy Bypass -File tools/prepare_webgl_site.ps1
```

macOS / Linux:

```bash
bash tools/prepare_webgl_site.sh
```

整理后必须存在：

```text
Submission/WebGLSite/index.html
Submission/WebGLSite/Build/
Submission/WebGLSite/TemplateData/
```

如果缺少这些路径，说明还没有真实 Unity WebGL 构建，不能部署。

## Validate For Render

Render Static Site 的 Build Command 使用：

```bash
bash tools/render_validate_static_site.sh
```

本地 Windows 可检查：

```powershell
powershell -ExecutionPolicy Bypass -File tools/render_validate_static_site.ps1
```

验证脚本只检查真实 WebGL 静态站点是否存在，不会运行 Unity，也不会创建假页面。

## Local Preview

Unity WebGL 不建议直接双击 `index.html` 运行。可以启动本地静态服务器：

```powershell
cd Submission/WebGLSite
python -m http.server 8000
```

浏览器打开：

```text
http://localhost:8000
```

`localhost` 只用于本地测试，不能填到比赛提交表单。

## Render

Render 专项步骤见：

```text
docs/RENDER_DEPLOYMENT.md
```

推荐 Render 设置：

```text
Repository: youkai-emmison/Craftsmen-and-Homo-sapiens
Branch: render-deploy
Root Directory: 留空
Build Command: bash tools/render_validate_static_site.sh
Publish Directory: Submission/WebGLSite
Environment Variables: SKIP_INSTALL_DEPS=true
```

## Common Issues

- 404：Publish Directory 写错，或 `Submission/WebGLSite/index.html` 没提交。
- Render build failed：验证脚本找不到真实 WebGL 构建。
- Unity 加载黑屏：检查 WebGL 压缩设置，建议禁用压缩或启用 Decompression Fallback 后重建。
- GitHub push 失败：单文件超过 100MB，需要 Git LFS 或换 Cloudflare Pages Direct Upload。
- 页面能打开但游戏不动：打开浏览器 Console 检查 404、`.wasm`、`.data`、`.br`、`.gz` 报错。

## Submission Reminder

正式提交前需要人工回填：

- WebGL 在线试玩链接
- Demo 视频链接
- PPT / PDF 链接
- CodeBuddy 历史导出文件
- 团队、学校、队长联系方式
