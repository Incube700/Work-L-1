# Roadmap

## Portfolio-Ready Core

- Keep `BootstrapScene -> MainMenuScene -> GameplayScene`.
- Keep `DefendGameplayEntryPoint` and `MainMenuEntryPoint` focused on scene binding and startup.
- Keep gameplay decisions in services, state machine, factories, and presenters.
- Keep UI views passive.

## Safe Next Steps

- Create `DefendUiIconConfig` and assign icons for currency, placeables, phases, and enemy visual labels.
- Create `DefendPresentationFeedbackConfig` and assign a small set of UI, placement, hit, win, and lose clips/effects.
- Wire presentation configs through `ConfigService` only after the assets exist.
- Add optional presenter-side UI icon updates without moving gameplay rules into views.
- Improve placement panel visuals in the Unity Editor using existing serialized references.
- Capture screenshots into `Docs/Screenshots`.

## Balance And Gameplay Polish

- Tune level wave order, enemy count, spawn interval, rest duration, rewards, and costs.
- Review turret/mine/puddle cost-to-impact relationship.
- Add a small visible TODO marker for unsupported restart-in-place if it is not implemented.
- Keep win/lose reward logic in `DefendResultService`.

## Technical Cleanup

- Audit old serialized fields that remain in level assets after config shape changes.
- Audit tracked Unity generated files such as `UserSettings`.
- Verify all imported asset packs that are actually used before removing anything.
- Add play-mode smoke tests only after the Unity project is stable in the Editor.

## Later

- Replace placeholder scene composition with deliberate art direction.
- Add audio mixer groups and volume settings.
- Add scalable UI layout pass for common desktop resolutions.
- Add build settings/release notes for a portfolio build.
