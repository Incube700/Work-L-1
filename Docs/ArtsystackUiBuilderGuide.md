# Artsystack UI Builder Guide

## Run the Builder

1. Open the project in Unity 2022.3.16f1.
2. Confirm the paid pack exists locally at `Assets/Artsystack - Fantasy RPG GUI`.
3. Run `Tools -> Defend -> Build Artsystack UI`.
4. Watch the Console for selected asset logs. Each selected candidate includes the asset path, matched keyword, and intended usage.

The tool is non-destructive: it does not modify vendor assets, gameplay code, open scenes, or existing active UI prefabs.

## Generated Assets

The builder creates local-only assets under:

`Assets/_LocalGenerated/DefendUI/`

Expected generated folders:

- `Assets/_LocalGenerated/DefendUI/Prefabs/`
- `Assets/_LocalGenerated/DefendUI/Configs/`

Expected generated prefabs:

- `Generated_MainMenuPanel.prefab`
- `Generated_GameplayHudPanel.prefab`
- `Generated_PlacementPanelSkin.prefab`
- `Generated_ResultPopupSkin.prefab`
- `Generated_TowerButtonSkin.prefab`
- `Generated_CurrencyRowSkin.prefab`

Expected generated configs:

- `DefendUiIconConfig.asset`
- `DefendPresentationFeedbackConfig.asset`

If any generated asset already exists, the builder asks before replacing it.

## Local-Only Files

`Assets/_LocalGenerated/` is ignored by Git because generated prefabs/configs may reference paid Artsystack sprites. Do not move required runtime scripts into this folder.

Do not commit:

- `Assets/Artsystack - Fantasy RPG GUI/`
- `Assets/Artsystack - Fantasy RPG GUI.meta`
- `Assets/_LocalGenerated/`
- `Assets/_LocalGenerated.meta`

## Assign Skins

Generated prefabs are visual helpers. They do not own gameplay rules.

Suggested assignments:

- Main menu: instantiate `Generated_MainMenuPanel.prefab` in `MainMenuScene`, then assign useful child objects/images to optional fields on `MainMenuScreenView` and `MainMenuView`.
- Gameplay HUD: instantiate `Generated_GameplayHudPanel.prefab` in `GameplayScene`, then assign top-bar visuals to optional fields on `DefendGameplayScreenView`, `DefendHudView`, and `CurrencyListView`.
- Placement panel: instantiate `Generated_PlacementPanelSkin.prefab`, then assign Mine/Turret/Puddle button components to `PlacementPanelView` only if you are intentionally replacing the old scene buttons.
- Result popup: use `Generated_ResultPopupSkin.prefab` as a visual reference for the existing `MessagePopupView.prefab`; keep close behavior routed through `MessagePopupView`.
- Currency row: use `Generated_CurrencyRowSkin.prefab` as a visual reference for `CurrencyRowView.prefab`.

Existing UI keeps working when optional generated skin fields are empty.

## Validate Buttons

After manual assignments, run:

`Tools -> Defend -> Validate Active UI Setup`

The validator checks active-scene instances of:

- `DefendGameplayScreenView`
- `DefendHudView`
- `PlacementPanelView`
- `CurrencyListView`
- `PopupLayer`

It also checks known button fields on `MainMenuView`, `PlacementPanelView`, and `MessagePopupView`, then logs any optional generated skin fields that are still empty.

## Config Notes

The local generated icon config can reference Artsystack icons safely because it lives under ignored `Assets/_LocalGenerated/`.

Runtime config loading still uses tracked assets under `Assets/_Project/Resources/Configs/DefendGame/`. If you want Artsystack icon assignments in the tracked config, copy the assignments manually and accept that public clones without the paid pack will have missing references.
