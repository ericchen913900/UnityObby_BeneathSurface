using System.Collections.Generic;
using BeneathSurface.Core;
using UnityEngine;

namespace BeneathSurface.Visual
{
    public class IndustrialVisualDirector : MonoBehaviour
    {
        [SerializeField] private Color baseColor = new Color(0.15f, 0.17f, 0.2f, 1f);
        [SerializeField] private Color trimColor = new Color(0.32f, 0.36f, 0.42f, 1f);
        [SerializeField] private Color emissiveColor = new Color(0.95f, 0.45f, 0.12f, 1f);
        [SerializeField] private float glowToRuinsX = 45f;
        [SerializeField] private float ruinsToFaultX = 86f;

        private readonly List<Texture2D> _runtimeTextures = new List<Texture2D>();
        private readonly List<Sprite> _runtimeSprites = new List<Sprite>();

        private Sprite _glowSprite;
        private Sprite _ruinsSprite;
        private Sprite _faultSprite;
        private Sprite _hazardSprite;

        private void Start()
        {
            CreatePatternSet();
            ApplyToAllSprites();
            CreateDepthBeacons();
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _runtimeTextures.Count; i++)
            {
                if (_runtimeTextures[i] != null)
                {
                    Destroy(_runtimeTextures[i]);
                }
            }

            for (var i = 0; i < _runtimeSprites.Count; i++)
            {
                if (_runtimeSprites[i] != null)
                {
                    Destroy(_runtimeSprites[i]);
                }
            }
        }

        private void CreatePatternSet()
        {
            _glowSprite = CreateSprite(BiomeVisualType.GlowCaves);
            _ruinsSprite = CreateSprite(BiomeVisualType.VerticalRuins);
            _faultSprite = CreateSprite(BiomeVisualType.CrustFault);
            _hazardSprite = CreateSprite(BiomeVisualType.Hazard);
        }

        private Sprite CreateSprite(BiomeVisualType type)
        {
            var texture = PatternTextureFactory.CreateBiomeTexture(type, 64);
            _runtimeTextures.Add(texture);
            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 32f);
            sprite.name = type + "Sprite";
            _runtimeSprites.Add(sprite);
            return sprite;
        }

        private void ApplyToAllSprites()
        {
            var renderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
            for (var i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                if (r == null)
                {
                    continue;
                }

                var isHazard = r.name.Contains("Laser") || r.name.Contains("Pit");
                var textureType = ResolveTextureType(r.transform.position.x, isHazard);
                var biome = ToBiomeKind(textureType);

                r.sprite = ResolveSprite(textureType);
                r.color = isHazard
                    ? trimColor
                    : Color.Lerp(baseColor, StageBiomeService.GetBiomeTint(biome), 0.58f);
            }
        }

        private BiomeVisualType ResolveTextureType(float x, bool isHazard)
        {
            if (isHazard)
            {
                return BiomeVisualType.Hazard;
            }

            if (x < glowToRuinsX)
            {
                return BiomeVisualType.GlowCaves;
            }

            if (x < ruinsToFaultX)
            {
                return BiomeVisualType.VerticalRuins;
            }

            return BiomeVisualType.CrustFault;
        }

        private Sprite ResolveSprite(BiomeVisualType visualType)
        {
            switch (visualType)
            {
                case BiomeVisualType.GlowCaves:
                    return _glowSprite;
                case BiomeVisualType.VerticalRuins:
                    return _ruinsSprite;
                case BiomeVisualType.CrustFault:
                    return _faultSprite;
                default:
                    return _hazardSprite;
            }
        }

        private static StageBiomeService.BiomeKind ToBiomeKind(BiomeVisualType visualType)
        {
            switch (visualType)
            {
                case BiomeVisualType.GlowCaves:
                    return StageBiomeService.BiomeKind.GlowCaves;
                case BiomeVisualType.VerticalRuins:
                    return StageBiomeService.BiomeKind.VerticalRuins;
                case BiomeVisualType.CrustFault:
                    return StageBiomeService.BiomeKind.CrustFault;
                default:
                    return StageBiomeService.BiomeKind.SurfaceBreak;
            }
        }

        private void CreateDepthBeacons()
        {
            for (var i = 0; i < 4; i++)
            {
                var marker = new GameObject("DepthBeacon_" + i);
                marker.transform.SetParent(transform);
                marker.transform.position = new Vector3(20f + i * 28f, 3.2f - i * 0.45f, 0f);
                marker.transform.localScale = new Vector3(1.1f, 1.1f, 1f);

                var renderer = marker.AddComponent<SpriteRenderer>();
                renderer.sprite = _hazardSprite;
                renderer.sortingOrder = 6;
                renderer.color = Color.Lerp(emissiveColor, StageBiomeService.GetBiomeTint((StageBiomeService.BiomeKind)Mathf.Clamp(i, 0, 3)), 0.35f);
            }
        }
    }
}
