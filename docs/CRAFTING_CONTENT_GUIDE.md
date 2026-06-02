# Crafting Content Guide

This file records the first batch of extra crafting content added for the demo. The goal is to give the Craft panel more than one recipe without turning it into a full equipment system.

## Added Materials

- CandyStick: existing material used by the original Candy Cane recipe.
- Rusty Gear: scrap machine part for rough weapons.
- Soul Thread: light magical thread for cloth gear.
- Moonlit Shard: blue shard for magic-leaning equipment.
- Burnt Cloth: armor and hood material.
- Aether Jam: strange magic core for charm and flask recipes.

## Added Equipment

- Gearspike Wand: Weapon, adds Damage and LightningDamage.
- Moonthread Hood: Helmet, adds MaxHealth and Dodge.
- Scrapguard Apron: Armor, adds Armor and MaxHealth.
- Static Charm: Amulet, adds Intelligence and LightningDamage.
- Emergency Flask: Flask, adds Vitality and MagicResist.

## Added Recipes

- Craft_02_GearspikeWand: Rusty Gear x2, Moonlit Shard x1.
- Craft_03_MoonthreadHood: Soul Thread x2, Burnt Cloth x1.
- Craft_04_ScrapguardApron: Burnt Cloth x2, Rusty Gear x1.
- Craft_05_StaticCharm: Moonlit Shard x2, Aether Jam x1.
- Craft_06_EmergencyFlask: Aether Jam x2, Soul Thread x1, Burnt Cloth x1.

## Scene Setup

`Assets/Scenes/SampleScene.unity` has been updated so the Player's `Crafting` component references the original Candy Cane recipe plus the five new recipes.

The Player's starting material list includes enough demo materials to craft each new recipe once. This is only for testing and can be reduced later.

## Icon Policy

The new material and equipment icons are original generated placeholder icons under:

`Assets/Art/Generated/Items/`

They use Unity 2022.3-compatible texture meta files with `serializedVersion: 12`. They are safe to commit and can be replaced later by approved third-party or final art icons.

## Test Steps

1. Open `Assets/Scenes/SampleScene.unity`.
2. Enter Play mode.
3. Open the backpack/crafting UI.
4. Click the Craft tab.
5. Confirm that Candy Cane plus five new recipes appear.
6. Select each recipe and confirm its icon, description, stats, and material requirements show.
7. Craft each new item once and confirm it appears in the inventory.

Do not add a full drop table, equipment balance pass, or shop system yet.
