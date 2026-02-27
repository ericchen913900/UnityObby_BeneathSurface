using BeneathSurface.Obstacles;
using BeneathSurface.Player;
using BeneathSurface.Stage;
using BeneathSurface.UI;
using BeneathSurface.Visual;
using UnityEngine;

namespace BeneathSurface.Core
{
    public static class AutoBootstrapObby
    {
        private const string RootName = "BeneathSurface_Runtime";
        private static Sprite _runtimeSprite;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void BuildIfMissing()
        {
            if (Object.FindFirstObjectByType<ObbyRunController>() != null)
            {
                return;
            }

            var root = new GameObject(RootName);
            var spawn = new GameObject("SpawnPoint");
            spawn.transform.SetParent(root.transform);
            spawn.transform.position = new Vector3(0f, 1.2f, 0f);

            var player = CreatePlayer(root.transform, spawn.transform.position);
            var runController = root.AddComponent<ObbyRunController>();
            runController.Configure(spawn.transform, player.GetComponent<RespawnablePlayer>());

            CreateCamera(root.transform, player.transform);
            CreateBaseFloor(root.transform);
            CreateSimpleCourse(root.transform, runController);
            CreateThemeController(root.transform, runController);
            CreateHud(root.transform, runController);

            var visualDirector = root.AddComponent<IndustrialVisualDirector>();
            visualDirector.enabled = true;
        }

        private static GameObject CreatePlayer(Transform parent, Vector3 spawnPosition)
        {
            var player = new GameObject("Player");
            player.transform.SetParent(parent);
            player.transform.position = spawnPosition;
            player.transform.localScale = new Vector3(0.9f, 1.8f, 1f);

            var renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = GetRuntimeSprite();
            renderer.color = new Color(0.93f, 0.95f, 0.98f, 1f);
            renderer.sortingOrder = 20;

            var body = player.AddComponent<Rigidbody2D>();
            body.freezeRotation = true;

            var collider = player.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.85f, 1f);

            player.AddComponent<PlayerMotor>();
            player.AddComponent<RespawnablePlayer>();
            return player;
        }

        private static void CreateCamera(Transform parent, Transform target)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                var camObj = new GameObject("Main Camera");
                cam = camObj.AddComponent<Camera>();
                cam.tag = "MainCamera";
            }

            cam.transform.SetParent(parent);
            cam.orthographic = true;
            cam.orthographicSize = 5.6f;
            cam.backgroundColor = new Color(0.08f, 0.1f, 0.14f, 1f);
            cam.transform.position = new Vector3(3f, 2.4f, -10f);
            var follow = cam.gameObject.GetComponent<FollowCamera>();
            if (follow == null)
            {
                follow = cam.gameObject.AddComponent<FollowCamera>();
            }

            follow.Configure(target);
        }

        private static void CreateBaseFloor(Transform parent)
        {
            var floor = CreateSpriteObject(
                parent,
                "BaseFloor",
                new Vector3(58f, -3f, 0f),
                new Vector2(150f, 1.2f),
                new Color(0.18f, 0.2f, 0.24f, 1f),
                -2);
            floor.gameObject.AddComponent<BoxCollider2D>();
        }

        private static void CreateSimpleCourse(Transform parent, ObbyRunController runController)
        {
            for (var i = 0; i < 10; i++)
            {
                var x = i * 12f;
                var y = -i * 0.35f;

                var platformRenderer = CreateSpriteObject(
                    parent,
                    $"Platform_{i + 1:00}",
                    new Vector3(x + 4f, y, 0f),
                    new Vector2(8f, 1.1f),
                    new Color(0.31f, 0.33f, 0.36f, 1f),
                    0);
                var platform = platformRenderer.gameObject;
                platform.AddComponent<BoxCollider2D>();

                if (i % 2 == 1)
                {
                    var mover = platform.AddComponent<MovingPlatform>();
                    mover.enabled = true;
                }

                var checkpointObj = new GameObject($"Checkpoint_{i + 1:00}");
                checkpointObj.transform.SetParent(parent);
                checkpointObj.transform.position = new Vector3(x + 8f, y + 1.2f, 0f);
                var cpCol = checkpointObj.AddComponent<BoxCollider2D>();
                cpCol.isTrigger = true;
                cpCol.size = new Vector2(1.2f, 2.2f);
                var cp = checkpointObj.AddComponent<CheckpointTrigger>();
                cp.Configure(i + 1, runController);

                var checkpointMarker = CreateSpriteObject(
                    checkpointObj.transform,
                    "CheckpointMarker",
                    Vector3.zero,
                    new Vector2(0.45f, 2f),
                    new Color(0.65f, 0.86f, 1f, 0.88f),
                    3);
                checkpointMarker.transform.localPosition = Vector3.zero;

                if (i == 9)
                {
                    var gate = CreateSpriteObject(
                        parent,
                        "FinalCoreGate",
                        new Vector3(x + 10.5f, y + 1.8f, 0f),
                        new Vector2(2.6f, 3.8f),
                        new Color(0.82f, 0.9f, 1f, 0.92f),
                        1);
                    var gateCol = gate.gameObject.AddComponent<BoxCollider2D>();
                    gateCol.isTrigger = false;
                }

                if (i % 3 == 2)
                {
                    var laser = CreateSpriteObject(
                        parent,
                        $"Laser_{i + 1:00}",
                        new Vector3(x + 2f, y + 1f, 0f),
                        new Vector2(4.8f, 0.45f),
                        new Color(0.94f, 0.26f, 0.22f, 1f),
                        4);
                    var laserCollider = laser.gameObject.AddComponent<BoxCollider2D>();
                    laserCollider.isTrigger = true;
                    var laserHazard = laser.gameObject.AddComponent<TimedLaserHazard>();
                    laserHazard.Configure(runController, laser);
                }

                var pit = new GameObject($"PitTrigger_{i + 1:00}");
                pit.transform.SetParent(parent);
                pit.transform.position = new Vector3(x + 4f, y - 2.5f, 0f);
                var pitCol = pit.AddComponent<BoxCollider2D>();
                pitCol.isTrigger = true;
                pitCol.size = new Vector2(8f, 1f);
                var pitKill = pit.AddComponent<KillOnTouch>();
                pitKill.Configure(runController);
            }
        }

        private static void CreateThemeController(Transform parent, ObbyRunController runController)
        {
            var themeObj = new GameObject("ThemeController");
            themeObj.transform.SetParent(parent);
            var controller = themeObj.AddComponent<StageThemeController>();

            var profiles = new[]
            {
                new StageThemeController.DepthProfile
                {
                    minCheckpoint = 0,
                    ambientLight = new Color(0.62f, 0.66f, 0.7f, 1f),
                    fogColor = new Color(0.75f, 0.78f, 0.82f, 1f),
                    fogDensity = 0.003f
                },
                new StageThemeController.DepthProfile
                {
                    minCheckpoint = 4,
                    ambientLight = new Color(0.45f, 0.42f, 0.38f, 1f),
                    fogColor = new Color(0.42f, 0.37f, 0.31f, 1f),
                    fogDensity = 0.01f
                },
                new StageThemeController.DepthProfile
                {
                    minCheckpoint = 8,
                    ambientLight = new Color(0.18f, 0.2f, 0.24f, 1f),
                    fogColor = new Color(0.1f, 0.12f, 0.16f, 1f),
                    fogDensity = 0.022f
                }
            };

            controller.Configure(runController, profiles);
        }

        private static void CreateHud(Transform parent, ObbyRunController runController)
        {
            var hudObject = new GameObject("HUD");
            hudObject.transform.SetParent(parent);
            var hud = hudObject.AddComponent<RunHudController>();
            hud.Configure(runController);
        }

        private static SpriteRenderer CreateSpriteObject(
            Transform parent,
            string name,
            Vector3 position,
            Vector2 size,
            Color color,
            int sortingOrder)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            go.transform.position = position;
            go.transform.localScale = new Vector3(size.x, size.y, 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = GetRuntimeSprite();
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }

        private static Sprite GetRuntimeSprite()
        {
            if (_runtimeSprite != null)
            {
                return _runtimeSprite;
            }

            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply(false, false);
            _runtimeSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _runtimeSprite.name = "RuntimeWhiteSprite";
            return _runtimeSprite;
        }
    }
}
