# Hackathon Submission Checklist

最终提交前按此清单检查。Codex 本次只准备材料，不实际部署，不录制最终视频，不伪造 CodeBuddy 历史。

## Required Materials

- [ ] Poster: `Submission/poster_1920x1080.png`
- [ ] Poster source: `Submission/poster_source.svg`
- [ ] Poster HTML source: `Submission/poster_source.html`
- [ ] Project deck: `Submission/project_deck.pptx`
- [ ] Project deck PDF: `Submission/project_deck.pdf`
- [ ] Source package: `Submission/Craftsmen-and-Homo-sapiens_Source.zip`
- [ ] Playable WebGL link
- [ ] Demo video link
- [ ] CodeBuddy / AI conversation history
- [ ] GitHub repository link
- [ ] Team / school / captain information

## Visual Proof

- [x] Real project asset lineup generated under `Submission/visual_assets/`.
- [x] Poster uses project player, enemy, NPC, item, device, and tile visuals.
- [x] PPT uses real visual assets on each slide.
- [ ] Real Unity screenshots captured under `Submission/screenshots/`.
- [ ] Poster / PPT screenshot placeholders replaced after real captures if time allows.

## Ready-To-Paste Copy

Use:

```text
docs/SUBMISSION_FORM_COPY.md
```

## WebGL

- [ ] Unity Console has no red compile errors.
- [ ] `Assets/Scenes/SampleScene.unity` is playable.
- [ ] WebGL build generated to `Build/WebGL`.
- [ ] Deploy folder prepared at `Submission/WebGLSite`.
- [ ] Online link opens in a browser.
- [ ] Link is not `localhost`.

## Demo Video

Use:

```text
docs/DEMO_RECORDING_GUIDE.md
```

The video should show:

- [ ] Title / opening
- [ ] NPC or memory-log narrative
- [ ] Movement and jump
- [ ] Combat
- [ ] Growth / progression
- [ ] Boss fight
- [ ] Victory / ending

## AI Usage

- [ ] AI worldbuilding described.
- [ ] AI-generated room memory logs described.
- [ ] Boss lore and ending text described.
- [ ] AI-assisted visual asset presentation described.
- [ ] Codex/AI development assistance described.
- [ ] CodeBuddy export placeholder filled later.

Use:

```text
docs/AI_CREATION_LOG.md
```

## Source Package

Generate locally:

```powershell
powershell -ExecutionPolicy Bypass -File tools/package_submission.ps1
```

Do not include:

- `Library`
- `Temp`
- `Logs`
- `Obj`
- `UserSettings`
- `Build`
- `Builds`
- `.git`
- `.vs`
- `.idea`
- `*.csproj`
- `*.sln`

## Final Link Backfill

When all external links are ready, update:

- `docs/SUBMISSION_FORM_COPY.md`
- final submission form
- optional existing files under `submissions/`

If using the existing link-backfill helper, read:

```text
submissions/LINK_BACKFILL_TOOL_ZH.md
```
