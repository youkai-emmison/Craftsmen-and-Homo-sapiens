# WebGL Deployment Guide

本任务只准备 WebGL 构建和部署材料，不实际部署。

## Project

- 中文名：能工智人：遗忘工坊
- English: Craftsmen and Homo Sapiens: The Forgotten Forge
- Track: 叙事类游戏 / Narrative Games
- Demo scene: `Assets/Scenes/SampleScene.unity`

## Unity Version

推荐使用：

- Unity 2022.3.53f1
- Unity 2022.3.53f1c1

不要为了部署无意义升级 Unity 版本，避免 `.meta`、`ProjectSettings` 或资源导入格式变化。

## Build Output

WebGL 构建输出目录：

```text
Build/WebGL
```

静态站点整理目录：

```text
Submission/WebGLSite
```

## Unity Editor Menu

项目提供两个编辑器菜单：

```text
Tools > Hackathon > Build WebGL
Tools > Hackathon > Prepare Deploy Folder
```

脚本位置：

```text
Assets/Scripts/Editor/WebGLBuildCommand.cs
Assets/Scripts/Editor/HackathonBuildMenu.cs
```

## Build Steps

1. 用 Unity 打开项目。
2. 打开 `Assets/Scenes/SampleScene.unity`。
3. 确认 Console 没有红色编译错误。
4. 确认 Build Settings 中包含 Demo 场景。
5. 点击 `Tools > Hackathon > Build WebGL`。
6. 等待 Unity 构建到 `Build/WebGL`。
7. 点击 `Tools > Hackathon > Prepare Deploy Folder`。
8. 确认 `Submission/WebGLSite/index.html` 存在。

也可以用脚本整理静态站点目录：

```powershell
powershell -ExecutionPolicy Bypass -File tools/prepare_webgl_site.ps1
```

macOS / Linux:

```bash
bash tools/prepare_webgl_site.sh
```

## Local Preview

Unity WebGL 不能稳定地直接双击 `index.html` 运行，建议使用本地静态服务器：

```powershell
cd Submission/WebGLSite
python -m http.server 8000
```

浏览器打开：

```text
http://localhost:8000
```

注意：`localhost` 只能用于测试，不能填入正式报名表。

## Compression Note

如果部署后浏览器黑屏或控制台出现 `.wasm`、`.data`、`.br`、`.gz` 相关加载问题，优先使用兼容性方案：

1. 打开 Unity Player Settings。
2. 找到 WebGL Publishing Settings。
3. 启用 Decompression Fallback。
4. 重新 Build WebGL。

这样文件可能更大，但更适合普通静态站点托管。

## Deployment Targets

推荐目标：

- Render Static Site
- Cloudflare Pages
- GitHub Pages

详细比较见：

```text
docs/DEPLOYMENT_OPTIONS.md
docs/FINAL_MANUAL_STEPS.md
```

## Submission Reminder

正式提交前需要人工回填：

- WebGL 在线试玩链接
- Demo 视频链接
- PPT / PDF 链接
- CodeBuddy 历史导出链接或文件
- 团队信息

本仓库已准备部署脚本和说明，但没有实际部署。
