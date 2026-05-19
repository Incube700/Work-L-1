# UI Manual Setup

Use this after running `Tools -> Defend -> Build Artsystack UI`.

## Main Menu Scene

1. Open `MainMenuScene`.
2. Keep the existing `MainMenuScreenView`, `MainMenuView`, `PopupLayer`, `CurrencyListView`, and `StatsView` references intact.
3. Instantiate `Assets/_LocalGenerated/DefendUI/Prefabs/Generated_MainMenuPanel.prefab`.
4. If replacing the visible Play button, assign `Generated_MainMenuPanel/CenterPanel/PlayButton` to `MainMenuView._playButton`.
5. Assign optional visual references such as `_optionalSkinRoot`, `_backgroundImage`, `_frameImage`, `_buttonImage`, and `_label` only for Inspector clarity and validation.

Do not add scene-loading behavior to generated buttons. `MainMenuView` already raises `PlayClicked`, and the presenter owns flow decisions.

## Gameplay Scene

1. Open `GameplayScene`.
2. Keep `DefendGameplayScreenView`, `DefendHudView`, `PlacementPanelView`, `CurrencyListView`, and `PopupLayer` references intact.
3. Instantiate `Generated_GameplayHudPanel.prefab` for the top-bar visual pass.
4. Instantiate `Generated_PlacementPanelSkin.prefab` for placement-button visuals.
5. If replacing the old placement buttons, assign:
   - `Generated_PlacementPanelSkin/Buttons/MineButton` -> `PlacementPanelView._mineButton`
   - `Generated_PlacementPanelSkin/Buttons/TurretButton` -> `PlacementPanelView._turretButton`
   - `Generated_PlacementPanelSkin/Buttons/PuddleButton` -> `PlacementPanelView._puddleButton`
6. Keep cost text fields assigned to `PlacementPanelView._mineCostText`, `_turretCostText`, and `_puddleCostText`.
7. Assign optional generated skin fields only after required view references are still green.

Do not add placement, affordability, wave, combat, tower, enemy, save/load, or economy logic to UI views.

## Result Popup

1. Keep `Assets/_Project/Resources/UI/Popups/MessagePopupView.prefab` as the runtime popup prefab unless you intentionally replace it.
2. Use `Generated_ResultPopupSkin.prefab` as a visual reference.
3. If building a new popup body, keep `MessagePopupView` close behavior wired through `_okButton`.
4. Do not wire `RestartButton` or `ReturnToMenuButton` directly to gameplay from the generated skin. The current result flow returns to menu when the message popup closes.

## Currency Row

1. Keep `Assets/_Project/Resources/UI/Currency/CurrencyRowView.prefab` as the runtime row prefab unless you intentionally replace it.
2. Use `Generated_CurrencyRowSkin.prefab` as a visual reference for frame, icon, and text layout.
3. Keep `CurrencyRowView._iconImage`, `_nameText`, and `_amountText` assigned.
4. Assign Artsystack gold/diamond icons locally only if you accept missing references in public clones without the paid pack.

## Local Configs

The builder creates local ignored config assets under:

`Assets/_LocalGenerated/DefendUI/Configs/`

Runtime loading remains on tracked config assets under:

`Assets/_Project/Resources/Configs/DefendGame/`

Copy local icon assignments into tracked configs only when you explicitly accept the missing-reference tradeoff for machines without the paid pack.

## Validation

Run:

`Tools -> Defend -> Validate Active UI Setup`

Fix `Debug.LogError` messages first. Optional generated skin warnings are informational and do not block gameplay.

## Local Graveyard Defense Setup

Use this only for local testing with ignored/generated assets:

`Tools -> Defend -> Build Graveyard Defense Theme (Local)`

The generated theme pass writes to:

`Assets/_LocalGenerated/GraveyardDefense/`

This folder is ignored. It may reference `Assets/Toon_Zombies_extended/`, `Assets/MasterMagicFX/`, and other local/vendor packs. Do not commit these generated prefabs/configs or tracked scenes/configs that reference them unless the asset policy changes.

The local builder creates Resources-compatible wrappers and configs:

- `Resources/Entities/GraveyardDefense/ZombieEnemyView.prefab`
- `Resources/Entities/GraveyardDefense/RangedUndeadEnemyView.prefab`
- `Resources/Entities/GraveyardDefense/HeavyZombieEnemyView.prefab`
- `Resources/Entities/GraveyardDefense/MagicRuneMineView.prefab`
- `Resources/Entities/GraveyardDefense/MagicTotemTurretView.prefab`
- `Resources/Entities/GraveyardDefense/PoisonSwampPuddleView.prefab`
- `Resources/Entities/GraveyardDefense/RitualStoneBaseView.prefab`
- `Resources/Prefabs/GraveyardDefense/PurpleBoltProjectileView.prefab`
- `Resources/Configs/DefendGame/GraveyardDefense/GraveyardDefense_Level_01.asset`

For a quick local playtest:

1. Run the local Graveyard builder.
2. Inspect the generated wrappers and confirm the original `MonoEntity`, colliders, and registrators remain on the copied base prefab.
3. Temporarily assign `GraveyardDefense_Level_01.asset` through the gameplay path you are testing, or temporarily add it to `Assets/_Project/Resources/Configs/DefendGame/DefendLevelsConfig.asset`.
4. Test from `BootstrapScene` through `MainMenuScene` into `GameplayScene`.
5. Revert any tracked scene/config assignment before committing if it points at `_LocalGenerated`, `Toon_Zombies_extended`, `MasterMagicFX`, or Artsystack.

The generated local UI icon config can display `Gold` as `Souls`, `Diamond` as `Crystals`, `Mine` as `Rune`, `Turret` as `Totem`, `Puddle` as `Curse`, `Rest` as `Prepare`, and `Wave` as `Defend`. Keep enum names and gameplay code unchanged.
