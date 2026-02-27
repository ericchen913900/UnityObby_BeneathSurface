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
        [SerializeField] private float glowToRuinsZ = 45f;
        [SerializeField] private float ruinsToFaultZ = 86f;

        private readonly List<Material> _runtimeMaterials = new List<Material>();
        private readonly List<Texture2D> _runtimeTextures = new List<Texture2D>();

        private Texture2D _glowTexture;
        private Texture2D _ruinsTexture;
        private Texture2D _faultTexture;
        private Texture2D _hazardTexture;

        private void Start()
        {
            CreatePatternSet();
            ApplyToAllRenderers();
            CreateDepthLights();
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _runtimeMaterials.Count; i++)
            {
                if (_runtimeMaterials[i] != null)
                {
                    Destroy(_runtimeMaterials[i]);
                }
            }

            for (var i = 0; i < _runtimeTextures.Count; i++)
            {
                if (_runtimeTextures[i] != null)
                {
                    Destroy(_runtimeTextures[i]);
                }
            }
        }

        private void CreatePatternSet()
        {
            _glowTexture = Track(PatternTextureFactory.CreateBiomeTexture(BiomeVisualType.GlowCaves, 64));
            _ruinsTexture = Track(PatternTextureFactory.CreateBiomeTexture(BiomeVisualType.VerticalRuins, 64));
            _faultTexture = Track(PatternTextureFactory.CreateBiomeTexture(BiomeVisualType.CrustFault, 64));
            _hazardTexture = Track(PatternTextureFactory.CreateBiomeTexture(BiomeVisualType.Hazard, 64));
        }

        private Texture2D Track(Texture2D texture)
        {
            _runtimeTextures.Add(texture);
            return texture;
        }

        private void ApplyToAllRenderers()
        {
            var renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            for (var i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                if (r == null)
                {
                    continue;
                }

                var mat = new Material(Shader.Find("Standard"));
                var isHazard = r.name.Contains("Laser") || r.name.Contains("Pit");
                var textureType = ResolveTextureType(r.transform.position.z, isHazard);
                var biome = ToBiomeKind(textureType);

                mat.mainTexture = ResolveTexture(textureType);
                mat.mainTextureScale = isHazard ? new Vector2(1f, 1f) : new Vector2(1.8f, 1.8f);
                mat.color = isHazard
                    ? trimColor
                    : Color.Lerp(baseColor, StageBiomeService.GetBiomeTint(biome), 0.58f);
                mat.SetFloat("_Glossiness", textureType == BiomeVisualType.CrustFault ? 0.34f : 0.22f);

                if (isHazard)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", emissiveColor * 0.8f);
                }

                r.material = mat;
                _runtimeMaterials.Add(mat);
            }
        }

        private BiomeVisualType ResolveTextureType(float z, bool isHazard)
        {
            if (isHazard)
            {
                return BiomeVisualType.Hazard;
            }

            if (z < glowToRuinsZ)
            {
                return BiomeVisualType.GlowCaves;
            }

            if (z < ruinsToFaultZ)
            {
                return BiomeVisualType.VerticalRuins;
            }

            return BiomeVisualType.CrustFault;
        }

        private Texture2D ResolveTexture(BiomeVisualType visualType)
        {
            switch (visualType)
            {
                case BiomeVisualType.GlowCaves:
                    return _glowTexture;
                case BiomeVisualType.VerticalRuins:
                    return _ruinsTexture;
                case BiomeVisualType.CrustFault:
                    return _faultTexture;
                default:
                    return _hazardTexture;
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

        private void CreateDepthLights()
        {
            for (var i = 0; i < 4; i++)
            {
                var marker = new GameObject("DepthLight_" + i);
                marker.transform.SetParent(transform);
                marker.transform.position = new Vector3((i % 2 == 0 ? -3.5f : 3.5f), 2.2f - i * 0.6f, 10f + i * 25f);

                var light = marker.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 12f;
                light.intensity = 1.2f;
                light.color = Color.Lerp(new Color(1f, 0.55f, 0.2f), new Color(0.6f, 0.7f, 0.9f), i / 3f);
            }
        }
    }
}
