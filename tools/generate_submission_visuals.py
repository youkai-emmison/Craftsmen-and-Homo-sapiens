"""Generate V2 submission visuals from real project assets.

The script creates poster/deck art from repository-owned or already generated
game assets. It does not use external screenshots, and all missing gameplay
screenshots remain clearly marked as placeholders.
"""

from __future__ import annotations

import base64
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
    "white": (255, 255, 255, 255),
    "dark_panel": (49, 42, 82, 235),
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
    scale = min(max_width / image.width, max_height / image.height)
    width = max(1, int(image.width * scale))
    height = max(1, int(image.height * scale))
    return image.resize((width, height), resample)


def paste_center(canvas_image: Image.Image, image: Image.Image, box: tuple[int, int, int, int]) -> None:
    left, top, right, bottom = box
    fitted = fit_image(image, right - left, bottom - top)
    x = left + (right - left - fitted.width) // 2
    y = top + (bottom - top - fitted.height) // 2
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


def panel(
    draw: ImageDraw.ImageDraw,
    box: tuple[int, int, int, int],
    fill: tuple[int, int, int, int],
    outline: tuple[int, int, int, int] | None = None,
    radius: int = 28,
    width: int = 2,
) -> None:
    draw.rounded_rectangle(box, radius=radius, fill=fill, outline=outline, width=width)


def draw_background(image: Image.Image, grid: bool = False) -> None:
    draw = ImageDraw.Draw(image, "RGBA")
    width, height = image.size
    for y in range(height):
        t = y / max(1, height - 1)
        r = int(255 * (1 - t) + 225 * t)
        g = int(248 * (1 - t) + 242 * t)
        b = int(253 * (1 - t) + 255 * t)
        draw.line((0, y, width, y), fill=(r, g, b, 255))
    if grid:
        for x in range(0, width, 150):
            draw.line((x, 0, x, height), fill=(255, 128, 184, 28), width=1)
        for y in range(0, height, 110):
            draw.line((0, y, width, y), fill=(116, 205, 255, 24), width=1)
    # Keep decorative color away from the content area; dense bubbles made the deck feel crowded.
    edge_blobs = [
        (90, 120, 46, (116, 205, 255, 32)),
        (width - 110, 100, 58, (255, 128, 184, 34)),
        (width - 120, height - 110, 62, (203, 240, 229, 38)),
        (150, height - 120, 40, (255, 128, 184, 28)),
    ]
    for x, y, radius, color in edge_blobs:
        draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill=color)


def tile_strip(width: int, height: int) -> Image.Image:
    result = Image.new("RGBA", (width, height), (0, 0, 0, 0))
    draw = ImageDraw.Draw(result, "RGBA")
    tile_names = ["floor_tile_1.png", "floor_tile_2.png", "brick_1.png", "platform_1.png"]
    tiles = [load_rgba(ASSET_ROOT / "Tiles" / name) for name in tile_names if (ASSET_ROOT / "Tiles" / name).exists()]
    if not tiles:
        draw.rectangle((0, 0, width, height), fill=(95, 82, 120, 255))
        return result
    scaled_tiles = [tile.resize((48, 48), Image.Resampling.NEAREST) for tile in tiles]
    for y in range(0, height, 48):
        for x in range(0, width, 48):
            result.alpha_composite(scaled_tiles[(x // 48 + y // 48) % len(scaled_tiles)], (x, y))
    draw.rectangle((0, 0, width, 8), fill=(255, 211, 107, 230))
    return result


def player_frame(index: int = 8) -> Image.Image:
    return extract_grid(PLAYER_MAIN, 4, 4, [index])[0]


def player_pose(index: int = 8, crop_left_half: bool = True) -> Image.Image:
    frame = player_frame(index)
    if crop_left_half:
        frame = frame.crop((0, 0, frame.width // 2, frame.height))
    # The generated sheet cells contain compact 2x2 pose groups; posters need one readable pose.
    if frame.height > frame.width * 0.8:
        frame = frame.crop((0, 0, frame.width, frame.height // 2))
    return trim_alpha(frame, padding=6)


def enemy_frame(enemy_index: int, frame_index: int = 0) -> Image.Image:
    return extract_grid(ENEMY_SHEETS[enemy_index][1], 4, 4, [frame_index])[0]


def npc_frame(index: int) -> Image.Image:
    return trim_alpha(load_rgba(NPC_FRAMES[index][1]), padding=6)


def create_player_lineup() -> Path:
    output = VISUAL_DIR / "player_lineup.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Player Action Lineup", (105, 95), font(62, True), COLORS["purple_dark"])
    draw_text(draw, "Idle / move / attack / hurt frames from the real player spritesheets.", (108, 175), font(27), COLORS["muted"], 1200)
    draw.rectangle((0, 620, 1600, 760), fill=(222, 245, 255, 150))
    image.alpha_composite(tile_strip(1600, 90), (0, 760))

    frames = [
        ("Idle", player_pose(0)),
        ("Ready", player_pose(4)),
        ("Attack", player_pose(8)),
        ("Hurt", player_pose(12)),
        ("Walk", trim_alpha(extract_grid(PLAYER_WALK, 7, 2, [0])[0], 6)),
        ("Move", trim_alpha(extract_grid(PLAYER_WALK, 7, 2, [2])[0], 6)),
        ("Climb", trim_alpha(extract_grid(PLAYER_WALK, 7, 2, [7])[0], 6)),
    ]
    for index, (label, frame) in enumerate(frames):
        x = 105 + index * 205
        sprite = fit_image(frame, 170, 250, Image.Resampling.LANCZOS)
        image.alpha_composite(sprite, (x + (170 - sprite.width) // 2, 388 + (250 - sprite.height) // 2))
        draw_text(draw, label, (x + 36, 665), font(25, True), COLORS["purple_dark"])

    image.convert("RGB").save(output, optimize=True)
    return output


def create_enemy_lineup() -> Path:
    output = VISUAL_DIR / "enemy_lineup.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Enemy Lineup", (105, 95), font(62, True), COLORS["purple_dark"])
    draw_text(draw, "Readable monster silhouettes for early room, mid room, aerial threat and boss placeholder.", (108, 175), font(27), COLORS["muted"], 1200)
    draw.rectangle((0, 620, 1600, 760), fill=(222, 245, 255, 150))
    image.alpha_composite(tile_strip(1600, 90), (0, 760))

    roles = ["Early Room", "Mid Room", "Aerial Threat", "Boss Variant"]
    for index, (name, path) in enumerate(ENEMY_SHEETS):
        frame = extract_grid(path, 4, 4, [0, 5, 8, 12])[index % 4]
        left = 140 + index * 365
        sprite = fit_image(frame, 265, 310, Image.Resampling.LANCZOS)
        image.alpha_composite(sprite, (left + (265 - sprite.width) // 2, 330 + (310 - sprite.height) // 2))
        draw_text(draw, name, (left - 8, 665), font(25, True), COLORS["purple_dark"], 300)
        draw_text(draw, roles[index], (left - 8, 707), font(22), COLORS["muted"], 300)

    image.convert("RGB").save(output, optimize=True)
    return output


def create_npc_lineup() -> Path:
    output = VISUAL_DIR / "npc_lineup.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=True)
    draw = ImageDraw.Draw(image, "RGBA")
    panel(draw, (80, 80, 1520, 820), (255, 255, 255, 230), COLORS["pink"], 40, 4)
    draw_text(draw, "NPC Dialogue Cast", (122, 120), font(56, True), COLORS["purple_dark"])
    draw_text(draw, "NPCs make controls, skill energy, crafting and memory logs feel like part of the story.", (126, 190), font(26), COLORS["muted"], 1210)

    descriptions = [
        "Opening guide. Explains the forgotten forge and pulls players into the memory-log narrative.",
        "Mid-run helper. Reminds players about crafting, skill energy rhythm and room danger.",
    ]
    for index, (name, path) in enumerate(NPC_FRAMES):
        frame = trim_alpha(load_rgba(path), padding=6)
        left = 180 + index * 650
        top = 300
        panel(draw, (left, top, left + 520, top + 410), COLORS["blue_soft"], COLORS["blue"], 36, 3)
        paste_center(image, frame, (left + 48, top + 30, left + 250, top + 320))
        draw_text(draw, name, (left + 276, top + 72), font(32, True), COLORS["purple_dark"], 220)
        draw_text(draw, descriptions[index], (left + 276, top + 136), font(23), COLORS["muted"], 205, 10)

    image.convert("RGB").save(output, optimize=True)
    return output


def create_asset_contact_sheet() -> Path:
    output = VISUAL_DIR / "asset_contact_sheet.png"
    image = Image.new("RGBA", (1920, 1080), COLORS["paper"])
    draw_background(image, grid=True)
    draw = ImageDraw.Draw(image, "RGBA")
    panel(draw, (70, 60, 1850, 1016), (255, 255, 255, 232), COLORS["pink"], 42, 4)
    draw_text(draw, "Real Game Visual Assets", (118, 98), font(64, True), COLORS["purple_dark"])
    draw_text(draw, "Player, enemies, NPCs, craft items, device art and dungeon tiles used by the Unity prototype.", (122, 178), font(28), COLORS["muted"], 1500)

    sections = [
        ("Player", [player_pose(0), player_pose(8), player_pose(12)], (120, 260, 510, 560)),
        ("Enemies", [enemy_frame(i) for i in range(4)], (540, 260, 1080, 560)),
        ("NPC", [npc_frame(0), npc_frame(1)], (1110, 260, 1490, 560)),
        ("Items", [trim_alpha(load_rgba(path), 4) for _, path in ITEM_ICONS], (120, 640, 720, 910)),
        ("Device", [trim_alpha(load_rgba(path), 4) for _, path in DEVICE_IMAGES], (760, 640, 1120, 910)),
        ("Tiles", [trim_alpha(load_rgba(ASSET_ROOT / "Tiles" / name), 2) for name in ["floor_tile_1.png", "brick_1.png", "platform_1.png", "wall_1.png"] if (ASSET_ROOT / "Tiles" / name).exists()], (1160, 640, 1760, 910)),
    ]

    for title, images, box in sections:
        left, top, right, bottom = box
        panel(draw, box, (255, 244, 250, 242), (146, 214, 255, 175), 28, 2)
        draw_text(draw, title, (left + 26, top + 20), font(31, True), COLORS["purple_dark"])
        cell_width = (right - left - 64) // max(1, min(len(images), 4))
        for index, asset_image in enumerate(images[:6]):
            col = index % 4
            row = index // 4
            cell_left = left + 28 + col * cell_width
            cell_top = top + 86 + row * 105
            paste_center(image, asset_image, (cell_left, cell_top, cell_left + cell_width - 12, cell_top + 92))

    image.convert("RGB").save(output, optimize=True)
    return output


def create_character_cards() -> Path:
    output = VISUAL_DIR / "character_cards.png"
    image = Image.new("RGBA", (1920, 1080), COLORS["paper"])
    draw_background(image, grid=True)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Character & Enemy Cards", (90, 70), font(66, True), COLORS["purple_dark"])
    draw_text(draw, "A compact cast board for the poster and pitch deck. All art below comes from repository assets.", (94, 150), font(28), COLORS["muted"], 1550)

    entries = [
        ("Maid Explorer", "Player", "Explores the forge, fights room enemies and collects memory fragments.", player_pose(0)),
        ("Archivist Guide", "NPC", "Explains archive logs and gives the opening narrative hook.", npc_frame(0)),
        ("Crystal Fox Spirit", "Enemy", "A readable early-to-mid combat target for the demo path.", enemy_frame(1)),
        ("Flower Slime Nymph", "Enemy", "A soft abnormal creature option for future enemy variety.", enemy_frame(3)),
        ("Ground Turret", "Device", "Crafted support device placeholder for shooting enemies.", trim_alpha(load_rgba(DEVICE_IMAGES[0][1]), 4)),
    ]

    for index, (name, role, description, asset_image) in enumerate(entries):
        left = 70 + index * 365
        top = 245
        panel(draw, (left, top, left + 340, top + 720), (255, 255, 255, 236), COLORS["pink"], 34, 3)
        panel(draw, (left + 22, top + 24, left + 318, top + 345), COLORS["blue_soft"], None, 26, 0)
        paste_center(image, asset_image, (left + 42, top + 48, left + 298, top + 320))
        draw_text(draw, name, (left + 28, top + 380), font(27 if len(name) > 16 else 31, True), COLORS["purple_dark"], 286)
        draw_text(draw, role, (left + 28, top + 436), font(24, True), COLORS["pink"], 286)
        draw_text(draw, description, (left + 28, top + 490), font(23), COLORS["muted"], 286, 10)
        panel(draw, (left + 28, top + 635, left + 312, top + 684), COLORS["pink_soft"], None, 18, 0)
        draw_text(draw, "AI narrative hook", (left + 48, top + 646), font(20, True), COLORS["purple"])

    image.convert("RGB").save(output, optimize=True)
    return output


def create_hero_stage() -> Path:
    output = VISUAL_DIR / "hero_stage.png"
    image = Image.new("RGBA", (1600, 900), (0, 0, 0, 0))
    draw = ImageDraw.Draw(image, "RGBA")
    draw_background(image, grid=False)
    image.alpha_composite(tile_strip(1600, 170), (0, 730))

    draw.rectangle((0, 540, 1600, 730), fill=(222, 245, 255, 110))
    panel(draw, (70, 64, 520, 292), COLORS["dark_panel"], COLORS["blue"], 28, 3)
    draw_text(draw, "Workshop Memory Log", (105, 98), font(31, True), COLORS["white"])
    draw_text(draw, "Room 02: The forge remembers every promise that broke inside the core.", (105, 150), font(23), (238, 246, 255, 235), 360)

    main = fit_image(player_pose(8), 360, 510, Image.Resampling.LANCZOS)
    image.alpha_composite(main, (690, 340))
    enemy_data = [(0, 1060, 630, 175), (1, 1185, 570, 205), (2, 1320, 625, 190), (3, 1435, 600, 195)]
    for enemy_index, x, y, size in enemy_data:
        enemy = fit_image(enemy_frame(enemy_index), size, size, Image.Resampling.LANCZOS)
        image.alpha_composite(enemy, (x, y))

    npc = fit_image(npc_frame(0), 160, 160, Image.Resampling.NEAREST)
    image.alpha_composite(npc, (420, 595))
    draw.line((560, 665, 660, 620), fill=COLORS["pink"], width=8)
    draw_text(draw, "AI narrative + room combat", (925, 130), font(42, True), COLORS["purple_dark"])
    draw_text(draw, "Cute workshop action loop.", (930, 190), font(25), COLORS["muted"], 520)

    image.convert("RGB").save(output, optimize=True)
    return output


def create_memory_log_mock() -> Path:
    output = VISUAL_DIR / "memory_log_mock.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    panel(draw, (120, 90, 1480, 810), COLORS["dark_panel"], COLORS["blue"], 42, 4)
    draw_text(draw, "工坊记忆日志 / Workshop Memory Log", (175, 145), font(48, True), COLORS["white"])
    draw_text(draw, "Archive Fragment 02", (178, 215), font(28, True), COLORS["pink"])
    log_text = "炉心记录：工匠们把判断力交给机器，把记忆交给人类。协议中断后，房间仍在重复旧命令。"
    draw_text(draw, log_text, (178, 275), font(34), (238, 246, 255, 245), 900, 16)
    draw_text(draw, "AI 生成内容进入游戏 UI：世界观、势力关系、房间日志、Boss 背景、结局文本。", (178, 510), font(27), (205, 235, 255, 235), 900, 12)
    paste_center(image, npc_frame(0), (1130, 190, 1395, 520))
    chips = ["Worldbuilding", "Room Logs", "Boss Lore", "Ending Text"]
    for index, chip in enumerate(chips):
        left = 178 + (index % 2) * 360
        top = 660 + (index // 2) * 58
        panel(draw, (left, top, left + 300, top + 40), COLORS["pink_soft"], None, 16, 0)
        draw_text(draw, chip, (left + 20, top + 7), font(22, True), COLORS["purple_dark"])
    image.convert("RGB").save(output, optimize=True)
    return output


def create_gameplay_loop_route() -> Path:
    output = VISUAL_DIR / "gameplay_loop_route.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Core Gameplay Loop", (90, 75), font(58, True), COLORS["purple_dark"])
    nodes = [
        ("进入房间", "Enter room"),
        ("读记忆日志", "Read log"),
        ("战斗", "Combat"),
        ("获得成长", "Growth"),
        ("解锁门", "Unlock"),
        ("Boss", "Boss"),
        ("结局", "Ending"),
    ]
    y = 430
    for index, (cn, en) in enumerate(nodes):
        x = 90 + index * 212
        panel(draw, (x, y - 82, x + 170, y + 82), COLORS["white"], COLORS["pink"] if index % 2 == 0 else COLORS["blue"], 30, 3)
        draw_text(draw, cn, (x + 24, y - 34), font(25, True), COLORS["purple_dark"], 122)
        draw_text(draw, en, (x + 24, y + 8), font(18), COLORS["muted"], 122)
        if index < len(nodes) - 1:
            draw.line((x + 172, y, x + 208, y), fill=COLORS["purple"], width=5)
            draw.polygon([(x + 208, y), (x + 193, y - 10), (x + 193, y + 10)], fill=COLORS["purple"])
    paste_center(image, player_pose(0), (120, 610, 260, 760))
    paste_center(image, enemy_frame(1), (640, 610, 800, 760))
    paste_center(image, trim_alpha(load_rgba(ITEM_ICONS[0][1]), 4), (950, 625, 1050, 725))
    paste_center(image, enemy_frame(3), (1260, 600, 1450, 780))
    image.convert("RGB").save(output, optimize=True)
    return output


def create_demo_timeline() -> Path:
    output = VISUAL_DIR / "demo_timeline.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "3-5 Minute Demo Route", (90, 70), font(58, True), COLORS["purple_dark"])
    points = [
        ("0:00", "开场"),
        ("0:30", "NPC / 日志"),
        ("1:00", "移动战斗"),
        ("2:00", "成长反馈"),
        ("3:00", "Boss"),
        ("4:00", "Victory"),
    ]
    draw.line((160, 310, 1440, 310), fill=COLORS["purple"], width=6)
    for index, (time, label) in enumerate(points):
        x = 160 + index * 256
        draw.ellipse((x - 24, 286, x + 24, 334), fill=COLORS["pink"] if index % 2 == 0 else COLORS["blue"])
        draw_text(draw, time, (x - 35, 230), font(25, True), COLORS["purple_dark"])
        draw_text(draw, label, (x - 65, 352), font(24, True), COLORS["muted"], 140)
    for index, (_, label) in enumerate(points):
        left = 95 + index * 245
        top = 515
        panel(draw, (left, top, left + 210, top + 145), COLORS["dark_panel"], COLORS["pink"], 20, 2)
        draw_text(draw, label, (left + 22, top + 30), font(22, True), COLORS["white"], 160)
        draw_text(draw, "录屏镜头节点", (left + 22, top + 78), font(17), (238, 246, 255, 230), 160)
    image.convert("RGB").save(output, optimize=True)
    return output


def create_tech_architecture() -> Path:
    output = VISUAL_DIR / "tech_architecture.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Unity 2D 技术结构", (90, 46), font(56, True), COLORS["purple_dark"])
    draw_text(draw, "按职责拆分模块：玩法、系统、叙事和构建流程彼此独立，方便多人协作。", (94, 118), font(25), COLORS["muted"], 1320)

    # The old radial chart crossed too many lines. This board uses lanes so the
    # PPT page stays readable at small size.
    lanes = [
        ("核心玩法", ["Player", "Combat", "Enemy", "Rooms"], COLORS["pink"], 170),
        ("成长系统", ["Backpack", "Craft", "Skill Tree", "Device"], COLORS["blue"], 398),
        ("叙事与界面", ["Dialogue", "NPC", "Memory Log", "UI"], COLORS["purple"], 626),
    ]

    for title, modules, accent, top in lanes:
        panel(draw, (100, top, 1500, top + 175), (255, 255, 255, 236), accent, 34, 4)
        draw_text(draw, title, (140, top + 36), font(34, True), COLORS["purple_dark"])
        for index, module in enumerate(modules):
            left = 370 + index * 270
            panel(draw, (left, top + 44, left + 230, top + 120), COLORS["blue_soft"] if index % 2 else COLORS["pink_soft"], None, 22, 0)
            draw_text(draw, module, (left + 24, top + 67), font(24, True), COLORS["purple_dark"], 200)

    panel(draw, (100, 828, 1500, 880), COLORS["dark_panel"], COLORS["blue"], 22, 3)
    draw_text(draw, "WebGL Build：可部署到 Render / Cloudflare Pages / GitHub Pages", (140, 840), font(23, True), COLORS["white"], 1200)
    image.convert("RGB").save(output, optimize=True)
    return output


def create_submission_status_board() -> Path:
    output = VISUAL_DIR / "submission_status_board.png"
    image = Image.new("RGBA", (1600, 900), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")
    draw_text(draw, "Submission Status", (90, 70), font(60, True), COLORS["purple_dark"])
    columns = [
        ("已完成 / Ready", ["海报 PNG + 源文件", "可编辑 PPTX + PDF", "真实素材展示图", "WebGL 构建脚本", "截图工具与指南"]),
        ("待人工回填 / Manual", ["WebGL 在线链接", "Demo 视频链接", "CodeBuddy 历史", "团队/学校信息", "Unity 实机截图"]),
    ]
    for index, (title, items) in enumerate(columns):
        left = 120 + index * 720
        panel(draw, (left, 190, left + 620, 760), COLORS["white"], COLORS["pink"] if index == 0 else COLORS["blue"], 34, 4)
        draw_text(draw, title, (left + 45, 235), font(36, True), COLORS["purple_dark"])
        for item_index, item in enumerate(items):
            top = 325 + item_index * 72
            panel(draw, (left + 48, top, left + 572, top + 46), COLORS["pink_soft"] if index == 0 else COLORS["blue_soft"], None, 16, 0)
            draw_text(draw, f"{'✓' if index == 0 else '□'} {item}", (left + 70, top + 8), font(23, True), COLORS["purple_dark"], 460)
    image.convert("RGB").save(output, optimize=True)
    return output


def screenshot_paths() -> list[Path]:
    directory = SUBMISSION_DIR / "screenshots"
    if not directory.exists():
        return []
    return sorted(path for path in directory.glob("*.png") if path.is_file())[:3]


def create_poster_png() -> Path:
    output = POSTER_PNG
    image = Image.new("RGBA", (1920, 1080), COLORS["paper"])
    draw_background(image, grid=False)
    draw = ImageDraw.Draw(image, "RGBA")

    image.alpha_composite(tile_strip(1920, 165), (0, 915))
    draw.rectangle((0, 735, 1920, 915), fill=(222, 245, 255, 105))

    # Title is important, but the game key visual owns the center.
    panel(draw, (70, 64, 820, 356), (255, 255, 255, 232), COLORS["pink"], 36, 4)
    draw_text(draw, "能工智人：遗忘工坊", (112, 102), font(58, True), COLORS["purple_dark"])
    draw_text(draw, "Craftsmen and Homo Sapiens: The Forgotten Forge", (116, 174), font(23, True), COLORS["purple"], 640)
    draw_text(draw, "叙事类游戏 / Narrative Games", (116, 222), font(25, True), COLORS["pink"])
    draw_text(draw, "AI 叙事驱动的横版动作冒险，在地下工坊的记忆日志中揭开文明冲突真相。", (116, 270), font(24), COLORS["ink"], 630)

    # Main confrontation.
    main = fit_image(player_pose(8), 390, 570, Image.Resampling.LANCZOS)
    image.alpha_composite(main, (800, 392))
    enemies = [(1, 1235, 505, 245), (0, 1330, 720, 190), (2, 1495, 640, 215), (3, 1645, 665, 205)]
    for enemy_index, x, y, size in enemies:
        sprite = fit_image(enemy_frame(enemy_index), size, size, Image.Resampling.LANCZOS)
        image.alpha_composite(sprite, (x, y))

    # Memory log panel as story proof.
    panel(draw, (86, 438, 680, 820), COLORS["dark_panel"], COLORS["blue"], 34, 4)
    draw_text(draw, "工坊记忆日志", (126, 482), font(36, True), COLORS["white"])
    draw_text(draw, "房间日志 02：人类与工匠的协议在炉心中断裂。仍在运行的装置只记得旧命令。", (126, 548), font(27), (238, 246, 255, 238), 500, 12)
    chips = ["世界观与剧情", "游戏原画", "房间日志", "Boss 背景"]
    for index, chip in enumerate(chips):
        left = 126 + (index % 2) * 250
        top = 708 + (index // 2) * 52
        panel(draw, (left, top, left + 214, top + 36), COLORS["pink_soft"], None, 14, 0)
        draw_text(draw, chip, (left + 16, top + 5), font(19, True), COLORS["purple_dark"])

    npc = fit_image(npc_frame(0), 180, 180, Image.Resampling.NEAREST)
    image.alpha_composite(npc, (610, 760))

    # Screenshot strip stays secondary until real captures exist.
    shots = screenshot_paths()
    labels = ["开场", "战斗", "Boss / 胜利"]
    for index in range(3):
        left = 880 + index * 308
        top = 274
        panel(draw, (left, top, left + 278, top + 150), (255, 255, 255, 236), COLORS["blue"], 22, 3)
        if index < len(shots):
            paste_center(image, load_rgba(shots[index]), (left + 10, top + 10, left + 268, top + 106))
            caption = labels[index]
        else:
            draw.rectangle((left + 12, top + 12, left + 266, top + 102), fill=(49, 42, 82, 220))
            draw_text(draw, "等待 Unity 截图回填", (left + 36, top + 43), font(18, True), COLORS["white"], 210)
            caption = f"{labels[index]} placeholder"
        draw_text(draw, caption, (left + 18, top + 116), font(16, True), COLORS["purple_dark"], 235)

    panel(draw, (1238, 64, 1810, 224), (255, 255, 255, 224), COLORS["pink"], 28, 3)
    draw_text(draw, "AI-assisted modules", (1272, 96), font(27, True), COLORS["purple_dark"])
    draw_text(draw, "Worldbuilding & Story  /  Game Key Art", (1274, 146), font(23), COLORS["muted"], 490)

    panel(draw, (70, 986, 1780, 1042), (255, 255, 255, 214), COLORS["amber"], 20, 2)
    draw_text(draw, "Team：待回填", (98, 1002), font(24, True), COLORS["purple_dark"])
    draw_text(draw, "Demo Link：待回填", (352, 1002), font(24, True), COLORS["purple_dark"])
    draw_text(draw, "No fake screenshots. Real Unity captures go under Submission/screenshots/.", (690, 1004), font(21), COLORS["purple"])

    image.convert("RGB").save(output, optimize=True, quality=92)
    return output


def create_svg_and_html() -> None:
    poster_b64 = base64.b64encode(POSTER_PNG.read_bytes()).decode("ascii")
    svg = f"""<svg xmlns="http://www.w3.org/2000/svg" width="1920" height="1080" viewBox="0 0 1920 1080">
<image href="data:image/png;base64,{poster_b64}" x="0" y="0" width="1920" height="1080"/>
<metadata>V2 poster source wrapper. Raster was generated from project assets by tools/generate_submission_visuals.py.</metadata>
</svg>
"""
    POSTER_SVG.write_text(svg, encoding="utf-8")
    POSTER_HTML.write_text(
        """<!doctype html>
<html lang="zh-CN">
<head>
  <meta charset="utf-8">
  <title>能工智人：遗忘工坊 Poster V2</title>
  <style>
    body { margin: 0; background: #fff8fd; font-family: "Microsoft YaHei", Arial, sans-serif; }
    main { max-width: 1280px; margin: 32px auto; padding: 24px; }
    img { width: 100%; border-radius: 24px; box-shadow: 0 24px 80px rgba(77, 48, 118, .22); }
    p { color: #5f527f; font-size: 18px; }
  </style>
</head>
<body>
<main>
  <img src="poster_1920x1080.png" alt="Craftsmen and Homo Sapiens V2 submission poster">
  <p>Run <code>python tools/generate_submission_visuals.py</code> after updating sprites or real Unity screenshots.</p>
</main>
</body>
</html>
""",
        encoding="utf-8",
    )


def deck_slides() -> list[dict[str, object]]:
    return [
        {
            "title": "能工智人：遗忘工坊",
            "subtitle": "AI 叙事驱动的横版动作冒险",
            "layout": "大主视觉 + 标题",
            "image": VISUAL_DIR / "hero_stage.png",
            "bullets": ["叙事类游戏 / Narrative Games", "Demo Link / Team：待回填"],
        },
        {
            "title": "游戏是什么",
            "subtitle": "进入会留下记忆的地下工坊",
            "layout": "左图右文",
            "image": VISUAL_DIR / "player_lineup.png",
            "bullets": ["横版动作", "房间推进", "AI 记忆日志"],
        },
        {
            "title": "核心玩法循环",
            "subtitle": "进入房间、读日志、战斗、成长、解锁、Boss、结局",
            "layout": "横向流程图",
            "image": VISUAL_DIR / "gameplay_loop_route.png",
            "bullets": ["短流程完整闭环", "适合 3-5 分钟答辩"],
        },
        {
            "title": "角色与怪物",
            "subtitle": "真实 spritesheet 支撑 Demo 表现",
            "layout": "阵容横排",
            "image": VISUAL_DIR / "enemy_lineup.png",
            "bullets": ["主角", "NPC", "普通怪 / 精英怪 / Boss 占位体"],
        },
        {
            "title": "AI 叙事如何进入游戏",
            "subtitle": "AI 内容变成工坊记忆日志和 NPC 对话",
            "layout": "日志 UI 模拟图",
            "image": VISUAL_DIR / "memory_log_mock.png",
            "bullets": ["世界观", "势力关系", "房间日志", "Boss 背景", "结局文本"],
        },
        {
            "title": "Demo 流程",
            "subtitle": "截图框保持诚实，等待 Unity 实机画面回填",
            "layout": "3-5 分钟时间轴",
            "image": VISUAL_DIR / "demo_timeline.png",
            "bullets": ["0:00 开场", "1:00 战斗", "3:00 Boss", "4:00 Victory"],
        },
        {
            "title": "技术与部署",
            "subtitle": "Unity 2D 原型，模块轻量拆分",
            "layout": "模块架构图",
            "image": VISUAL_DIR / "tech_architecture.png",
            "bullets": ["Player / Combat / Enemy / Rooms", "UI / Dialogue / Craft / Skill", "WebGL Build 准备"],
        },
        {
            "title": "亮点与提交状态",
            "subtitle": "已完成材料与待人工回填项分开呈现",
            "layout": "清单 + 路线图",
            "image": VISUAL_DIR / "submission_status_board.png",
            "bullets": ["已完成：海报、PPT、部署文档、截图工具", "待人工：部署、视频、CodeBuddy、团队信息"],
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
        layout = str(slide["layout"])
        bullets = [str(item) for item in slide["bullets"]]
        image_path = Path(str(slide["image"]))

        pdf.setFillColor(colors.HexColor("#FFF8FD"))
        pdf.rect(0, 0, 1280, 720, fill=1, stroke=0)
        pdf.setStrokeColor(colors.HexColor("#FF80B8"))
        pdf.setFillColor(colors.white)
        pdf.roundRect(36, 32, 1208, 656, 28, fill=1, stroke=1)

        if index == 1:
            pdf.drawImage(str(image_path), 410, 120, width=760, height=470, preserveAspectRatio=True, mask="auto")
            text_x, text_y = 72, 604
        elif index in (3, 5, 6, 7, 8):
            pdf.drawImage(str(image_path), 110, 164, width=1060, height=420, preserveAspectRatio=True, mask="auto")
            text_x, text_y = 72, 628
        elif index == 4:
            pdf.drawImage(str(image_path), 92, 250, width=1090, height=330, preserveAspectRatio=True, mask="auto")
            text_x, text_y = 72, 628
        else:
            pdf.drawImage(str(image_path), 70, 190, width=540, height=390, preserveAspectRatio=True, mask="auto")
            text_x, text_y = 670, 604

        pdf.setFont(font_name, 13)
        pdf.setFillColor(colors.HexColor("#FF80B8"))
        pdf.drawString(text_x, text_y + 38, f"{index:02d} / 08  {layout}")
        pdf.setFont(font_name, 34)
        pdf.setFillColor(colors.HexColor("#322853"))
        pdf.drawString(text_x, text_y - 10, title)
        pdf.setFont(font_name, 18)
        pdf.setFillColor(colors.HexColor("#695F84"))
        pdf.drawString(text_x, text_y - 46, subtitle[:56])

        y = text_y - 104
        pdf.setFont(font_name, 18)
        for bullet in bullets[:5]:
            pdf.setFillColor(colors.HexColor("#FF80B8"))
            pdf.circle(text_x + 8, y + 6, 6, fill=1, stroke=0)
            pdf.setFillColor(colors.HexColor("#322853"))
            pdf.drawString(text_x + 28, y, bullet[:40])
            y -= 34

        pdf.setFont(font_name, 10)
        pdf.setFillColor(colors.HexColor("#695F84"))
        pdf.drawString(72, 58, "Craftsmen and Homo Sapiens: The Forgotten Forge | V2 diverse-layout deck")
        pdf.showPage()

    pdf.save()
    return DECK_PDF


def create_project_deck_markdown() -> None:
    lines = [
        "# 能工智人：遗忘工坊 - Project Deck V2",
        "",
        "这份 Markdown 与 `Submission/project_deck.pptx` 的 V2 版式保持一致，用于快速改稿。",
        "",
    ]
    for index, slide in enumerate(deck_slides(), start=1):
        lines.append(f"## Slide {index}. {slide['title']}")
        lines.append("")
        lines.append(f"- Layout: {slide['layout']}")
        lines.append(f"- Subtitle: {slide['subtitle']}")
        lines.append(f"- Visual: `{Path(str(slide['image'])).as_posix()}`")
        lines.append("")
        for bullet in slide["bullets"]:
            lines.append(f"- {bullet}")
        lines.append("")
    DECK_MD.write_text("\n".join(lines), encoding="utf-8")


def create_notes() -> None:
    POSTER_NOTES.write_text(
        """# Poster Notes

V2 海报按照 `Submission/layout_plan_v2.md` 重做，目标是更像游戏海报而不是文档封面。

使用的真实素材：

- Player: `Assets/Art/Generated/Characters/cat_maid_magic_wand_spritesheet.png`
- Enemies: `Assets/Art/Generated/Enemies/*.png`
- NPC: `Assets/Art/Generated/NPC/`
- Items / Devices: `Assets/Art/Generated/Items/`, `Assets/Art/Generated/Devices/`
- Tiles: `Assets/Art/Tiles/`

海报仍然保留诚实截图占位。真实 Unity 截图需要后续放入 `Submission/screenshots/` 后重新生成。
""",
        encoding="utf-8",
    )

    DECK_NOTES.write_text(
        """# Project Deck Notes

V2 PPT 不再使用单一母版复制页面，而是按 `Submission/layout_plan_v2.md` 使用不同布局。

## Slide Layouts

1. Cover: 大主视觉 + 标题，使用 `hero_stage.png`。
2. 游戏是什么: 左图右文，使用 `player_lineup.png`。
3. 核心玩法循环: 横向流程图，使用 `gameplay_loop_route.png`。
4. 角色与怪物: 阵容展示，使用 `enemy_lineup.png`。
5. AI 叙事如何进入游戏: 日志 UI 模拟图，使用 `memory_log_mock.png`。
6. Demo 流程: 时间轴，使用 `demo_timeline.png`。
7. 技术与部署: 模块架构图，使用 `tech_architecture.png`。
8. 亮点与提交状态: 两栏状态板，使用 `submission_status_board.png`。

## Waiting For Human Backfill

- WebGL 在线链接
- Demo 视频链接
- CodeBuddy 历史导出
- Unity 真实截图，放入 `Submission/screenshots/`

PDF 和 PPTX 均由脚本重新导出。
""",
        encoding="utf-8",
    )

    SPEAKER_NOTES.write_text(
        """# PPT Speaker Notes

## 3-5 分钟讲稿

1. 封面：介绍项目《能工智人：遗忘工坊》，这是叙事类游戏赛道，一个 AI 叙事驱动的 2D 横版动作原型。
2. 游戏是什么：玩家进入地下工坊遗迹，通过移动、跳跃、近战战斗和房间推进读取记忆日志。
3. 核心循环：进入房间、读日志、战斗、获得成长、解锁下一房间，最后挑战 Boss 并看到结局文本。
4. 角色与怪物：展示主角、NPC 和怪物阵容，说明当前 Demo 已经具备可录屏的视觉资产。
5. AI 叙事：强调 AI 不是只写文档，而是生成世界观、房间档案、Boss 背景和结局文本，并进入 UI 和 NPC 对话。
6. Demo 流程：按时间轴说明录屏路线，真实截图稍后由 Unity 截图工具回填。
7. 技术结构：Unity 2D，Player、Combat、Enemy、Rooms、UI、Dialogue、Craft、Skill、WebGL Build 分模块。
8. 提交状态：海报、PPT、文档、截图工具已准备；WebGL 链接、Demo 视频、CodeBuddy 导出和团队信息需要人工回填。
""",
        encoding="utf-8",
    )


def visual_asset_rows() -> list[VisualAsset]:
    return [
        VisualAsset("Cat Maid Magic Wand", PLAYER_MAIN, "Player", "Poster hero, Slide 1/2, player lineup"),
        VisualAsset("Cat Maid Walk/Climb", PLAYER_WALK, "Player", "Player movement sheet and animation proof"),
        *[VisualAsset(name, path, "Enemy", "Enemy lineup, character cards, combat/boss explanation") for name, path in ENEMY_SHEETS],
        *[VisualAsset(name, path, "NPC", "Dialogue cast slide and AI narrative slide") for name, path in NPC_FRAMES],
        *[VisualAsset(name, path, "Item", "Crafting and progression visual accents") for name, path in ITEM_ICONS],
        *[VisualAsset(name, path, "Device", "Crafted device / turret visual proof") for name, path in DEVICE_IMAGES],
    ]


def write_visual_asset_inventory() -> None:
    lines = [
        "# Visual Asset Inventory",
        "",
        "This inventory records the real project assets used for the rebuilt poster and PPT. It avoids external game screenshots and avoids pretending that placeholder screenshot frames are real playtest captures.",
        "",
        "## Summary",
        "",
        "- Player assets are used for the poster hero, cover slide, and action lineup.",
        "- Enemy assets include four readable prototype enemies for demo variety.",
        "- NPC assets support dialogue and the memory-log narrative pitch.",
        "- Item and device icons show crafting, growth, and support-device features.",
        "- Real Unity screenshots are still needed for final submission polish.",
        "",
        "## Asset List",
        "",
        "| Category | Asset | Path | Suggested Use | Status |",
        "| --- | --- | --- | --- | --- |",
    ]
    for asset in visual_asset_rows():
        status = "Found" if asset.path.exists() else "Missing"
        rel = asset.path.relative_to(ROOT).as_posix()
        lines.append(f"| {asset.category} | {asset.name} | `{rel}` | {asset.suggested_use} | {status} |")

    lines.extend(
        [
            "",
            "## V2 Submission Visuals",
            "",
            "- `Submission/visual_assets/hero_stage.png`: poster/cover main visual.",
            "- `Submission/visual_assets/gameplay_loop_route.png`: gameplay flow slide.",
            "- `Submission/visual_assets/memory_log_mock.png`: AI narrative UI slide.",
            "- `Submission/visual_assets/demo_timeline.png`: recording route slide.",
            "- `Submission/visual_assets/tech_architecture.png`: technical structure slide.",
            "- `Submission/visual_assets/submission_status_board.png`: submission checklist slide.",
            "",
            "## Needs Human Capture",
            "",
            "The final deck still needs real Unity screenshots for opening, memory log UI, combat, growth, boss, and victory. Use `Tools/Hackathon/Capture Submission Screenshots` inside Unity and place the output under `Submission/screenshots/`.",
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
        create_hero_stage(),
        create_memory_log_mock(),
        create_gameplay_loop_route(),
        create_demo_timeline(),
        create_tech_architecture(),
        create_submission_status_board(),
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
