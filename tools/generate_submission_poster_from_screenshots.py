"""Generate the submission poster from cleaned real gameplay screenshots."""

from __future__ import annotations

import base64
from pathlib import Path

from PIL import Image, ImageDraw, ImageEnhance, ImageFilter, ImageFont, ImageOps


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


def add_gradient_overlay(canvas: Image.Image) -> Image.Image:
    width, height = canvas.size
    overlay = Image.new("RGBA", canvas.size, (0, 0, 0, 0))
    pixels = overlay.load()
    for y in range(height):
        top_strength = max(0, 1 - y / 520)
        bottom_strength = max(0, (y - 650) / 430)
        for x in range(width):
            left_strength = max(0, 1 - x / 980)
            alpha = int(42 + 150 * left_strength + 70 * top_strength + 95 * bottom_strength)
            pixels[x, y] = (31, 23, 49, min(alpha, 235))
    return Image.alpha_composite(canvas.convert("RGBA"), overlay)


def draw_text_shadow(
    draw: ImageDraw.ImageDraw,
    position: tuple[int, int],
    value: str,
    text_font: ImageFont.FreeTypeFont,
    fill: tuple[int, int, int, int],
    shadow: tuple[int, int, int, int] = (28, 18, 44, 190),
    offset: tuple[int, int] = (4, 5),
) -> None:
    x, y = position
    draw.text((x + offset[0], y + offset[1]), value, font=text_font, fill=shadow)
    draw.text((x, y), value, font=text_font, fill=fill)


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
    background = ImageEnhance.Brightness(background).enhance(0.82)
    background = ImageEnhance.Contrast(background).enhance(1.08)
    poster = ImageOps.fit(background, (1920, 1080), method=Image.Resampling.LANCZOS)
    poster = poster.filter(ImageFilter.GaussianBlur(7))
    poster = add_gradient_overlay(poster)
    draw = ImageDraw.Draw(poster, "RGBA")

    # Poster V3: one clear focal image, strong title hierarchy, very few text groups.
    hero_box = (830, 164, 1798, 738)
    shadow = Image.new("RGBA", poster.size, (0, 0, 0, 0))
    shadow_draw = ImageDraw.Draw(shadow, "RGBA")
    shadow_draw.rounded_rectangle((hero_box[0] + 22, hero_box[1] + 24, hero_box[2] + 22, hero_box[3] + 24), radius=38, fill=(24, 15, 36, 180))
    shadow = shadow.filter(ImageFilter.GaussianBlur(16))
    poster = Image.alpha_composite(poster, shadow)
    draw = ImageDraw.Draw(poster, "RGBA")
    paste_cover(poster, load_rgb(CLEAN_DIR / "06_boss_combat_clean.png"), hero_box, radius=34)
    draw.rounded_rectangle(hero_box, radius=34, outline=(255, 255, 255, 170), width=3)
    draw.rounded_rectangle((hero_box[0], hero_box[3] - 72, hero_box[2], hero_box[3]), radius=34, fill=(33, 24, 52, 205))
    draw.text((hero_box[0] + 34, hero_box[3] - 54), "Boss 战实机画面", font=font(30, True), fill=WHITE)
    draw.text((hero_box[2] - 318, hero_box[3] - 52), "Demo Link：待回填", font=font(24, True), fill=(255, 221, 239, 255))

    draw_text_shadow(draw, (92, 94), "能工智人：", font(86, True), WHITE)
    draw_text_shadow(draw, (92, 188), "糖芯工坊", font(96, True), WHITE)
    draw.text((98, 304), "Craftsmen and Homo Sapiens:", font=font(30, True), fill=(229, 246, 255, 255))
    draw.text((98, 340), "The Candy Forge", font=font(32, True), fill=(229, 246, 255, 255))
    draw.rounded_rectangle((100, 402, 520, 456), radius=24, fill=(255, 128, 184, 230))
    draw.text((128, 413), "叙事类游戏 / Narrative Games", font=font(24, True), fill=(42, 31, 66, 255))

    pitch = "理工男穿越成异世界女仆工程师，用糖果材料搓科技，打败 Boss 找到回家的路。"
    draw_wrapped_text(draw, pitch, (100, 486), font(40, True), WHITE, 660, 14)

    tags = [("AI 叙事", PINK), ("合成成长", BLUE), ("Boss 战", (126, 227, 180, 255))]
    tag_x = 102
    for label, color in tags:
        text_width = draw.textbbox((0, 0), label, font=font(25, True))[2]
        draw.rounded_rectangle((tag_x, 686, tag_x + text_width + 44, 740), radius=24, fill=color)
        draw.text((tag_x + 22, 696), label, font=font(25, True), fill=(42, 31, 66, 255))
        tag_x += text_width + 68

    draw.line((102, 812, 684, 812), fill=(255, 221, 239, 180), width=3)
    draw.text((102, 842), "AI modules：Worldbuilding & Story / Game Key Art", font=font(23, True), fill=(232, 244, 255, 230))
    draw.text((102, 890), "Team：落云宗  |  秦天 / 陈磊", font=font(28, True), fill=(255, 246, 232, 255))

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
  <title>能工智人：糖芯工坊 Screenshot Poster</title>
  <style>
    body { margin: 0; background: #261f3a; font-family: "Microsoft YaHei", Arial, sans-serif; }
    main { max-width: 1280px; margin: 32px auto; padding: 24px; }
    img { width: 100%; border-radius: 24px; box-shadow: 0 24px 80px rgba(0,0,0,.28); }
    p { color: #fff8fd; font-size: 18px; }
  </style>
</head>
<body>
<main>
  <img src="poster_1920x1080.png" alt="Craftsmen and Homo Sapiens: The Candy Forge screenshot-driven poster">
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

This screenshot-driven poster replaces the earlier asset-collage style and uses the Candy Forge world setting.

## Real Screenshots Used

- `Submission/clean_screenshots/06_boss_combat_hero_crop.png`: main poster background and gameplay climax.
- `Submission/clean_screenshots/06_boss_combat_clean.png`: the single primary gameplay screenshot.

## Cleanup Notes

The source screenshots were cleaned by `tools/clean_submission_screenshots.py`.
Video subtitles, the NVIDIA prompt, and irrelevant margins were removed. No gameplay UI was fabricated.

## Design Notes

The poster follows a single-focal-point layout: one large gameplay image, a strong title block, three short feature tags, and minimal footer information. Earlier multi-screenshot strips were removed because they made the poster feel crowded.

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
