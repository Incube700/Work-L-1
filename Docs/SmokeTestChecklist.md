# Smoke Test Checklist

Use Unity `2022.3.16f1`.

## Bootstrap Flow

1. Open `Assets/_Project/Scenes/BootstrapScene.unity`.
2. Enter Play Mode.
3. Confirm the main menu opens.
4. Confirm currency and stats are visible.
5. Click Play.
6. Confirm `GameplayScene` opens.

## Gameplay HUD

1. Confirm wave text is visible.
2. Confirm phase/status text changes between build/rest and defend/wave.
3. Confirm base health text and slider are visible.
4. Confirm currency rows show values and icons.
5. Confirm result popup appears on win or loss.

## Placement Panel

1. During build/rest phase, confirm Mine, Turret, and Puddle controls are visible.
2. Confirm costs display as gold costs.
3. Spend enough gold to make an option unaffordable.
4. Confirm unaffordable placement buttons become non-interactable.
5. Confirm the placement services still reject unaffordable placement if invoked.

## Combat Loop

1. During wave phase, click the field to shoot from the base.
2. Confirm enemies spawn and move toward the base.
3. Confirm clearing all configured waves produces victory.
4. Confirm base destruction produces defeat.
5. Confirm the result popup returns to the main menu.

## Main Menu

1. Confirm Play starts a tower-defence level.
2. Confirm Reset either resets progress or shows a not-enough-currency popup.
3. Confirm the permanent upgrades panel opens and closes.
4. Confirm upgrade buttons update based on purchase and diamonds.

## Direct Scene Testing

1. Open `Assets/_Project/Scenes/MainMenuScene.unity`.
2. Enter Play Mode and confirm the standalone menu path initializes.
3. Open `Assets/_Project/Scenes/GameplayScene.unity`.
4. Enter Play Mode and confirm direct gameplay fallback starts the first configured level.

## Local Graveyard Defense Theme

Run this only if the local packs are present:

`Tools -> Defend -> Build Graveyard Defense Theme (Local)`

Then run a local-only playtest:

1. Confirm generated wrappers exist under `Assets/_LocalGenerated/GraveyardDefense/Resources/`.
2. Temporarily test `GraveyardDefense_Level_01.asset` without committing tracked config or scene changes.
3. Confirm zombies spawn using the same enemy movement, health, death, and base-damage behavior as the old enemies.
4. Confirm ranged undead can shoot and purple projectiles still collide/damage through the existing projectile systems.
5. Confirm Rune, Totem, and Curse placement buttons still raise the existing Mine, Turret, and Puddle view events.
6. Confirm the themed wrappers preserve colliders and `MonoEntity` registration from the base dummy prefabs.
7. Confirm the graveyard/ritual stone visuals do not block placement, projectile movement, or enemy movement.
8. Confirm the HUD still updates wave/horde, phase, base health, and currency through presenters.
9. Confirm the result popup still appears and closes/returns to menu through the existing popup flow.
10. Remove or revert any tracked scene/config changes that reference `_LocalGenerated`, `Toon_Zombies_extended`, `MasterMagicFX`, or Artsystack before committing.

If local-only assets are absent, the tracked prototype should still run with the original dummy visuals and generated wrapper creation may fall back to primitive placeholder visuals.
