# WebGL 构建与部署说明

## 目标

比赛要求作品提供可在浏览器中直接在线体验的网页地址。因此最终需要把 Unity 项目构建为 WebGL，并部署到静态网页托管平台。

最终上传当天可以直接按照 `submissions/WEBGL_UPLOAD_RUNBOOK.md` 执行。

## 推荐 Unity 版本

- Unity `2022.3.53f1`
- Unity `2022.3.53f1c1`

不要无意义升级 Unity 版本，避免 `.meta`、ProjectSettings 或资源导入格式变化。

## 本地构建步骤

1. 打开 Unity Hub。
2. 使用 Unity 2022.3.53f1 / 2022.3.53f1c1 打开项目。
3. 打开当前 Demo 场景：
   - 当前可用：`Assets/Scenes/SampleScene.unity`
   - 若后续新增正式场景：`Assets/Scenes/HackathonDemo.unity`
4. 进入 `File > Build Settings...`
5. 选择 `WebGL`
6. 点击 `Switch Platform`
7. 确认 Scenes In Build 中包含 Demo 场景。
8. 点击 `Build`
9. 输出目录建议选择：

`Build/WebGL`

## 本地预览

Unity WebGL 不能直接双击 `index.html` 可靠运行，建议使用本地静态服务器。

可选方式：

```powershell
cd Build/WebGL
python -m http.server 8000
```

浏览器打开：

`http://localhost:8000`

如果本机 Python 命令不可用，可以使用 VS Code Live Server、Node.js 静态服务器或其他本地服务器。

## 部署方式建议

### 方案 A：腾讯云 Cloud Studio

适合比赛语境。

1. 创建 Cloud Studio 工作空间。
2. 上传或拉取项目仓库。
3. 构建 WebGL。
4. 将 `Build/WebGL` 作为静态站点目录。
5. 获取公开访问链接。

### 方案 B：GitHub Pages

适合快速公开展示。

1. 将 `Build/WebGL` 内容放入单独分支或 `docs/` 静态目录。
2. 在 GitHub 仓库 Settings 中开启 Pages。
3. 选择对应分支与目录。
4. 等待部署完成。

注意：如果仓库包含第三方 Asset Store 原始素材，请先确认许可证和仓库公开策略。

### 方案 C：其他静态托管

任何能托管静态文件的平台都可以，例如：

- Cloudflare Pages
- Vercel
- Netlify
- 自有服务器

## WebGL 构建前检查

- [ ] 当前 Demo 场景能 Play。
- [ ] Console 没有红色 C# 编译错误。
- [ ] 没有关键 `NullReferenceException` 刷屏。
- [ ] UI 字体能在 WebGL 中显示。
- [ ] 操作提示清楚。
- [ ] 打开网页后能开始游戏。
- [ ] 3 分钟内能走完 Demo 路线。

## 提交时需要填写

- 在线试玩链接：
  - 待填写
- Demo 视频链接：
  - 待填写
- GitHub 仓库链接：
  - 待填写

## 常见问题

### 页面加载很慢

Unity WebGL 首次加载较慢，录视频时可以提前打开页面并等待加载完成。

### 中文乱码

如果 TextMeshPro 没有中文字体资产，建议提交版 UI 使用英文文本，或者制作包含中文字符的 TMP 字体资产。

### 浏览器黑屏

检查：

- WebGL 构建是否完整上传。
- 浏览器控制台是否有资源 404。
- 服务器是否正确提供 `.wasm`、`.data`、`.js` 文件。
- 当前场景是否加入 Build Settings。

### Git 不建议提交 Build

`Build/WebGL` 可以作为最终部署产物，但一般不建议长期提交到主项目仓库。可以用单独部署分支或发布包保存。
