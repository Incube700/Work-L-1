# UI Polish Notes

## Gameplay HUD

- `GameplayScene` keeps the existing editable HUD, placement panel, currency list, and result popup objects.
- `DefendHudView` remains display-only. It formats wave, phase, rest timer, and base health values provided by `DefendHudPresenter`.
- `DefendHudPresenter` owns player-facing phase labels and keeps subscriptions as named methods with matching unsubscriptions.

## Currency

- `CurrencyRowView` now uses the existing `Assets/_Project/Art/UI.png` sprites for currency icons.
- `CurrencyRowPresenter` still reads values from `WalletService`; the view only displays the selected currency and amount.

## Result Panel

- The existing `MessagePopupView` remains the result panel and already uses the available victory/defeat sprites.
- The result popup flow still returns through its presenter/service path; no gameplay rules were moved into UI.

## Old UI

- No UI assets or source files were deleted.
- The active tower-defence UI was upgraded in place. Legacy typing-game/prototype UI remains outside the tower-defence playable flow and is still a cleanup candidate after manual Unity verification.

## Manual Verification

- Open `Assets/_Project/Scenes/GameplayScene.unity` or start from `Assets/_Project/Scenes/BootstrapScene.unity`.
- Confirm the HUD shows wave, build/defend status, base health, and currency rows with icons while the core loop runs.
- Confirm the existing result popup still appears for victory/defeat and returns through the supported flow.
