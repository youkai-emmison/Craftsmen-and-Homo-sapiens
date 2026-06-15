// Generate the editable hackathon PPTX with varied layouts and real game assets.
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
  pinkSoft: "#FFDDEF",
  blue: "#74CDFF",
  blueSoft: "#DEF5FF",
  mint: "#CBF0E5",
  amber: "#FFD36B",
  purple: "#634893",
  dark: "#322853",
  muted: "#695F84",
  white: "#FFFFFF",
};

const slides = [
  {
    title: "能工智人：遗忘工坊",
    subtitle: "Craftsmen and Homo Sapiens: The Forgotten Forge",
    layout: "大主视觉封面",
    assets: ["hero_stage.png"],
    purpose: "第一眼说明这是可爱工坊风的叙事动作游戏。",
  },
  {
    title: "游戏是什么",
    subtitle: "AI 叙事驱动的横版动作冒险",
    layout: "左图右文",
    assets: ["player_lineup.png"],
    purpose: "用主角动作展示和三个关键词解释游戏类型。",
  },
  {
    title: "核心玩法循环",
    subtitle: "短流程，但闭环完整",
    layout: "横向路线图",
    assets: ["gameplay_loop_route.png"],
    purpose: "把房间推进、战斗、成长和结局画成路线。",
  },
  {
    title: "角色与怪物",
    subtitle: "真实 spritesheet 支撑 Demo 表现",
    layout: "阵容卡牌墙",
    assets: ["character_cards.png", "enemy_lineup.png"],
    purpose: "展示主角、NPC、普通怪、精英怪和 Boss 占位。",
  },
  {
    title: "AI 叙事如何进入游戏",
    subtitle: "不是只写在文档里，而是进入 UI 和对话流程",
    layout: "日志 UI 展示",
    assets: ["memory_log_mock.png"],
    purpose: "说明 AI 生成内容如何变成工坊记忆日志。",
  },
  {
    title: "Demo 录屏路线",
    subtitle: "3–5 分钟让评委看懂完整闭环",
    layout: "时间轴 + 截图占位",
    assets: ["demo_timeline.png"],
    purpose: "给同学录视频和后续截图回填使用。",
  },
  {
    title: "技术与部署结构",
    subtitle: "Unity 2D 原型，WebGL 静态部署准备",
    layout: "模块架构图",
    assets: ["tech_architecture.png"],
    purpose: "展示 Player、Combat、Enemy、UI、Dialogue、WebGL 的关系。",
  },
  {
    title: "亮点与提交状态",
    subtitle: "材料已准备，真实链接与视频等待人工回填",
    layout: "左右清单看板",
    assets: ["submission_status_board.png"],
    purpose: "诚实说明已完成与待完成事项。",
  },
];

async function writeBlob(filePath, blob) {
  await fs.writeFile(filePath, new Uint8Array(await blob.arrayBuffer()));
}

async function readImageBlob(filePath) {
  const bytes = await fs.readFile(filePath);
  return bytes.buffer.slice(bytes.byteOffset, bytes.byteOffset + bytes.byteLength);
}

function addText(slide, text, x, y, w, h, style) {
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

function addPanel(slide, x, y, w, h, fill = COLORS.white, stroke = COLORS.pink, radius = "rounded-xl", width = 2) {
  return slide.shapes.add({
    geometry: "roundRect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: stroke, width },
    borderRadius: radius,
  });
}

function addCircle(slide, x, y, size, fill) {
  slide.shapes.add({
    geometry: "ellipse",
    position: { left: x, top: y, width: size, height: size },
    fill,
    line: { style: "solid", fill: "none", width: 0 },
  });
}

function addSlideChrome(slide, number, label) {
  slide.background.fill = COLORS.paper;
  addCircle(slide, -38, 52, 96, COLORS.blue);
  addCircle(slide, 1168, -46, 150, COLORS.pink);
  addCircle(slide, 1186, 598, 112, COLORS.mint);
  addText(slide, `${String(number).padStart(2, "0")} / 08`, 58, 34, 96, 24, {
    fontSize: 13,
    bold: true,
    color: COLORS.pink,
  });
  addText(slide, label, 154, 33, 460, 26, {
    fontSize: 13,
    bold: true,
    color: COLORS.muted,
  });
}

async function addImage(slide, fileName, x, y, w, h, alt, fit = "contain", stroke = COLORS.blue) {
  const filePath = path.join(visualDir, fileName);
  const blob = await readImageBlob(filePath);
  addPanel(slide, x, y, w, h, COLORS.white, stroke, "rounded-2xl", 2);
  slide.images.add({
    blob,
    contentType: "image/png",
    alt,
    fit,
    geometry: "roundRect",
    borderRadius: "rounded-xl",
    position: { left: x + 12, top: y + 12, width: w - 24, height: h - 24 },
  });
}

function addBullet(slide, text, x, y, w, color = COLORS.pinkSoft) {
  addPanel(slide, x, y, w, 54, color, color === COLORS.pinkSoft ? COLORS.pink : COLORS.blue, "rounded-lg", 1.5);
  addCircle(slide, x + 18, y + 16, 20, color === COLORS.pinkSoft ? COLORS.pink : COLORS.blue);
  addText(slide, text, x + 52, y + 12, w - 68, 28, {
    fontSize: 21,
    bold: true,
    color: COLORS.dark,
  });
}

function addFooter(slide) {
  addText(slide, "Demo Link / Video Link / Team：待回填", 58, 684, 520, 24, {
    fontSize: 12,
    color: COLORS.muted,
  });
  addText(slide, "No fake deployment, no fake demo video, no fake Unity screenshots.", 720, 684, 500, 24, {
    fontSize: 11,
    color: COLORS.purple,
    fontFace: "Aptos",
  });
}

async function slideCover(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 1, "Title Slide");
  await addImage(slide, "hero_stage.png", 450, 88, 760, 520, "Hero stage", "contain", COLORS.pink);
  addPanel(slide, 58, 104, 500, 320, COLORS.white, COLORS.pink, "rounded-2xl", 3);
  addText(slide, "能工智人：遗忘工坊", 92, 142, 430, 74, { fontSize: 44, bold: true });
  addText(slide, "Craftsmen and Homo Sapiens:\nThe Forgotten Forge", 94, 220, 390, 64, {
    fontSize: 24,
    bold: true,
    color: COLORS.purple,
  });
  addText(slide, "叙事类游戏 / Narrative Games", 94, 304, 380, 30, {
    fontSize: 23,
    bold: true,
    color: COLORS.pink,
  });
  addText(slide, "AI 叙事 + 横版动作 + 工坊记忆日志。", 94, 352, 410, 44, {
    fontSize: 21,
    color: COLORS.dark,
  });
  addPanel(slide, 90, 474, 408, 90, COLORS.dark, COLORS.blue, "rounded-xl", 2);
  addText(slide, "AI 世界观与剧情生成  /  游戏原画整理", 118, 500, 350, 32, {
    fontSize: 18,
    bold: true,
    color: COLORS.white,
  });
  addFooter(slide);
}

async function slideWhatGame(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 2, "What It Is");
  await addImage(slide, "player_lineup.png", 54, 96, 560, 500, "Player action lineup", "contain", COLORS.blue);
  addText(slide, "游戏是什么", 670, 92, 440, 58, { fontSize: 42, bold: true });
  addText(slide, "玩家进入一座会留下记忆的地下工坊，在轻量动作战斗中拼合文明冲突的真相。", 672, 160, 500, 86, {
    fontSize: 24,
    color: COLORS.muted,
  });
  addBullet(slide, "横版动作：移动、跳跃、近战攻击", 670, 285, 470, COLORS.pinkSoft);
  addBullet(slide, "房间推进：清敌、解锁、进入下一段", 706, 365, 470, COLORS.blueSoft);
  addBullet(slide, "AI 记忆日志：战斗中逐步补全世界观", 670, 445, 470, COLORS.pinkSoft);
  addFooter(slide);
}

async function slideLoop(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 3, "Gameplay Loop");
  addText(slide, "核心玩法循环", 60, 86, 430, 54, { fontSize: 42, bold: true });
  addText(slide, "不要让评委读大段说明：用一条路线展示“读日志 -> 战斗 -> 成长 -> Boss -> 结局”。", 62, 148, 760, 42, {
    fontSize: 21,
    color: COLORS.muted,
  });
  await addImage(slide, "gameplay_loop_route.png", 74, 218, 1110, 390, "Gameplay route", "contain", COLORS.pink);
  addFooter(slide);
}

async function slideCast(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 4, "Cast & Enemies");
  addText(slide, "角色与怪物", 62, 84, 380, 54, { fontSize: 42, bold: true });
  addText(slide, "这一页不讲系统，只让评委看到：主角、NPC、普通怪、精英怪和 Boss 占位都已经有真实素材。", 64, 145, 780, 42, {
    fontSize: 21,
    color: COLORS.muted,
  });
  await addImage(slide, "character_cards.png", 66, 220, 540, 386, "Character cards", "contain", COLORS.pink);
  await addImage(slide, "enemy_lineup.png", 650, 220, 540, 386, "Enemy lineup", "contain", COLORS.blue);
  addFooter(slide);
}

async function slideNarrative(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 5, "Narrative AI");
  await addImage(slide, "memory_log_mock.png", 58, 90, 730, 500, "Memory log mock", "contain", COLORS.blue);
  addText(slide, "AI 叙事如何进入游戏", 832, 92, 360, 64, { fontSize: 36, bold: true });
  addText(slide, "AI 内容不是只放在项目书里，而是以“工坊记忆日志”和 NPC 对话进入玩家流程。", 834, 165, 360, 92, {
    fontSize: 22,
    color: COLORS.muted,
  });
  addBullet(slide, "世界观与势力关系", 832, 295, 338, COLORS.pinkSoft);
  addBullet(slide, "房间日志与 NPC 引导", 860, 365, 338, COLORS.blueSoft);
  addBullet(slide, "Boss 背景与结局文本", 832, 435, 338, COLORS.pinkSoft);
  addFooter(slide);
}

async function slideTimeline(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 6, "Demo Flow");
  addText(slide, "Demo 录屏路线", 62, 84, 440, 54, { fontSize: 42, bold: true });
  addText(slide, "按时间轴录到：开场、日志、战斗、成长、Boss、Victory。", 64, 146, 760, 42, {
    fontSize: 21,
    color: COLORS.muted,
  });
  await addImage(slide, "demo_timeline.png", 70, 210, 1110, 400, "Demo timeline", "contain", COLORS.pink);
  addFooter(slide);
}

async function slideTech(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 7, "Tech & WebGL");
  addText(slide, "技术与部署结构", 62, 84, 440, 54, { fontSize: 42, bold: true });
  addText(slide, "Unity 2D 原型模块保持轻量拆分，WebGL 构建和静态站点部署已经准备，但不伪造已部署状态。", 64, 146, 790, 42, {
    fontSize: 21,
    color: COLORS.muted,
  });
  await addImage(slide, "tech_architecture.png", 70, 206, 1110, 408, "Tech architecture", "contain", COLORS.blue);
  addFooter(slide);
}

async function slideStatus(presentation) {
  const slide = presentation.slides.add();
  addSlideChrome(slide, 8, "Submission Status");
  await addImage(slide, "submission_status_board.png", 58, 88, 720, 514, "Submission status board", "contain", COLORS.pink);
  addText(slide, "亮点与提交状态", 824, 92, 360, 56, { fontSize: 38, bold: true });
  addText(slide, "这页要诚实：哪些已经准备好，哪些必须由人类最后回填。", 826, 158, 330, 64, {
    fontSize: 22,
    color: COLORS.muted,
  });
  addBullet(slide, "已完成：海报、PPT、部署文档、截图工具", 824, 275, 360, COLORS.pinkSoft);
  addBullet(slide, "待完成：WebGL 链接、Demo 视频、CodeBuddy 导出", 824, 355, 360, COLORS.blueSoft);
  addBullet(slide, "下一步：Unity 截图回填后录最终视频", 824, 435, 360, COLORS.pinkSoft);
  addFooter(slide);
}

async function writeDeckDocs() {
  const slideList = slides
    .map((slide, index) => `| ${index + 1} | ${slide.title} | ${slide.layout} | ${slide.assets.join(", ")} | ${slide.purpose} |`)
    .join("\n");

  await fs.writeFile(
    deckNotesPath,
    `# Project Deck Notes V2\n\n` +
      `新版 PPT 先依据 \`Submission/layout_plan_v2.md\` 规划，再用 \`@oai/artifact-tool\` 生成可编辑 PPTX。\n\n` +
      `## Slide Layouts\n\n` +
      `| # | Title | Layout | Real Assets | Purpose |\n| - | - | - | - | - |\n${slideList}\n\n` +
      `## QA Notes\n\n` +
      `- 每页采用不同布局：封面、左图右文、路线图、卡牌墙、日志 UI、时间轴、架构图、提交看板。\n` +
      `- 所有游戏视觉来自仓库内真实素材或由脚本从真实素材整理出的展示图。\n` +
      `- \`demo_timeline.png\` 和海报中的截图框仍明确标注“等待 Unity 截图回填”。\n` +
      `- 本次没有伪造 WebGL 部署、Demo 视频、Unity 截图或 CodeBuddy 历史。\n` +
      `- PPTX 已导出到 \`Submission/project_deck.pptx\`，PNG 预览导出到 \`Submission/project_deck_assets/\`。\n`,
    "utf8",
  );

  await fs.writeFile(
    deckMarkdownPath,
    `# 能工智人：遗忘工坊 - Project Deck V2\n\n` +
      `## 1. 封面\n能工智人：遗忘工坊 / Craftsmen and Homo Sapiens: The Forgotten Forge。\n\n` +
      `## 2. 游戏是什么\nAI 叙事驱动的横版动作冒险：横版动作、房间推进、AI 记忆日志。\n\n` +
      `## 3. 核心玩法循环\n进入房间 -> 读记忆日志 -> 战斗 -> 获得成长 -> 解锁下一房间 -> Boss -> 结局。\n\n` +
      `## 4. 角色与怪物\n展示主角、NPC、普通怪、精英怪和 Boss 占位素材。\n\n` +
      `## 5. AI 叙事如何进入游戏\nAI 生成世界观、房间日志、Boss 背景和结局文本，并通过 UI/对话进入游戏流程。\n\n` +
      `## 6. Demo 录屏路线\n0:00 开场，0:30 NPC/记忆日志，1:00 移动与战斗，2:00 成长反馈，3:00 Boss，4:00 Victory。\n\n` +
      `## 7. 技术与部署结构\nUnity 2D、Player、Combat、Enemy、Rooms、UI、Dialogue、WebGL Build。\n\n` +
      `## 8. 亮点与提交状态\n已完成海报/PPT/部署文档/素材整理/截图工具；待人工完成 WebGL 链接、Demo 视频、CodeBuddy 导出。\n`,
    "utf8",
  );

  await fs.writeFile(
    speakerNotesPath,
    `# PPT Speaker Notes V2\n\n` +
      `目标时长：3 到 5 分钟。\n\n` +
      `## Slide 1\n先用一句话介绍项目：这是一个 AI 叙事驱动的横版动作冒险原型，玩家进入被遗忘的地下工坊，通过战斗和记忆日志拼合真相。\n\n` +
      `## Slide 2\n说明游戏不是完整商业体量，而是一个短闭环 Demo：移动、跳跃、战斗、房间推进和叙事日志都能串起来。\n\n` +
      `## Slide 3\n按路线图讲流程：进入房间、读日志、战斗、成长、开门、Boss、结局。强调短但完整。\n\n` +
      `## Slide 4\n展示真实素材：主角、NPC、怪物和 Boss 占位。说明这些不是外部截图，而是项目内资产整理。\n\n` +
      `## Slide 5\n讲 AI 的核心价值：AI 生成世界观、房间日志、Boss 背景和结局文本，然后以游戏 UI/对话进入流程。\n\n` +
      `## Slide 6\n按时间轴说明 Demo 视频应该录什么。现在截图位是占位，最终会用 Unity 真实截图回填。\n\n` +
      `## Slide 7\n用模块图说明 Unity 2D 技术结构：玩家、战斗、敌人、房间、UI、对话，以及 WebGL 部署准备。\n\n` +
      `## Slide 8\n诚实说明提交状态：材料和脚本已准备，WebGL 链接、Demo 视频、CodeBuddy 导出需要赛前人工完成。\n`,
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
  await slideStatus(presentation);

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
