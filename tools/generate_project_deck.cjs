// Generate a cleaner editable hackathon PPTX with fewer boxes and no status page.
// Key variables:
// - finalPptx: editable PowerPoint output.
// - previewDir: rendered slide PNGs used for visual QA.

const fs = require("node:fs/promises");
const path = require("node:path");
const { Presentation, PresentationFile } = require("@oai/artifact-tool");

const root = path.resolve(__dirname, "..");
const outputDir = path.join(root, "Submission");
const previewDir = path.join(outputDir, "project_deck_assets");
const finalPptx = path.join(outputDir, "project_deck.pptx");
const visualDir = path.join(outputDir, "visual_assets");
const deckNotesPath = path.join(outputDir, "project_deck_notes.md");
const deckMarkdownPath = path.join(outputDir, "project_deck.md");
const speakerNotesPath = path.join(root, "docs", "PPT_SPEAKER_NOTES.md");

const W = 1280;
const H = 720;
const COLORS = {
  paper: "#FFF8FD",
  cream: "#FFF4E4",
  pink: "#FF80B8",
  blue: "#74CDFF",
  mint: "#CBF0E5",
  purple: "#634893",
  dark: "#322853",
  muted: "#695F84",
  white: "#FFFFFF",
};

const slides = [
  {
    title: "能工智人：遗忘工坊",
    subtitle: "Craftsmen and Homo Sapiens: The Forgotten Forge",
    label: "Title",
    layout: "主视觉封面",
    assets: ["hero_stage.png"],
    purpose: "用主角、NPC、怪物和工坊场景建立第一眼游戏感。",
  },
  {
    title: "游戏是什么",
    subtitle: "AI 叙事驱动的横版动作冒险。",
    label: "Game",
    layout: "左图右文",
    assets: ["player_lineup.png"],
    purpose: "用主角动作展示和短文本解释游戏类型。",
  },
  {
    title: "核心玩法循环",
    subtitle: "进入房间、读日志、战斗、成长、开门、Boss、结局。",
    label: "Loop",
    layout: "横向流程图",
    assets: ["gameplay_loop_route.png"],
    purpose: "把核心闭环画成路线，不用大段文字解释。",
  },
  {
    title: "角色与怪物",
    subtitle: "主角、NPC、普通怪、精英怪和 Boss 占位都有真实素材。",
    label: "Cast",
    layout: "双图阵容展示",
    assets: ["player_lineup.png", "enemy_lineup.png"],
    purpose: "展示真实项目素材，不堆文字。",
  },
  {
    title: "AI 叙事进入游戏",
    subtitle: "AI 生成内容会进入工坊记忆日志和 NPC 对话。",
    label: "Narrative",
    layout: "日志 UI 展示",
    assets: ["memory_log_mock.png"],
    purpose: "证明 AI 内容和游戏流程有关，而不是只在项目书里。",
  },
  {
    title: "Demo 录屏路线",
    subtitle: "开场、日志、战斗、成长、Boss、Victory。",
    label: "Demo",
    layout: "时间轴",
    assets: ["demo_timeline.png"],
    purpose: "让录屏路线清楚，不讲额外状态。",
  },
  {
    title: "技术结构",
    subtitle: "Unity 2D 原型拆成 Player、Combat、Enemy、Rooms、UI、Dialogue、WebGL。",
    label: "Tech",
    layout: "模块图",
    assets: ["tech_architecture.png"],
    purpose: "简洁展示工程结构。",
  },
];

async function writeBlob(filePath, blob) {
  await fs.writeFile(filePath, new Uint8Array(await blob.arrayBuffer()));
}

async function readImageBlob(filePath) {
  const bytes = await fs.readFile(filePath);
  return bytes.buffer.slice(bytes.byteOffset, bytes.byteOffset + bytes.byteLength);
}

function addText(slide, text, x, y, w, h, style = {}) {
  const box = slide.shapes.add({
    geometry: "textbox",
    position: { left: x, top: y, width: w, height: h },
    fill: "none",
    line: { style: "solid", fill: "none", width: 0 },
  });
  box.text = text;
  box.text.style = {
    fontFace: "Microsoft YaHei",
    color: COLORS.dark,
    ...style,
  };
  return box;
}

function addPanel(slide, x, y, w, h, fill = COLORS.white, stroke = COLORS.pink, radius = "rounded-xl", width = 1.5) {
  return slide.shapes.add({
    geometry: "roundRect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: stroke, width },
    borderRadius: radius,
  });
}

function addSubtleShape(slide, x, y, w, h, fill) {
  slide.shapes.add({
    geometry: "ellipse",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: "none", width: 0 },
  });
}

function addSlideChrome(slide, number, label) {
  slide.background.fill = COLORS.paper;
  addSubtleShape(slide, -52, 54, 120, 120, "#DDF4FF");
  addSubtleShape(slide, 1180, -52, 140, 140, "#FFE0EF");
  addText(slide, `${String(number).padStart(2, "0")} / 07`, 64, 34, 90, 22, {
    fontSize: 12,
    bold: true,
    color: COLORS.pink,
  });
  addText(slide, label, 160, 34, 220, 22, {
    fontSize: 12,
    bold: true,
    color: COLORS.muted,
  });
}

async function addImage(slide, fileName, x, y, w, h, alt, fit = "contain") {
  const filePath = path.join(visualDir, fileName);
  const blob = await readImageBlob(filePath);
  slide.images.add({
    blob,
    contentType: "image/png",
    alt,
    fit,
    geometry: "rect",
    position: { left: x, top: y, width: w, height: h },
  });
}

function addFooter(slide) {
  addText(slide, "能工智人：遗忘工坊", 64, 684, 250, 22, {
    fontSize: 12,
    color: COLORS.muted,
  });
  addText(slide, "叙事类游戏 / Narrative Games", 900, 684, 260, 22, {
    fontSize: 12,
    color: COLORS.muted,
  });
}

async function slideCover(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 1, "Title");
  await addImage(slide, "hero_stage.png", 500, 96, 660, 500, "Hero stage", "contain");
  addPanel(slide, 72, 118, 470, 300, COLORS.white, COLORS.pink, "rounded-2xl", 2);
  addText(slide, "能工智人：遗忘工坊", 104, 154, 410, 62, { fontSize: 39, bold: true });
  addText(slide, "Craftsmen and Homo Sapiens:\nThe Forgotten Forge", 106, 224, 380, 58, {
    fontSize: 22,
    bold: true,
    color: COLORS.purple,
  });
  addText(slide, "叙事类游戏 / Narrative Games", 106, 304, 360, 28, {
    fontSize: 21,
    bold: true,
    color: COLORS.pink,
  });
  addText(slide, "AI 叙事 + 横版动作 + 工坊记忆日志", 106, 350, 360, 30, {
    fontSize: 20,
    color: COLORS.dark,
  });
  addFooter(slide);
}

async function slideWhatGame(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 2, "Game");
  await addImage(slide, "player_lineup.png", 66, 120, 560, 420, "Player action lineup", "contain");
  addText(slide, "游戏是什么", 700, 120, 360, 54, { fontSize: 43, bold: true });
  addText(slide, "玩家进入一座会留下记忆的地下工坊，在轻量动作战斗中拼合文明冲突的真相。", 702, 190, 420, 78, {
    fontSize: 23,
    color: COLORS.muted,
  });
  addText(slide, "横版动作", 704, 318, 220, 30, { fontSize: 27, bold: true, color: COLORS.pink });
  addText(slide, "房间推进", 704, 382, 220, 30, { fontSize: 27, bold: true, color: COLORS.blue });
  addText(slide, "AI 记忆日志", 704, 446, 260, 30, { fontSize: 27, bold: true, color: COLORS.purple });
  addFooter(slide);
}

async function slideLoop(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 3, "Loop");
  addText(slide, "核心玩法循环", 76, 84, 400, 54, { fontSize: 43, bold: true });
  addText(slide, "一条路线讲清楚 Demo：读日志、战斗、成长、开门、Boss、结局。", 78, 150, 760, 36, {
    fontSize: 22,
    color: COLORS.muted,
  });
  await addImage(slide, "gameplay_loop_route.png", 96, 218, 1060, 370, "Gameplay route", "contain");
  addFooter(slide);
}

async function slideCast(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 4, "Cast");
  addText(slide, "角色与怪物", 76, 84, 360, 54, { fontSize: 43, bold: true });
  addText(slide, "用真实素材展示主角、NPC、普通怪、精英怪和 Boss 占位。", 78, 150, 720, 36, {
    fontSize: 22,
    color: COLORS.muted,
  });
  await addImage(slide, "player_lineup.png", 70, 220, 540, 350, "Player lineup", "contain");
  await addImage(slide, "enemy_lineup.png", 670, 220, 520, 350, "Enemy lineup", "contain");
  addFooter(slide);
}

async function slideNarrative(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 5, "Narrative");
  await addImage(slide, "memory_log_mock.png", 74, 126, 680, 430, "Memory log mock", "contain");
  addText(slide, "AI 叙事进入游戏", 820, 126, 350, 54, { fontSize: 38, bold: true });
  addText(slide, "AI 辅助生成世界观、房间日志、Boss 背景和结局文本，并通过工坊记忆日志与 NPC 对话进入流程。", 822, 196, 350, 112, {
    fontSize: 22,
    color: COLORS.muted,
  });
  addText(slide, "世界观", 824, 360, 180, 30, { fontSize: 26, bold: true, color: COLORS.pink });
  addText(slide, "房间日志", 824, 420, 180, 30, { fontSize: 26, bold: true, color: COLORS.blue });
  addText(slide, "Boss 背景与结局", 824, 480, 260, 30, { fontSize: 26, bold: true, color: COLORS.purple });
  addFooter(slide);
}

async function slideTimeline(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 6, "Demo");
  addText(slide, "Demo 录屏路线", 76, 84, 420, 54, { fontSize: 43, bold: true });
  addText(slide, "开场、日志、战斗、成长、Boss、Victory。", 78, 150, 620, 36, {
    fontSize: 22,
    color: COLORS.muted,
  });
  await addImage(slide, "demo_timeline.png", 110, 218, 1000, 380, "Demo timeline", "contain");
  addFooter(slide);
}

async function slideTech(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 7, "Tech");
  addText(slide, "技术结构", 76, 84, 320, 54, { fontSize: 43, bold: true });
  addText(slide, "Unity 2D 原型模块保持拆分：玩家、战斗、敌人、房间、UI、对话与 WebGL。", 78, 150, 780, 36, {
    fontSize: 22,
    color: COLORS.muted,
  });
  await addImage(slide, "tech_architecture.png", 130, 218, 960, 380, "Tech architecture", "contain");
  addFooter(slide);
}

async function writeDeckDocs() {
  const slideList = slides
    .map((slide, index) => `| ${index + 1} | ${slide.title} | ${slide.layout} | ${slide.assets.join(", ")} | ${slide.purpose} |`)
    .join("\n");

  await fs.writeFile(
    deckNotesPath,
    `# Project Deck Notes V3\n\n` +
      `新版 PPT 删除了“提交状态/待完成事项”页面，减少边框、圆点和框套框，重点修复重叠问题。\n\n` +
      `## Slide Layouts\n\n` +
      `| # | Title | Layout | Real Assets | Purpose |\n| - | - | - | - | - |\n${slideList}\n\n` +
      `## QA Notes\n\n` +
      `- 总页数从 8 页调整为 7 页。\n` +
      `- 删除了“亮点与提交状态”页面，PPT 不再专门讲未完成事项。\n` +
      `- 图片外层不再额外套粗边框，避免框套框。\n` +
      `- 装饰圆点减少到页面边缘，避免遮挡标题、正文和图片。\n` +
      `- PPTX 已导出到 \`Submission/project_deck.pptx\`，PNG 预览导出到 \`Submission/project_deck_assets/\`。\n`,
    "utf8",
  );

  await fs.writeFile(
    deckMarkdownPath,
    `# 能工智人：遗忘工坊 - Project Deck V3\n\n` +
      `## 1. 封面\n能工智人：遗忘工坊 / Craftsmen and Homo Sapiens: The Forgotten Forge。\n\n` +
      `## 2. 游戏是什么\nAI 叙事驱动的横版动作冒险：横版动作、房间推进、AI 记忆日志。\n\n` +
      `## 3. 核心玩法循环\n进入房间 -> 读记忆日志 -> 战斗 -> 获得成长 -> 解锁下一房间 -> Boss -> 结局。\n\n` +
      `## 4. 角色与怪物\n展示主角、NPC、普通怪、精英怪和 Boss 占位素材。\n\n` +
      `## 5. AI 叙事进入游戏\nAI 生成世界观、房间日志、Boss 背景和结局文本，并通过 UI/对话进入游戏流程。\n\n` +
      `## 6. Demo 录屏路线\n开场、日志、战斗、成长、Boss、Victory。\n\n` +
      `## 7. 技术结构\nUnity 2D、Player、Combat、Enemy、Rooms、UI、Dialogue、WebGL。\n`,
    "utf8",
  );

  await fs.writeFile(
    speakerNotesPath,
    `# PPT Speaker Notes V3\n\n` +
      `目标时长：3 到 5 分钟。\n\n` +
      `## Slide 1\n用一句话介绍项目：这是一个 AI 叙事驱动的横版动作冒险原型，玩家进入被遗忘的地下工坊，通过战斗和记忆日志拼合真相。\n\n` +
      `## Slide 2\n说明游戏短闭环：移动、跳跃、战斗、房间推进和叙事日志串在一起。\n\n` +
      `## Slide 3\n按路线图讲流程：进入房间、读日志、战斗、成长、开门、Boss、结局。\n\n` +
      `## Slide 4\n展示真实素材：主角、NPC、怪物和 Boss 占位。\n\n` +
      `## Slide 5\n讲 AI 的核心价值：AI 生成世界观、房间日志、Boss 背景和结局文本，然后进入游戏 UI/对话。\n\n` +
      `## Slide 6\n按时间轴说明 Demo 视频应该录什么：开场、日志、战斗、成长、Boss、Victory。\n\n` +
      `## Slide 7\n用模块图说明 Unity 2D 技术结构：玩家、战斗、敌人、房间、UI、对话和 WebGL。\n`,
    "utf8",
  );
}

async function main() {
  await fs.mkdir(outputDir, { recursive: true });
  await fs.rm(previewDir, { recursive: true, force: true });
  await fs.mkdir(previewDir, { recursive: true });

  const presentation = Presentation.create({ slideSize: { width: W, height: H } });
  await slideCover(presentation);
  await slideWhatGame(presentation);
  await slideLoop(presentation);
  await slideCast(presentation);
  await slideNarrative(presentation);
  await slideTimeline(presentation);
  await slideTech(presentation);

  for (const [index, slide] of presentation.slides.items.entries()) {
    const stem = `slide-${String(index + 1).padStart(2, "0")}`;
    await writeBlob(path.join(previewDir, `${stem}.png`), await presentation.export({ slide, format: "png", scale: 1 }));
    const layout = await slide.export({ format: "layout" });
    await fs.writeFile(path.join(previewDir, `${stem}.layout.json`), await layout.text(), "utf8");
  }
  await writeBlob(path.join(previewDir, "deck-montage.webp"), await presentation.export({ format: "webp", montage: true, scale: 1 }));

  const pptx = await PresentationFile.exportPptx(presentation);
  await pptx.save(finalPptx);
  await writeDeckDocs();
  console.log(`Generated PPTX: ${finalPptx}`);
}

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
