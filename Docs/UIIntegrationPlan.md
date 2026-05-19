# UI Integration Plan

## Goal

Polish the Tower Defence UI while preserving the existing `BootstrapScene -> MainMenuScene -> GameplayScene` flow and keeping paid Artsystack source assets out of the public repository.

## Current Active UI

Gameplay:

- `DefendGameplayScreenView` is the scene-owned root for the active gameplay UI.
- `DefendHudView` displays wave, phase, rest timer, and base health.
- `PlacementPanelView` displays Mine, Turret, and Puddle choices and raises selection events.
- `CurrencyListView` hosts runtime-created currency rows.
- `MessagePopupView` is the result popup used for victory/defeat.

Main menu:

- `MainMenuScreenView` binds menu UI, popup layer, currency list, and stats.
- `MainMenuView` raises play/reset/upgrades events.
- `PermanentUpgradesMenuView` displays upgrade entries and purchase events.
- `StatsView` displays wins/losses.

## Currently Used UI Prefabs

- `Assets/_Project/Resources/UI/Currency/CurrencyRowView.prefab`
- `Assets/_Project/Resources/UI/Popups/MessagePopupView.prefab`
- `Assets/_Project/Resources/Prefabs/DefendHudView.prefab` exists, though the active `GameplayScene` also owns its HUD hierarchy directly.

The gameplay HUD and placement panel are currently scene-owned in `GameplayScene`.

## Asset Strategy

Project-owned assets:

- `Assets/_Project/Art/UI.png`
- `Assets/_Project/Resources/UI/Currency/CurrencyRowView.prefab`
- `Assets/_Project/Resources/UI/Popups/MessagePopupView.prefab`
- `Assets/_Project/Resources/Configs/DefendGame/DefendUIIconConfig.asset`
- `Assets/_Project/Resources/Configs/DefendGame/DefendPresentationFeedbackConfig.asset`

Local-only paid reference:

- `Assets/Artsystack - Fantasy RPG GUI`

The Artsystack pack is ignored by Git and should be used as a local Inspector reference only. Do not copy its sprites, prefabs, PSDs, fonts, previews, or demo scenes into `Assets/_Project`.

## Artsystack Candidate Usage

Main menu:

- `button_01.png` or `button_02.png` for the Play button background.
- `BlueFrame_bg.png`, `header_box.png`, or `panel_name_header.png` for title/menu panels.
- `Title.prefab` and `Preview Images/title.png` as composition references only.

Gameplay HUD:

- `coins_frame.png` or `coins_frame_2.png` for the currency panel.
- `coin_1.png`, `btn_coin.png`, `crystal_1.png`, or `btn_crystal_1.png` for local resource icons.
- `heart_fill.png`, `heart_frame.png`, or `btn_Heart.png` for base health visuals.
- `progress_bar_bg.png` and `progress_bar_top.png` for health/progress bar styling.
- `SmallBlueFrame_bg.png`, `GreenFrame_bg.png`, and `RedFrame_bg.png` for wave/phase labels.

Placement panel:

- `ingame_icon_slot.png`, `icon_slot_active.png`, and `icon_slot_locked.png` for placeable slots.
- `btn_bomb.png` for Mine.
- `btn_archery.png` or `btn_castle.png` for Turret.
- Puddle has no exact icon match; use text for now or assign a local placeholder only.

Result popup:

- `pop_up.png`, `panel_name_header.png`, `btn_check.png`, and `btn_caution.png` are good local candidates.
- Current supported result flow is OK/close, then return to main menu.

## Config Preparation

`DefendUiIconConfig` exists as a minimal ScriptableObject for:

- Currency display names/icons
- Placeable display names/icons
- Phase display names/icons
- Enemy display names/icons

`DefendPresentationFeedbackConfig` exists as a minimal ScriptableObject for:

- UI click/confirm/denied SFX
- Wave/result/placement/combat SFX cues
- Lightweight presentation VFX prefab cues

These configs are prepared but not wired into runtime systems yet. Keep runtime fallback behavior in place before depending on local-only asset assignments.

## Validation Added

The active UI views now log clear `Debug.LogError` messages for missing important serialized references. Messages include the GameObject name and component name.

Validated areas:

- Main menu buttons and upgrade menu references
- Gameplay screen root references
- HUD text/slider references
- Placement buttons and cost text references
- Currency list/row references
- Result popup title/message/icon/button references
- Stats text references

An optional read-only Editor tool is available at:

`Tools -> Defend -> UI -> Validate Selected UI`

It can also generate:

`Docs/DefendUiSetupValidationReport.md`

Only run the report command when you intentionally want to create or update that report.

## Manual Setup Risks

- Hand-editing scene/prefab YAML for UI references can break GUID/fileID references.
- The Artsystack folder is ignored, so committed required references to it will be missing on public clones.
- Vendor prefabs may depend on vendor scripts, demo hierarchy, fonts, or materials.
- Font/TMP assignment should keep a project-owned fallback.
- SFX/VFX cues should be presentation-level only and must not own damage, economy, wave, or placement rules.

## Recommended Integration Order

1. Open `BootstrapScene` and verify current flow before visual changes.
2. In Unity, make local Inspector assignments from the Artsystack pack to scene UI or copied local test scenes only.
3. Keep public committed UI functional with project-owned/default sprites and TMP fonts.
4. Fill `DefendUIIconConfig.asset` with local optional icons only after deciding whether references should remain uncommitted.
5. Fill `DefendPresentationFeedbackConfig.asset` with local optional SFX/VFX cues only after fallback behavior exists.
6. Run `Tools -> Defend -> UI -> Validate Selected UI` on `MainMenuScreenView`, `DefendGameplayScreenView`, `CurrencyRowView.prefab`, and `MessagePopupView.prefab`.
7. Test from `BootstrapScene`.
8. Only commit project-owned scripts/docs/config shells that do not redistribute paid source assets.
