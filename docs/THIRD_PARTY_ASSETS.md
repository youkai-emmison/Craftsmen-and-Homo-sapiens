# Third Party Assets

This file records candidate or locally imported third-party assets. Do not treat an asset as cleared for public repository commits until its license and package files have been checked.

## Current Policy

- If the GitHub repository is public, do not commit Unity Asset Store raw files.
- Raw files include PNG, sprite sheets, animations, animator controllers, prefabs, demo scenes, audio, and package samples.
- Use third-party assets as local placeholders unless the team confirms that the repository is private and the license permits the workflow.

## Asset Records

| Asset Name | Asset Store URL | Publisher | Price | License | Status | Used In | Raw Asset Committed To Repository | License File Checked | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Bringer Of Death (free) | Unknown | Unknown | Free | Check Unity Asset Store page and package license | Imported Locally / Candidate | DemoBoss visual placeholder | Do Not Commit While Public | No | Use as a temporary dark boss visual only. Do not copy its design into original lore. |
| Enemy Galore 1 - Pixel Art | Unknown | Unknown | Free | Check Unity Asset Store page and package license | Imported Locally / Candidate | BasicEnemy visual placeholder | Do Not Commit While Public | No | Use for Early Room and Mid Room enemy placeholders if the style fits. |

## Update Rule

When an asset is actually used in the main scene, add:

- Exact Asset Store URL.
- Publisher name.
- Imported date.
- Which scene or prefab uses it.
- Whether raw files are committed.
- Whether the license file was checked.
