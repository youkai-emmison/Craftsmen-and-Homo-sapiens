# Third Party Assets

This file records candidate or locally imported third-party assets. Do not treat an asset as cleared for public repository commits until its license and package files have been checked.

## Current Policy

- If the GitHub repository is public, do not commit Unity Asset Store raw files.
- Raw files include PNG, sprite sheets, animations, animator controllers, prefabs, demo scenes, audio, and package samples.
- Use third-party assets as local placeholders unless the team confirms that the repository is private and the license permits the workflow.
- Current scene setup may reference locally imported Asset Store files by their Unity package GUIDs. Teammates should import the same packages from Package Manager before testing the polished scene.

## Asset Records

| Asset Name | Asset Store URL | Publisher | Price | License | Status | Used In | Raw Asset Committed To Repository | License File Checked | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Bringer Of Death (free) | Unity Asset Store package page | Clembod | Free | Package license says personal/commercial use is allowed, but redistribution/resale is not allowed | Imported Locally / Used In Demo | DemoBoss visual placeholder | No, ignored while repository is public | Yes, package License.txt checked | Use as a temporary dark boss visual only. Do not copy its design into original lore. |
| Enemy Galore 1 - Pixel Art | Unity Asset Store package page | Admurin | Free | Check Unity Asset Store page and package files | Imported Locally / Used In Demo | BasicEnemy visual placeholder | No, ignored while repository is public | No package license file found locally | Used for Early Room and Mid Room enemy placeholders. |
| [FW]2D Animated Cute Girl Full & SD Character | Unity Asset Store package page | freeworkStudio | Free | Check Unity Asset Store page and package files | Imported Locally / Used In Demo | Player visual placeholder | No, ignored while repository is public | Readme PDF present, license still needs manual confirmation | Used as a temporary Player visual. Gameplay collider and scripts stay on the Player root. |
| Free 2D Cartoon Parallax Background | Unity Asset Store package page | CPasteGame | Free | Check Unity Asset Store page and package files | Imported Locally / Used In Demo | SampleScene backdrop | No, ignored while repository is public | No package license file found locally | Used as a temporary background to avoid a black or empty scene. |
| Free Casual GUI | Unity Asset Store package page | Sky Den Games | Free | Check Unity Asset Store page and package files | Imported Locally / Not Used Yet | Future UI polish candidate | No, ignored while repository is public | Not checked | Imported locally only; not referenced by the current scene. |

## Update Rule

When an asset is actually used in the main scene, add:

- Exact Asset Store URL.
- Publisher name.
- Imported date.
- Which scene or prefab uses it.
- Whether raw files are committed.
- Whether the license file was checked.
