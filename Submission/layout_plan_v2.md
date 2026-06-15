# Layout Plan V3

This plan is the visual contract for the cleaner poster and 7-slide deck. The goal is to keep the cute workshop game style while removing repeated frames, crowded cards, and status pages that talk about unfinished work.

## Poster Layout

### Purpose

Show the project as a cute workshop side-scrolling narrative action game at first glance.

### Structure

- Canvas: 1920 x 1080, 16:9.
- Safe margin: 72 px minimum.
- Left top title block: project title, English subtitle, track, one-sentence pitch.
- Center stage: player sprite standing on tile ground.
- Right stage: enemy sprites forming a readable combat relationship.
- Left lower panel: "Workshop Memory Log" UI mock, showing that AI narrative appears inside the game experience.
- Bottom strip: team and demo link placeholders.

### Overlap Avoidance

- Title block cannot exceed 42% of poster width.
- Main character must not overlap title text.
- Memory log panel text uses high contrast and short lines.
- Footer text sits on a light strip so it does not disappear into the tile ground.

## PPT Layout Plan

The deck now has 7 slides. The old submission/status page has been removed.

### Slide 1. Cover

- Purpose: make the deck feel like a game pitch immediately.
- Layout type: hero key visual.
- Visuals: `hero_stage.png`, player, enemies, tile ground.
- Text area: top-left title group only.
- Avoid overlap: no bullet cards on this slide.

### Slide 2. What Is The Game

- Purpose: explain the game in one sentence and three keywords.
- Layout type: left image, right explanation.
- Visuals: `player_lineup.png`.
- Text area: right column with one short paragraph and three large keywords.
- Avoid overlap: no chip boxes; keywords are plain colored text.

### Slide 3. Core Gameplay Loop

- Purpose: show the full loop without bullet-page fatigue.
- Layout type: horizontal route / flow map.
- Visuals: `gameplay_loop_route.png`.
- Avoid overlap: route diagram is centered with generous whitespace.

### Slide 4. Cast And Enemies

- Purpose: show character and enemy variety.
- Layout type: two clean lineup images.
- Visuals: `player_lineup.png`, `enemy_lineup.png`.
- Avoid overlap: no outer card frame; lineups use one stage band instead of many small cards.

### Slide 5. AI Narrative In Game

- Purpose: prove AI content enters UI and gameplay.
- Layout type: large memory-log UI mock plus short right-side explanation.
- Visuals: `memory_log_mock.png`.
- Avoid overlap: the log window and text column are separate.

### Slide 6. Demo Flow

- Purpose: give teammate a 3-5 minute recording route.
- Layout type: timeline.
- Visuals: `demo_timeline.png`.
- Avoid overlap: screenshot-node labels stay inside one clean timeline image.

### Slide 7. Tech Structure

- Purpose: show implementation structure quickly.
- Layout type: module architecture diagram.
- Visuals: `tech_architecture.png`.
- Avoid overlap: node graph is centered and not wrapped in another heavy frame.

## Diversity Check

- Slide 1: hero key visual.
- Slide 2: left image / right text.
- Slide 3: route diagram.
- Slide 4: two-lineup visual spread.
- Slide 5: UI mock.
- Slide 6: timeline.
- Slide 7: architecture diagram.

No two adjacent slides use the same layout.

## Shared Visual Rules

- Titles: at least 40 pt in PPT.
- Body text: at least 22 pt in PPT where possible.
- Fewer decorative circles; keep them near page edges.
- No image stretching.
- Avoid nested borders and card walls.
- No page dedicated to unfinished submission status.
- No fake deployment, video, screenshot, or CodeBuddy history.
