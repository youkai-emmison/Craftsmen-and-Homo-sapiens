"""Generate the submission poster from cleaned real gameplay screenshots."""

from __future__ import annotations

import base64
from pathlib import Path

from PIL import Image, ImageDraw, ImageEnhance, ImageFont, ImageOps


ROOT = Path(__file__).resolve().parents[1]
SUBMISSION_DIR = ROOT / "Submission"
CLEAN_DIR = SUBMISSION_DIR / "clean_screenshots"
POSTER_PNG = SUBMISSION_DIR / "poster_1920x1080.png"
POSTER_SVG = SUBMISSION_DIR / "poster_source.svg"
POSTER_HTML = SUBMISSION_DIR / "poster_source.html"
POSTER_NOTES = SUBMISSION_DIR / "poster_notes.md"

FONT_REGULAR = Path("C:/Windows/Fonts/msyh.ttc")
FONT_BOLD = Path("C:/Windows/Fonts/msyhbd.ttc")

PINK = (255, 128, 184, 255)
BLUE = (116, 205, 255, 255)
PURPLE = (99, 72, 147, 255)
DARK = (38, 31, 58, 255)
WHITE = (255, 255, 255, 255)
CREAM = (255, 246, 232, 255)


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont:
    path = FONT_BOLD if bold and FONT_BOLD.exists() else FONT_REGULAR
    if path.exists():
        return ImageFont.truetype(str(path), size=size)
    return ImageFont.load_default()


def load_rgb(path: Path) -> Image.Image:
    return Image.open(path).convert("RGB")


def rounded_mask(size: tuple[int, int], radius: int) -> Image.Image:
    mask = Image.new("L", size, 0)
    draw = ImageDraw.Draw(mask)
    draw.rounded_rectangle((0, 0, size[0], size[1]), radius=radius, fill=255)
    return mask


def paste_cover(canvas: Image.Image, image: Image.Image, box: tuple[int, int, int, int], radius: int = 0) -> None:
    left, top, right, bottom = box
    fitted = ImageOps.fit(image, (right - left, bottom - top), method=Image.Resampling.LANCZOS)
    if radius:
        canvas.paste(fitted, (left, top), rounded_mask(fitted.size, radius))
    else:
        canvas.paste(fitted, (left, top))


def draw_wrapped_text(
    draw: ImageDraw.ImageDraw,
    text: str,
    xy: tuple[int, int],
    text_font: ImageFont.FreeTypeFont,
    fill: tuple[int, int, int, int],
    max_width: int,
    line_gap: int = 9,
) -> int:
    x, y = xy
    line = ""
    lines: list[str] = []
    for char in text:
        candidate = line + char
        width = draw.textbbox((0, 0), candidate, font=text_font)[2]
        if width <= max_width:
            line = candidate
        else:
            if line:
                lines.append(line)
            line = char
    if line:
        lines.append(line)

    for line in lines:
        draw.text((x, y), line, font=text_font, fill=fill)
        y += text_font.size + line_gap
    return y


def add_shadow_panel(draw: ImageDraw.ImageDraw, box: tuple[int, int, int, int], radius: int = 34) -> None:
    left, top, right, bottom = box
    draw.rounded_rectangle((left + 10, top + 12, right + 10, bottom + 12), radius=radius, fill=(29, 20, 44, 132))
    draw.rounded_rectangle(box, radius=radius, fill=(39, 31, 61, 232), outline=(255, 255, 255, 72), width=2)


def require_clean_screenshots() -> None:
    required = [
        "01_move_jump_attack_clean.png",
        "02_npc_dialogue_clean.png",
        "03_backpack_clean.png",
        "04_crafting_clean.png",
        "05_skilltree_clean.png",
        "06_boss_combat_clean.png",
        "06_boss_combat_hero_crop.png",
    ]
    missing = [name for name in required if not (CLEAN_DIR / name).exists()]
    if missing:
        raise FileNotFoundError(f"Missing cleaned screenshots: {', '.join(missing)}")


def create_poster() -> None:
    require_clean_screenshots()

    background = load_rgb(CLEAN_DIR / "06_boss_combat_hero_crop.png")
    background = ImageEnhance.Brightness(background).enhance(0.78)
    background = ImageEnhance.Contrast(background).enhance(1.08)
    poster = ImageOps.fit(background, (1920, 1080), method=Image.Resampling.LANCZOS)
    overlay = Image.new("RGBA", poster.size, (37, 26, 52, 74))
    poster = Image.alpha_composite(poster.convert("RGBA"), overlay)
    draw = ImageDraw.Draw(poster, "RGBA")

    # A single readable title panel replaces the old many-box poster layout.
    add_shadow_panel(draw, (74, 68, 830, 354), 36)
    draw.text((116, 106), "能工智人：遗忘工坊", font=font(72, True), fill=WHITE)
    draw.text((120, 190), "Craftsmen and Homo Sapiens", font=font(34, True), fill=(222, 245, 255, 255))
    draw.text((120, 234), "The Forgotten Forge", font=font(34, True), fill=(255, 221, 239, 255))
    draw.text((120, 292), "叙事类游戏 / Narrative Games", font=font(31, True), fill=(255, 178, 210, 255))

    pitch = "AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。"
    draw_wrapped_text(draw, pitch, (98, 392), font(36, True), WHITE, 760, 10)

    add_shadow_panel(draw, (92, 548, 738, 806), 32)
    draw.text((128, 586), "工坊记忆日志", font=font(42, True), fill=(255, 221, 239, 255))
    draw_wrapped_text(
        draw,
        "AI 生成世界观、房间日志、Boss 背景与结局文本，并通过 NPC 对话进入游戏流程。",
        (128, 650),
        font(28),
        (239, 247, 255, 255),
        520,
        10,
    )
    paste_cover(poster, load_rgb(CLEAN_DIR / "02_npc_dialogue_clean.png"), (758, 544, 1222, 806), radius=26)
    draw.rounded_rectangle((758, 544, 1222, 806), radius=26, outline=(116, 205, 255, 210), width=3)

    strip = [
        ("移动/战斗", "01_move_jump_attack_clean.png"),
        ("NPC 对话", "02_npc_dialogue_clean.png"),
        ("背包装备", "03_backpack_clean.png"),
        ("合成系统", "04_crafting_clean.png"),
        ("技能成长", "05_skilltree_clean.png"),
        ("Boss 战", "06_boss_combat_clean.png"),
    ]
    start_x = 82
    y = 858
    card_w = 278
    card_h = 144
    gap = 22
    for index, (label, file_name) in enumerate(strip):
        left = start_x + index * (card_w + gap)
        paste_cover(poster, load_rgb(CLEAN_DIR / file_name), (left, y, left + card_w, y + card_h), radius=18)
        draw.rounded_rectangle((left, y, left + card_w, y + card_h), radius=18, outline=(255, 255, 255, 150), width=2)
        draw.rounded_rectangle((left, y + card_h - 36, left + card_w, y + card_h), radius=18, fill=(39, 31, 61, 204))
        draw.text((left + 18, y + card_h - 31), label, font=font(22, True), fill=WHITE)

    draw.rounded_rectangle((1252, 550, 1812, 760), radius=30, fill=(255, 248, 253, 232))
    draw.text((1288, 590), "AI-assisted modules", font=font(31, True), fill=PURPLE)
    draw.text((1292, 648), "Worldbuilding & Story", font=font(27, True), fill=(56, 47, 84, 255))
    draw.text((1292, 698), "Game Key Art", font=font(27, True), fill=(56, 47, 84, 255))

    draw.rounded_rectangle((1252, 782, 1812, 848), radius=22, fill=(255, 246, 232, 238))
    draw.text((1284, 792), "Team：落云宗", font=font(25, True), fill=DARK)
    draw.text((1284, 820), "成员：秦天 / 陈磊    Demo Link：待回填", font=font(20, True), fill=PURPLE)

    poster.convert("RGB").save(POSTER_PNG, optimize=True, quality=91)


def write_sources() -> None:
    poster_b64 = base64.b64encode(POSTER_PNG.read_bytes()).decode("ascii")
    POSTER_SVG.write_text(
        f"""<svg xmlns="http://www.w3.org/2000/svg" width="1920" height="1080" viewBox="0 0 1920 1080">
<image href="data:image/png;base64,{poster_b64}" x="0" y="0" width="1920" height="1080"/>
<metadata>Screenshot-driven poster generated from Submission/clean_screenshots.</metadata>
</svg>
""",
        encoding="utf-8",
    )
    POSTER_HTML.write_text(
        """<!doctype html>
<html lang="zh-CN">
<head>
  <meta charset="utf-8">
  <title>能工智人：遗忘工坊 Screenshot Poster</title>
  <style>
    body { margin: 0; background: #261f3a; font-family: "Microsoft YaHei", Arial, sans-serif; }
    main { max-width: 1280px; margin: 32px auto; padding: 24px; }
    img { width: 100%; border-radius: 24px; box-shadow: 0 24px 80px rgba(0,0,0,.28); }
    p { color: #fff8fd; font-size: 18px; }
  </style>
</head>
<body>
<main>
  <img src="poster_1920x1080.png" alt="Craftsmen and Homo Sapiens screenshot-driven poster">
  <p>Generated from cleaned real gameplay screenshots under Submission/clean_screenshots.</p>
</main>
</body>
</html>
""",
        encoding="utf-8",
    )


def write_notes() -> None:
    POSTER_NOTES.write_text(
        """# Poster Notes

This screenshot-driven poster replaces the earlier asset-collage style.

## Real Screenshots Used

- `Submission/clean_screenshots/06_boss_combat_hero_crop.png`: main poster background and gameplay climax.
- `Submission/clean_screenshots/02_npc_dialogue_clean.png`: AI narrative / NPC dialogue proof panel.
- `Submission/clean_screenshots/01_move_jump_attack_clean.png`: bottom screenshot strip, movement and combat.
- `Submission/clean_screenshots/03_backpack_clean.png`: bottom screenshot strip, backpack and equipment.
- `Submission/clean_screenshots/04_crafting_clean.png`: bottom screenshot strip, crafting.
- `Submission/clean_screenshots/05_skilltree_clean.png`: bottom screenshot strip, skill growth.
- `Submission/clean_screenshots/06_boss_combat_clean.png`: bottom screenshot strip, Boss combat.

## Cleanup Notes

The source screenshots were cleaned by `tools/clean_submission_screenshots.py`.
Video subtitles, the NVIDIA prompt, and irrelevant margins were removed. No gameplay UI was fabricated.

## Honest Submission Notes

- Team: 落云宗。
- Members: 秦天 / 陈磊。
- Demo Link is still a placeholder until humans fill it in.
- This poster does not claim that deployment, video recording, or CodeBuddy export has already happened.
""",
        encoding="utf-8",
    )


def main() -> None:
    create_poster()
    write_sources()
    write_notes()
    print(f"Generated poster: {POSTER_PNG.relative_to(ROOT).as_posix()}")


if __name__ == "__main__":
    main()
