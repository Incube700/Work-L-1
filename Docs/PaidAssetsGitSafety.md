# Paid Assets Git Safety

## Protected Paid Pack

The local paid UI pack is:

`Assets/Artsystack - Fantasy RPG GUI`

The public repository must not redistribute this pack or its `.meta` file.

## Ignore Rules

`.gitignore` now contains:

```gitignore
/Assets/Artsystack - Fantasy RPG GUI/
/Assets/Artsystack - Fantasy RPG GUI.meta
```

Verified ignore coverage:

- `Assets/Artsystack - Fantasy RPG GUI`
- `Assets/Artsystack - Fantasy RPG GUI.meta`
- Nested source files such as `Assets/Artsystack - Fantasy RPG GUI/ResourcesData/Sprites/components/button_01.png`

## Tracking Status

`git ls-files -- 'Assets/Artsystack - Fantasy RPG GUI' 'Assets/Artsystack - Fantasy RPG GUI.meta'` returned no tracked files.

No cleanup command is needed for the Artsystack pack at the time of this audit.

## Cleanup Commands If Tracking Appears Later

If Git ever shows Artsystack files as tracked, use these commands to stop tracking them while keeping the local files on disk:

```bash
git rm -r --cached "Assets/Artsystack - Fantasy RPG GUI"
git rm --cached "Assets/Artsystack - Fantasy RPG GUI.meta"
```

Do not run history rewriting commands as part of normal cleanup. If the paid pack was already pushed to the public repository, handle that as a separate repository-history incident.

## Public Repo Warning

Do not commit, copy, duplicate, export, or repackage paid Artsystack source assets into project-owned folders. Local Inspector references may be used for personal portfolio screenshots, but public repository content should remain usable without redistributing the paid pack.
