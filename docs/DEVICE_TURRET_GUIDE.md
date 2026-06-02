# Device Turret Guide

## Current Scope

This pass adds one demo mechanical device:

- Equipment item: `Arcane Bolt Turret`
- Device data: `ArcaneBoltTurretDevice`
- Deploy effect: `DeployArcaneBoltTurret`
- Runtime prefab: `ArcaneBoltTurret`
- Projectile prefab: `ArcaneBoltProjectile`

It is a small demo device, not a complete mechanical system.

## Asset Locations

Generated original placeholder art:

- `Assets/Art/Generated/Devices/ArcaneBoltTurretIcon.png`
- `Assets/Art/Generated/Devices/ArcaneBoltTurretSprite.png`
- `Assets/Art/Generated/Devices/ArcaneBoltProjectile.png`

Prefabs:

- `Assets/Art/Prefabs/DevicePrefabs/ArcaneBoltTurret.prefab`
- `Assets/Art/Prefabs/DevicePrefabs/ArcaneBoltProjectile.prefab`

Item data:

- `Assets/Art/Prefabs/ItemPrefabs/Equipments/ArcaneBoltTurret.asset`
- `Assets/Art/Prefabs/ItemPrefabs/Equipments/ArcaneBoltTurretDevice.asset`
- `Assets/Art/Prefabs/ItemPrefabs/Equipments/DeployArcaneBoltTurret.asset`
- `Assets/Art/Prefabs/ItemPrefabs/Equipments/Craft_07_ArcaneBoltTurret.asset`

The PNG meta files use Unity 2022.3-compatible `TextureImporter.serializedVersion: 12`.

## How It Works

`Arcane Bolt Turret` is a `DeviceEquipmentData` item. When the player equips it from the backpack, `Equipment` runs the item effect attached to the equipment. The effect deploys `ArcaneBoltTurret.prefab` slightly in front of the player.

The turret uses `DeviceController`:

- reads attack range, duration, interval, and projectile prefab from `DeviceData`;
- targets objects with `Enemy` Tag or `Enemy` Layer;
- fires `ArcaneBoltProjectile`;
- expires after its configured duration.

The projectile uses `DeviceProjectile` and damages targets implementing `IDamageable`.

## SampleScene Setup

`Assets/Scenes/SampleScene.unity` now includes:

- `Arcane Bolt Turret` in the player's starting equipment list;
- `Craft_07_ArcaneBoltTurret` in the player's crafting recipe list.

The existing starting materials are enough to craft the turret once.

## How To Test

1. Open `Assets/Scenes/SampleScene.unity`.
2. Press Play.
3. Open the backpack.
4. Click `Arcane Bolt Turret` to equip it.
5. A turret should appear in front of the player.
6. Move near an enemy.
7. The turret should fire arcane bolts automatically.

If it deploys but does not shoot, check the enemy root object:

- Tag should be `Enemy`, or
- Layer should be `Enemy`.

The current code supports both because existing enemy prefabs may use the layer without the tag.

## Not Implemented

This pass does not add:

- object pooling;
- advanced turret targeting;
- formal device UI;
- durability display;
- upgrade trees;
- crafting balance;
- new boss behavior.

Keep the turret as a small demo proof that the mechanical-device scripts are usable.
