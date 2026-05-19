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
