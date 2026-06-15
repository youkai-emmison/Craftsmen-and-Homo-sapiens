"""Generate hackathon poster and PDF deck preview assets.

This script creates original vector/raster submission visuals. It does not
download third-party images and does not deploy the project.
"""

from __future__ import annotations

import math
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont
from reportlab.lib import colors
from reportlab.lib.pagesizes import landscape
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.pdfgen import canvas


ROOT = Path(__file__).resolve().parents[1]
SUBMISSION_DIR = ROOT / "Submission"
ASSET_DIR = SUBMISSION_DIR / "project_deck_assets"

POSTER_PNG = SUBMISSION_DIR / "poster_1920x1080.png"
POSTER_SVG = SUBMISSION_DIR / "poster_source.svg"
POSTER_HTML = SUBMISSION_DIR / "poster_source.html"
POSTER_NOTES = SUBMISSION_DIR / "poster_notes.md"
DECK_PDF = SUBMISSION_DIR / "project_deck.pdf"
DECK_MD = SUBMISSION_DIR / "project_deck.md"
DECK_NOTES = SUBMISSION_DIR / "project_deck_notes.md"
SPEAKER_NOTES = ROOT / "docs" / "PPT_SPEAKER_NOTES.md"

FONT_REGULAR = Path("C:/Windows/Fonts/msyh.ttc")
FONT_BOLD = Path("C:/Windows/Fonts/msyhbd.ttc")

SLIDES = [
    {
        "title": "能工智人：遗忘工坊",
        "subtitle": "Craftsmen and Homo Sapiens: The Forgotten Forge",
        "kicker": "叙事类游戏 / Narrative Games",
        "bullets": [
            "AI 叙事驱动的横版动作冒险",
            "在地下工坊的记忆日志中揭开文明冲突真相",
            "Team / School / Demo Link：待回填",
        ],
    },
    {
        "title": "背景与创意",
        "subtitle": "被遗忘工坊中的文明档案",
        "bullets": [
            "玩家进入地下工坊遗迹，读取残缺的工坊记忆日志。",
            "故事围绕工匠文明与智人文明的冲突、合作和遗忘展开。",
            "用短流程 Demo 表达“探索、战斗、读取档案、接近真相”。",
        ],
    },
    {
        "title": "核心玩法循环",
        "subtitle": "3–5 分钟内能看懂的完整体验",
        "bullets": [
            "开场叙事 → 房间探索 → 近战战斗",
            "击败敌人 → 获得经验 / 成长反馈",
            "Boss 战 → 结局文本 / Demo Complete",
        ],
    },
    {
        "title": "叙事与 AI 亮点",
        "subtitle": "把 AI 变成游戏内的档案记忆",
        "bullets": [
            "AI 辅助生成世界观、角色势力和 Boss 背景。",
            "房间记忆日志嵌入 UI，让剧情跟随战斗推进。",
            "Codex/AI 辅助完成 Unity Demo、构建脚本和展示材料。",
        ],
    },
    {
        "title": "技术结构",
        "subtitle": "Unity 2D 原型，模块拆分清晰",
        "bullets": [
            "Player：移动、跳跃、攻击、技力与生命值。",
            "Combat / Enemy / Room：敌人、清房、出口解锁。",
            "UI / Dialogue / Craft：背包、制作、NPC 对话和叙事面板。",
            "WebGL：准备静态部署到 Render / Cloudflare / GitHub Pages。",
        ],
    },
    {
        "title": "Demo 流程",
        "subtitle": "评委看到的路线",
        "bullets": [
            "出生区：NPC 叙事引导和基础操作。",
            "中段房：战斗、经验成长、制作/技力提示。",
            "Boss 房：最终敌人、胜利反馈、结局档案。",
        ],
    },
    {
        "title": "部署与提交材料",
        "subtitle": "先准备，不在本任务中实际部署",
        "bullets": [
            "WebGL 构建输出：Build/WebGL。",
            "提交物料：海报、PPT、源码包、表单文案。",
            "外部链接：Playable Demo / Demo Video / CodeBuddy History 后续回填。",
        ],
    },
    {
        "title": "竞争亮点",
        "subtitle": "短流程、强叙事、可浏览器体验",
        "bullets": [
            "AI 叙事不是说明文字，而是游戏内记忆日志。",
            "玩法闭环短但完整：走、打、成长、Boss、结局。",
            "材料结构清楚，便于评委快速打开和理解。",
        ],
    },
    {
        "title": "后续计划",
        "subtitle": "不扩系统，先把体验打磨完整",
        "bullets": [
            "扩展更多房间档案与分支文本。",
            "增加敌人类型、技能和更完整的 Boss 表现。",
            "统一美术、音效和 UI，增强沉浸感。",
        ],
    },
    {
        "title": "谢谢观看",
        "subtitle": "Demo Link / Video Link：待回填",
        "bullets": [
            "项目名：能工智人：遗忘工坊",
            "赛道：叙事类游戏 / Narrative Games",
            "Team / Contact：待回填",
        ],
    },
]


def ensure_dirs() -> None:
    SUBMISSION_DIR.mkdir(parents=True, exist_ok=True)
    ASSET_DIR.mkdir(parents=True, exist_ok=True)
    SPEAKER_NOTES.parent.mkdir(parents=True, exist_ok=True)


def load_font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont:
    font_path = FONT_BOLD if bold and FONT_BOLD.exists() else FONT_REGULAR
    if font_path.exists():
        return ImageFont.truetype(str(font_path), size=size)
    return ImageFont.load_default()


def draw_wrapped(
    draw: ImageDraw.ImageDraw,
    text: str,
    xy: tuple[int, int],
    font: ImageFont.FreeTypeFont,
    fill: tuple[int, int, int],
    max_width: int,
    line_spacing: int = 10,
) -> int:
    x, y = xy
    current = ""
    lines: list[str] = []
    for char in text:
        test = current + char
        if draw.textbbox((0, 0), test, font=font)[2] <= max_width:
            current = test
        else:
            if current:
                lines.append(current)
            current = char
    if current:
        lines.append(current)

    for line in lines:
        draw.text((x, y), line, font=font, fill=fill)
        y += font.size + line_spacing
    return y


def vertical_gradient(width: int, height: int) -> Image.Image:
    image = Image.new("RGB", (width, height), "#080B16")
    pixels = image.load()
    for y in range(height):
        t = y / max(1, height - 1)
        r = int(7 + 10 * t)
        g = int(10 + 10 * t)
        b = int(23 + 24 * t)
        for x in range(width):
            pixels[x, y] = (r, g, b)
    return image


def add_glow(draw: ImageDraw.ImageDraw, center: tuple[int, int], radius: int, color: tuple[int, int, int]) -> None:
    cx, cy = center
    for step in range(18, 0, -1):
        alpha_scale = step / 18
        r = int(radius * alpha_scale)
        outline = tuple(int(c * alpha_scale + 10 * (1 - alpha_scale)) for c in color)
        draw.ellipse((cx - r, cy - r, cx + r, cy + r), outline=outline, width=2)


def create_poster_png() -> None:
    width, height = 1920, 1080
    image = vertical_gradient(width, height).convert("RGBA")
    draw = ImageDraw.Draw(image, "RGBA")

    # Background grid and workshop silhouettes.
    for x in range(0, width, 96):
        draw.line((x, 0, x, height), fill=(29, 62, 88, 46), width=1)
    for y in range(0, height, 72):
        draw.line((0, y, width, y), fill=(29, 62, 88, 36), width=1)
    for x in range(-200, width, 260):
        draw.polygon([(x, 820), (x + 120, 610), (x + 280, 820)], fill=(15, 18, 31, 210))
    draw.rectangle((0, 830, width, height), fill=(5, 8, 15, 220))
    draw.rectangle((0, 788, width, 830), fill=(22, 40, 59, 190))

    # Forge core.
    core = (1375, 520)
    add_glow(draw, core, 315, (66, 220, 255))
    draw.ellipse((core[0] - 106, core[1] - 106, core[0] + 106, core[1] + 106), fill=(12, 42, 67, 238), outline=(78, 229, 255, 230), width=5)
    draw.ellipse((core[0] - 52, core[1] - 52, core[0] + 52, core[1] + 52), fill=(120, 77, 255, 230), outline=(176, 242, 255, 240), width=3)
    for angle in range(0, 360, 45):
        x1 = core[0] + int(math.cos(math.radians(angle)) * 128)
        y1 = core[1] + int(math.sin(math.radians(angle)) * 128)
        x2 = core[0] + int(math.cos(math.radians(angle)) * 230)
        y2 = core[1] + int(math.sin(math.radians(angle)) * 230)
        draw.line((x1, y1, x2, y2), fill=(80, 228, 255, 140), width=4)

    # Panels.
    draw.rounded_rectangle((110, 90, 1140, 870), radius=34, fill=(11, 18, 34, 205), outline=(54, 221, 255, 130), width=2)
    draw.rounded_rectangle((1190, 780, 1810, 1000), radius=26, fill=(15, 24, 45, 218), outline=(127, 103, 255, 130), width=2)

    title_font = load_font(86, bold=True)
    sub_font = load_font(34, bold=True)
    body_font = load_font(31)
    small_font = load_font(26)
    label_font = load_font(24, bold=True)

    draw.text((155, 135), "能工智人：遗忘工坊", font=title_font, fill=(242, 249, 255, 255))
    draw.text((158, 245), "Craftsmen and Homo Sapiens: The Forgotten Forge", font=sub_font, fill=(106, 225, 255, 255))
    draw.text((158, 315), "叙事类游戏 / Narrative Games", font=label_font, fill=(186, 157, 255, 255))

    pitch = "AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。"
    draw_wrapped(draw, pitch, (158, 385), body_font, (224, 234, 255, 255), 900, 14)

    highlights = [
        ("AI 世界观与剧情生成", "房间档案、Boss 背景、结局文本以“工坊记忆日志”进入游戏。"),
        ("横版动作战斗与房间推进", "移动、跳跃、近战、成长、Boss 战组成短而完整的 Demo 闭环。"),
        ("记忆日志驱动的叙事体验", "玩家边战斗边读取档案，逐步拼合工匠文明与智人的冲突真相。"),
    ]
    y = 520
    for idx, (head, text) in enumerate(highlights, start=1):
        draw.rounded_rectangle((155, y - 10, 1038, y + 88), radius=18, fill=(22, 34, 58, 190), outline=(74, 205, 255, 90), width=1)
        draw.ellipse((178, y + 16, 224, y + 62), fill=(117, 86, 255, 210), outline=(108, 229, 255, 220), width=2)
        draw.text((192, y + 18), str(idx), font=label_font, fill=(255, 255, 255, 255))
        draw.text((248, y), head, font=label_font, fill=(255, 255, 255, 255))
        draw_wrapped(draw, text, (248, y + 34), small_font, (187, 205, 230, 255), 740, 8)
        y += 118

    draw.text((1235, 815), "AI-assisted modules", font=label_font, fill=(108, 229, 255, 255))
    ai_items = ["Worldbuilding", "Room memory logs", "Boss lore", "Unity workflow"]
    for index, item in enumerate(ai_items):
        item_x = 1235 + (index % 2) * 280
        item_y = 865 + (index // 2) * 54
        draw.rounded_rectangle((item_x, item_y, item_x + 240, item_y + 36), radius=10, fill=(34, 45, 76, 220))
        draw.text((item_x + 18, item_y + 5), item, font=small_font, fill=(222, 232, 255, 255))

    draw.text((155, 930), "Team：待回填", font=small_font, fill=(186, 157, 255, 255))
    draw.text((430, 930), "Playable Demo：待回填", font=small_font, fill=(186, 157, 255, 255))
    draw.text((155, 982), "Original poster generated for hackathon submission. No copyrighted third-party image used.", font=load_font(22), fill=(134, 158, 188, 255))

    image = image.convert("RGB")
    image.save(POSTER_PNG, optimize=True)


def svg_text(x: int, y: int, text: str, size: int, fill: str, weight: int = 400) -> str:
    return f'<text x="{x}" y="{y}" font-size="{size}" font-weight="{weight}" fill="{fill}" font-family="Microsoft YaHei, Arial, sans-serif">{text}</text>'


def create_poster_svg() -> None:
    svg = f"""<svg xmlns="http://www.w3.org/2000/svg" width="1920" height="1080" viewBox="0 0 1920 1080">
  <defs>
    <linearGradient id="bg" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0%" stop-color="#070b18"/>
      <stop offset="100%" stop-color="#11182e"/>
    </linearGradient>
    <radialGradient id="coreGlow" cx="70%" cy="48%" r="40%">
      <stop offset="0%" stop-color="#6ff3ff" stop-opacity="0.85"/>
      <stop offset="42%" stop-color="#7357ff" stop-opacity="0.22"/>
      <stop offset="100%" stop-color="#050813" stop-opacity="0"/>
    </radialGradient>
    <filter id="softGlow"><feGaussianBlur stdDeviation="12"/></filter>
  </defs>
  <rect width="1920" height="1080" fill="url(#bg)"/>
  <rect width="1920" height="1080" fill="url(#coreGlow)"/>
  <g opacity="0.18" stroke="#4ee6ff" stroke-width="1">
    <path d="M0 144H1920 M0 288H1920 M0 432H1920 M0 576H1920 M0 720H1920 M0 864H1920"/>
    <path d="M96 0V1080 M288 0V1080 M480 0V1080 M672 0V1080 M864 0V1080 M1056 0V1080 M1248 0V1080 M1440 0V1080 M1632 0V1080 M1824 0V1080"/>
  </g>
  <g fill="#050812" opacity="0.85">
    <polygon points="-80,830 80,600 260,830"/>
    <polygon points="260,830 430,640 620,830"/>
    <polygon points="1500,830 1635,620 1800,830"/>
  </g>
  <rect x="0" y="830" width="1920" height="250" fill="#050812" opacity="0.92"/>
  <rect x="0" y="788" width="1920" height="42" fill="#16283b" opacity="0.78"/>
  <rect x="110" y="90" width="1030" height="780" rx="34" fill="#0b1222" opacity="0.86" stroke="#42ddff" stroke-opacity="0.55" stroke-width="2"/>
  <circle cx="1375" cy="520" r="106" fill="#0c2a43" stroke="#4ee6ff" stroke-width="5"/>
  <circle cx="1375" cy="520" r="52" fill="#784dff" stroke="#b5f8ff" stroke-width="3"/>
  <g stroke="#56e8ff" stroke-width="4" stroke-opacity="0.62">
    <path d="M1503 520H1605 M1247 520H1145 M1375 648V750 M1375 392V290"/>
    <path d="M1465 610L1538 683 M1285 430L1212 357 M1465 430L1538 357 M1285 610L1212 683"/>
  </g>
  {svg_text(155, 220, "能工智人：遗忘工坊", 86, "#f2f9ff", 700)}
  {svg_text(158, 290, "Craftsmen and Homo Sapiens: The Forgotten Forge", 34, "#6ae1ff", 700)}
  {svg_text(158, 355, "叙事类游戏 / Narrative Games", 24, "#ba9dff", 700)}
  {svg_text(158, 435, "AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。", 31, "#e0eaff", 400)}
  <g>
    <rect x="155" y="510" width="883" height="98" rx="18" fill="#16223a" opacity="0.82" stroke="#4acfff" stroke-opacity="0.35"/>
    {svg_text(248, 548, "AI 世界观与剧情生成", 24, "#ffffff", 700)}
    {svg_text(248, 584, "房间档案、Boss 背景、结局文本以“工坊记忆日志”进入游戏。", 24, "#bbcee6", 400)}
    <rect x="155" y="628" width="883" height="98" rx="18" fill="#16223a" opacity="0.82" stroke="#4acfff" stroke-opacity="0.35"/>
    {svg_text(248, 666, "横版动作战斗与房间推进", 24, "#ffffff", 700)}
    {svg_text(248, 702, "移动、跳跃、近战、成长、Boss 战组成短而完整的 Demo 闭环。", 24, "#bbcee6", 400)}
    <rect x="155" y="746" width="883" height="98" rx="18" fill="#16223a" opacity="0.82" stroke="#4acfff" stroke-opacity="0.35"/>
    {svg_text(248, 784, "记忆日志驱动的叙事体验", 24, "#ffffff", 700)}
    {svg_text(248, 820, "玩家边战斗边读取档案，逐步拼合文明冲突真相。", 24, "#bbcee6", 400)}
  </g>
  <rect x="1190" y="780" width="620" height="220" rx="26" fill="#0f182d" opacity="0.88" stroke="#7f67ff" stroke-opacity="0.55"/>
  {svg_text(1235, 830, "AI-assisted modules", 24, "#6ce5ff", 700)}
  {svg_text(1235, 888, "Worldbuilding        Room memory logs", 24, "#dee8ff", 400)}
  {svg_text(1235, 945, "Boss lore             Unity workflow", 24, "#dee8ff", 400)}
  {svg_text(155, 955, "Team：待回填      Playable Demo：待回填", 26, "#ba9dff", 700)}
  {svg_text(155, 1005, "Original poster generated for hackathon submission. No copyrighted third-party image used.", 22, "#869ebc", 400)}
</svg>
"""
    POSTER_SVG.write_text(svg, encoding="utf-8")
    POSTER_HTML.write_text(
        """<!doctype html>
<html lang="zh-CN">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>能工智人：遗忘工坊 Poster Source</title>
  <style>body{margin:0;background:#050813}svg{width:100vw;height:auto;display:block}</style>
</head>
<body>
"""
        + svg
        + """
</body>
</html>
""",
        encoding="utf-8",
    )


def register_pdf_font() -> str:
    for name, path in [("MSYH", FONT_REGULAR), ("SimHei", Path("C:/Windows/Fonts/simhei.ttf"))]:
        if path.exists():
            try:
                pdfmetrics.registerFont(TTFont(name, str(path)))
                return name
            except Exception:
                continue
    return "Helvetica"


def draw_pdf_slide(page: canvas.Canvas, slide: dict[str, object], index: int, font_name: str) -> None:
    width, height = landscape((960, 540))
    page.setFillColor(colors.HexColor("#070B18"))
    page.rect(0, 0, width, height, fill=1, stroke=0)
    page.setFillColor(colors.HexColor("#111A33"))
    page.roundRect(42, 46, 876, 448, 18, fill=1, stroke=0)
    page.setStrokeColor(colors.HexColor("#43DFFF"))
    page.setLineWidth(1.2)
    page.roundRect(42, 46, 876, 448, 18, fill=0, stroke=1)
    page.setFillColor(colors.HexColor("#784DFF"))
    page.circle(805, 120, 50, fill=1, stroke=0)
    page.setFillColor(colors.HexColor("#4EE6FF"))
    page.circle(805, 120, 24, fill=1, stroke=0)

    page.setFont(font_name, 13)
    page.setFillColor(colors.HexColor("#BA9DFF"))
    page.drawString(72, 462, f"{index:02d} / 10  Narrative Games")

    page.setFont(font_name, 34)
    page.setFillColor(colors.white)
    page.drawString(72, 402, str(slide["title"]))

    page.setFont(font_name, 18)
    page.setFillColor(colors.HexColor("#6AE1FF"))
    page.drawString(72, 366, str(slide["subtitle"]))

    page.setFont(font_name, 15)
    page.setFillColor(colors.HexColor("#DCE9FF"))
    y = 300
    for bullet in slide["bullets"]:  # type: ignore[index]
        lines = textwrap.wrap(str(bullet), width=48)
        page.setFillColor(colors.HexColor("#4EE6FF"))
        page.circle(86, y + 4, 4, fill=1, stroke=0)
        page.setFillColor(colors.HexColor("#DCE9FF"))
        for line in lines:
            page.drawString(104, y, line)
            y -= 24
        y -= 12

    page.setFont(font_name, 9)
    page.setFillColor(colors.HexColor("#8295B5"))
    page.drawString(72, 72, "Craftsmen and Homo Sapiens: The Forgotten Forge | AI-assisted hackathon submission deck")


def create_deck_pdf() -> None:
    font_name = register_pdf_font()
    pdf = canvas.Canvas(str(DECK_PDF), pagesize=landscape((960, 540)))
    for index, slide in enumerate(SLIDES, start=1):
        draw_pdf_slide(pdf, slide, index, font_name)
        pdf.showPage()
    pdf.save()


def create_markdown_and_notes() -> None:
    POSTER_NOTES.write_text(
        """# Poster Notes

- File: `Submission/poster_1920x1080.png`
- Source: `Submission/poster_source.svg` and `Submission/poster_source.html`
- Aspect ratio: 16:9
- Target size: 1920 x 1080
- Visual direction: dark underground workshop, cyan/purple forge core, narrative archive panels.
- Copyright note: the poster uses original vector/raster shapes generated for this repository. It does not use third-party game images.

## Export

The PNG was generated from the repository script:

```powershell
python tools/generate_submission_visuals.py
```

If you edit the SVG manually, export at 1920 x 1080 and keep the final PNG under 5 MB.
""",
        encoding="utf-8",
    )

    deck_lines = ["# Project Deck Source\n"]
    for index, slide in enumerate(SLIDES, start=1):
        deck_lines.append(f"## {index}. {slide['title']}\n")
        deck_lines.append(f"**{slide['subtitle']}**\n")
        for bullet in slide["bullets"]:  # type: ignore[index]
            deck_lines.append(f"- {bullet}")
        deck_lines.append("")
    DECK_MD.write_text("\n".join(deck_lines), encoding="utf-8")

    DECK_NOTES.write_text(
        """# Project Deck Notes

- Editable PPTX target: `Submission/project_deck.pptx`
- PDF preview target: `Submission/project_deck.pdf`
- Source outline: `Submission/project_deck.md`
- Style: dark hackathon game pitch deck, cyan/purple glow accents, concise Chinese copy.
- The deck does not claim real deployment, real demo video, or CodeBuddy export completion.
""",
        encoding="utf-8",
    )

    SPEAKER_NOTES.write_text(
        """# PPT Speaker Notes

## 3-5 Minute Script

大家好，我们的作品是《能工智人：遗忘工坊》，赛道是叙事类游戏。它是一个 AI 叙事驱动的 2D 横版动作游戏原型，玩家进入一座地下工坊遗迹，一边战斗推进，一边通过“工坊记忆日志”拼合文明冲突的真相。

开场时，玩家不是直接看一大段设定，而是通过 NPC 和房间档案逐步知道：这里曾经存在工匠文明与智人文明之间的合作与分裂。AI 在本项目中的作用主要体现在世界观、房间日志、角色背景、Boss 设定和结局文本的生成上，这些内容会嵌入游戏 UI，而不是只放在说明文档里。

玩法上，我们把 Demo 压缩成一个短闭环：出生区阅读引导，中段房间进行移动、跳跃、近战、经验成长和制作/技力展示，最后进入 Boss 房完成挑战。评委在 3 到 5 分钟内可以看到完整流程：探索、战斗、成长、Boss、结局。

技术结构方面，项目使用 Unity 2D。脚本按职责拆分为 Player、Combat、Enemy、Rooms、UI、Dialogue、Crafting 和 WebGL 构建准备。我们本次提交已经准备了 WebGL 构建菜单、静态站点整理脚本，以及 Render、Cloudflare Pages、GitHub Pages 的部署说明，但实际部署会在最终链接确认后由队员手动完成。

作品的竞争亮点是：它不是做一个庞大的 Roguelike，而是用短流程把 AI 生成叙事、横版动作战斗和可浏览器试玩的提交体验结合起来。后续如果继续开发，会扩展更多房间日志、敌人类型、分支叙事和美术音效打磨。

最后，当前的 Demo 链接、视频链接和团队信息会在最终提交前回填。谢谢大家。
""",
        encoding="utf-8",
    )


def main() -> None:
    ensure_dirs()
    create_poster_png()
    create_poster_svg()
    create_deck_pdf()
    create_markdown_and_notes()
    print(f"Generated {POSTER_PNG}")
    print(f"Generated {POSTER_SVG}")
    print(f"Generated {DECK_PDF}")


if __name__ == "__main__":
    main()
