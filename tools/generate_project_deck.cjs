// Generates the editable hackathon PPTX with @oai/artifact-tool.
// The deck uses only editable shapes and text, not full-slide screenshots.

const fs = require("node:fs/promises");
const path = require("node:path");
const { Presentation, PresentationFile } = require("@oai/artifact-tool");

const root = path.resolve(__dirname, "..");
const outputDir = path.join(root, "Submission");
const previewDir = path.join(outputDir, "project_deck_assets");
const finalPptx = path.join(outputDir, "project_deck.pptx");

const slides = [
  {
    title: "能工智人：遗忘工坊",
    subtitle: "Craftsmen and Homo Sapiens: The Forgotten Forge",
    kicker: "叙事类游戏 / Narrative Games",
    bullets: [
      "AI 叙事驱动的横版动作冒险",
      "在地下工坊的记忆日志中揭开文明冲突真相",
      "Team / School / Demo Link：待回填",
    ],
  },
  {
    title: "背景与创意",
    subtitle: "被遗忘工坊中的文明档案",
    bullets: [
      "玩家进入地下工坊遗迹，读取残缺的工坊记忆日志。",
      "故事围绕工匠文明与智人文明的冲突、合作和遗忘展开。",
      "短流程 Demo 表达：探索、战斗、读取档案、接近真相。",
    ],
  },
  {
    title: "核心玩法循环",
    subtitle: "3–5 分钟内能看懂的完整体验",
    bullets: [
      "开场叙事 → 房间探索 → 近战战斗",
      "击败敌人 → 获得经验 / 成长反馈",
      "Boss 战 → 结局文本 / Demo Complete",
    ],
  },
  {
    title: "叙事与 AI 亮点",
    subtitle: "把 AI 变成游戏内的档案记忆",
    bullets: [
      "AI 辅助生成世界观、角色势力和 Boss 背景。",
      "房间记忆日志嵌入 UI，让剧情跟随战斗推进。",
      "Codex/AI 辅助完成 Unity Demo、构建脚本和展示材料。",
    ],
  },
  {
    title: "技术结构",
    subtitle: "Unity 2D 原型，模块拆分清晰",
    bullets: [
      "Player：移动、跳跃、攻击、技力与生命值。",
      "Combat / Enemy / Room：敌人、清房、出口解锁。",
      "UI / Dialogue / Craft：背包、制作、NPC 对话和叙事面板。",
      "WebGL：准备静态部署到 Render / Cloudflare / GitHub Pages。",
    ],
  },
  {
    title: "Demo 流程",
    subtitle: "评委看到的路线",
    bullets: [
      "出生区：NPC 叙事引导和基础操作。",
      "中段房：战斗、经验成长、制作/技力提示。",
      "Boss 房：最终敌人、胜利反馈、结局档案。",
    ],
  },
  {
    title: "部署与提交材料",
    subtitle: "先准备，不在本任务中实际部署",
    bullets: [
      "WebGL 构建输出：Build/WebGL。",
      "提交物料：海报、PPT、源码包、表单文案。",
      "外部链接：Playable Demo / Demo Video / CodeBuddy History 后续回填。",
    ],
  },
  {
    title: "竞争亮点",
    subtitle: "短流程、强叙事、可浏览器体验",
    bullets: [
      "AI 叙事不是说明文字，而是游戏内记忆日志。",
      "玩法闭环短但完整：走、打、成长、Boss、结局。",
      "材料结构清楚，便于评委快速打开和理解。",
    ],
  },
  {
    title: "后续计划",
    subtitle: "不扩系统，先把体验打磨完整",
    bullets: [
      "扩展更多房间档案与分支文本。",
      "增加敌人类型、技能和更完整的 Boss 表现。",
      "统一美术、音效和 UI，增强沉浸感。",
    ],
  },
  {
    title: "谢谢观看",
    subtitle: "Demo Link / Video Link：待回填",
    bullets: [
      "项目名：能工智人：遗忘工坊",
      "赛道：叙事类游戏 / Narrative Games",
      "Team / Contact：待回填",
    ],
  },
];

async function writeBlob(filePath, blob) {
  await fs.writeFile(filePath, new Uint8Array(await blob.arrayBuffer()));
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

function addRect(slide, x, y, w, h, fill, stroke = "#263555") {
  return slide.shapes.add({
    geometry: "roundRect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: stroke, width: 1 },
    borderRadius: "rounded-xl",
  });
}

function buildSlide(presentation, data, index) {
  const slide = presentation.slides.add();
  slide.background.fill = "#070B18";

  // Background frame and forge-core motif.
  addRect(slide, 40, 38, 1200, 644, "#0B1224", "#2FD8FF");
  slide.shapes.add({
    geometry: "ellipse",
    position: { left: 1020, top: 390, width: 160, height: 160 },
    fill: "#3B2C84",
    line: { style: "solid", fill: "#58E6FF", width: 2 },
  });
  slide.shapes.add({
    geometry: "ellipse",
    position: { left: 1070, top: 440, width: 60, height: 60 },
    fill: "#64E6FF",
    line: { style: "solid", fill: "#B8F8FF", width: 1 },
  });

  addText(slide, `${String(index).padStart(2, "0")} / 10  Narrative Games`, 72, 64, 320, 26, {
    fontSize: 13,
    bold: true,
    color: "#BA9DFF",
    fontFace: "Microsoft YaHei",
  });
  addText(slide, data.title, 72, 132, 800, 72, {
    fontSize: index === 1 ? 44 : 38,
    bold: true,
    color: "#F2F9FF",
    fontFace: "Microsoft YaHei",
  });
  addText(slide, data.subtitle, 74, 212, 780, 44, {
    fontSize: 21,
    bold: true,
    color: "#6AE1FF",
    fontFace: "Microsoft YaHei",
  });
  if (data.kicker) {
    addText(slide, data.kicker, 74, 270, 520, 32, {
      fontSize: 17,
      bold: true,
      color: "#BA9DFF",
      fontFace: "Microsoft YaHei",
    });
  }

  let y = data.kicker ? 335 : 305;
  data.bullets.forEach((bullet, bulletIndex) => {
    addRect(slide, 86, y - 8, 824, 58, "#111D38", "#24456A");
    slide.shapes.add({
      geometry: "ellipse",
      position: { left: 108, top: y + 8, width: 24, height: 24 },
      fill: bulletIndex % 2 === 0 ? "#6AE1FF" : "#835DFF",
      line: { style: "solid", fill: "none", width: 0 },
    });
    addText(slide, bullet, 150, y, 720, 46, {
      fontSize: 20,
      color: "#DCE9FF",
      fontFace: "Microsoft YaHei",
    });
    y += 78;
  });

  addText(slide, "Craftsmen and Homo Sapiens: The Forgotten Forge", 72, 640, 620, 22, {
    fontSize: 10,
    color: "#8295B5",
    fontFace: "Aptos",
  });

  return slide;
}

async function main() {
  await fs.mkdir(outputDir, { recursive: true });
  await fs.mkdir(previewDir, { recursive: true });

  const presentation = Presentation.create({
    slideSize: { width: 1280, height: 720 },
  });

  slides.forEach((slide, index) => buildSlide(presentation, slide, index + 1));

  for (const [index, slide] of presentation.slides.items.entries()) {
    const stem = `slide-${String(index + 1).padStart(2, "0")}`;
    await writeBlob(
      path.join(previewDir, `${stem}.png`),
      await presentation.export({ slide, format: "png", scale: 1 }),
    );
    const layout = await slide.export({ format: "layout" });
    await fs.writeFile(path.join(previewDir, `${stem}.layout.json`), await layout.text(), "utf8");
  }

  await writeBlob(
    path.join(previewDir, "deck-montage.webp"),
    await presentation.export({ format: "webp", montage: true, scale: 1 }),
  );

  const pptx = await PresentationFile.exportPptx(presentation);
  await pptx.save(finalPptx);
  console.log(`Generated ${finalPptx}`);
}

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
