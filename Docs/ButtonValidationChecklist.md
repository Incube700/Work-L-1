# Button Validation Checklist

This checklist covers the active Tower Defence UI buttons and the safe validation added around serialized references. Views remain passive: they display state and raise events; presenters/services own gameplay decisions.

## Main Menu

| Button | View field | Event/handler | Owner of decision | Validation |
| --- | --- | --- | --- | --- |
| Play | `MainMenuView._playButton` | `PlayClicked` -> `MainMenuPresenter.OnPlayClicked` | `MainMenuPresenter` loads a configured level through `GameFlowService` | Logs `MainMenuView` missing reference with GameObject name |
| Reset progress | `MainMenuView._resetButton` | `ResetClicked` -> `MainMenuPresenter.OnResetClicked` | `ProgressResetService` and `PopupService` | Logs missing button/text references |
| Upgrades | `MainMenuView._upgradesButton` | `UpgradesClicked` -> `PermanentUpgradesMenuPresenter` | Permanent upgrade presenter/service | Logs missing button/menu references |
| Close upgrades | `PermanentUpgradesMenuView._closeButton` | `CloseClicked` -> presenter | `PermanentUpgradesMenuPresenter` | Logs missing close button and entry references |
| Buy upgrade entries | `PermanentUpgradeEntryView._buyButton` | `BuyClicked` -> menu presenter | `PermanentUpgradesService` and `WalletService` | Logs missing entry text/button references |

## Gameplay

| Button | View field | Event/handler | Owner of decision | Validation |
| --- | --- | --- | --- | --- |
| Mine | `PlacementPanelView._mineButton` | `MineSelected` -> `PlacementPanelPresenter` | `PlacementSelectionService` and placement services | Logs missing button/cost references |
| Turret | `PlacementPanelView._turretButton` | `TurretSelected` -> `PlacementPanelPresenter` | `PlacementSelectionService` and placement services | Logs missing button/cost references |
| Puddle | `PlacementPanelView._puddleButton` | `PuddleSelected` -> `PlacementPanelPresenter` | `PlacementSelectionService` and placement services | Logs missing button/cost references |
| Start Wave / Continue | Not currently present | Wave state machine advances from configured runtime flow | `DefendStateMachine` | No button to validate yet |

## Result Popup

| Button | View field | Event/handler | Owner of decision | Validation |
| --- | --- | --- | --- | --- |
| Close / OK | `MessagePopupView._okButton` | `OnCloseButtonClicked` -> `PopupService` | `DefendResultPresenter.OnPopupClosed` returns to main menu | Logs missing title/message/icon/button references |
| Restart | Not currently present | TODO if restart is added | Should belong to result presenter/game flow, not the view | Document and validate when supported |
| Return to Menu | Uses popup close callback today | `DefendResultPresenter.OnPopupClosed` | `GameFlowService.OpenMainMenu` | Existing OK button validates the supported path |

## Manual Unity Checks

1. Open `Assets/_Project/Scenes/BootstrapScene.unity`.
2. Enter Play Mode and use the Main Menu Play button.
3. In `GameplayScene`, verify Mine, Turret, and Puddle buttons change selection and affordability state.
4. Finish or fail a wave run and verify the result popup OK button returns to the main menu.
5. Open `MainMenuScene` directly for menu-only checks: reset, upgrades, close upgrades, and upgrade buy buttons.
6. Optional: select the UI root or UI prefab and run `Tools -> Defend -> UI -> Validate Selected UI`.

Missing serialized references should produce `Debug.LogError` messages containing the GameObject name and component name.
