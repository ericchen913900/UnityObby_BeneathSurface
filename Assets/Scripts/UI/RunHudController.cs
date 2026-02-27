using BeneathSurface.Core;
using UnityEngine;

namespace BeneathSurface.UI
{
    public class RunHudController : MonoBehaviour
    {
        [SerializeField] private ObbyRunController runController;
        [SerializeField] private bool showHud = true;

        private GUIStyle _checkpointStyle;
        private GUIStyle _statusStyle;
        private GUIStyle _storyStyle;
        private GUIStyle _biomeStyle;

        private static readonly Color PanelColor = new Color(0.05f, 0.06f, 0.08f, 0.72f);
        private static readonly Color ProgressBackColor = new Color(0.18f, 0.2f, 0.24f, 0.95f);
        private static readonly Color ProgressFillColor = new Color(0.93f, 0.46f, 0.17f, 0.98f);
        private static readonly Color TextColor = new Color(0.93f, 0.94f, 0.96f, 1f);

        private void Update()
        {
            if (runController == null)
            {
                runController = Object.FindFirstObjectByType<ObbyRunController>();
            }
        }

        private void OnGUI()
        {
            if (!showHud || runController == null)
            {
                return;
            }

            EnsureStyles();

            var checkpoint = runController.GetCurrentCheckpointIndex();
            var finish = runController.GetFinishCheckpoint();
            var progress = runController.GetProgress01();
            var biomeKind = StageBiomeService.GetBiomeKind(checkpoint, finish);
            var biome = StageBiomeService.GetBiomeLabel(checkpoint, finish);
            var status = runController.HasFinished() ? "Core Reached - Run Complete" : "Climb Up Before Collapse";
            var story = StorylineService.GetObjectiveLine(checkpoint, finish, runController.HasFinished());

            DrawSolid(new Rect(0f, 0f, Screen.width, 136f), PanelColor);
            GUI.Label(new Rect(20f, 12f, 600f, 28f), "Depth Progress " + checkpoint + " / " + finish, _checkpointStyle);
            GUI.Label(new Rect(Screen.width - 320f, 14f, 300f, 24f), "Biome: " + biome, _biomeStyle);
            GUI.Label(new Rect(20f, 42f, 600f, 24f), status, _statusStyle);
            GUI.Label(new Rect(20f, 66f, Screen.width - 40f, 52f), story, _storyStyle);
            DrawBiomeBadge(new Rect(Screen.width - 94f, 42f, 64f, 64f), biomeKind);

            const float barWidth = 280f;
            const float barHeight = 18f;
            var barX = Screen.width - barWidth - 20f;
            var barY = 102f;
            DrawSolid(new Rect(barX, barY, barWidth, barHeight), ProgressBackColor);
            DrawSolid(new Rect(barX, barY, barWidth * Mathf.Clamp01(progress), barHeight), ProgressFillColor);
        }

        private void EnsureStyles()
        {
            if (_checkpointStyle != null && _statusStyle != null && _storyStyle != null && _biomeStyle != null)
            {
                return;
            }

            _checkpointStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                alignment = TextAnchor.UpperLeft
            };
            _checkpointStyle.normal.textColor = TextColor;

            _statusStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                alignment = TextAnchor.UpperLeft
            };
            _statusStyle.normal.textColor = TextColor;

            _storyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                alignment = TextAnchor.UpperLeft,
                wordWrap = true
            };
            _storyStyle.normal.textColor = TextColor;

            _biomeStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                alignment = TextAnchor.UpperRight,
                fontStyle = FontStyle.Bold
            };
            _biomeStyle.normal.textColor = TextColor;
        }

        private static void DrawSolid(Rect rect, Color color)
        {
            var previous = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private static void DrawBiomeBadge(Rect rect, StageBiomeService.BiomeKind biome)
        {
            var tint = StageBiomeService.GetBiomeTint(biome);
            var accent = StageBiomeService.GetBiomeAccent(biome);

            DrawSolid(rect, Color.Lerp(tint, Color.black, 0.28f));
            DrawSolid(new Rect(rect.x + 2f, rect.y + 2f, rect.width - 4f, rect.height - 4f), Color.Lerp(tint, Color.black, 0.12f));

            switch (biome)
            {
                case StageBiomeService.BiomeKind.GlowCaves:
                    DrawSolid(new Rect(rect.x + 10f, rect.y + 12f, 10f, 10f), accent);
                    DrawSolid(new Rect(rect.x + 34f, rect.y + 20f, 9f, 9f), accent);
                    DrawSolid(new Rect(rect.x + 20f, rect.y + 38f, 12f, 12f), accent);
                    break;
                case StageBiomeService.BiomeKind.VerticalRuins:
                    for (var i = 0; i < 4; i++)
                    {
                        DrawSolid(new Rect(rect.x + 8f + i * 12f, rect.y + 10f, 2f, 44f), accent);
                    }

                    for (var j = 0; j < 4; j++)
                    {
                        DrawSolid(new Rect(rect.x + 8f, rect.y + 10f + j * 12f, 44f, 2f), accent);
                    }

                    break;
                case StageBiomeService.BiomeKind.CrustFault:
                    for (var k = -1; k < 4; k++)
                    {
                        DrawSolid(new Rect(rect.x + 6f + k * 14f, rect.y + 8f, 6f, 48f), accent);
                    }

                    break;
                default:
                    DrawSolid(new Rect(rect.x + 8f, rect.y + 36f, 48f, 4f), accent);
                    DrawSolid(new Rect(rect.x + 14f, rect.y + 20f, 36f, 3f), accent);
                    break;
            }
        }

        public void Configure(ObbyRunController controller)
        {
            runController = controller;
        }
    }
}
