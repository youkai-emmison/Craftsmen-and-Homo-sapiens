// Generate the editable hackathon PPTX with real game visual assets.
// Key variables:
// - finalPptx: submission PPTX path.
// - previewDir: rendered PNG previews for visual QA.

const fs = require("node:fs/promises");
const path = require("node:path");
const { Presentation, PresentationFile } = require("@oai/artifact-tool");

const root = path.resolve(__dirname, "..");
const outputDir = path.join(root, "Submission");
const previewDir = path.join(outputDir, "project_deck_assets");
const finalPptx = path.join(outputDir, "project_deck.pptx");
const visualDir = path.join(outputDir, "visual_assets");

const slides = [
  {
    title: "能工智人：遗忘工坊",
    subtitle: "AI 叙事驱动的横版动作冒险",
    image: "character_cards.png",
    bullets: ["叙事类游戏 / Narrative Games", "可爱工坊美术 + 地下遗迹探索", "Demo Link / Team：待回填"],
    footer: "Cover",
  },
  {
    title: "游戏是什么",
    subtitle: "玩家进入一座会留下记忆的地下工坊",
    image: "player_lineup.png",
    bullets: ["横版动作：移动、跳跃、近战攻击", "房间推进：清敌、解锁门、进入下一段", "AI 记忆日志：战斗中逐步补全世界观"],
    footer: "Player / Loop",
  },
  {
    title: "核心玩法循环",
    subtitle: "短流程，但闭环完整",
    image: "asset_contact_sheet.png",
    bullets: ["进入房间 -> 读记忆日志", "战斗 -> 获得经验 / 道具 / 制作材料", "解锁下一房间 -> Boss -> 结局文本"],
    footer: "Gameplay Loop",
  },
  {
    title: "角色与怪物",
    subtitle: "真实 spritesheet 支撑 Demo 表现",
    image: "enemy_lineup.png",
    bullets: ["主角：猫耳女仆探索者", "NPC：档案员与技术员引导玩法", "敌人：普通怪、精英怪、Boss 占位体"],
    footer: "Cast",
  },
  {
    title: "AI 叙事如何进入游戏",
    subtitle: "AI 内容会变成工坊记忆日志和 NPC 对话",
    image: "npc_lineup.png",
    bullets: ["AI 生成世界观、势力关系、房间档案", "NPC 对话把操作提示包装进剧情", "Boss 背景与结局文本用于演示闭环"],
    footer: "Narrative AI",
  },
  {
    title: "Demo 录屏路线",
    subtitle: "截图框保持诚实：等待 Unity 实机画面回填",
    image: "screenshot_placeholders.png",
    bullets: ["开场 + NPC 对话", "早期战斗 + 成长反馈", "Boss 房 + Victory / Demo Complete"],
    footer: "Recording Plan",
  },
  {
    title: "技术结构",
    subtitle: "Unity 2D 原型，模块保持轻量拆分",
    image: "asset_contact_sheet.png",
    bullets: ["Player / Combat / Enemy / Rooms", "Dialogue / Craft / Inventory / Skill Energy", "WebGL Build + 静态部署准备"],
    footer: "Tech",
  },
  {
    title: "提交状态与下一步",
    subtitle: "材料可编辑，真实截图与在线链接等待人工回填",
    image: "character_cards.png",
    bullets: ["已准备：海报、PPT、部署文档、提交文案", "待人工：WebGL 部署、Demo 视频、CodeBuddy 导出", "下一步：用截图工具补齐真实游戏画面"],
    footer: "Submission",
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
  box.text.style = style;
  return box;
}

function addPanel(slide, x, y, w, h, fill, stroke = "#FF80B8", radius = "rounded-xl") {
  return slide.shapes.add({
    geometry: "roundRect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: stroke, width: 2 },
    borderRadius: radius,
  });
}

function addAccentCircle(slide, x, y, size, fill) {
  slide.shapes.add({
    geometry: "ellipse",
    position: { left: x, top: y, width: size, height: size },
    fill,
    line: { style: "solid", fill: "none", width: 0 },
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
    geometry: "roundRect",
    borderRadius: "rounded-xl",
    position: { left: x, top: y, width: w, height: h },
  });
}

async function buildSlide(presentation, data, index) {
  const slide = presentation.slides.add();
  slide.background.fill = "#FFF8FD";

  addAccentCircle(slide, -38, 44, 90, "#74CDFF");
  addAccentCircle(slide, 1128, -36, 132, "#FF80B8");
  addAccentCircle(slide, 1184, 596, 96, "#C9F0E5");

  addPanel(slide, 34, 30, 1212, 660, "#FFFFFF", "#FF80B8", "rounded-2xl");
  addPanel(slide, 62, 60, 430, 116, "#FFF4E4", "#FFD36B", "rounded-xl");
  addText(slide, `${String(index).padStart(2, "0")} / 08`, 84, 80, 110, 26, {
    fontSize: 15,
    bold: true,
    color: "#FF80B8",
    fontFace: "Aptos",
  });
  addText(slide, data.footer, 84, 112, 360, 38, {
    fontSize: 25,
    bold: true,
    color: "#322853",
    fontFace: "Microsoft YaHei",
  });

  addText(slide, data.title, 70, 208, 520, 62, {
    fontSize: index === 1 ? 43 : 38,
    bold: true,
    color: "#322853",
    fontFace: "Microsoft YaHei",
  });
  addText(slide, data.subtitle, 72, 274, 520, 52, {
    fontSize: 21,
    bold: true,
    color: "#695F84",
    fontFace: "Microsoft YaHei",
  });

  let y = 366;
  data.bullets.forEach((bullet, bulletIndex) => {
    const fill = bulletIndex % 2 === 0 ? "#FFDDEF" : "#DEF5FF";
    addPanel(slide, 72, y - 8, 500, 58, fill, bulletIndex % 2 === 0 ? "#FF80B8" : "#74CDFF");
    addAccentCircle(slide, 94, y + 8, 24, bulletIndex % 2 === 0 ? "#FF80B8" : "#74CDFF");
    addText(slide, bullet, 132, y, 400, 42, {
      fontSize: 19,
      color: "#322853",
      fontFace: "Microsoft YaHei",
    });
    y += 76;
  });

  addPanel(slide, 630, 88, 560, 460, "#F9F2FF", "#74CDFF", "rounded-2xl");
  await addImage(slide, data.image, 650, 108, 520, 420, data.title, "contain");

  addPanel(slide, 632, 575, 560, 72, "#322853", "#322853", "rounded-xl");
  addText(slide, "真实素材展示 + 等待 Unity 截图补齐", 660, 595, 500, 32, {
    fontSize: 20,
    bold: true,
    color: "#FFFFFF",
    fontFace: "Microsoft YaHei",
  });
  addText(slide, "No fake deployment / no fake demo video / no external screenshots", 662, 625, 500, 20, {
    fontSize: 11,
    color: "#CFEFFF",
    fontFace: "Aptos",
  });

  addText(slide, "Craftsmen and Homo Sapiens: The Forgotten Forge", 72, 650, 540, 22, {
    fontSize: 10,
    color: "#695F84",
    fontFace: "Aptos",
  });
}

async function main() {
  await fs.mkdir(outputDir, { recursive: true });
  await fs.mkdir(previewDir, { recursive: true });

  const presentation = Presentation.create({ slideSize: { width: 1280, height: 720 } });
  for (let index = 0; index < slides.length; index += 1) {
    await buildSlide(presentation, slides[index], index + 1);
  }

  for (const [index, slide] of presentation.slides.items.entries()) {
    const stem = `slide-${String(index + 1).padStart(2, "0")}`;
    await writeBlob(path.join(previewDir, `${stem}.png`), await presentation.export({ slide, format: "png", scale: 1 }));
    const layout = await slide.export({ format: "layout" });
    await fs.writeFile(path.join(previewDir, `${stem}.layout.json`), await layout.text(), "utf8");
  }
  await writeBlob(path.join(previewDir, "deck-montage.webp"), await presentation.export({ format: "webp", montage: true, scale: 1 }));

  const pptx = await PresentationFile.exportPptx(presentation);
  await pptx.save(finalPptx);
  console.log(`Generated PPTX: ${finalPptx}`);
}

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
