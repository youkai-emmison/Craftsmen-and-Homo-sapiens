"""Generate submission visuals from real project game assets.

This script intentionally uses the repository's own generated player, enemy,
NPC, item, and device images instead of abstract placeholder graphics. It also
creates the poster PNG/source files and a PDF preview of the pitch deck.
"""

from __future__ import annotations

import base64
import html
import math
import textwrap
from dataclasses import dataclass
from pathlib import Path
from typing import Iterable

from PIL import Image, ImageDraw, ImageFont
from reportlab.lib import colors
from reportlab.lib.pagesizes import landscape
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.pdfgen import canvas


ROOT = Path(__file__).resolve().parents[1]
ASSET_ROOT = ROOT / "Assets" / "Art"
GENERATED_ROOT = ASSET_ROOT / "Generated"
SUBMISSION_DIR = ROOT / "Submission"
VISUAL_DIR = SUBMISSION_DIR / "visual_assets"
DECK_ASSET_DIR = SUBMISSION_DIR / "project_deck_assets"

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

PLAYER_MAIN = GENERATED_ROOT / "Characters" / "cat_maid_magic_wand_spritesheet.png"
PLAYER_WALK = GENERATED_ROOT / "Characters" / "cat_maid_magic_wand_walk_climb_spritesheet.png"
ENEMY_SHEETS = [
    ("Cream Bun Monster", GENERATED_ROOT / "Enemies" / "cream_bun_monster_spritesheet.png"),
    ("Crystal Fox Spirit", GENERATED_ROOT / "Enemies" / "crystal_fox_spirit_girl_spritesheet.png"),
    ("Ember Bat Familiar", GENERATED_ROOT / "Enemies" / "ember_bat_familiar_girl_spritesheet.png"),
    ("Flower Slime Nymph", GENERATED_ROOT / "Enemies" / "flower_slime_nymph_spritesheet.png"),
]
NPC_FRAMES = [
    ("Archivist Guide", GENERATED_ROOT / "NPC" / "archivist_guide" / "archivist_guide_idle_02.png"),
    ("Field Technician", GENERATED_ROOT / "NPC" / "field_technician" / "field_technician_idle_02.png"),
]
ITEM_ICONS = [
    ("Gearspike Wand", GENERATED_ROOT / "Items" / "Equipment" / "GearspikeWandIcon.png"),
    ("Ground Turret", GENERATED_ROOT / "Items" / "Equipment" / "GroundTurretIcon.png"),
    ("Moonlit Shard", GENERATED_ROOT / "Items" / "Materials" / "MoonlitShardIcon.png"),
    ("Rusty Gear", GENERATED_ROOT / "Items" / "Materials" / "RustyGearIcon.png"),
    ("Aether Jam", GENERATED_ROOT / "Items" / "Materials" / "AetherJamIcon.png"),
]
DEVICE_IMAGES = [
    ("Ground Turret", GENERATED_ROOT / "Devices" / "ground_turret.png"),
    ("Projectile", GENERATED_ROOT / "Devices" / "ground_turret_projectile.png"),
]

COLORS = {
    "paper": (255, 248, 253, 255),
    "cream": (255, 244, 228, 255),
    "pink": (255, 128, 184, 255),
    "pink_soft": (255, 221, 239, 255),
    "blue": (116, 205, 255, 255),
    "blue_soft": (222, 245, 255, 255),
    "purple": (99, 72, 147, 255),
    "purple_dark": (50, 40, 83, 255),
    "ink": (47, 42, 72, 255),
    "muted": (105, 95, 132, 255),
    "mint": (203, 240, 229, 255),
    "amber": (255, 211, 107, 255),
}


@dataclass(frozen=True)
class VisualAsset:
    name: str
    path: Path
    category: str
    suggested_use: str


def ensure_dirs() -> None:
    SUBMISSION_DIR.mkdir(parents=True, exist_ok=True)
    VISUAL_DIR.mkdir(parents=True, exist_ok=True)
    DECK_ASSET_DIR.mkdir(parents=True, exist_ok=True)
    SPEAKER_NOTES.parent.mkdir(parents=True, exist_ok=True)


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont:
    path = FONT_BOLD if bold and FONT_BOLD.exists() else FONT_REGULAR
    if path.exists():
        return ImageFont.truetype(str(path), size=size)
    return ImageFont.load_default()


def load_rgba(path: Path) -> Image.Image:
    return Image.open(path).convert("RGBA")


def trim_alpha(image: Image.Image, padding: int = 8) -> Image.Image:
    if image.mode != "RGBA":
        image = image.convert("RGBA")
    bbox = image.getchannel("A").getbbox()
    if bbox is None:
        return image
    left = max(0, bbox[0] - padding)
    top = max(0, bbox[1] - padding)
    right = min(image.width, bbox[2] + padding)
    bottom = min(image.height, bbox[3] + padding)
    return image.crop((left, top, right, bottom))


def extract_grid(path: Path, columns: int, rows: int, frame_indexes: Iterable[int]) -> list[Image.Image]:
    sheet = load_rgba(path)
    frame_width = sheet.width // columns
    frame_height = sheet.height // rows
    frames: list[Image.Image] = []
    for index in frame_indexes:
        column = index % columns
        row = index // columns
        if row >= rows:
            continue
        box = (
            column * frame_width,
            row * frame_height,
            (column + 1) * frame_width,
            (row + 1) * frame_height,
        )
        frames.append(trim_alpha(sheet.crop(box), padding=4))
    return frames


def fit_image(image: Image.Image, max_width: int, max_height: int, resample: int = Image.Resampling.NEAREST) -> Image.Image:
    if image.width == 0 or image.height == 0:
        return image
    scale = min(max_width / image.width, max_height / image.height)
    width = max(1, int(image.width * scale))
    height = max(1, int(image.height * scale))
    return image.resize((width, height), resample)


def paste_center(canvas_image: Image.Image, image: Image.Image, box: tuple[int, int, int, int]) -> None:
    left, top, right, bottom = box
    max_width = right - left
    max_height = bottom - top
    fitted = fit_image(image, max_width, max_height)
    x = left + (max_width - fitted.width) // 2
    y = top + (max_height - fitted.height) // 2
    canvas_image.alpha_composite(fitted, (x, y))


def draw_text(
    draw: ImageDraw.ImageDraw,
    text: str,
    xy: tuple[int, int],
    text_font: ImageFont.FreeTypeFont,
    fill: tuple[int, int, int, int],
    max_width: int | None = None,
    line_spacing: int = 8,
) -> int:
    x, y = xy
    if max_width is None:
        draw.text((x, y), text, font=text_font, fill=fill)
        return y + text_font.size + line_spacing

    lines: list[str] = []
    current = ""
    for char in text:
        test = current + char
        if draw.textbbox((0, 0), test, font=text_font)[2] <= max_width:
            current = test
        else:
            if current:
                lines.append(current)
            current = char
    if current:
        lines.append(current)

    for line in lines:
        draw.text((x, y), line, font=text_font, fill=fill)
        y += text_font.size + line_spacing
    return y


def rounded_panel(
    draw: ImageDraw.ImageDraw,
    box: tuple[int, int, int, int],
    fill: tuple[int, int, int, int],
    outline: tuple[int, int, int, int] | None = None,
    radius: int = 28,
    width: int = 2,
) -> None:
    draw.rounded_rectangle(box, radius=radius, fill=fill, outline=outline, width=width)


def draw_soft_background(image: Image.Image) -> None:
    draw = ImageDraw.Draw(image, "RGBA")
    width, height = image.size
    for y in range(height):
        t = y / max(1, height - 1)
        r = int(255 * (1 - t) + 221 * t)
        g = int(248 * (1 - t) + 241 * t)
        b = int(253 * (1 - t) + 255 * t)
        draw.line((0, y, width, y), fill=(r, g, b, 255))

    for x in range(0, width, 120):
        draw.line((x, 0, x, height), fill=(255, 159, 206, 30), width=1)
    for y in range(0, height, 90):
        draw.line((0, y, width, y), fill=(116, 205, 255, 28), width=1)

    for index in range(28):
        x = (index * 191 + 80) % width
        y = (index * 113 + 60) % height
        radius = 26 + (index % 5) * 10
        fill = (255, 128, 184, 28) if index % 2 else (116, 205, 255, 26)
        draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=fill)


def create_tile_strip(width: int, height: int) -> Image.Image:
    strip = Image.new("RGBA", (width, height), (0, 0, 0, 0))
    draw = ImageDraw.Draw(strip, "RGBA")
    tile_candidates = [
        ASSET_ROOT / "Tiles" / "floor_tile_1.png",
        ASSET_ROOT / "Tiles" / "floor_tile_2.png",
        ASSET_ROOT / "Tiles" / "floor_tile_3.png",
        ASSET_ROOT / "Tiles" / "brick_1.png",
    ]
    tiles = [load_rgba(path) for path in tile_candidates if path.exists()]
    if not tiles:
        draw.rounded_rectangle((0, 0, width, height), radius=10, fill=(143, 103, 132, 255))
        return strip

    scaled_tiles = [tile.resize((48, 48), Image.Resampling.NEAREST) for tile in tiles]
    for y in range(0, height, 48):
        for x in range(0, width, 48):
            strip.alpha_composite(scaled_tiles[(x // 48 + y // 48) % len(scaled_tiles)], (x, y))
    draw.rectangle((0, 0, width, 6), fill=(255, 228, 170, 190))
    return strip


def create_player_lineup() -> Path:
    frames = extract_grid(PLAYER_MAIN, 4, 4, [0, 1, 4, 5, 8, 9, 12, 13])
    walk_frames = extract_grid(PLAYER_WALK, 7, 2, [0, 1, 2, 7, 8, 9])
    frames.extend(walk_frames[:4])

    output = VISUAL_DIR / "player_lineup.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")
    rounded_panel(draw, (70, 70, 1530, 830), (255, 255, 255, 224), COLORS["pink"], 42, 4)
    draw_text(draw, "Player Action Lineup", (112, 106), font(54, True), COLORS["purple_dark"])
    draw_text(draw, "Cat maid explorer: idle, attack, hurt, walk and climb frames from the real spritesheets.", (116, 176), font(25), COLORS["muted"], 1180)

    labels = ["Idle", "Idle", "Attack", "Attack", "Hurt", "Hurt", "Walk", "Walk", "Climb", "Climb", "Move", "Move"]
    start_x = 105
    start_y = 290
    cell_w = 230
    cell_h = 220
    for index, frame in enumerate(frames[:12]):
        column = index % 6
        row = index // 6
        left = start_x + column * 238
        top = start_y + row * 255
        rounded_panel(draw, (left, top, left + cell_w, top + cell_h), COLORS["blue_soft"], (146, 214, 255, 190), 24, 2)
        paste_center(image, frame, (left + 20, top + 18, left + cell_w - 20, top + cell_h - 50))
        draw_text(draw, labels[index], (left + 26, top + cell_h - 42), font(22, True), COLORS["purple"])

    image.convert("RGB").save(output, optimize=True)
    return output


def create_enemy_lineup() -> Path:
    output = VISUAL_DIR / "enemy_lineup.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")
    rounded_panel(draw, (70, 70, 1530, 830), (255, 255, 255, 226), COLORS["blue"], 42, 4)
    draw_text(draw, "Enemy Lineup", (112, 106), font(56, True), COLORS["purple_dark"])
    draw_text(draw, "Four prototype enemies used for dungeon combat, boss placeholders, and visual variety.", (116, 178), font(26), COLORS["muted"], 1200)

    card_w = 330
    card_h = 500
    for index, (name, path) in enumerate(ENEMY_SHEETS):
        frame = extract_grid(path, 4, 4, [0, 5, 8, 12])[index % 4]
        left = 116 + index * 365
        top = 290
        rounded_panel(draw, (left, top, left + card_w, top + card_h), (255, 244, 250, 248), COLORS["pink"], 32, 3)
        paste_center(image, frame, (left + 36, top + 42, left + card_w - 36, top + 325))
        draw_text(draw, name, (left + 26, top + 345), font(27, True), COLORS["purple_dark"], card_w - 52)
        role = ["Early Room", "Mid Room", "Aerial Threat", "Boss Variant"][index]
        draw_text(draw, role, (left + 26, top + 405), font(24), COLORS["muted"], card_w - 52)

    image.convert("RGB").save(output, optimize=True)
    return output


def create_npc_lineup() -> Path:
    output = VISUAL_DIR / "npc_lineup.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")
    rounded_panel(draw, (80, 80, 1520, 820), (255, 255, 255, 225), COLORS["pink"], 40, 4)
    draw_text(draw, "NPC Dialogue Cast", (122, 120), font(56, True), COLORS["purple_dark"])
    draw_text(draw, "Dialogue NPCs guide the player through controls, memory logs, crafting, skill energy and danger cues.", (126, 190), font(26), COLORS["muted"], 1210)

    for index, (name, path) in enumerate(NPC_FRAMES):
        frame = trim_alpha(load_rgba(path), padding=6)
        left = 180 + index * 650
        top = 300
        rounded_panel(draw, (left, top, left + 520, top + 410), COLORS["blue_soft"], COLORS["blue"], 36, 3)
        paste_center(image, frame, (left + 60, top + 30, left + 260, top + 320))
        draw_text(draw, name, (left + 278, top + 75), font(34, True), COLORS["purple_dark"], 210)
        if index == 0:
            text = "Opening guide. Explains the forgotten forge and pulls players into the memory-log narrative."
        else:
            text = "Mid-run helper. Reminds players about crafting, skill energy rhythm, and room danger."
        draw_text(draw, text, (left + 278, top + 136), font(24), COLORS["muted"], 205, 10)

    image.convert("RGB").save(output, optimize=True)
    return output


def create_asset_contact_sheet() -> Path:
    output = VISUAL_DIR / "asset_contact_sheet.png"
    image = Image.new("RGBA", (1920, 1080), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")
    rounded_panel(draw, (70, 60, 1850, 1016), (255, 255, 255, 226), COLORS["pink"], 42, 4)
    draw_text(draw, "Real Game Visual Assets", (118, 98), font(64, True), COLORS["purple_dark"])
    draw_text(draw, "Player, enemies, NPCs, craft items, device art and dungeon tiles used by the Unity prototype.", (122, 178), font(28), COLORS["muted"], 1500)

    sections = [
        ("Player", [extract_grid(PLAYER_MAIN, 4, 4, [0])[0], extract_grid(PLAYER_MAIN, 4, 4, [8])[0]], (120, 260, 510, 560)),
        ("Enemies", [extract_grid(path, 4, 4, [0])[0] for _, path in ENEMY_SHEETS], (540, 260, 1080, 560)),
        ("NPC", [trim_alpha(load_rgba(path), 4) for _, path in NPC_FRAMES], (1110, 260, 1490, 560)),
        ("Items", [trim_alpha(load_rgba(path), 4) for _, path in ITEM_ICONS], (120, 640, 720, 910)),
        ("Device", [trim_alpha(load_rgba(path), 4) for _, path in DEVICE_IMAGES], (760, 640, 1120, 910)),
        ("Tiles", [trim_alpha(load_rgba(ASSET_ROOT / "Tiles" / name), 2) for name in ["floor_tile_1.png", "brick_1.png", "platform_1.png", "wall_1.png"] if (ASSET_ROOT / "Tiles" / name).exists()], (1160, 640, 1760, 910)),
    ]

    for title, images, box in sections:
        left, top, right, bottom = box
        rounded_panel(draw, box, (255, 244, 250, 242), (146, 214, 255, 175), 28, 2)
        draw_text(draw, title, (left + 26, top + 20), font(31, True), COLORS["purple_dark"])
        if not images:
            draw_text(draw, "No usable image found", (left + 26, top + 76), font(22), COLORS["muted"], right - left - 52)
            continue
        cell_w = (right - left - 64) // min(len(images), 4)
        for index, asset_image in enumerate(images[:6]):
            column = index % 4
            row = index // 4
            cell_left = left + 28 + column * cell_w
            cell_top = top + 86 + row * 105
            paste_center(image, asset_image, (cell_left, cell_top, cell_left + cell_w - 12, cell_top + 92))

    image.convert("RGB").save(output, optimize=True)
    return output


def create_character_cards() -> Path:
    output = VISUAL_DIR / "character_cards.png"
    image = Image.new("RGBA", (1920, 1080), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Character & Enemy Cards", (90, 70), font(66, True), COLORS["purple_dark"])
    draw_text(draw, "A compact cast board for the poster and pitch deck. All art below comes from repository assets.", (94, 150), font(28), COLORS["muted"], 1550)

    entries = [
        ("Maid Explorer", "Player", "Explores the forge, fights room enemies, and collects memory fragments.", extract_grid(PLAYER_MAIN, 4, 4, [0])[0]),
        ("Archivist Guide", "NPC", "Explains archive logs and gives the opening narrative hook.", trim_alpha(load_rgba(NPC_FRAMES[0][1]), 4)),
        ("Crystal Fox Spirit", "Enemy", "A readable early-to-mid combat target for the demo path.", extract_grid(ENEMY_SHEETS[1][1], 4, 4, [0])[0]),
        ("Flower Slime Nymph", "Enemy", "A softer abnormal creature option for variety and future mechanics.", extract_grid(ENEMY_SHEETS[3][1], 4, 4, [0])[0]),
        ("Ground Turret", "Device", "Crafted support device placeholder for shooting enemies.", trim_alpha(load_rgba(DEVICE_IMAGES[0][1]), 4)),
    ]

    card_w = 340
    card_h = 720
    start_x = 70
    for index, (name, role, description, asset_image) in enumerate(entries):
        left = start_x + index * 365
        top = 245
        rounded_panel(draw, (left, top, left + card_w, top + card_h), (255, 255, 255, 236), COLORS["pink"], 34, 3)
        rounded_panel(draw, (left + 22, top + 24, left + card_w - 22, top + 345), COLORS["blue_soft"], None, 26, 0)
        paste_center(image, asset_image, (left + 42, top + 48, left + card_w - 42, top + 320))
        title_font = font(27, True) if len(name) > 16 else font(31, True)
        draw_text(draw, name, (left + 28, top + 380), title_font, COLORS["purple_dark"], card_w - 56)
        draw_text(draw, role, (left + 28, top + 436), font(24, True), COLORS["pink"], card_w - 56)
        draw_text(draw, description, (left + 28, top + 490), font(23), COLORS["muted"], card_w - 56, 10)
        rounded_panel(draw, (left + 28, top + 635, left + card_w - 28, top + 684), COLORS["pink_soft"], None, 18, 0)
        draw_text(draw, "AI narrative hook", (left + 48, top + 646), font(20, True), COLORS["purple"])

    image.convert("RGB").save(output, optimize=True)
    return output


def create_screenshot_placeholder_grid() -> Path:
    output = VISUAL_DIR / "screenshot_placeholders.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Unity Screenshots Needed", (90, 80), font(58, True), COLORS["purple_dark"])
    draw_text(draw, "These boxes are honest placeholders. Replace them with captures from Submission/screenshots/ after Unity playtesting.", (94, 154), font(27), COLORS["muted"], 1360)

    labels = [
        "01 Spawn / Opening",
        "02 Memory Log",
        "03 Early Combat",
        "04 Growth",
        "05 Boss Room",
        "06 Victory",
    ]
    for index, label in enumerate(labels):
        column = index % 3
        row = index // 3
        left = 95 + column * 500
        top = 270 + row * 270
        rounded_panel(draw, (left, top, left + 435, top + 220), (45, 39, 76, 230), COLORS["blue"], 28, 2)
        draw_text(draw, label, (left + 28, top + 34), font(27, True), (255, 255, 255, 255), 360)
        draw_text(draw, "Waiting for real Unity screenshot", (left + 28, top + 92), font(22), (232, 245, 255, 220), 350)
        draw.line((left + 28, top + 166, left + 405, top + 166), fill=(255, 128, 184, 170), width=4)

    image.convert("RGB").save(output, optimize=True)
    return output


def find_existing_screenshots() -> list[Path]:
    screenshot_dir = SUBMISSION_DIR / "screenshots"
    if not screenshot_dir.exists():
        return []
    return sorted([path for path in screenshot_dir.glob("*.png") if path.is_file()])[:6]


def create_poster_png() -> Path:
    output = POSTER_PNG
    image = Image.new("RGBA", (1920, 1080), COLORS["paper"])
    draw_soft_background(image)
    draw = ImageDraw.Draw(image, "RGBA")

    # Playful platform base so the poster reads as a real side-scroller.
    image.alpha_composite(create_tile_strip(1920, 160), (0, 920))
    draw.rectangle((0, 902, 1920, 922), fill=(255, 203, 120, 220))
    draw.rectangle((0, 760, 1920, 820), fill=(222, 245, 255, 120))

    rounded_panel(draw, (70, 58, 1210, 540), (255, 255, 255, 230), COLORS["pink"], 42, 4)
    draw_text(draw, "能工智人：遗忘工坊", (116, 110), font(74, True), COLORS["purple_dark"])
    draw_text(draw, "Craftsmen and Homo Sapiens: The Forgotten Forge", (120, 198), font(32, True), COLORS["purple"])
    draw_text(draw, "叙事类游戏 / Narrative Games", (120, 250), font(29, True), COLORS["pink"])
    pitch = "AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。"
    draw_text(draw, pitch, (120, 314), font(31), COLORS["ink"], 970, 12)

    highlights = [
        "AI 世界观与剧情生成",
        "横版动作战斗与房间推进",
        "记忆日志驱动的叙事体验",
    ]
    for index, item in enumerate(highlights):
        left = 120 + index * 345
        top = 438
        rounded_panel(draw, (left, top, left + 305, top + 64), COLORS["blue_soft"] if index % 2 else COLORS["pink_soft"], None, 22, 0)
        draw_text(draw, item, (left + 22, top + 17), font(22, True), COLORS["purple_dark"], 258)

    # Main hero and enemy stage.
    player = extract_grid(PLAYER_MAIN, 4, 4, [8])[0]
    player_big = fit_image(player, 420, 520, Image.Resampling.LANCZOS)
    image.alpha_composite(player_big, (1310, 360))

    enemy_positions = [
        (ENEMY_SHEETS[0][1], 1210, 768, 170),
        (ENEMY_SHEETS[1][1], 1515, 746, 190),
        (ENEMY_SHEETS[3][1], 1700, 785, 150),
    ]
    for path, x, y, size in enemy_positions:
        frame = extract_grid(path, 4, 4, [0])[0]
        fitted = fit_image(frame, size, size, Image.Resampling.LANCZOS)
        image.alpha_composite(fitted, (x, y))

    # Memory log and AI feature panel.
    rounded_panel(draw, (80, 586, 760, 884), (49, 42, 82, 232), COLORS["blue"], 34, 3)
    draw_text(draw, "工坊记忆日志", (120, 628), font(34, True), (255, 255, 255, 255))
    draw_text(draw, "AI 生成的房间档案会在探索中弹出：玩家一边战斗，一边拼合工匠文明与智人文明冲突的真相。", (120, 686), font(25), (239, 246, 255, 235), 570, 10)
    for index, item in enumerate(["世界观", "房间日志", "Boss 背景", "结局文本"]):
        left = 120 + (index % 2) * 290
        top = 795 + (index // 2) * 48
        rounded_panel(draw, (left, top, left + 245, top + 34), COLORS["pink_soft"], None, 12, 0)
        draw_text(draw, item, (left + 16, top + 5), font(20, True), COLORS["purple_dark"])

    screenshots = find_existing_screenshots()
    card_labels = ["开场", "战斗", "Boss / 胜利"]
    for index in range(3):
        left = 812 + index * 340
        top = 596
        rounded_panel(draw, (left, top, left + 302, top + 178), (255, 255, 255, 236), COLORS["pink"], 24, 3)
        if index < len(screenshots):
            shot = load_rgba(screenshots[index])
            paste_center(image, shot, (left + 10, top + 10, left + 292, top + 138))
            label = card_labels[index]
        else:
            draw.rectangle((left + 12, top + 12, left + 290, top + 132), fill=(49, 42, 82, 210))
            draw_text(draw, "等待 Unity 截图回填", (left + 34, top + 54), font(22, True), (255, 255, 255, 255), 230)
            label = f"{card_labels[index]} placeholder"
        draw_text(draw, label, (left + 22, top + 142), font(19, True), COLORS["purple_dark"], 250)

    rounded_panel(draw, (1345, 86, 1835, 278), (255, 255, 255, 220), COLORS["blue"], 30, 3)
    draw_text(draw, "Demo Assets", (1376, 118), font(30, True), COLORS["purple_dark"])
    mini_assets = [
        trim_alpha(load_rgba(ITEM_ICONS[0][1]), 4),
        trim_alpha(load_rgba(ITEM_ICONS[1][1]), 4),
        trim_alpha(load_rgba(DEVICE_IMAGES[0][1]), 4),
        trim_alpha(load_rgba(NPC_FRAMES[0][1]), 4),
    ]
    for index, asset_image in enumerate(mini_assets):
        paste_center(image, asset_image, (1370 + index * 108, 166, 1450 + index * 108, 250))

    draw_text(draw, "Team：待回填", (96, 996), font(25, True), COLORS["purple_dark"])
    draw_text(draw, "Playable Demo：待回填", (392, 996), font(25, True), COLORS["purple_dark"])
    draw_text(draw, "AI-assisted Unity production + narrative content", (790, 996), font(23), COLORS["purple"])

    image.convert("RGB").save(output, optimize=True, quality=92)
    return output


def create_svg_and_html() -> None:
    poster_b64 = base64.b64encode(POSTER_PNG.read_bytes()).decode("ascii")
    svg = f"""<svg xmlns="http://www.w3.org/2000/svg" width="1920" height="1080" viewBox="0 0 1920 1080">
<image href="data:image/png;base64,{poster_b64}" x="0" y="0" width="1920" height="1080"/>
<metadata>Editable source wrapper for poster_1920x1080.png. Poster raster is generated from project-owned assets by tools/generate_submission_visuals.py.</metadata>
</svg>
"""
    POSTER_SVG.write_text(svg, encoding="utf-8")

    html_doc = f"""<!doctype html>
<html lang="zh-CN">
<head>
  <meta charset="utf-8">
  <title>能工智人：遗忘工坊 Poster Source</title>
  <style>
    body {{ margin: 0; background: #fff8fd; font-family: "Microsoft YaHei", Arial, sans-serif; }}
    main {{ max-width: 1280px; margin: 32px auto; padding: 24px; }}
    img {{ width: 100%; border-radius: 24px; box-shadow: 0 24px 80px rgba(77, 48, 118, .22); }}
    p {{ color: #5f527f; font-size: 18px; }}
  </style>
</head>
<body>
<main>
  <img src="poster_1920x1080.png" alt="Craftsmen and Homo Sapiens submission poster">
  <p>Poster generated from real repository assets. Re-run <code>python tools/generate_submission_visuals.py</code> after updating sprites or screenshots.</p>
</main>
</body>
</html>
"""
    POSTER_HTML.write_text(html_doc, encoding="utf-8")


def deck_slides() -> list[dict[str, object]]:
    return [
        {
            "title": "能工智人：遗忘工坊",
            "subtitle": "AI 叙事驱动的横版动作冒险",
            "image": VISUAL_DIR / "character_cards.png",
            "bullets": ["叙事类游戏 / Narrative Games", "可爱工坊美术 + 地下遗迹探索", "Demo Link / Team：待回填"],
        },
        {
            "title": "游戏是什么",
            "subtitle": "玩家进入一座会留下记忆的地下工坊",
            "image": VISUAL_DIR / "player_lineup.png",
            "bullets": ["横版动作：移动、跳跃、近战攻击", "房间推进：清敌、解锁门、进入下一段", "AI 记忆日志：战斗中逐步补全世界观"],
        },
        {
            "title": "核心玩法循环",
            "subtitle": "3-5 分钟能讲清楚的闭环",
            "image": VISUAL_DIR / "asset_contact_sheet.png",
            "bullets": ["进入房间 -> 读记忆日志", "战斗 -> 获得经验 / 道具 / 制作材料", "解锁下一房间 -> Boss -> 结局文本"],
        },
        {
            "title": "角色与怪物",
            "subtitle": "用真实 spritesheet 做阵容展示",
            "image": VISUAL_DIR / "enemy_lineup.png",
            "bullets": ["主角：猫耳女仆探索者", "NPC：档案员与技术员引导玩法", "敌人：普通怪、精英怪、Boss 占位体"],
        },
        {
            "title": "AI 叙事如何进入游戏",
            "subtitle": "不是只写在文档里，而是变成工坊记忆日志",
            "image": VISUAL_DIR / "npc_lineup.png",
            "bullets": ["AI 生成世界观、势力关系、房间档案", "NPC 对话把操作提示包装进剧情", "Boss 背景与结局文本用于演示闭环"],
        },
        {
            "title": "Demo 录屏路线",
            "subtitle": "这些镜头需要用 Unity 截图/录屏补齐",
            "image": VISUAL_DIR / "screenshot_placeholders.png",
            "bullets": ["开场 + NPC 对话", "早期战斗 + 成长反馈", "Boss 房 + Victory / Demo Complete"],
        },
        {
            "title": "技术结构",
            "subtitle": "Unity 2D 原型，系统保持轻量拆分",
            "image": VISUAL_DIR / "asset_contact_sheet.png",
            "bullets": ["Player / Combat / Enemy / Rooms", "Dialogue / Craft / Inventory / Skill Energy", "WebGL Build + 静态部署准备"],
        },
        {
            "title": "提交状态与下一步",
            "subtitle": "材料已经可编辑，真实截图和在线链接等待人工回填",
            "image": VISUAL_DIR / "character_cards.png",
            "bullets": ["已准备：海报、PPT、部署文档、提交文案", "待人工：WebGL 部署、Demo 视频、CodeBuddy 导出", "下一步：用截图工具补齐真实游戏画面"],
        },
    ]


def register_pdf_font() -> str:
    if FONT_REGULAR.exists():
        pdfmetrics.registerFont(TTFont("MSYH", str(FONT_REGULAR)))
        return "MSYH"
    return "Helvetica"


def create_pdf_deck() -> Path:
    font_name = register_pdf_font()
    pdf = canvas.Canvas(str(DECK_PDF), pagesize=landscape((1280, 720)))
    for index, slide in enumerate(deck_slides(), start=1):
        title = str(slide["title"])
        subtitle = str(slide["subtitle"])
        bullets = [str(item) for item in slide["bullets"]]
        image_path = Path(str(slide["image"]))

        pdf.setFillColor(colors.HexColor("#FFF8FD"))
        pdf.rect(0, 0, 1280, 720, fill=1, stroke=0)
        pdf.setFillColor(colors.HexColor("#FFFFFF"))
        pdf.roundRect(36, 32, 1208, 656, 28, fill=1, stroke=0)
        pdf.setStrokeColor(colors.HexColor("#FF80B8"))
        pdf.setLineWidth(2)
        pdf.roundRect(36, 32, 1208, 656, 28, fill=0, stroke=1)

        pdf.setFont(font_name, 13)
        pdf.setFillColor(colors.HexColor("#FF80B8"))
        pdf.drawString(70, 652, f"{index:02d} / 08  Narrative Games")
        pdf.setFont(font_name, 34)
        pdf.setFillColor(colors.HexColor("#322853"))
        pdf.drawString(70, 595, title)
        pdf.setFont(font_name, 19)
        pdf.setFillColor(colors.HexColor("#695F84"))
        pdf.drawString(72, 558, subtitle)

        if image_path.exists():
            pdf.drawImage(str(image_path), 650, 112, width=540, height=405, preserveAspectRatio=True, mask="auto")

        y = 456
        pdf.setFont(font_name, 20)
        for bullet in bullets:
            pdf.setFillColor(colors.HexColor("#FF80B8"))
            pdf.circle(88, y + 6, 7, fill=1, stroke=0)
            pdf.setFillColor(colors.HexColor("#322853"))
            for line in textwrap.wrap(bullet, width=32):
                pdf.drawString(112, y, line)
                y -= 32
            y -= 22

        pdf.setFont(font_name, 10)
        pdf.setFillColor(colors.HexColor("#695F84"))
        pdf.drawString(70, 58, "Craftsmen and Homo Sapiens: The Forgotten Forge | poster/deck visuals generated from repository assets")
        pdf.showPage()

    pdf.save()
    return DECK_PDF


def create_project_deck_markdown() -> None:
    lines = [
        "# 能工智人：遗忘工坊 - Project Deck",
        "",
        "这份 Markdown 与 `Submission/project_deck.pptx` 保持同一页结构，用于快速改稿。",
        "",
    ]
    for index, slide in enumerate(deck_slides(), start=1):
        lines.append(f"## Slide {index}. {slide['title']}")
        lines.append("")
        lines.append(str(slide["subtitle"]))
        lines.append("")
        for bullet in slide["bullets"]:
            lines.append(f"- {bullet}")
        lines.append("")
        lines.append(f"Visual: `{Path(str(slide['image'])).as_posix()}`")
        lines.append("")
    DECK_MD.write_text("\n".join(lines), encoding="utf-8")


def create_notes() -> None:
    POSTER_NOTES.write_text(
        """# Poster Notes

新版海报不再使用抽象网格模板，而是从仓库真实素材生成：

- Player: `Assets/Art/Generated/Characters/cat_maid_magic_wand_spritesheet.png`
- Enemies: `Assets/Art/Generated/Enemies/*.png`
- NPC and item/device accents: `Assets/Art/Generated/NPC/`, `Assets/Art/Generated/Items/`, `Assets/Art/Generated/Devices/`
- Tile strip: `Assets/Art/Tiles/`

海报中的三张截图框如果没有真实截图，会明确标记为“等待 Unity 截图回填”，没有伪造 Demo 截图。
""",
        encoding="utf-8",
    )

    DECK_NOTES.write_text(
        """# Project Deck Notes

新版 PPT 使用 `Submission/visual_assets/` 中的真实素材展示图，不再是纯文字或空卡片。

PDF 是脚本生成的预览版；最终可编辑 PPTX 由 `tools/generate_project_deck.cjs` 使用 `@oai/artifact-tool` 生成。

仍需人工补齐：

- WebGL 在线链接
- Demo 视频链接
- CodeBuddy 历史导出
- Unity 真实截图，放入 `Submission/screenshots/`
""",
        encoding="utf-8",
    )

    SPEAKER_NOTES.write_text(
        """# PPT Speaker Notes

## 3-5 分钟讲稿

1. 开场先说项目定位：这是《能工智人：遗忘工坊》，叙事类游戏赛道，一个 AI 叙事驱动的 2D 横版动作原型。
2. 说明玩家角色：玩家进入地下工坊遗迹，通过战斗和记忆日志寻找工匠文明与智人文明冲突的真相。
3. 讲核心循环：进入房间、读日志、战斗、获得成长、解锁下一房间，最后挑战 Boss 并看到结局文本。
4. 强调 AI 不是只帮忙写文档，而是生成世界观、NPC 台词、房间档案、Boss 背景和结局内容，并进入游戏 UI。
5. 展示角色、怪物和 NPC 资产，说明素材已经能支撑可录屏 Demo。
6. 展示 Demo 流程页，告诉评委录屏会依次展示开场、战斗、成长、Boss 和 Victory。
7. 技术页简短带过：Unity 2D，Player/Combat/Enemy/Room/UI/Dialogue/Craft 分模块，已准备 WebGL 构建和静态部署文档。
8. 最后一页说提交状态：海报、PPT、部署说明、提交文案已准备；WebGL 链接、Demo 视频和 CodeBuddy 导出由人工最后回填。
""",
        encoding="utf-8",
    )


def write_visual_asset_inventory() -> None:
    generated_assets = [
        VisualAsset("Cat Maid Magic Wand", PLAYER_MAIN, "Player", "Poster hero, Slide 1/2, player lineup"),
        VisualAsset("Cat Maid Walk/Climb", PLAYER_WALK, "Player", "Player movement sheet and animation proof"),
        *[
            VisualAsset(name, path, "Enemy", "Enemy lineup, character cards, combat/boss explanation")
            for name, path in ENEMY_SHEETS
        ],
        *[
            VisualAsset(name, path, "NPC", "Dialogue cast slide and AI narrative slide")
            for name, path in NPC_FRAMES
        ],
        *[
            VisualAsset(name, path, "Item", "Crafting and progression visual accents")
            for name, path in ITEM_ICONS
        ],
        *[
            VisualAsset(name, path, "Device", "Crafted device / turret visual proof")
            for name, path in DEVICE_IMAGES
        ],
    ]

    lines = [
        "# Visual Asset Inventory",
        "",
        "This inventory records the real project assets used for the rebuilt poster and PPT. It avoids external game screenshots and avoids pretending that placeholder screenshot frames are real playtest captures.",
        "",
        "## Summary",
        "",
        "- Player assets are strong enough for the poster hero and action lineup.",
        "- Enemy assets include four readable prototype enemies for demo variety.",
        "- NPC assets support the dialogue / memory-log narrative pitch.",
        "- Item and device icons help show crafting, growth, and support-device features.",
        "- Real Unity screenshots are still needed for final submission polish.",
        "",
        "## Asset List",
        "",
        "| Category | Asset | Path | Suggested Use | Status |",
        "| --- | --- | --- | --- | --- |",
    ]
    for asset in generated_assets:
        status = "Found" if asset.path.exists() else "Missing"
        rel = asset.path.relative_to(ROOT).as_posix()
        lines.append(f"| {asset.category} | {asset.name} | `{rel}` | {asset.suggested_use} | {status} |")

    lines.extend(
        [
            "",
            "## Good Poster Candidates",
            "",
            "- `cat_maid_magic_wand_spritesheet.png`: main character silhouette and action pose.",
            "- `crystal_fox_spirit_girl_spritesheet.png`: cute but hostile enemy highlight.",
            "- `archivist_guide_idle_02.png`: NPC / memory archive narrative cue.",
            "- `GearspikeWandIcon.png`, `GroundTurretIcon.png`: crafting and support-device accents.",
            "",
            "## Good PPT Candidates",
            "",
            "- `Submission/visual_assets/player_lineup.png` for the player slide.",
            "- `Submission/visual_assets/enemy_lineup.png` for the enemy slide.",
            "- `Submission/visual_assets/npc_lineup.png` for AI narrative and dialogue.",
            "- `Submission/visual_assets/asset_contact_sheet.png` for production proof.",
            "",
            "## Needs Human Capture",
            "",
            "The final deck still needs real Unity screenshots for opening, memory log UI, combat, growth, boss, and victory. Use `Tools/Hackathon/Capture Submission Screenshots` inside Unity and place the output under `Submission/screenshots/`.",
            "",
            "## Avoid For Submission Visuals",
            "",
            "- Do not use ignored `Assets/Art/ThirdParty/` raw Asset Store files in public submission materials unless licensing and repository policy are confirmed.",
            "- Do not use external game screenshots or copied poster art.",
            "- Do not use blank placeholders without clear labels.",
        ]
    )
    (ROOT / "docs" / "VISUAL_ASSET_INVENTORY.md").write_text("\n".join(lines), encoding="utf-8")


def write_visual_benchmark() -> None:
    (ROOT / "docs" / "VISUAL_BENCHMARK.md").write_text(
        """# Visual Benchmark

This document records structure principles for the rebuilt poster and deck. It is not a license to copy another game's art, screenshots, UI, logo, or text.

## What To Learn From Strong Game Materials

- Lead with a strong first visual: player, enemy, scene, or screenshot.
- Use real gameplay visuals instead of abstract feature icons.
- Show the character and enemy lineup early.
- Keep every slide focused on one message.
- Keep text short and make screenshots or diagrams do the proof work.
- Make the gameplay loop visible as a route, not as a paragraph.
- Show that AI content enters the game UI through memory logs and NPC dialogue.
- Keep placeholders honest: label missing screenshots as placeholders.

## What Not To Do

- Do not download or use another game's screenshots.
- Do not copy another game's logo, UI, poster layout, or exact composition.
- Do not rewrite another game's marketing copy with only minor word changes.
- Do not use copyright-unclear art in poster or PPT.
- Do not claim deployment, video recording, or CodeBuddy export has happened before it is done.

## Current Style Direction

- Cute workshop adventure rather than plain document template.
- Pink, blue, purple, and cream are allowed, but must be anchored by real characters, monsters, NPCs, tiles, and UI.
- The submission should read as a playable Unity game prototype at first glance.
""",
        encoding="utf-8",
    )


def create_outputs() -> None:
    ensure_dirs()
    created = [
        create_player_lineup(),
        create_enemy_lineup(),
        create_npc_lineup(),
        create_asset_contact_sheet(),
        create_character_cards(),
        create_screenshot_placeholder_grid(),
        create_poster_png(),
    ]
    create_svg_and_html()
    create_pdf_deck()
    create_project_deck_markdown()
    create_notes()
    write_visual_asset_inventory()
    write_visual_benchmark()
    print("Generated submission visuals:")
    for path in created:
        print(f"- {path.relative_to(ROOT).as_posix()}")
    print(f"- {DECK_PDF.relative_to(ROOT).as_posix()}")


if __name__ == "__main__":
    create_outputs()
