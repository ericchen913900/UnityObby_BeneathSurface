# Vertical Echo 2D Side-Scroller Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Convert the existing 3D obby runtime into a playable 2D side-scroller while preserving checkpoint/progression/story systems.

**Architecture:** Keep the core run-state services (`CheckpointService`, `RunProgressService`, `ObbyRunController`, story/biome services) and incrementally replace runtime-facing 3D dependencies with 2D equivalents (`Rigidbody2D`, `Collider2D`, orthographic camera, sprite visuals). Migrate in vertical slices so the game remains playable after each task.

**Tech Stack:** Unity 2022 LTS, C#, Unity Physics2D, Unity Test Framework (EditMode/PlayMode), existing runtime bootstrap scripts.

---

### Task 1: Lock 2D biome contracts first

**Files:**
- Modify: `Assets/Scripts/Core/StageBiomeService.cs`
- Test: `Assets/Tests/EditMode/StageBiomeServiceTests.cs`

**Step 1: Write the failing test**

- Add assertions that biome kind/label mapping is stable for side-scroller checkpoints (early/mid/late/finish buckets).

**Step 2: Run test to verify it fails**

- Run (Unity Test Runner, EditMode): `StageBiomeServiceTests`
- Expected: FAIL due to missing/new mapping behavior.

**Step 3: Write minimal implementation**

- Add/adjust `GetBiomeKind`, `GetBiomeLabel`, and tint/accent helpers needed by 2D UI/visual flows.

**Step 4: Run test to verify it passes**

- Re-run EditMode test group for `StageBiomeServiceTests`.
- Expected: PASS.

**Step 5: Commit**

```bash
git add Assets/Scripts/Core/StageBiomeService.cs Assets/Tests/EditMode/StageBiomeServiceTests.cs
git commit -m "test(core): lock 2D biome mapping contracts"
```

### Task 2: Introduce 2D player movement baseline

**Files:**
- Modify: `Assets/Scripts/Player/PlayerMotor.cs`
- Modify: `Assets/Scripts/Player/RespawnablePlayer.cs`
- Modify: `Assets/Scripts/Core/ObbyRunController.cs`
- Test: `Assets/Tests/EditMode/RunProgressServiceTests.cs`

**Step 1: Write the failing test**

- Add a narrow test that verifies respawn flow still resets player state after movement API changes.

**Step 2: Run test to verify it fails**

- Run EditMode test target for the new/updated test.
- Expected: FAIL before migration.

**Step 3: Write minimal implementation**

- Replace `CharacterController` movement logic with `Rigidbody2D`-based horizontal movement and jump.
- Preserve existing input fallback behavior.
- Ensure respawn clears 2D velocity and places player on the correct 2D plane.

**Step 4: Run test to verify it passes**

- Re-run the focused test, then run all existing core EditMode tests.
- Expected: PASS.

**Step 5: Commit**

```bash
git add Assets/Scripts/Player/PlayerMotor.cs Assets/Scripts/Player/RespawnablePlayer.cs Assets/Scripts/Core/ObbyRunController.cs Assets/Tests/EditMode/RunProgressServiceTests.cs
git commit -m "feat(player): migrate baseline movement and respawn to 2D physics"
```

### Task 3: Convert hazards and checkpoints to Physics2D callbacks

**Files:**
- Modify: `Assets/Scripts/Core/CheckpointTrigger.cs`
- Modify: `Assets/Scripts/Obstacles/KillOnTouch.cs`
- Modify: `Assets/Scripts/Obstacles/TimedLaserHazard.cs`
- Test: `Assets/Tests/EditMode/CheckpointServiceTests.cs`

**Step 1: Write the failing test**

- Add a test covering checkpoint acceptance/rejection assumptions used by new trigger code path.

**Step 2: Run test to verify it fails**

- Run EditMode tests for checkpoint service.
- Expected: FAIL if behavior changed or callback assumptions are wrong.

**Step 3: Write minimal implementation**

- Switch `Collider`/`BoxCollider` requirements to `Collider2D` variants.
- Replace `OnTriggerEnter(Collider)` with `OnTriggerEnter2D(Collider2D)`.
- Keep run-controller method calls unchanged.

**Step 4: Run test to verify it passes**

- Re-run checkpoint tests and a quick PlayMode trigger smoke check.
- Expected: PASS; player dies/respawns and checkpoints register.

**Step 5: Commit**

```bash
git add Assets/Scripts/Core/CheckpointTrigger.cs Assets/Scripts/Obstacles/KillOnTouch.cs Assets/Scripts/Obstacles/TimedLaserHazard.cs Assets/Tests/EditMode/CheckpointServiceTests.cs
git commit -m "feat(gameplay): migrate checkpoint and hazard triggers to 2D"
```

### Task 4: Migrate camera to orthographic side-scroller behavior

**Files:**
- Modify: `Assets/Scripts/Player/FollowCamera.cs`
- Modify: `Assets/Scripts/Core/AutoBootstrapObby.cs`

**Step 1: Write the failing test**

- Add a small deterministic test/helper assertion for camera follow target projection (X/Y follow, fixed Z).

**Step 2: Run test to verify it fails**

- Run focused EditMode test.
- Expected: FAIL before camera formula update.

**Step 3: Write minimal implementation**

- Remove `LookAt`-based behavior.
- Follow only X/Y with smooth damp; keep constant camera Z.
- Ensure bootstrap camera is orthographic.

**Step 4: Run test to verify it passes**

- Re-run focused camera tests + quick PlayMode check.
- Expected: PASS; no camera spin, stable side view.

**Step 5: Commit**

```bash
git add Assets/Scripts/Player/FollowCamera.cs Assets/Scripts/Core/AutoBootstrapObby.cs
git commit -m "feat(camera): switch runtime follow camera to orthographic 2D"
```

### Task 5: Rewire bootstrap and stage layout for 2D axis mapping

**Files:**
- Modify: `Assets/Scripts/Core/AutoBootstrapObby.cs`
- Modify: `Assets/Scripts/Stage/StageLayoutPlanner.cs`
- Modify: `Assets/Scripts/Stage/StageBuilder.cs`
- Modify: `Assets/Scripts/Stage/StageSegmentDefinition.cs`
- Test: `Assets/Tests/EditMode/StageLayoutPlannerTests.cs`

**Step 1: Write the failing test**

- Update/add tests to assert side-scroller axis progression (X-forward) and depth offsets (Y adjustments), with stable ordering.

**Step 2: Run test to verify it fails**

- Run `StageLayoutPlannerTests`.
- Expected: FAIL with old 3D Z-forward assumptions.

**Step 3: Write minimal implementation**

- Remap stage progression from Z-forward to side-scroller axis.
- Ensure bootstrap-generated checkpoints/hazards/platforms align to the 2D world plane.
- Keep runtime root bootstrap behavior intact.

**Step 4: Run test to verify it passes**

- Re-run `StageLayoutPlannerTests`, then PlayMode smoke test from empty scene.
- Expected: PASS; runtime builds valid 2D course.

**Step 5: Commit**

```bash
git add Assets/Scripts/Core/AutoBootstrapObby.cs Assets/Scripts/Stage/StageLayoutPlanner.cs Assets/Scripts/Stage/StageBuilder.cs Assets/Scripts/Stage/StageSegmentDefinition.cs Assets/Tests/EditMode/StageLayoutPlannerTests.cs
git commit -m "feat(stage): map bootstrap and layout pipeline to 2D side-scroller axes"
```

### Task 6: Replace 3D visual pass with sprite-compatible biome patterns

**Files:**
- Modify: `Assets/Scripts/Visual/IndustrialVisualDirector.cs`
- Modify: `Assets/Scripts/Visual/PatternTextureFactory.cs`
- Test: `Assets/Tests/EditMode/PatternTextureFactoryTests.cs`

**Step 1: Write the failing test**

- Add tests for pattern texture validity and pixel variation for each biome visual type.

**Step 2: Run test to verify it fails**

- Run `PatternTextureFactoryTests`.
- Expected: FAIL before sprite-compatible updates.

**Step 3: Write minimal implementation**

- Remove assumptions tied to `Shader.Find("Standard")` + 3D renderer-only behavior.
- Apply biome patterns/tints in a sprite-compatible way.
- Keep hazard visual distinction strong.

**Step 4: Run test to verify it passes**

- Re-run texture tests and PlayMode visual smoke check.
- Expected: PASS; visible biome-specific pattern differences.

**Step 5: Commit**

```bash
git add Assets/Scripts/Visual/IndustrialVisualDirector.cs Assets/Scripts/Visual/PatternTextureFactory.cs Assets/Tests/EditMode/PatternTextureFactoryTests.cs
git commit -m "feat(visual): apply sprite-compatible biome pattern rendering"
```

### Task 7: Final HUD and input migration pass

**Files:**
- Modify: `Assets/Scripts/UI/RunHudController.cs`
- Modify: `Assets/Scripts/Core/StorylineService.cs`
- Modify: `docs/RunInUnity.md`
- Modify: `README.md`

**Step 1: Write failing checks**

- Add/update text expectations for HUD labels/biome badge messaging in existing tests where relevant.

**Step 2: Run check to verify mismatch**

- Run focused tests and manual HUD validation checklist.

**Step 3: Write minimal implementation**

- Ensure HUD reflects side-scroller context, biome, and story lines clearly in 2D layout.
- Document `Active Input Handling = Both` migration requirement.

**Step 4: Run verification**

- EditMode tests + manual PlayMode checklist:
  - Move/jump works.
  - Hazard death/respawn works.
  - Checkpoint progression works.
  - HUD story + biome updates correctly.

**Step 5: Commit**

```bash
git add Assets/Scripts/UI/RunHudController.cs Assets/Scripts/Core/StorylineService.cs docs/RunInUnity.md README.md
git commit -m "docs(ui): finalize 2D HUD narrative and input migration guidance"
```

### Task 8: End-to-end verification gate

**Files:**
- Verify only (no mandatory code changes)

**Step 1: Run full EditMode suite**

- Run: Unity Test Runner EditMode all tests (with `BENEATH_SURFACE_ENABLE_TESTS` define if needed).

**Step 2: Run manual PlayMode smoke pass**

- Empty scene auto-bootstrap launch.
- Complete one full run from checkpoint 0 to finish.
- Confirm no obsolete 3D API warnings in migrated files.

**Step 3: Final commit if verification introduced fixes**

```bash
git add .
git commit -m "chore: finalize 2D migration verification fixes"
```
