# Vertical Echo 2D Side-Scroller Design

## Goal

Convert the current 3D obby runtime into a 2D horizontal parkour game while preserving checkpoint flow, collapse pressure pacing, biome progression, and the new narrative HUD beats.

## Scope Decision (Locked)

- Target format: **2D side-scroller** (X-axis forward, Y-axis jump).
- Keep existing core loop: checkpoint -> fail/respawn -> push forward.
- Keep content theme: Glow Caves -> Vertical Ruins -> Crust Fault -> Surface Break.

## Approach Options

### Option A - Full clean rewrite to 2D components

- Replace all gameplay systems with new 2D scripts (`Rigidbody2D`, `Collider2D`, orthographic camera, 2D prefabs).
- Pros: clean architecture and less legacy baggage.
- Cons: high risk, larger timeline, likely regressions.

### Option B - Incremental migration in place (Recommended)

- Keep existing system boundaries (`ObbyRunController`, `CheckpointService`, `RunProgressService`, HUD/story services).
- Swap 3D dependencies subsystem-by-subsystem: movement, camera, hazards, bootstrap/stage build, visuals.
- Pros: lower risk, playable at every phase, easy rollback.
- Cons: temporary mixed code during migration.

### Option C - 2.5D compatibility bridge

- Keep most 3D scene logic and fake 2D with camera constraints.
- Pros: fastest visual continuity.
- Cons: physics/input complexity remains high; does not actually simplify the stack.

## Chosen Strategy

Use **Option B**. We keep game-state and progression services, then replace runtime-facing 3D APIs with 2D APIs in controlled phases.

## Architecture Changes

### 1) Movement and Player Physics

- Replace `CharacterController` flow with `Rigidbody2D` + `Collider2D` grounded checks.
- Keep `RespawnablePlayer` contract but snap via `Rigidbody2D.position` and clear velocity.

### 2) Camera

- Convert to orthographic camera follow.
- Follow only X/Y; keep camera Z fixed.
- Remove `LookAt` behavior.

### 3) Hazards and Checkpoints

- Replace `OnTriggerEnter(Collider)` with `OnTriggerEnter2D(Collider2D)`.
- Convert `Collider`/`BoxCollider` usage to 2D colliders.
- Preserve existing run-controller calls (`KillPlayer`, `RegisterCheckpoint`).

### 4) Stage Layout and Bootstrap

- Replace runtime primitive generation with 2D sprite-backed objects.
- Stage progression axis in 2D: map previous Z-forward behavior to X-forward.
- Keep depth feeling by Y offsets and biome tint/pattern transitions.

### 5) Visual Pipeline

- Keep procedural pattern generation concept.
- Apply patterns via sprite-compatible materials/tinting (no Standard 3D shader dependency).
- Preserve biome badge + story text in HUD.

### 6) Input

- Maintain current dual input compatibility code path.
- Project setting target: `Active Input Handling = Both` during migration.

## Risk Areas and Controls

- **Physics feel drift**: lock jump arc with numeric tuning tests and checkpointed playtests.
- **Trigger regressions**: migrate hazards/checkpoints together to avoid mixed 2D/3D collider mismatch.
- **Bootstrap breakage**: keep runtime autogen but gate each converted subsystem with smoke checks.
- **Visual mismatch**: allow temporary fallback tint-only mode before full sprite-pattern polish.

## Validation Plan

1. EditMode tests for biome mapping and progression services remain green.
2. PlayMode smoke checklist each phase:
   - Player can move/jump.
   - Checkpoint updates and respawn works.
   - Hazard kills and resets correctly.
   - Camera follows cleanly.
   - HUD shows biome + story progression.
3. No obsolete API warnings for replaced 3D lookup methods.

## Completion Criteria

- Core loop fully playable in 2D side-scroller format.
- All checkpoint/hazard/camera/player interactions use 2D physics APIs.
- HUD retains progression + story + biome indicators.
- Runtime still boots automatically from empty scene.
