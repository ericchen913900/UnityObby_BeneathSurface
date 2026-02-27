# Scene Setup (2D Side-Scroller Obby)

If you just want to play immediately, you can skip this file and press Play in an empty scene after importing `Assets/`.
Runtime auto-bootstrap will create a full demo course.

## 1) Core Objects

- `GameController` (empty)
  - Attach: `ObbyRunController`
  - Assign:
    - `initialSpawn` -> object `SpawnPoint`
    - `player` -> object `Player`

- `StageBuilder` (empty)
  - Attach: `StageBuilder`
  - Set `stageCount` to 10
  - Assign 2-4 `StageSegmentDefinition` assets

- `ThemeController` (empty)
  - Attach: `StageThemeController`
  - Assign `runController` -> `GameController`
  - Configure profiles:
    - checkpoint 0: bright surface
    - checkpoint 3: dusty mid layer
    - checkpoint 7: dark deep layer

## 2) Player

- `Player` (2D sprite object)
  - Tag: `Player`
  - Components:
    - `Rigidbody2D`
    - `Collider2D` (CapsuleCollider2D recommended)
    - `PlayerMotor`
    - `RespawnablePlayer`

- Main Camera
  - Attach: `FollowCamera`
  - Assign `target` -> `Player`
  - Camera type: Orthographic

## 3) Checkpoints and Hazards

- For each checkpoint object:
  - BoxCollider2D (isTrigger = true)
  - `CheckpointTrigger` with increasing `checkpointIndex`

- For hazards:
  - `KillOnTouch` for spikes/fall zones
  - `TimedLaserHazard` for rhythm timing challenges
  - `MovingPlatform` for traversal variation

## 4) WebGL Build Settings

- Player Settings:
  - Color Space: Linear (if performance allows), else Gamma
  - Texture Compression: Enabled
  - Strip Engine Code: Enabled
- Keep texture resolution conservative (1K-2K max where possible).

## 5) Input Migration Safety

- During migration, keep `Project Settings -> Player -> Active Input Handling` as `Both`.
- After validating controls, you can switch to `Input System Package (New)` only.
