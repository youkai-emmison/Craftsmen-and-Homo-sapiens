# Ground Turret Device Guide

This document records the first deployable device prefab used by the demo.

## Added Device

- Equipment item: `Clockwork Ground Turret`
- Device data: `GroundTurretDeviceData`
- Deploy effect: `DeployGroundTurretEffect`
- Player buff effect: `GroundTurretFocusBuff`
- Device prefab: `Assets/Art/Prefabs/DevicePrefabs/GroundTurret.prefab`
- Projectile prefab: `Assets/Art/Prefabs/DevicePrefabs/GroundTurretProjectile.prefab`
- Sprite assets:
  - `Assets/Art/Generated/Devices/ground_turret.png`
  - `Assets/Art/Generated/Devices/ground_turret_projectile.png`
  - `Assets/Art/Generated/Items/Equipment/GroundTurretIcon.png`

## Runtime Flow

1. `Clockwork Ground Turret` starts in the Player backpack for demo testing.
2. Click it in the backpack to equip it into a weapon hot slot.
3. Press `5` to use the first Device found in the weapon hot slots.
4. `DeployDeviceEffect` raycasts down to the `Ground` layer and places the turret on the floor.
5. `DeviceController` initializes from `GroundTurretDeviceData`.
6. The turret searches for `Enemy`, `Boss`, or objects on the `Enemy` layer.
7. The turret fires `GroundTurretProjectile`.
8. `GroundTurretFocusBuff` gives the player a short demo damage buff.

## Important Setup

- `SampleScene.unity` contains a `DeviceManager` root object.
- The turret prefab uses the `Hittable` layer so future enemies can treat it as a device target.
- The turret collider is a trigger so it does not block player movement.
- Texture meta files use Unity 2022.3-compatible `TextureImporter.serializedVersion: 12`.

## Test Steps

1. Open `Assets/Scenes/SampleScene.unity`.
2. Enter Play mode.
3. Open the backpack.
4. Click `Clockwork Ground Turret` to equip it into a weapon slot.
5. Stand on ground near enemies.
6. Press `5`.
7. Confirm the turret appears on the floor in front of the player.
8. Confirm the turret fires at nearby enemies or the Boss.
9. Confirm the turret expires after its duration or durability runs out.

## Not Implemented

- No full device crafting chain.
- No object pooling.
- No formal device UI.
- No advanced turret animation.
- No new equipment system beyond allowing Device equipment in weapon hot slots.
