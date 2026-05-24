# Inventory UI Guide

## Current Goal

This branch adds the first minimal backpack UI for testing only.

The goal is:

1. Press `I` to open or close the backpack.
2. Press `Esc` to close the backpack when it is open.
3. Show a fixed 8-slot inventory panel.
4. Display several test items from `InventoryManager`.
5. Keep inventory data separate from UI rendering.

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
6. Press `Esc`.
7. The backpack panel should close.
8. Press `I` twice to confirm toggle open and close also works.

## Inspector Variables

`InventoryManager`

- `startingItems`: Test items loaded when the scene starts.
- `itemId`: Stable id used when stackable items merge.
- `displayName`: Name shown in a slot.
- `description`: Reserved for later tooltip text.
- `quantity`: Stack count shown in a slot.
- `canStack`: Whether items with the same id merge.
- `icon`: Optional sprite icon.
- `itemColor`: Color swatch shown when no icon is assigned.

`InventoryInputController`

- `inventoryPanel`: The panel controlled by keyboard input.
- `toggleKey`: Opens or closes the backpack. Current value: `I`.
- `closeKey`: Closes the backpack only. Current value: `Escape`.

`InventoryPanel`

- `inventoryManager`: Data source for the UI.
- `canvasGroup`: Shows or hides the panel without destroying objects.
- `slotViews`: Fixed slot views used by the current 8-slot backpack.
- `emptyMessageText`: Message shown only when the inventory has no items.
- `startsVisible`: Whether the backpack starts open.

`InventorySlotView`

- `slotBackgroundImage`: Background image for one slot.
- `iconImage`: Item icon or color swatch.
- `itemNameText`: Text used for the item name.
- `quantityText`: Text used for stack count.
- `emptyColor`: Slot color when empty.
- `filledColor`: Slot color when an item exists.

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
