# UI Manual Setup

Use these steps when wiring presentation assets in the Unity Editor. Do not hand-edit scene YAML for new visual references unless absolutely necessary.

## Paid Asset Boundary

The Artsystack pack is local-only:

`Assets/Artsystack - Fantasy RPG GUI`

Rules:

- Do not copy Artsystack sprites, prefabs, PSDs, fonts, previews, or scenes into `Assets/_Project`.
- Do not modify vendor assets directly.
- Do not move or rename the vendor folder.
- Do not commit paid asset files or their `.meta` files.
- Avoid committing required references from public project-owned prefabs/scenes/configs to this ignored folder.

## Main Menu Local Polish

Target scene:

`Assets/_Project/Scenes/MainMenuScene.unity`

Safe steps:

1. Keep `MainMenuEntryPoint` and `MainMenuScreenView` references intact.
2. Select the existing `MenuPanel`, `TitleText`, `PlayButton`, `ResetButton`, `AbilitiesButton`, and `PermanentUpgadesMenu` objects.
3. Assign local-only Artsystack sprites through `Image` components where appropriate:
   - Play button: `ResourcesData/Sprites/components/button_01.png`
   - Secondary buttons: `button_02.png`
   - Menu/title panel: `BlueFrame_bg.png`, `header_box.png`, or `panel_name_header.png`
4. Keep `MainMenuView` as the event source only. Do not add scene loading, save reset, or upgrade purchase logic to the view.
5. Run `Tools -> Defend -> UI -> Validate Selected UI` on the `MainMenuScreenView` hierarchy.

If the local Artsystack folder is absent, keep the current project-owned/default UI visuals.

## Gameplay HUD Local Polish

Target scene:

`Assets/_Project/Scenes/GameplayScene.unity`

Safe steps:

1. Keep `DefendGameplayScreenView`, `DefendHudView`, `PlacementPanelView`, `CurrencyListView`, and `PopupLayer` references intact.
2. Add visual-only child `Image` objects only when needed; do not move gameplay logic into them.
3. Candidate local-only assignments:
   - Currency panel: `coins_frame.png` or `coins_frame_2.png`
   - Gold icon: `coin_1.png` or `btn_coin.png`
   - Diamond icon: `crystal_1.png` or `btn_crystal_1.png`
   - Base health: `heart_fill.png`, `heart_frame.png`, or `btn_Heart.png`
   - Health bar: `progress_bar_bg.png` and `progress_bar_top.png`
   - Wave/phase frames: `SmallBlueFrame_bg.png`, `GreenFrame_bg.png`, `RedFrame_bg.png`
4. Verify the HUD still updates wave, phase, rest timer, and base health through `DefendHudPresenter`.
5. Run validation on the `DefendGameplayScreenView` hierarchy.

## Placement Panel Local Polish

Target object:

`GameplayScene -> PlacementPanel`

Safe steps:

1. Keep the existing Mine, Turret, and Puddle `Button` references assigned on `PlacementPanelView`.
2. Candidate local-only slots:
   - `ingame_icon_slot.png`
   - `ingame_icon_slot_2.png`
   - `icon_slot_active.png`
   - `icon_slot_locked.png`
3. Candidate local-only icons:
   - Mine: `ResourcesData/Sprites/flaticon/textured/btn_bomb.png`
   - Turret: `btn_archery.png` or `btn_castle.png`
   - Puddle: keep text for now or assign a clearly temporary local placeholder.
4. Do not add placement, economy, or affordability decisions to `PlacementPanelView`.
5. Verify presenter-driven affordability still disables unaffordable/selected options.

## Result Popup Local Polish

Target prefab:

`Assets/_Project/Resources/UI/Popups/MessagePopupView.prefab`

Safe steps:

1. Keep `PopupViewBase` references assigned: main group, anticlicker, and body.
2. Keep `MessagePopupView` references assigned: title, message, icon image, victory icon, defeat icon, and OK button.
3. Candidate local-only assignments:
   - Popup body: `ResourcesData/Sprites/components/pop_up.png`
   - Title header: `panel_name_header.png`
   - OK button: `button_01.png`
   - Victory icon: `btn_check.png`
   - Defeat icon: `btn_caution.png`
4. Current supported flow is OK/close, then return to main menu. Add restart only through presenter/game-flow work, not inside the view.
5. Run validation on the prefab after assignment.

## DefendUIIconConfig

Existing asset:

`Assets/_Project/Resources/Configs/DefendGame/DefendUIIconConfig.asset`

Suggested local entries:

- `Gold`: display name `Gold`, optional local icon `coin_1.png` or project-owned fallback.
- `Diamond`: display name `Diamonds`, optional local icon `crystal_1.png` or project-owned fallback.
- `Mine`: optional local icon `btn_bomb.png`.
- `Turret`: optional local icon `btn_archery.png`.
- `Puddle`: leave icon empty until a project-owned or acceptable local placeholder is chosen.
- `Rest`: display name `Build`.
- `Wave`: display name `Defend`.
- `Ended`: display name `Result`.

Do not wire this into runtime code until missing icons have safe fallback behavior.

## DefendPresentationFeedbackConfig

Existing asset:

`Assets/_Project/Resources/Configs/DefendGame/DefendPresentationFeedbackConfig.asset`

Suggested cues:

- UI click
- Confirm
- Denied
- Wave start
- Victory
- Defeat
- Placeable selected
- Placeable confirmed
- Base hit
- Enemy death

Use lightweight project-owned/free-pack SFX/VFX first. If assigning paid/local-only references, keep them optional and verify public clones still run without the pack.

## Editor Validation Tool

Menu items:

- `Tools -> Defend -> UI -> Validate Selected UI`
- `Tools -> Defend -> UI -> Generate Selected UI Validation Report`

Use the first command for normal checks. Use the report command only when you intentionally want to write or update `Docs/DefendUiSetupValidationReport.md`.
