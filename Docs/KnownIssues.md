# Known Issues

## Playable Scene

- Full game flow starts from `Assets/_Project/Scenes/BootstrapScene.unity`.
- The main editable gameplay scene is `Assets/_Project/Scenes/GameplayScene.unity`.
- `GameplayScene` can be opened directly for core-loop testing. If no `DefendGameplayArgs` are provided, it starts with the first level from `Resources/Configs/DefendGame/DefendLevelsConfig`.

## Test Path

1. Open `BootstrapScene`.
2. Enter Play Mode.
3. Use Play from the main menu.
4. In `GameplayScene`, waves spawn around the base.
5. During wave phase, click the field to shoot from the base.
6. During rest phase, use the placement panel to select and place mines, turrets, or puddles.
7. Clearing all configured waves wins. Losing the base loses.
8. The result popup returns to the main menu.

## Remaining Notes

- Directly starting from `MainMenuScene` is supported for manual UI testing, but the full persistent scene-argument flow is most reliable from `BootstrapScene`.
- Legacy typing-game and homework/prototype scenes remain in the project and are not part of the current tower-defence playable flow.
- `DefendUiIconConfig` and `DefendPresentationFeedbackConfig` are code-level preparation only. They need Unity Editor asset creation and assignment before runtime wiring.
- Some imported asset packs contain useful UI, VFX, SFX, and tower-defence art, but they are not fully integrated into the active scene flow.
- The Artsystack Fantasy RPG GUI pack is local-only and ignored by Git. Public committed UI should not require that paid folder.
- Result flow currently supports closing the result popup to return to the main menu. A dedicated restart button is not present yet.
- Start Wave/Continue buttons are not part of the active gameplay UI yet; wave progression is driven by the gameplay state machine.
- `UserSettings` appears in source control on some checkouts. It is Unity editor state and should be untracked with `git rm -r --cached -- UserSettings` when cleanup is approved.
- Existing level assets may contain old serialized fields that no longer exist in `DefendLevelConfig`; Unity should be allowed to reserialize them after manual verification.
