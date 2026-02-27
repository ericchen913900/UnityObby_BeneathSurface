using System.Collections.Generic;
using UnityEngine;

namespace BeneathSurface.Visual
{
    public class IndustrialVisualDirector : MonoBehaviour
    {
        [SerializeField] private Color baseColor = new Color(0.15f, 0.17f, 0.2f, 1f);
        [SerializeField] private Color trimColor = new Color(0.32f, 0.36f, 0.42f, 1f);
        [SerializeField] private Color emissiveColor = new Color(0.95f, 0.45f, 0.12f, 1f);

        private readonly List<Material> _runtimeMaterials = new List<Material>();

        private void Start()
        {
            ApplyToAllRenderers();
            CreateDepthLights();
        }

        private void ApplyToAllRenderers()
        {
            var renderers = FindObjectsOfType<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                var mat = new Material(Shader.Find("Standard"));
                var isHazard = r.name.Contains("Laser") || r.name.Contains("Pit");
                mat.color = isHazard ? trimColor : baseColor;
                mat.SetFloat("_Glossiness", isHazard ? 0.4f : 0.2f);

                if (isHazard)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", emissiveColor * 0.8f);
                }

                r.material = mat;
                _runtimeMaterials.Add(mat);
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
