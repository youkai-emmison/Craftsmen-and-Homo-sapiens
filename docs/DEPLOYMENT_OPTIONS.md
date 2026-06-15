# Deployment Options

本项目本次只准备部署材料，不在 Codex 中实际部署。

## 推荐顺序

1. Render Static Site
2. Cloudflare Pages
3. GitHub Pages

## 共同前提

- Unity 版本：2022.3.53f1 或 2022.3.53f1c1。
- Demo 场景：`Assets/Scenes/SampleScene.unity`。
- WebGL 构建输出：`Build/WebGL`。
- 静态站点整理输出：`Submission/WebGLSite`。
- 如果 WebGL 压缩导致托管平台加载失败，优先在 Unity Player Settings 中启用 Decompression Fallback。

## Render Static Site

适合最终提交一个公开试玩链接。

1. 在 Unity 中执行 `Tools/Hackathon/Build WebGL`。
2. 再执行 `Tools/Hackathon/Prepare Deploy Folder`，或运行：

```powershell
powershell -ExecutionPolicy Bypass -File tools/prepare_webgl_site.ps1
```

3. 确认 `Submission/WebGLSite/index.html` 存在。
4. 在 Render 创建 Static Site。
5. 如果使用本仓库作为部署源，Publish Directory 填：

```text
Submission/WebGLSite
```

6. 如果不想把 WebGL build 提交到仓库，可以单独新建一个私有/部署仓库，只放 `Submission/WebGLSite` 内容。

## Cloudflare Pages

适合静态文件托管。

1. 生成 `Submission/WebGLSite`。
2. 创建 Cloudflare Pages 项目。
3. 上传文件夹，或连接一个只包含 WebGL 静态文件的部署仓库。
4. Build command 留空。
5. Output directory 选择根目录或 `Submission/WebGLSite`，取决于你的仓库结构。

## GitHub Pages

适合快速公开演示，但要注意仓库公开后的第三方素材授权。

1. 生成 `Submission/WebGLSite`。
2. 建议创建单独的 `gh-pages` 分支或单独部署仓库。
3. 把 `Submission/WebGLSite` 的内容放到 Pages 发布目录。
4. 在 GitHub Settings > Pages 中启用。

## 不建议

- 不建议把 `Library`、`Temp`、`UserSettings`、`Build` 长期提交到主仓库。
- 不建议在 Public 仓库中公开提交未经确认可再分发的 Unity Asset Store 原始素材。
- 不要把 `localhost` 链接填进正式提交表单。
