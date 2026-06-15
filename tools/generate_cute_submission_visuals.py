"""Generate cute-style poster and PDF deck preview for hackathon submission.

The generated visuals use original pastel vector shapes only. No third-party
game screenshots or Asset Store images are embedded.
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
    ("能工智人：遗忘工坊", "AI 叙事驱动的横版动作冒险", ["叙事类游戏 / Narrative Games", "地下工坊、记忆日志、可爱风横版战斗", "Demo Link / Team：待回填"]),
    ("创意背景", "一座会说话的遗忘工坊", ["玩家进入地下工坊遗迹", "通过房间记忆日志拼合文明冲突", "可爱画面包裹一点点神秘感"]),
    ("核心循环", "走、跳、打、成长、读日志", ["NPC 引导进入房间", "近战战斗与房间推进", "经验成长后挑战 Boss"]),
    ("AI 叙事亮点", "AI 生成内容进入游戏体验", ["世界观与势力设定", "房间记忆日志与 Boss 背景", "结局文本和展示材料由 AI 辅助整理"]),
    ("技术结构", "Unity 2D + 模块化脚本", ["Player / Combat / Enemy / Rooms", "Dialogue / Craft / Inventory / Skill Energy", "WebGL 构建与静态部署准备"]),
    ("Demo 流程", "评委 3–5 分钟能看懂", ["出生区：NPC 和操作引导", "中段房：战斗、成长、制作/技力", "Boss 房：最终挑战和结局反馈"]),
    ("提交材料", "先准备，不实际部署", ["海报、PPT、源码包脚本", "Render / Cloudflare / GitHub Pages 文档", "视频和在线试玩链接后续回填"]),
    ("竞争亮点", "小而完整，叙事明确", ["AI 内容不是摆设，是房间日志", "玩法闭环短，适合录屏展示", "材料结构清楚，评委容易打开"]),
    ("后续计划", "继续打磨，不盲目堆系统", ["更多房间日志", "更多敌人和技能内容", "更统一的美术、音效和 UI"]),
    ("谢谢观看", "Craftsmen and Homo Sapiens: The Forgotten Forge", ["Demo Link：待回填", "Video Link：待回填", "Team / Contact：待回填"]),
]


def ensure_dirs() -> None:
    SUBMISSION_DIR.mkdir(parents=True, exist_ok=True)
    (SUBMISSION_DIR / "project_deck_assets").mkdir(parents=True, exist_ok=True)
    SPEAKER_NOTES.parent.mkdir(parents=True, exist_ok=True)


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont:
    path = FONT_BOLD if bold and FONT_BOLD.exists() else FONT_REGULAR
    return ImageFont.truetype(str(path), size=size) if path.exists() else ImageFont.load_default()


def star_points(x: int, y: int, radius: int) -> list[tuple[float, float]]:
    points = []
    for index in range(10):
        angle = math.pi / 5 * index - math.pi / 2
        current_radius = radius if index % 2 == 0 else radius * 0.45
        points.append((x + math.cos(angle) * current_radius, y + math.sin(angle) * current_radius))
    return points


def wrapped(draw: ImageDraw.ImageDraw, text: str, x: int, y: int, max_width: int, text_font: ImageFont.FreeTypeFont, fill: tuple[int, int, int, int]) -> int:
    current = ""
    lines: list[str] = []
    for char in text:
        test = current + char
        if draw.textbbox((0, 0), test, font=text_font)[2] <= max_width:
            current = test
        else:
            lines.append(current)
            current = char
    if current:
        lines.append(current)

    for line in lines:
        draw.text((x, y), line, font=text_font, fill=fill)
        y += text_font.size + 12
    return y


def create_poster_png() -> None:
    width, height = 1920, 1080
    image = Image.new("RGBA", (width, height), (255, 246, 251, 255))
    draw = ImageDraw.Draw(image, "RGBA")

    for y in range(height):
        tone = int(250 - y * 24 / height)
        draw.line((0, y, width, y), fill=(255, tone, 252, 255))

    for x in range(0, width, 96):
        draw.line((x, 0, x, height), fill=(255, 179, 220, 44), width=1)
    for y in range(0, height, 72):
        draw.line((0, y, width, y), fill=(137, 205, 255, 38), width=1)

    for index in range(42):
        x = (index * 151 + 74) % width
        y = (index * 97 + 55) % 760
        color = (255, 210, 98, 135) if index % 2 else (198, 183, 255, 145)
        draw.polygon(star_points(x, y, 10 + index % 6), fill=color)

    draw.rounded_rectangle((0, 790, width, 1120), radius=0, fill=(236, 224, 255, 255))
    draw.rounded_rectangle((0, 842, width, 1120), radius=0, fill=(255, 219, 236, 255))
    draw.rounded_rectangle((0, 900, width, 1120), radius=0, fill=(195, 235, 228, 255))
    for x in range(-120, width, 245):
        draw.rounded_rectangle((x, 805, x + 180, 885), radius=34, fill=(255, 255, 255, 128), outline=(255, 148, 207, 120), width=2)

    draw.rounded_rectangle((110, 88, 1145, 878), radius=46, fill=(255, 255, 255, 238), outline=(255, 145, 206, 220), width=5)
    draw.rounded_rectangle((132, 110, 1123, 856), radius=36, outline=(132, 206, 255, 150), width=2)

    core = (1390, 510)
    for radius in range(270, 80, -20):
        alpha = int((radius - 80) / 190 * 45)
        draw.ellipse((core[0] - radius, core[1] - radius, core[0] + radius, core[1] + radius), outline=(255, 126, 191, alpha), width=4)
    draw.ellipse((core[0] - 145, core[1] - 145, core[0] + 145, core[1] + 145), fill=(255, 255, 255, 230), outline=(255, 145, 206, 238), width=6)
    draw.rounded_rectangle((core[0] - 118, core[1] - 38, core[0] + 118, core[1] + 122), radius=50, fill=(201, 242, 250, 245), outline=(137, 100, 255, 220), width=4)
    draw.ellipse((core[0] - 54, core[1] - 76, core[0] + 54, core[1] + 32), fill=(255, 126, 191, 245), outline=(255, 255, 255, 235), width=4)
    draw.polygon([(core[0] - 52, core[1] - 15), (core[0], core[1] + 68), (core[0] + 52, core[1] - 15)], fill=(255, 126, 191, 245))

    title_font = font(86, True)
    subtitle_font = font(34, True)
    body_font = font(31)
    label_font = font(24, True)
    small_font = font(25)

    draw.text((155, 135), "能工智人：遗忘工坊", font=title_font, fill=(78, 56, 118, 255))
    draw.text((158, 245), "Craftsmen and Homo Sapiens: The Forgotten Forge", font=subtitle_font, fill=(59, 157, 206, 255))
    draw.text((158, 315), "叙事类游戏 / Narrative Games", font=label_font, fill=(236, 82, 150, 255))
    wrapped(draw, "AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。", 158, 385, 890, body_font, (96, 82, 128, 255))

    highlights = [
        ("AI 世界观与剧情生成", "房间档案、Boss 背景、结局文本以“工坊记忆日志”进入游戏。"),
        ("横版动作战斗与房间推进", "移动、跳跃、近战、成长、Boss 战组成短而完整的 Demo 闭环。"),
        ("记忆日志驱动的叙事体验", "玩家边战斗边读取档案，逐步拼合工匠文明与智人的冲突真相。"),
    ]
    y = 520
    for index, (heading, body) in enumerate(highlights, start=1):
        panel_fill = (255, 237, 247, 246) if index % 2 else (232, 247, 255, 246)
        draw.rounded_rectangle((155, y - 10, 1040, y + 92), radius=24, fill=panel_fill, outline=(255, 145, 206, 150), width=2)
        draw.ellipse((178, y + 16, 226, y + 64), fill=(255, 126, 191, 230), outline=(255, 255, 255, 245), width=3)
        draw.text((193, y + 18), str(index), font=label_font, fill=(255, 255, 255, 255))
        draw.text((250, y), heading, font=label_font, fill=(78, 56, 118, 255))
        wrapped(draw, body, 250, y + 35, 745, small_font, (103, 93, 134, 255))
        y += 118

    draw.rounded_rectangle((1200, 778, 1810, 1002), radius=34, fill=(255, 255, 255, 235), outline=(137, 100, 255, 180), width=3)
    draw.text((1235, 815), "AI-assisted modules", font=label_font, fill=(236, 82, 150, 255))
    for index, item in enumerate(["Worldbuilding", "Room memory logs", "Boss lore", "Unity workflow"]):
        item_x = 1235 + (index % 2) * 280
        item_y = 865 + (index // 2) * 54
        draw.rounded_rectangle((item_x, item_y, item_x + 240, item_y + 36), radius=14, fill=(232, 247, 255, 255), outline=(132, 206, 255, 170), width=1)
        draw.text((item_x + 18, item_y + 5), item, font=small_font, fill=(78, 56, 118, 255))

    draw.text((155, 930), "Team：待回填", font=small_font, fill=(236, 82, 150, 255))
    draw.text((430, 930), "Playable Demo：待回填", font=small_font, fill=(236, 82, 150, 255))
    draw.text((155, 982), "Original cute poster generated for hackathon submission. No copyrighted third-party image used.", font=font(22), fill=(118, 111, 145, 255))

    image.convert("RGB").save(POSTER_PNG, optimize=True)


def create_svg_and_html() -> None:
    svg = """<svg xmlns="http://www.w3.org/2000/svg" width="1920" height="1080" viewBox="0 0 1920 1080">
<rect width="1920" height="1080" fill="#fff6fb"/>
<rect y="790" width="1920" height="290" fill="#ece0ff"/>
<rect y="842" width="1920" height="238" fill="#ffdbec"/>
<rect y="900" width="1920" height="180" fill="#c3ebe4"/>
<rect x="110" y="88" width="1035" height="790" rx="46" fill="#fff" stroke="#ff91ce" stroke-width="5"/>
<rect x="1200" y="778" width="610" height="224" rx="34" fill="#fff" stroke="#8964ff" stroke-width="3"/>
<circle cx="1390" cy="510" r="145" fill="#fff" stroke="#ff91ce" stroke-width="6"/>
<rect x="1272" y="472" width="236" height="160" rx="50" fill="#c9f2fa" stroke="#8964ff" stroke-width="4"/>
<circle cx="1390" cy="434" r="54" fill="#ff7ebf" stroke="#fff" stroke-width="4"/>
<polygon points="1338,495 1390,575 1442,495" fill="#ff7ebf"/>
<text x="155" y="220" font-size="86" font-weight="700" fill="#4e3876" font-family="Microsoft YaHei, Arial">能工智人：遗忘工坊</text>
<text x="158" y="290" font-size="34" font-weight="700" fill="#3b9dce" font-family="Microsoft YaHei, Arial">Craftsmen and Homo Sapiens: The Forgotten Forge</text>
<text x="158" y="355" font-size="24" font-weight="700" fill="#ec5296" font-family="Microsoft YaHei, Arial">叙事类游戏 / Narrative Games</text>
<text x="158" y="435" font-size="31" fill="#605280" font-family="Microsoft YaHei, Arial">AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。</text>
<g font-family="Microsoft YaHei, Arial">
<rect x="155" y="510" width="885" height="102" rx="24" fill="#ffedf7" stroke="#ff91ce"/>
<text x="250" y="548" font-size="24" font-weight="700" fill="#4e3876">AI 世界观与剧情生成</text>
<text x="250" y="584" font-size="24" fill="#675d86">房间档案、Boss 背景、结局文本以“工坊记忆日志”进入游戏。</text>
<rect x="155" y="628" width="885" height="102" rx="24" fill="#e8f7ff" stroke="#84ceff"/>
<text x="250" y="666" font-size="24" font-weight="700" fill="#4e3876">横版动作战斗与房间推进</text>
<text x="250" y="702" font-size="24" fill="#675d86">移动、跳跃、近战、成长、Boss 战组成短而完整的 Demo 闭环。</text>
<rect x="155" y="746" width="885" height="102" rx="24" fill="#ffedf7" stroke="#ff91ce"/>
<text x="250" y="784" font-size="24" font-weight="700" fill="#4e3876">记忆日志驱动的叙事体验</text>
<text x="250" y="820" font-size="24" fill="#675d86">玩家边战斗边读取档案，逐步拼合文明冲突真相。</text>
<text x="1235" y="830" font-size="24" font-weight="700" fill="#ec5296">AI-assisted modules</text>
<text x="1235" y="888" font-size="24" fill="#4e3876">Worldbuilding        Room memory logs</text>
<text x="1235" y="945" font-size="24" fill="#4e3876">Boss lore             Unity workflow</text>
<text x="155" y="955" font-size="26" font-weight="700" fill="#ec5296">Team：待回填      Playable Demo：待回填</text>
<text x="155" y="1005" font-size="22" fill="#766f91">Original cute poster generated for hackathon submission. No copyrighted third-party image used.</text>
</g>
</svg>"""
    POSTER_SVG.write_text(svg, encoding="utf-8")
    POSTER_HTML.write_text(f"<!doctype html><meta charset='utf-8'><body style='margin:0;background:#fff6fb'>{svg}</body>", encoding="utf-8")


def register_font() -> str:
    if FONT_REGULAR.exists():
        pdfmetrics.registerFont(TTFont("MSYH", str(FONT_REGULAR)))
        return "MSYH"
    return "Helvetica"


def create_pdf() -> None:
    font_name = register_font()
    pdf = canvas.Canvas(str(DECK_PDF), pagesize=landscape((960, 540)))
    for index, (title, subtitle, bullets) in enumerate(SLIDES, start=1):
        pdf.setFillColor(colors.HexColor("#FFF6FB"))
        pdf.rect(0, 0, 960, 540, fill=1, stroke=0)
        pdf.setFillColor(colors.white)
        pdf.roundRect(42, 40, 876, 458, 22, fill=1, stroke=0)
        pdf.setStrokeColor(colors.HexColor("#FF91CE"))
        pdf.setLineWidth(2)
        pdf.roundRect(42, 40, 876, 458, 22, fill=0, stroke=1)
        pdf.setFillColor(colors.HexColor("#E8F7FF"))
        pdf.roundRect(685, 80, 168, 168, 24, fill=1, stroke=0)
        pdf.setFillColor(colors.HexColor("#FF7EBF"))
        pdf.circle(806, 124, 48, fill=1, stroke=0)
        pdf.setFillColor(colors.HexColor("#64E6FF"))
        pdf.circle(806, 124, 22, fill=1, stroke=0)

        pdf.setFont(font_name, 13)
        pdf.setFillColor(colors.HexColor("#EC5296"))
        pdf.drawString(72, 462, f"{index:02d} / 10  Narrative Games")
        pdf.setFont(font_name, 34)
        pdf.setFillColor(colors.HexColor("#4E3876"))
        pdf.drawString(72, 402, title)
        pdf.setFont(font_name, 18)
        pdf.setFillColor(colors.HexColor("#3B9DCE"))
        pdf.drawString(72, 366, subtitle)

        y = 300
        pdf.setFont(font_name, 15)
        for bullet in bullets:
            pdf.setFillColor(colors.HexColor("#FF7EBF"))
            pdf.circle(86, y + 4, 4, fill=1, stroke=0)
            pdf.setFillColor(colors.HexColor("#675D86"))
            for line in textwrap.wrap(bullet, width=48):
                pdf.drawString(104, y, line)
                y -= 24
            y -= 12

        pdf.setFont(font_name, 9)
        pdf.setFillColor(colors.HexColor("#766F91"))
        pdf.drawString(72, 72, "Craftsmen and Homo Sapiens: The Forgotten Forge | Cute hackathon pitch deck")
        pdf.showPage()
    pdf.save()


def create_notes() -> None:
    POSTER_NOTES.write_text(
        """# Poster Notes

- File: `Submission/poster_1920x1080.png`
- Source: `Submission/poster_source.svg` and `Submission/poster_source.html`
- Aspect ratio: 16:9
- Target size: 1920 x 1080
- Visual direction: cute pastel game poster, rounded panels, pink/blue/purple accents, glowing workshop heart.
- Copyright note: original vector/raster shapes only; no third-party game image is embedded.

Run:

```powershell
python tools/generate_cute_submission_visuals.py
```
""",
        encoding="utf-8",
    )

    lines = ["# Project Deck Source\n"]
    for index, (title, subtitle, bullets) in enumerate(SLIDES, start=1):
        lines.append(f"## {index}. {title}\n")
        lines.append(f"**{subtitle}**\n")
        lines.extend([f"- {bullet}" for bullet in bullets])
        lines.append("")
    DECK_MD.write_text("\n".join(lines), encoding="utf-8")

    DECK_NOTES.write_text(
        """# Project Deck Notes

- Editable PPTX target: `Submission/project_deck.pptx`
- PDF preview target: `Submission/project_deck.pdf`
- Source outline: `Submission/project_deck.md`
- Style: cute pastel hackathon game pitch deck.
- Note: the downloaded PPT template could not be imported by the available artifact-tool renderer because it contains unsupported image/canvas content, so this deck rebuilds the style direction with editable objects.
""",
        encoding="utf-8",
    )

    SPEAKER_NOTES.write_text(
        """# PPT Speaker Notes

大家好，我们的作品是《能工智人：遗忘工坊》，赛道是叙事类游戏。它是一款 AI 叙事驱动的 2D 横版动作游戏原型，玩家进入一座地下工坊遗迹，一边战斗推进，一边读取“工坊记忆日志”，慢慢拼合工匠文明与智人文明冲突的真相。

我们这次把展示重点放在短而完整的 Demo 闭环上：出生区 NPC 引导，中段房间战斗与成长，最后进入 Boss 房完成挑战。评委在 3 到 5 分钟内可以看到移动、跳跃、攻击、成长、Boss 战和结局文本。

AI 的作用不只是帮我们写说明，而是参与世界观、角色势力、房间日志、Boss 背景和结局文本的生成。这些内容会放进游戏 UI，成为玩家推进时实际看到的叙事材料。

技术上，项目使用 Unity 2D，拆成 Player、Combat、Enemy、Rooms、UI、Dialogue、Crafting 和 WebGL 构建准备等模块。本次也准备了 WebGL 构建菜单、静态站点整理脚本、部署文档、海报、PPT 和报名表文案。

当前任务没有实际部署，也没有录制最终视频。接下来只需要人工构建 WebGL、部署到静态站点、录制 Demo 视频，并把外部链接回填到提交表单即可。
""",
        encoding="utf-8",
    )


def main() -> None:
    ensure_dirs()
    create_poster_png()
    create_svg_and_html()
    create_pdf()
    create_notes()
    print(f"Generated cute poster: {POSTER_PNG}")
    print(f"Generated cute PDF preview: {DECK_PDF}")


if __name__ == "__main__":
    main()
