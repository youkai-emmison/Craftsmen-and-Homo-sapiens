// Generates the editable cute-style hackathon PPTX with @oai/artifact-tool.
// Key variables:
// - finalPptx: output PPTX path for submission.
// - previewDir: rendered slide previews for visual QA.

const fs = require("node:fs/promises");
const path = require("node:path");
const { Presentation, PresentationFile } = require("@oai/artifact-tool");

const root = path.resolve(__dirname, "..");
const outputDir = path.join(root, "Submission");
const previewDir = path.join(outputDir, "project_deck_assets");
const finalPptx = path.join(outputDir, "project_deck.pptx");

const slides = [
  ["能工智人：遗忘工坊", "AI 叙事驱动的横版动作冒险", ["叙事类游戏 / Narrative Games", "地下工坊、记忆日志、可爱风横版战斗", "Demo Link / Team：待回填"]],
  ["创意背景", "一座会说话的遗忘工坊", ["玩家进入地下工坊遗迹", "通过房间记忆日志拼合文明冲突", "可爱画面包裹一点点神秘感"]],
  ["核心循环", "走、跳、打、成长、读日志", ["NPC 引导进入房间", "近战战斗与房间推进", "经验成长后挑战 Boss"]],
  ["AI 叙事亮点", "AI 生成内容进入游戏体验", ["世界观与势力设定", "房间记忆日志与 Boss 背景", "结局文本和展示材料由 AI 辅助整理"]],
  ["技术结构", "Unity 2D + 模块化脚本", ["Player / Combat / Enemy / Rooms", "Dialogue / Craft / Inventory / Skill Energy", "WebGL 构建与静态部署准备"]],
  ["Demo 流程", "评委 3–5 分钟能看懂", ["出生区：NPC 和操作引导", "中段房：战斗、成长、制作/技力", "Boss 房：最终挑战和结局反馈"]],
  ["提交材料", "先准备，不实际部署", ["海报、PPT、源码包脚本", "Render / Cloudflare / GitHub Pages 文档", "视频和在线试玩链接后续回填"]],
  ["竞争亮点", "小而完整，叙事明确", ["AI 内容不是摆设，是房间日志", "玩法闭环短，适合录屏展示", "材料结构清楚，评委容易打开"]],
  ["后续计划", "继续打磨，不盲目堆系统", ["更多房间日志", "更多敌人和技能内容", "更统一的美术、音效和 UI"]],
  ["谢谢观看", "Craftsmen and Homo Sapiens: The Forgotten Forge", ["Demo Link：待回填", "Video Link：待回填", "Team / Contact：待回填"]],
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

function addRound(slide, x, y, w, h, fill, stroke = "#FF91CE", radius = "rounded-xl") {
  return slide.shapes.add({
    geometry: "roundRect",
    position: { left: x, top: y, width: w, height: h },
    fill,
    line: { style: "solid", fill: stroke, width: 1 },
    borderRadius: radius,
  });
}

function addStar(slide, x, y, size, fill) {
  slide.shapes.add({
    geometry: "star5",
    position: { left: x, top: y, width: size, height: size },
    fill,
    line: { style: "solid", fill: "none", width: 0 },
  });
}

function buildSlide(presentation, data, index) {
  const [title, subtitle, bullets] = data;
  const slide = presentation.slides.add();
  slide.background.fill = "#FFF6FB";

  addRound(slide, 40, 38, 1200, 644, "#FFFFFF", "#FF91CE", "rounded-2xl");
  addRound(slide, 68, 545, 1148, 58, "#FFDBEC", "#FFDBEC");
  addRound(slide, 68, 588, 1148, 54, "#C3EBE4", "#C3EBE4");
  addRound(slide, 990, 356, 230, 236, "#E8F7FF", "#84CEFF", "rounded-2xl");

  for (let i = 0; i < 14; i += 1) {
    addStar(slide, 982 + ((i * 47) % 220), 76 + ((i * 61) % 230), 18 + (i % 3) * 4, i % 2 ? "#FFD76A" : "#C7B7FF");
  }

  slide.shapes.add({
    geometry: "ellipse",
    position: { left: 1050, top: 408, width: 122, height: 122 },
    fill: "#FF7EBF",
    line: { style: "solid", fill: "#FFFFFF", width: 3 },
  });
  slide.shapes.add({
    geometry: "ellipse",
    position: { left: 1086, top: 444, width: 50, height: 50 },
    fill: "#64E6FF",
    line: { style: "solid", fill: "#FFFFFF", width: 2 },
  });

  addText(slide, `${String(index).padStart(2, "0")} / 10  Narrative Games`, 72, 64, 330, 28, {
    fontSize: 13,
    bold: true,
    color: "#EC5296",
    fontFace: "Microsoft YaHei",
  });
  addText(slide, title, 72, 132, 820, 78, {
    fontSize: index === 1 ? 46 : 40,
    bold: true,
    color: "#4E3876",
    fontFace: "Microsoft YaHei",
  });
  addText(slide, subtitle, 74, 214, 800, 44, {
    fontSize: 22,
    bold: true,
    color: "#3B9DCE",
    fontFace: "Microsoft YaHei",
  });

  let y = 306;
  bullets.forEach((bullet, bulletIndex) => {
    const fill = bulletIndex % 2 === 0 ? "#FFEDF7" : "#E8F7FF";
    const stroke = bulletIndex % 2 === 0 ? "#FF91CE" : "#84CEFF";
    addRound(slide, 86, y - 8, 824, 64, fill, stroke);
    slide.shapes.add({
      geometry: "ellipse",
      position: { left: 108, top: y + 8, width: 26, height: 26 },
      fill: bulletIndex % 2 === 0 ? "#FF7EBF" : "#64E6FF",
      line: { style: "solid", fill: "#FFFFFF", width: 2 },
    });
    addText(slide, bullet, 152, y, 720, 48, {
      fontSize: 20,
      color: "#675D86",
      fontFace: "Microsoft YaHei",
    });
    y += 80;
  });

  addText(slide, "Craftsmen and Homo Sapiens: The Forgotten Forge", 72, 640, 650, 22, {
    fontSize: 10,
    color: "#766F91",
    fontFace: "Aptos",
  });
}

async function main() {
  await fs.mkdir(outputDir, { recursive: true });
  await fs.mkdir(previewDir, { recursive: true });

  const presentation = Presentation.create({ slideSize: { width: 1280, height: 720 } });
  slides.forEach((slide, index) => buildSlide(presentation, slide, index + 1));

  for (const [index, slide] of presentation.slides.items.entries()) {
    const stem = `slide-${String(index + 1).padStart(2, "0")}`;
    await writeBlob(path.join(previewDir, `${stem}.png`), await presentation.export({ slide, format: "png", scale: 1 }));
    const layout = await slide.export({ format: "layout" });
    await fs.writeFile(path.join(previewDir, `${stem}.layout.json`), await layout.text(), "utf8");
  }
  await writeBlob(path.join(previewDir, "deck-montage.webp"), await presentation.export({ format: "webp", montage: true, scale: 1 }));

  const pptx = await PresentationFile.exportPptx(presentation);
  await pptx.save(finalPptx);
  console.log(`Generated cute PPTX: ${finalPptx}`);
}

main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
