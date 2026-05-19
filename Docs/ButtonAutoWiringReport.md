# Button Auto-Wiring Report

Generated UI prefabs are visual-only helpers. The builder does not attach gameplay decisions or persistent gameplay listeners to generated buttons.

## Post-Builder Validation

- `Assets/_LocalGenerated/DefendUI/` exists locally and is ignored by Git.
- `Assets/Artsystack - Fantasy RPG GUI/` remains ignored and local-only.
- No tracked `.unity`, `.prefab`, or `.asset` files currently reference the generated `_LocalGenerated/DefendUI` GUIDs.
- No tracked `.unity`, `.prefab`, or `.asset` files currently contain path or GUID references to `_LocalGenerated`, Artsystack, `Toon_Zombies_extended`, or `MasterMagicFX`.
- No tracked scene or prefab was safely auto-wired by the builder.
- `ProjectSettings/PackageManagerSettings.asset`, `UserSettings/EditorUserSettings.asset`, and `UserSettings/Layouts/default-2022.dwlt` changed from Unity/editor activity only; avoid committing them unless intentionally needed.

## Button Status

| Button | Found in generated/local prefab | Safely wired automatically | Required action |
| --- | --- | --- | --- |
| Main Menu Play | `Generated_MainMenuPanel/CenterPanel/PlayButton` | No | Keep the existing `MainMenuView._playButton`, or manually assign this generated `Button` to that field. `MainMenuView.OnPlayClicked` already raises `PlayClicked`. |
| Mine / Rune | `Generated_PlacementPanelSkin/Buttons/MineButton` | No | Keep or manually assign `PlacementPanelView._mineButton`. The view handler raises `MineSelected`; presenter/service logic remains outside the view. |
| Turret / Totem | `Generated_PlacementPanelSkin/Buttons/TurretButton` | No | Keep or manually assign `PlacementPanelView._turretButton`. The view handler raises `TurretSelected`. |
| Puddle / Curse | `Generated_PlacementPanelSkin/Buttons/PuddleButton` | No | Keep or manually assign `PlacementPanelView._puddleButton`. The view handler raises `PuddleSelected`. |
| Start Wave / Continue | `Generated_GameplayHudPanel/WaveActionPanel/StartWaveButton` | No | No existing view event was found. Do not wire directly to wave services from generated UI. |
| Restart | `Generated_ResultPopupSkin/Body/Buttons/RestartButton` | No | No existing restart view event was found. Leave visual-only until presenter-owned restart flow exists. |
| Return to Menu | `Generated_ResultPopupSkin/Body/Buttons/ReturnToMenuButton` | No | Current flow returns to menu through `DefendResultPresenter.OnPopupClosed`; use popup close behavior, not direct scene loading. |
| Result OK/Close | `Generated_ResultPopupSkin/Body/Buttons/OkButton` and `Generated_ResultPopupSkin/Body/CloseButton` | No | If replacing popup visuals, assign one close button to `MessagePopupView._okButton`; `MessagePopupView.OnOkClicked` already raises the popup close request. |

## Validation Command

Run `Tools -> Defend -> Validate Active UI Setup` after manual Inspector assignments. Treat missing required references as errors; optional generated skin warnings are informational.
