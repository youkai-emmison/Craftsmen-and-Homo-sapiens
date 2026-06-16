# Render Deployment Guide

The Render playable demo link is now available: https://craftsmen-and-homo-sapiens.onrender.com.

## 1. 准备 Unity WebGL 构建

1. 打开 Unity。
2. 打开正式 Demo 场景：`Assets/Scenes/SampleScene.unity`。
3. 确认 Console 没有红色编译错误。
4. 打开 `File > Build Settings...`，切换到 WebGL。
5. 确认 Demo 场景已加入 `Scenes In Build`。
6. 在 WebGL Publishing Settings 中优先使用 `Compression Format: Disabled`，或启用 `Decompression Fallback`。
7. 点击：

```text
Tools > Hackathon > Build WebGL
```

构建完成后应存在：

```text
Build/WebGL/index.html
Build/WebGL/Build/
Build/WebGL/TemplateData/
```

## 2. 整理 Render 发布目录

Windows:

```powershell
powershell -ExecutionPolicy Bypass -File tools/prepare_webgl_site.ps1
```

macOS / Linux:

```bash
bash tools/prepare_webgl_site.sh
```

整理后应存在：

```text
Submission/WebGLSite/index.html
Submission/WebGLSite/Build/
Submission/WebGLSite/TemplateData/
Submission/WebGLSite/DEPLOYMENT_README.md
```

## 3. 本地验证

Windows:

```powershell
powershell -ExecutionPolicy Bypass -File tools/render_validate_static_site.ps1
```

macOS / Linux:

```bash
bash tools/render_validate_static_site.sh
```

成功输出：

```text
Unity WebGL static site is ready for Render.
```

## 4. 提交到部署分支

推荐用独立部署分支，避免把 WebGL 构建产物直接塞进 `master`。

```bash
git checkout master
git pull origin master
git checkout -B render-deploy
git add render.yaml tools/render_validate_static_site.sh tools/render_validate_static_site.ps1 tools/prepare_webgl_site.sh tools/prepare_webgl_site.ps1 docs/RENDER_DEPLOYMENT.md docs/WEBGL_DEPLOYMENT.md docs/FINAL_MANUAL_STEPS.md README.md
git add -f Submission/WebGLSite
git commit -m "Deploy Unity WebGL site to Render"
git push origin render-deploy
```

如果 push 时提示单个文件超过 100MB，请停止。普通 GitHub 仓库不能直接提交超过 100MB 的文件，需要 Git LFS，或改用 Cloudflare Pages Direct Upload。

## 5. Render 控制台设置

1. 打开 Render Dashboard。
2. 点击 `New > Static Site`。
3. Connect GitHub repository：

```text
youkai-emmison/Craftsmen-and-Homo-sapiens
```

4. Branch：

```text
render-deploy
```

5. Root Directory：留空。
6. Build Command：

```bash
bash tools/render_validate_static_site.sh
```

7. Publish Directory：

```text
Submission/WebGLSite
```

8. Environment Variables：

```text
SKIP_INSTALL_DEPS=true
```

9. 点击 Create Static Site。
10. Deployment result link: `https://craftsmen-and-homo-sapiens.onrender.com`.

## 6. 部署后检查

1. 打开 Render 链接。
2. 确认 Unity 加载条出现。
3. 确认游戏能进入。
4. 测试移动、跳跃、攻击。
5. 展示 NPC 对话、背包、合成、技能树、Boss 战。
6. 打开浏览器 Console，确认没有明显 404 或 WebGL 压缩解析错误。
7. Fill the competition form `Playable Demo Link` with `https://craftsmen-and-homo-sapiens.onrender.com`.

## 7. 常见错误

- `404`：Publish Directory 写错，或没有提交 `Submission/WebGLSite/index.html`。
- Render build failed：忘记提交 `Submission/WebGLSite`，验证脚本主动失败。
- Unity 卡加载：WebGL 压缩不兼容，重新构建时禁用压缩或启用 Decompression Fallback。
- GitHub push 失败：WebGL 文件超过 100MB，需要 Git LFS 或换 Cloudflare Pages Direct Upload。
- 页面黑屏：检查浏览器 Console 和 Unity WebGL build logs。

## 8. 回填位置

部署成功后，把 Render 链接回填到：

- `docs/SUBMISSION_FORM_COPY.md`
- `README.md`
- PPT 的 Demo Link 已回填
- 比赛提交表单
