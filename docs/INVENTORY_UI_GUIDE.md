# Inventory UI Guide

## Current Goal

This branch adds the first minimal backpack UI for testing only.

The goal is:

1. Press `I` to open or close the backpack.
2. Show a fixed 8-slot inventory panel.
3. Display several test items from `InventoryManager`.
4. Keep inventory data separate from UI rendering.

## Scene Setup

Open:

`Assets/Scenes/SampleScene.unity`

The scene now contains:

- `InventorySystem`
- `InventoryCanvas`
- `InventoryPanel`
- `InventoryHint`
- `EventSystem`

`InventorySystem` owns the data and input components:

- `InventoryManager`
- `InventoryInputController`

`InventoryCanvas` owns the visible UI objects:

- `InventoryPanel`
- `Slot_01` to `Slot_08`

## How To Test

1. Open `Assets/Scenes/SampleScene.unity`.
2. Press Play.
3. Click the Game view once so it has keyboard focus.
4. Press `I`.
5. The backpack panel should open in the top-right corner.
6. Press `I` again.
7. The backpack panel should close.

## Script Responsibilities

`InventoryItem`

- Stores one item entry.
- Does not create UI.
- Does not change player stats.

`InventoryManager`

- Owns the current item list.
- Sends a change event when item data changes.
- Does not render UI.

`InventoryPanel`

- Shows items from `InventoryManager`.
- Controls panel visibility through `CanvasGroup`.
- Does not read input.

`InventorySlotView`

- Shows one item slot.
- Displays a color swatch when no icon is assigned.
- Does not modify inventory data.

`InventoryInputController`

- Reads the `I` key.
- Toggles `InventoryPanel`.
- Does not own item data.

`InventorySceneSetupBuilder`

- Editor-only tool.
- Menu path: `Tools/Inventory/Create Minimal Inventory UI`.
- Rebuilds the minimal inventory UI in the open scene.

## Not Implemented Yet

This branch does not implement:

- Item pickup.
- Enemy drops.
- Equipment stats.
- Drag and drop.
- Item tooltips.
- Save/load.
- Shop.
- Crafting.
- Skill tree.
- Full UI art polish.

## Notes

The player can still be a red square on `master`.
That is separate from this backpack task.
