# WebGL 在线试玩链接执行清单

这个文件用于最终生成“作品网页链接”。比赛提交需要一个评委能直接在浏览器中打开的在线试玩地址。

## 目标产物

- 一个可公开访问的 WebGL 网页链接。
- 链接能直接打开游戏，不需要评委下载 Unity、下载压缩包或手动运行服务器。
- 链接最终填写到：
  - `submissions/FINAL_SUBMISSION_INFO.md`
  - `docs/SUBMISSION_FORM_DRAFT.md`
  - PPT 最后一页或备注

## 构建前检查

- [ ] 当前仓库已拉取最新 `master`。
- [ ] Unity 版本使用 `2022.3.53f1` 或 `2022.3.53f1c1`。
- [ ] 当前 Demo 场景能 Play。
- [ ] Console 没有红色 C# 编译错误。
- [ ] Player 能移动、跳跃、攻击。
- [ ] 至少能看到一次 AI 叙事文本、NPC 对话或档案内容。
- [ ] Demo 能在 3 分钟内走到胜利或 Boss 结尾。

## Unity WebGL 构建步骤

1. 打开 Unity 项目。
2. 打开当前 Demo 场景，优先检查 `Assets/Scenes/SampleScene.unity`。
3. 进入 `File > Build Settings...`。
4. 选择 `WebGL`。
5. 点击 `Switch Platform`，等待平台切换完成。
6. 确认 `Scenes In Build` 包含当前 Demo 场景。
7. 点击 `Player Settings...`，检查项目名和默认分辨率。
8. 点击 `Build`。
9. 输出目录建议：

```text
Build/WebGL
```

## 本地预览

Unity WebGL 不建议直接双击 `index.html`，请用本地静态服务器预览。

```powershell
cd Build/WebGL
python -m http.server 8000
```

然后浏览器打开：

```text
http://localhost:8000
```

本地预览必须检查：

- [ ] 页面能加载完成。
- [ ] 不黑屏。
- [ ] 按键输入有效。
- [ ] UI 文字可读。
- [ ] 没有浏览器控制台资源 404。
- [ ] 能走完核心 Demo 流程。

## 上传方式

任选一个能公开访问静态网页的平台即可。推荐优先级：

1. 腾讯云相关静态网页托管，适合比赛语境。
2. itch.io，适合游戏原型公开试玩。
3. GitHub Pages，适合快速托管静态网页。
4. Netlify / Vercel / Cloudflare Pages 等静态托管平台。

上传时通常需要上传 `Build/WebGL` 目录中的全部内容，包括：

- `index.html`
- `Build/`
- `TemplateData/`
- 其他 Unity 生成的 `.js`、`.data`、`.wasm`、`.symbols.json` 文件

不要只上传 `index.html`，否则页面会黑屏或资源 404。

## 上传后检查

- [ ] 用无痕窗口打开公开链接。
- [ ] 用另一台设备或让队友打开一次。
- [ ] 页面不是登录后才能访问。
- [ ] 页面不是本地 `localhost` 地址。
- [ ] 第一次加载虽然慢，但最终能进入游戏。
- [ ] WebGL 链接已填入 `submissions/FINAL_SUBMISSION_INFO.md`。
- [ ] WebGL 链接已填入 PPT 最后一页或备注。
- [ ] WebGL 链接已放进 Demo 视频简介或片尾。
- [ ] 试玩页面标题、简介、操作说明已参考 `submissions/WEBGL_PAGE_COPY.md` 填写。

## 常见问题排查

### 页面黑屏

- 检查是否上传了整个 `Build/WebGL` 目录。
- 检查浏览器控制台是否有 404。
- 检查 `.wasm`、`.data` 文件是否被托管平台拦截。
- 检查 Demo 场景是否加入 `Scenes In Build`。

### 中文乱码

- 如果 WebGL 里中文字体缺失，优先用英文 UI 或确认 TextMeshPro 字体资产包含所需中文字符。

### 评委打不开

- 确认链接不是私有链接。
- 确认不需要登录账号。
- 确认手机和电脑至少一个端能打开。
- 如果平台限制跨域或压缩格式，换一个静态托管平台重新上传。

## 最终链接记录

- WebGL 在线试玩链接：https://craftsmen-and-homo-sapiens.onrender.com
- Upload platform: Render Static Site
- Upload date: 2026-06-16
- 最后检查人：TODO
