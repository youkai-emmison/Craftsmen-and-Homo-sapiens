"""Clean user-provided gameplay screenshots for submission materials.

The script keeps the screenshots honest: it removes recording overlays and
video captions, but it does not invent UI or gameplay that was not captured.
"""

from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter, ImageOps


ROOT = Path(__file__).resolve().parents[1]
RAW_DIR = ROOT / "Submission" / "raw_screenshots"
CLEAN_DIR = ROOT / "Submission" / "clean_screenshots"
OUTPUT_SIZE = (1280, 720)


@dataclass(frozen=True)
class ScreenshotRecipe:
    source: str
    output: str
    crop_box: tuple[int, int, int, int]
    note: str
    cover_boxes: tuple[tuple[int, int, int, int, tuple[int, int, int]], ...] = ()


RECIPES = [
    ScreenshotRecipe(
        "01_move_jump_attack_raw.png",
        "01_move_jump_attack_clean.png",
        (0, 0, 1257, 650),
        "Cropped bottom video subtitle and covered the NVIDIA recording prompt.",
        ((880, 0, 1257, 150, (119, 191, 229)),),
    ),
    ScreenshotRecipe(
        "02_npc_dialogue_raw.png",
        "02_npc_dialogue_clean.png",
        (0, 0, 1254, 640),
        "Cropped bottom video subtitle while preserving the NPC dialogue box.",
    ),
    ScreenshotRecipe(
        "03_backpack_raw.png",
        "03_backpack_clean.png",
        (34, 42, 1218, 662),
        "Cropped to the backpack, equipment, stat and character panels.",
    ),
    ScreenshotRecipe(
        "04_crafting_raw.png",
        "04_crafting_clean.png",
        (430, 80, 1160, 632),
        "Cropped away the top subtitle and focused on materials, item preview and Craft button.",
    ),
    ScreenshotRecipe(
        "05_skilltree_raw.png",
        "05_skilltree_clean.png",
        (46, 42, 1182, 596),
        "Cropped away the bottom subtitle and kept the skill tree and skill detail panel.",
    ),
    ScreenshotRecipe(
        "06_boss_combat_raw.png",
        "06_boss_combat_clean.png",
        (0, 0, 1248, 730),
        "Kept the full Boss combat frame as the clean gameplay climax shot.",
    ),
]


def ensure_dirs() -> None:
    CLEAN_DIR.mkdir(parents=True, exist_ok=True)


def require_sources() -> None:
    missing = [recipe.source for recipe in RECIPES if not (RAW_DIR / recipe.source).exists()]
    if missing:
        missing_text = ", ".join(missing)
        raise FileNotFoundError(f"Missing raw screenshots under {RAW_DIR}: {missing_text}")


def cover_non_game_overlays(image: Image.Image, cover_boxes: tuple[tuple[int, int, int, int, tuple[int, int, int]], ...]) -> None:
    draw = ImageDraw.Draw(image)
    for left, top, right, bottom, color in cover_boxes:
        draw.rectangle((left, top, right, bottom), fill=color)


def fit_without_distortion(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    """Place a crop on a blurred copy so non-16:9 crops do not stretch."""

    background = ImageOps.fit(image, size, method=Image.Resampling.LANCZOS, centering=(0.5, 0.5))
    background = background.filter(ImageFilter.GaussianBlur(18)).convert("RGB")
    foreground = ImageOps.contain(image, size, method=Image.Resampling.LANCZOS)
    left = (size[0] - foreground.width) // 2
    top = (size[1] - foreground.height) // 2
    background.paste(foreground, (left, top))
    return background


def clean_one(recipe: ScreenshotRecipe) -> Path:
    source_path = RAW_DIR / recipe.source
    image = Image.open(source_path).convert("RGB")
    cover_non_game_overlays(image, recipe.cover_boxes)
    cropped = image.crop(recipe.crop_box)
    cleaned = fit_without_distortion(cropped, OUTPUT_SIZE)
    output_path = CLEAN_DIR / recipe.output
    cleaned.save(output_path, optimize=True, quality=92)
    return output_path


def create_boss_hero_crop() -> Path:
    source_path = RAW_DIR / "06_boss_combat_raw.png"
    image = Image.open(source_path).convert("RGB")
    # Full-frame 16:9 conversion keeps the player, Boss and health bar together.
    hero = ImageOps.fit(image, (1920, 1080), method=Image.Resampling.LANCZOS, centering=(0.5, 0.48))
    output_path = CLEAN_DIR / "06_boss_combat_hero_crop.png"
    hero.save(output_path, optimize=True, quality=92)
    return output_path


def create_contact_sheet(clean_paths: list[Path]) -> Path:
    thumbnail_size = (384, 216)
    gap = 28
    label_height = 36
    columns = 3
    rows = 2
    sheet_width = columns * thumbnail_size[0] + (columns + 1) * gap
    sheet_height = rows * (thumbnail_size[1] + label_height) + (rows + 1) * gap
    sheet = Image.new("RGB", (sheet_width, sheet_height), (255, 248, 253))
    draw = ImageDraw.Draw(sheet)

    for index, path in enumerate(clean_paths):
        row = index // columns
        column = index % columns
        left = gap + column * (thumbnail_size[0] + gap)
        top = gap + row * (thumbnail_size[1] + label_height + gap)
        thumbnail = Image.open(path).convert("RGB").resize(thumbnail_size, Image.Resampling.LANCZOS)
        sheet.paste(thumbnail, (left, top + label_height))
        draw.text((left, top), path.name, fill=(50, 40, 83))

    output_path = CLEAN_DIR / "screenshot_contact_sheet.png"
    sheet.save(output_path, optimize=True, quality=90)
    return output_path


def write_readme(clean_paths: list[Path], hero_path: Path) -> None:
    lines = [
        "# Clean Submission Screenshots",
        "",
        "These images are cleaned from the six gameplay screenshots provided by the team.",
        "Cleaning only removes recording overlays, subtitles, or irrelevant margins; it does not fabricate gameplay UI.",
        "",
        "## Files",
        "",
        "| Clean File | Raw Source | Cleanup | Best Use |",
        "| --- | --- | --- | --- |",
    ]

    uses = {
        "01_move_jump_attack_clean.png": "PPT Slide 2, Demo timeline",
        "02_npc_dialogue_clean.png": "PPT Slide 3, poster narrative panel",
        "03_backpack_clean.png": "PPT Slide 4, system-growth proof",
        "04_crafting_clean.png": "PPT Slide 5, crafting system proof",
        "05_skilltree_clean.png": "PPT Slide 5, skill growth proof",
        "06_boss_combat_clean.png": "PPT Slide 7, boss proof",
    }

    for recipe, path in zip(RECIPES, clean_paths):
        lines.append(f"| `{path.name}` | `{recipe.source}` | {recipe.note} | {uses[path.name]} |")

    lines.extend(
        [
            f"| `{hero_path.name}` | `06_boss_combat_raw.png` | 16:9 hero crop for large visual placement. | Poster background, PPT cover |",
            "",
            "## Manual Check",
            "",
            "- Confirm no video subtitles remain in the final poster and deck.",
            "- Confirm the cropped UI still shows the required backpack, crafting and skill-tree information.",
            "- If the final demo recording changes, replace the raw files and rerun `python tools/clean_submission_screenshots.py`.",
        ]
    )

    (CLEAN_DIR / "README.md").write_text("\n".join(lines), encoding="utf-8")


def main() -> None:
    ensure_dirs()
    require_sources()
    clean_paths = [clean_one(recipe) for recipe in RECIPES]
    hero_path = create_boss_hero_crop()
    contact_sheet = create_contact_sheet(clean_paths)
    write_readme(clean_paths, hero_path)
    print("Cleaned submission screenshots:")
    for path in clean_paths:
        print(f"- {path.relative_to(ROOT).as_posix()}")
    print(f"- {hero_path.relative_to(ROOT).as_posix()}")
    print(f"- {contact_sheet.relative_to(ROOT).as_posix()}")


if __name__ == "__main__":
    main()
