// Generate an editable screenshot-driven hackathon PPTX.
// Key variables:
// - cleanDir: cleaned real gameplay screenshots used as primary visuals.
// - previewDir: rendered slide PNGs used for visual QA.

const fs = require("node:fs/promises");
const path = require("node:path");
const { Presentation, PresentationFile } = require("@oai/artifact-tool");

const root = path.resolve(__dirname, "..");
const outputDir = path.join(root, "Submission");
const cleanDir = path.join(outputDir, "clean_screenshots");
const visualDir = path.join(outputDir, "visual_assets");
const previewDir = path.join(outputDir, "project_deck_assets");
const finalPptx = path.join(outputDir, "project_deck.pptx");
const deckNotesPath = path.join(outputDir, "project_deck_notes.md");
const deckMarkdownPath = path.join(outputDir, "project_deck.md");
const layoutPlanPath = path.join(outputDir, "layout_plan_v3.md");
const speakerNotesPath = path.join(root, "docs", "PPT_SPEAKER_NOTES.md");

const W = 1280;
const H = 720;
const COLORS = {
  paper: "#FFF8FD",
  cream: "#FFF4E4",
  pink: "#FF80B8",
  blue: "#74CDFF",
  purple: "#634893",
  dark: "#2C2348",
  darker: "#211B33",
  muted: "#695F84",
  white: "#FFFFFF",
  candyGreen: "#7FE3B4",
};

const screenshots = {
  move: "01_move_jump_attack_clean.png",
  npc: "02_npc_dialogue_clean.png",
  backpack: "03_backpack_clean.png",
  craft: "04_crafting_clean.png",
  skill: "05_skilltree_clean.png",
  boss: "06_boss_combat_clean.png",
  bossHero: "06_boss_combat_hero_crop.png",
};

const slides = [
  {
    title: "封面：Boss 战主视觉",
    layout: "全屏截图 + 左侧标题",
    assets: [screenshots.bossHero],
    purpose: "让评委第一眼看到真实游戏画面和可爱动作冒险调性。",
  },
  {
    title: "世界观：糖芯工坊",
    layout: "左侧游戏截图 + 右侧三张设定卡",
    assets: [screenshots.move, screenshots.npc],
    purpose: "解释理工男穿越成女仆工程师、用糖果材料搓科技回家的故事。",
  },
  {
    title: "游戏是什么",
    layout: "左侧大截图 + 右侧关键词",
    assets: [screenshots.move],
    purpose: "用移动、跳跃、攻击画面解释横版动作和房间推进。",
  },
  {
    title: "AI 叙事进入游戏",
    layout: "NPC 对话截图 + 叙事说明",
    assets: [screenshots.npc],
    purpose: "证明 AI 叙事不是只写在文档中，而是进入 NPC 对话和糖芯工坊日志。",
  },
  {
    title: "背包、装备与成长",
    layout: "背包 UI 大截图 + 系统说明",
    assets: [screenshots.backpack],
    purpose: "展示背包、装备说明和属性成长。",
  },
  {
    title: "合成与技能树",
    layout: "上下双截图 + 成长路线",
    assets: [screenshots.craft, screenshots.skill],
    purpose: "展示材料、合成、技能学习如何连接战斗成长。",
  },
  {
    title: "Demo 体验流程",
    layout: "六截图时间轴",
    assets: [screenshots.move, screenshots.npc, screenshots.backpack, screenshots.craft, screenshots.skill, screenshots.boss],
    purpose: "用 3-5 分钟展示完整游戏闭环。",
  },
  {
    title: "技术结构与部署准备",
    layout: "模块节点图 + Boss 小截图",
    assets: [screenshots.boss, "tech_architecture.png"],
    purpose: "说明 Unity 2D 模块拆分和 WebGL 静态部署准备。",
  },
];

async function writeBlob(filePath, blob) {
  await fs.writeFile(filePath, new Uint8Array(await blob.arrayBuffer()));
}

async function readImageBlob(filePath) {
  const bytes = await fs.readFile(filePath);
  return bytes.buffer.slice(bytes.byteOffset, bytes.byteOffset + bytes.byteLength);
}

function screenshotPath(fileName) {
  return path.join(cleanDir, fileName);
}

function visualPath(fileName) {
  return path.join(visualDir, fileName);
}

function text(slide, value, x, y, w, h, style = {}) {
  const box = slide.shapes.add({
    geometry: "textbox",
    position: { left: x, top: y, width: w, height: h },
    fill: "none",
    line: { style: "solid", fill: "none", width: 0 },
  });
  box.text = value;
  box.text.style = {
    typeface: "Microsoft YaHei",
    fontFace: "Microsoft YaHei",
    color: COLORS.dark,
    ...style,
  };
  return box;
}

function panel(slide, x, y, w, h, fill = COLORS.white, line = "none", width = 0, radius = "rounded-xl") {
  slide.shapes.add({
    geometry: "roundRect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: line, width },
    borderRadius: radius,
  });
}

function rectangle(slide, x, y, w, h, fill, line = "none", width = 0) {
  slide.shapes.add({
    geometry: "rect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: line, width },
  });
}

async function image(slide, filePath, x, y, w, h, alt, fit = "contain") {
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

function footer(slide, number) {
  text(slide, `0${number} / 08`, 56, 684, 84, 22, { fontSize: 12, bold: true, color: COLORS.pink });
  text(slide, "能工智人：糖芯工坊 | 落云宗：秦天 / 陈磊", 150, 684, 460, 22, { fontSize: 12, color: COLORS.muted });
}

function bullet(slide, value, x, y, color = COLORS.pink) {
  rectangle(slide, x, y + 8, 10, 10, color);
  text(slide, value, x + 24, y, 360, 34, { fontSize: 24, bold: true, color: COLORS.dark });
}

function settingCard(slide, title, body, x, y, color) {
  panel(slide, x, y, 332, 132, COLORS.white, color, 2, "rounded-2xl");
  rectangle(slide, x + 24, y + 24, 14, 14, color);
  text(slide, title, x + 50, y + 15, 230, 34, { fontSize: 24, bold: true, color: COLORS.purple });
  text(slide, body, x + 28, y + 58, 276, 54, { fontSize: 18, color: COLORS.muted });
}

async function slideCover(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = COLORS.darker;
  await image(slide, screenshotPath(screenshots.bossHero), 0, 0, W, H, "Boss combat hero crop", "cover");
  rectangle(slide, 0, 0, 520, H, "#211B33");
  text(slide, "能工智人：糖芯工坊", 58, 100, 420, 116, { fontSize: 48, bold: true, color: COLORS.white });
  text(slide, "Craftsmen and Homo Sapiens:\nThe Candy Forge", 60, 232, 400, 78, {
    fontSize: 22,
    bold: true,
    color: "#DDF4FF",
  });
  text(slide, "叙事类游戏 / Narrative Games", 62, 336, 350, 32, { fontSize: 22, bold: true, color: "#FFB4D1" });
  text(slide, "理工男穿越成异世界女仆工程师，用糖果材料搓科技，打败 Boss 找到回家的路。", 62, 398, 382, 104, {
    fontSize: 22,
    color: COLORS.white,
  });
  text(slide, "Team：落云宗    成员：秦天 / 陈磊", 62, 586, 390, 26, { fontSize: 18, color: "#FFF4E4" });
  text(slide, "Demo Link：待回填", 62, 616, 260, 24, { fontSize: 16, color: "#FFF4E4" });
}

async function slideWorldSetting(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = COLORS.paper;
  text(slide, "世界观：糖芯工坊", 64, 54, 430, 54, { fontSize: 42, bold: true });
  text(
    slide,
    "理工男穿越成异世界女仆工程师，用工程学、番剧储备和糖果材料搓科技，打败 Boss 找到回家的路。",
    66,
    112,
    890,
    34,
    { fontSize: 21, color: COLORS.muted },
  );
  await image(slide, screenshotPath(screenshots.move), 58, 172, 596, 360, "Candy side-scroller world screenshot", "cover");
  await image(slide, screenshotPath(screenshots.npc), 82, 454, 354, 154, "Candy forge dialogue proof", "cover");
  panel(slide, 700, 172, 454, 420, "#FFFFFF", "#FF80B8", 2, "rounded-2xl");
  settingCard(
    slide,
    "穿越身份",
    "现实世界的理工男大学生意外穿越，被糖芯系统误绑定为“见习女仆工程师”。",
    760,
    204,
    COLORS.pink,
  );
  settingCard(
    slide,
    "糖果规则",
    "甜点不是食物，而是能源、武器、材料和技能。怪物掉落材料可合成装备与道具。",
    760,
    334,
    COLORS.blue,
  );
  settingCard(
    slide,
    "回家目标",
    "修复糖芯传送装置，击败污染糖芯炉的糖蚀巫师，重新打开回到现实世界的传送门。",
    760,
    464,
    COLORS.candyGreen,
  );
  footer(slide, 2);
}

async function slideGame(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = COLORS.paper;
  await image(slide, screenshotPath(screenshots.move), 54, 96, 700, 480, "Move jump attack screenshot", "cover");
  text(slide, "游戏是什么", 820, 110, 320, 58, { fontSize: 44, bold: true });
  text(slide, "玩家在糖果异世界中推进房间，一边移动、跳跃和战斗，一边读取糖芯工坊日志寻找回家的线索。", 822, 184, 340, 104, {
    fontSize: 22,
    color: COLORS.muted,
  });
  bullet(slide, "横版动作", 824, 330, COLORS.pink);
  bullet(slide, "房间推进", 824, 392, COLORS.blue);
  bullet(slide, "AI 记忆日志", 824, 454, COLORS.purple);
  footer(slide, 3);
}

async function slideNarrative(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = "#F6F2FF";
  text(slide, "AI 叙事进入游戏", 68, 56, 500, 56, { fontSize: 42, bold: true });
  text(slide, "真实 NPC 对话截图说明：AI 生成内容进入 UI 与流程，而不是只停留在文档里。", 70, 116, 780, 34, {
    fontSize: 20,
    color: COLORS.muted,
  });
  await image(slide, screenshotPath(screenshots.npc), 64, 178, 760, 430, "NPC dialogue screenshot", "cover");
  panel(slide, 876, 178, 328, 430, "#FFFFFF", "#FF80B8", 2, "rounded-2xl");
  text(slide, "AI 生成内容", 910, 218, 250, 36, { fontSize: 28, bold: true, color: COLORS.purple });
  bullet(slide, "糖果异世界", 910, 292, COLORS.pink);
  bullet(slide, "糖芯工坊日志", 910, 354, COLORS.blue);
  bullet(slide, "Boss 背景", 910, 416, COLORS.purple);
  bullet(slide, "结局文本", 910, 478, COLORS.pink);
  footer(slide, 4);
}

async function slideBackpack(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = COLORS.paper;
  text(slide, "背包、装备与成长", 70, 64, 500, 56, { fontSize: 42, bold: true });
  await image(slide, screenshotPath(screenshots.backpack), 64, 148, 720, 430, "Backpack screenshot", "cover");
  text(slide, "系统在 Demo 中承担“变强”的可见反馈。", 840, 156, 340, 64, {
    fontSize: 24,
    bold: true,
    color: COLORS.dark,
  });
  bullet(slide, "装备说明", 842, 260, COLORS.pink);
  bullet(slide, "角色属性", 842, 322, COLORS.blue);
  bullet(slide, "道具与材料", 842, 384, COLORS.purple);
  bullet(slide, "战斗数值成长", 842, 446, COLORS.pink);
  footer(slide, 5);
}

async function slideCraftSkill(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = "#FFF4E4";
  text(slide, "合成与技能树", 70, 52, 430, 54, { fontSize: 42, bold: true });
  text(slide, "材料掉落 -> 合成道具 -> 学习技能 -> 强化战斗", 72, 112, 740, 32, {
    fontSize: 22,
    bold: true,
    color: COLORS.purple,
  });
  await image(slide, screenshotPath(screenshots.craft), 66, 174, 520, 370, "Crafting screenshot", "cover");
  text(slide, "Crafting", 224, 562, 180, 28, { fontSize: 24, bold: true, color: COLORS.purple });
  text(slide, "→", 610, 330, 50, 50, { fontSize: 42, bold: true, color: COLORS.pink });
  await image(slide, screenshotPath(screenshots.skill), 690, 174, 520, 370, "Skill tree screenshot", "cover");
  text(slide, "Skill Tree", 850, 562, 180, 28, { fontSize: 24, bold: true, color: COLORS.purple });
  footer(slide, 6);
}

async function slideTimeline(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = COLORS.paper;
  text(slide, "Demo 体验流程", 64, 48, 430, 54, { fontSize: 42, bold: true });
  text(slide, "3–5 分钟展示完整游戏闭环：穿越开场、NPC 对话、战斗、成长、合成、Boss 战与结局。", 66, 108, 860, 32, {
    fontSize: 21,
    color: COLORS.muted,
  });
  const shots = [
    ["0:00 移动", screenshots.move],
    ["0:30 对话", screenshots.npc],
    ["1:30 背包", screenshots.backpack],
    ["2:00 合成", screenshots.craft],
    ["2:30 技能", screenshots.skill],
    ["3:30 Boss", screenshots.boss],
  ];
  for (let index = 0; index < shots.length; index += 1) {
    const [label, fileName] = shots[index];
    const col = index % 3;
    const row = Math.floor(index / 3);
    const x = 66 + col * 398;
    const y = 170 + row * 226;
    await image(slide, screenshotPath(fileName), x, y, 348, 174, label, "cover");
    text(slide, label, x, y + 184, 220, 26, { fontSize: 19, bold: true, color: COLORS.dark });
  }
  footer(slide, 7);
}

async function slideTech(presentation) {
  const slide = presentation.slides.add();
  slide.background.fill = "#F6F2FF";
  text(slide, "技术结构与部署准备", 68, 56, 520, 54, { fontSize: 40, bold: true });
  await image(slide, visualPath("tech_architecture.png"), 60, 150, 600, 420, "Tech architecture", "contain");
  await image(slide, screenshotPath(screenshots.boss), 716, 158, 470, 270, "Boss combat screenshot", "cover");
  text(slide, "Unity 2D 模块", 730, 468, 260, 30, { fontSize: 26, bold: true, color: COLORS.purple });
  text(slide, "Player / Combat / Enemy / Rooms / UI / Dialogue / Craft / Skill", 730, 510, 430, 54, {
    fontSize: 20,
    color: COLORS.muted,
  });
  text(slide, "WebGL Build 可部署到 Render、Cloudflare Pages 或 GitHub Pages。", 730, 582, 420, 42, {
    fontSize: 18,
    color: COLORS.dark,
  });
  footer(slide, 8);
}

async function writeTextArtifacts() {
  const slideRows = slides
    .map((slide, index) => `| ${index + 1} | ${slide.title} | ${slide.layout} | ${slide.assets.join(", ")} | ${slide.purpose} |`)
    .join("\n");

  await fs.writeFile(
    layoutPlanPath,
    `# Submission Layout Plan V3\n\n` +
      `V3 的核心原则是以真实游戏截图为主视觉，并把世界观统一为“糖芯工坊 / Candy Forge”。\n\n` +
      `## Poster Layout\n\n` +
      `- 主视觉：使用 \`Submission/clean_screenshots/06_boss_combat_hero_crop.png\` 作为大背景。\n` +
      `- 叙事证明：使用 \`02_npc_dialogue_clean.png\` 做“糖芯工坊日志 / NPC 对话”区域。\n` +
      `- 底部截图条：使用 01、02、03、04、05、06 六张 clean 图。\n` +
      `- 标题区：左上深色遮罩，避免文字压在复杂截图上。\n` +
      `- Team：落云宗；成员：秦天 / 陈磊。\n` +
      `- Demo Link：保留待回填位置，不伪造链接。\n\n` +
      `## PPT Layouts\n\n` +
      `| # | Page | Layout | Screenshots / Assets | Purpose |\n| - | - | - | - | - |\n${slideRows}\n\n` +
      `## Screenshot Usage\n\n` +
      `- 01：基础操作 / Slide 2 / Slide 3 / Slide 7。\n` +
      `- 02：NPC 对话与 AI 叙事 / Slide 2 / Slide 4 / Slide 7 / Poster。\n` +
      `- 03：背包与成长 / Slide 5 / Slide 7。\n` +
      `- 04：合成系统 / Slide 6 / Slide 7。\n` +
      `- 05：技能树 / Slide 6 / Slide 7。\n` +
      `- 06：Boss 战 / Poster / Slide 1 / Slide 7 / Slide 8。\n\n` +
      `## Anti-Overlap Rules\n\n` +
      `- 文字不直接压在复杂截图上；封面使用深色侧栏。\n` +
      `- 每页保留大图，最多 4 个短文本块。\n` +
      `- 图片全部等比例缩放或裁剪，不拉伸。\n` +
      `- 装饰减少到最低，不再堆随机圆点和多层框。\n`,
    "utf8",
  );

  await fs.writeFile(
    deckNotesPath,
    `# Project Deck Notes V5\n\n` +
      `本版 PPT 使用 6 张真实游戏截图，并新增“世界观：糖芯工坊”页面，统一为可爱糖果异世界叙事。\n\n` +
      `## Slide Layouts\n\n` +
      `| # | Page | Layout | Screenshot / Asset | Purpose |\n| - | - | - | - | - |\n${slideRows}\n\n` +
      `## Export Notes\n\n` +
      `- PPTX: \`Submission/project_deck.pptx\`\n` +
      `- PDF: \`Submission/project_deck.pdf\`\n` +
      `- Slide previews: \`Submission/project_deck_assets/slide-01.png\` 到 \`slide-08.png\`\n` +
      `- Clean screenshots: \`Submission/clean_screenshots/\`\n\n` +
      `## Manual Check\n\n` +
      `- Team 已填写为：落云宗；成员：秦天 / 陈磊。\n` +
      `- 最终答辩前请检查 Demo Link 和 Video Link 是否已经回填。\n`,
    "utf8",
  );

  await fs.writeFile(
    deckMarkdownPath,
    `# 能工智人：糖芯工坊 - Project Deck V5\n\n` +
      `## 1. 封面\n真实 Boss 战主视觉，展示游戏高潮。\n\n` +
      `## 2. 世界观：糖芯工坊\n理工男穿越成异世界女仆工程师，用糖果材料搓科技回家。\n\n` +
      `## 3. 游戏是什么\n基础移动、跳跃、攻击和横版场景。\n\n` +
      `## 4. AI 叙事进入游戏\nNPC 对话截图证明 AI 叙事进入 UI 与流程。\n\n` +
      `## 5. 背包、装备与成长\n展示背包、装备说明和角色属性。\n\n` +
      `## 6. 合成与技能树\n展示材料、合成、技能学习和战斗成长的连接。\n\n` +
      `## 7. Demo 体验流程\n六张真实截图组成 3-5 分钟体验时间轴。\n\n` +
      `## 8. 技术结构与部署准备\nUnity 2D 模块和 WebGL 静态部署准备。\n`,
    "utf8",
  );

  await fs.writeFile(
    speakerNotesPath,
    `# PPT Speaker Notes V5\n\n` +
      `目标时长：3 到 5 分钟。\n\n` +
      `## Slide 1\n用 Boss 战真实画面开场：这是一个 AI 叙事驱动的 2D 横版动作冒险原型，玩家在糖果异世界中挑战污染糖芯炉的最终敌人。\n\n` +
      `## Slide 2\n讲世界观：主角洛辰原本是现实世界的理工男大学生，穿越后被系统误绑定为见习女仆工程师。他要用工程知识、番剧储备和糖果材料修复回家装置。\n\n` +
      `## Slide 3\n说明基础玩法：移动、跳跃、攻击和房间推进已经形成可录屏的横版动作体验。\n\n` +
      `## Slide 4\n强调赛题契合度：AI 生成的世界观、糖芯工坊日志、Boss 背景和结局文本通过 NPC 对话进入游戏流程，而不是只写在文档里。\n\n` +
      `## Slide 5\n说明成长反馈：玩家可以通过背包看到装备、道具、属性和战斗数值变化。\n\n` +
      `## Slide 6\n说明系统深度：材料掉落后可以合成道具，技能树提供进一步成长，最后服务于 Boss 战表现。\n\n` +
      `## Slide 7\n按时间轴讲 Demo 体验：开场移动、NPC 对话、背包、合成、技能树、Boss 战，Victory 画面由最终录屏补拍。\n\n` +
      `## Slide 8\n说明工程结构：Unity 2D 模块拆分清晰，WebGL 构建和静态部署文档已经准备。结尾补一句团队是落云宗，成员为秦天和陈磊。\n`,
    "utf8",
  );
}

async function verifySources() {
  const required = Object.values(screenshots);
  for (const fileName of required) {
    await fs.access(screenshotPath(fileName));
  }
  await fs.access(visualPath("tech_architecture.png"));
}

async function main() {
  await verifySources();
  await fs.mkdir(outputDir, { recursive: true });
  await fs.rm(previewDir, { recursive: true, force: true });
  await fs.mkdir(previewDir, { recursive: true });

  const presentation = Presentation.create({ slideSize: { width: W, height: H } });
  await slideCover(presentation);
  await slideWorldSetting(presentation);
  await slideGame(presentation);
  await slideNarrative(presentation);
  await slideBackpack(presentation);
  await slideCraftSkill(presentation);
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
  await writeTextArtifacts();
  console.log(`Generated PPTX: ${finalPptx}`);
}

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
