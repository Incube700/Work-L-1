# Existing UI Audit

## Active Scene Flow

- `Assets/_Project/Scenes/BootstrapScene.unity`
- `Assets/_Project/Scenes/MainMenuScene.unity`
- `Assets/_Project/Scenes/GameplayScene.unity`

The current Tower Defence UI is scene-owned where possible and uses presenters/services for gameplay decisions.

## Active Main Menu UI

Views:

- `MainMenuScreenView`
- `MainMenuView`
- `PermanentUpgradesMenuView`
- `PermanentUpgradeEntryView`
- `CurrencyListView`
- `CurrencyRowView`
- `StatsView`
- `PopupLayer`
- `MessagePopupView`

Presenters:

- `MainMenuPresenter`
- `PermanentUpgradesMenuPresenter`
- `CurrencyListPresenter`
- `CurrencyRowPresenter`
- `StatsPresenter`
- `MessagePopupPresenter`

Scene-owned references:

- `MainMenuScreenView` binds `MainMenuView`, `PopupLayer`, `CurrencyListView`, and `StatsView`.
- `MainMenuEntryPoint` validates the main menu screen references before resolving presenters.
- `CurrencyListView` hosts runtime-created `CurrencyRowView` rows.
- `PermanentUpgradesMenuView` owns only display/event references for the close button and upgrade entries.

## Active Gameplay UI

Views:

- `DefendGameplayScreenView`
- `DefendHudView`
- `PlacementPanelView`
- `CurrencyListView`
- `CurrencyRowView`
- `PopupLayer`
- `MessagePopupView`

Presenters:

- `DefendGameplayScreenPresenter`
- `DefendHudPresenter`
- `PlacementPanelPresenter`
- `CurrencyListPresenter`
- `CurrencyRowPresenter`
- `DefendResultPresenter`
- `MessagePopupPresenter`

Scene-owned HUD references:

- `DefendGameplayScreenView` binds `DefendHudView`, `PlacementPanelView`, `PopupLayer`, and `CurrencyListView`.
- `DefendHudView` displays `WaveText`, `PhaseText`, `RestTimerText`, `BaseHpText`, and `SliderHp`.
- `PlacementPanelView` displays Mine, Turret, and Puddle buttons plus gold cost labels.
- `PopupLayer` is the spawn parent for runtime popups.

## Active UI Prefabs

- `Assets/_Project/Resources/UI/Currency/CurrencyRowView.prefab`
- `Assets/_Project/Resources/UI/Popups/MessagePopupView.prefab`
- `Assets/_Project/Resources/Prefabs/DefendHudView.prefab` exists as a HUD prefab resource, while the active gameplay scene also owns HUD objects directly.

## Current Currency Row

`CurrencyRowView.prefab` includes:

- Icon image
- Name text
- Amount text
- Gold and diamond sprite slots on `CurrencyRowView`

The prefab is project-owned. Artsystack icons can be assigned locally for visual testing, but do not commit required references to the paid pack unless the fallback behavior is accepted.

## Current Popup Layer

- `PopupLayer` is an empty marker MonoBehaviour.
- `PopupService` instantiates `MessagePopupView` under the scene-owned popup layer.
- `MessagePopupView` has title, message, icon, victory icon, defeat icon, and OK button references.
- Closing the result popup returns to the main menu through `DefendResultPresenter`.

## Serialized Reference Risks

- Unity scene/prefab YAML changes are risky for UI wiring because missing GUID/fileID references can fail only at runtime.
- The Artsystack pack is ignored and local-only; committed project-owned prefabs should not require it.
- Existing result flow supports an OK/close button returning to the main menu, not a dedicated restart button.
- `Start Wave` and `Continue` buttons are not currently part of the active flow.
- Main menu and gameplay scenes both create runtime currency rows from `Resources/UI/Currency/CurrencyRowView`.
