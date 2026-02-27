# Vertical Echo: Hollow Mountain - Unity 2D Obby

Theme: "Beneath the Surface" (Chinese: 表面之下)

This is a modular 2D side-scroller obby scaffold for Unity with:
- 10-stage progression support via segment definitions
- checkpoint and instant respawn loop
- obstacle scripts (moving platform, touch kill, timed laser)
- biome-based atmosphere and storyline progression
- patterned sprite visual pass and HUD progress UI

## Unity Version

- Recommended: Unity 2022.3 LTS or newer

## Quick Setup (Instant Play)

1. Create a new 2D Unity project (2022.3 LTS+ recommended).
2. Copy this folder's `Assets/` into your Unity project root.
3. Open any scene (even an empty scene).
4. Press Play.

`AutoBootstrapObby` will auto-generate a playable 2D course (player, camera, checkpoints, hazards, biome theme) if no `ObbyRunController` exists.

## Controls

- Move: A/D or Left/Right
- Jump: Space
- Goal: pass checkpoints and escape from Glow Caves to Surface Break

## Input System Note

Set `Project Settings -> Player -> Active Input Handling` to `Both` during migration.
You can move to `Input System Package (New)` only after confirming your bindings.

Detailed open-source asset references are in `docs/OpenSourceAssets.md`.

## Pull Open-Source Assets

```bash
bash scripts/pull_open_assets.sh
```
