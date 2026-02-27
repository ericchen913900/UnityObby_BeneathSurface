# Run in Unity (Playable Immediately)

## Steps

1. Open Unity Hub and create a new 3D project (Unity 2022.3 LTS+).
2. Copy `UnityObby_BeneathSurface/Assets` into your Unity project root.
3. Open any scene (an empty scene is fine).
4. Click Play.

If there is no `ObbyRunController` in the scene, `AutoBootstrapObby` will automatically create:
- player + camera
- 10 course segments
- checkpoints and kill hazards
- depth atmosphere changes (surface -> deeper layers)
- industrial visual pass (materials + depth lights)
- HUD with progress and completion status

## Controls

- `WASD`: move
- `Space`: jump

## Optional Open-Source Asset Pull

```bash
bash scripts/pull_open_assets.sh
```

Then unpack selected archives from `Assets/OpenSource/Raw` into your Unity `Assets` subfolders.

## Troubleshooting

- If nothing auto-spawns, check Console for compile errors.
- If player does not move, verify the project input settings keep default axes (`Horizontal`, `Vertical`, `Jump`).
- If auto-bootstrap does not run because your scene already has `ObbyRunController`, remove it and press Play again.
