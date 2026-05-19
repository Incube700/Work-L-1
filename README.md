# Unity Tower Defence Prototype

A portfolio-oriented Unity 2022.3.16f1 tower defence prototype built around an existing scene flow and lightweight service/presenter architecture.

The current playable flow is:

1. `BootstrapScene`
2. `MainMenuScene`
3. `GameplayScene`

The project is being polished in place. It is not a rewrite, and the active tower-defence runtime still lives mainly under `Assets/_Project/Scripts/DefendGame`.

## Current Playable Loop

- Start from the main menu.
- Load one configured defence level.
- Defend the base across configured enemy waves.
- During wave phase, click the battlefield to fire from the base.
- During build/rest phase, place mines, turrets, and puddles if enough gold is available.
- Win by clearing configured waves.
- Lose if the base is destroyed.
- Return to the main menu through the result popup.

## Current Status

Implemented:

- Bootstrap-to-menu-to-gameplay scene flow.
- Defend gameplay runtime, state machine, enemy spawning, placement, wallet spending, rewards, and result popup.
- Config-driven levels, waves, building, enemies, projectiles, and placeables.
- Main menu with play flow, stats, currency display, progress reset, and permanent upgrades.
- Gameplay HUD with wave, phase, currency, base health, placement costs, and affordability feedback.

Prepared but not fully wired:

- UI icon presentation config type.
- VFX/SFX presentation feedback config type.
- Manual asset assignment plan for richer HUD icons, tower button icons, SFX, and lightweight VFX.
- Local-only Artsystack Fantasy RPG GUI reference workflow for optional portfolio UI polish.

Not claimed complete:

- Final art direction.
- Full balancing pass.
- Full VFX/SFX integration.
- Mobile/controller UX.
- Legacy/prototype cleanup.

## How To Run

1. Open the project in Unity `2022.3.16f1`.
2. Open `Assets/_Project/Scenes/BootstrapScene.unity`.
3. Enter Play Mode.
4. Click Play in the main menu.
5. Test the core loop in `GameplayScene`.

See `Docs/SmokeTestChecklist.md` for a fuller manual test pass.

## Documentation

- `Docs/Architecture.md`
- `Docs/UIIntegrationPlan.md`
- `Docs/UIManualSetup.md`
- `Docs/Roadmap.md`
- `Docs/KnownIssues.md`
- `Docs/LegacyCleanupPlan.md`
- `Docs/SmokeTestChecklist.md`
- `Docs/ArtsystackFantasyRpgGuiCatalog.md`
- `Docs/PaidAssetsGitSafety.md`
- `Docs/ButtonValidationChecklist.md`
- `Docs/ExistingUIAudit.md`

## Notes For Reviewers

- The project intentionally keeps MonoBehaviours thin where practical.
- Gameplay decisions remain in services, runtime, state machine, factories, and presenters.
- UI views are passive: they display state and raise button events.
- Legacy typing-game/homework/prototype areas are still present and documented as cleanup candidates only.
- Paid/local-only assets are not required for the public repository flow and should not be redistributed.
