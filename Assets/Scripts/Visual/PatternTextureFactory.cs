using UnityEngine;

namespace BeneathSurface.Visual
{
    public enum BiomeVisualType
    {
        GlowCaves,
        VerticalRuins,
        CrustFault,
        Hazard
    }

    public static class PatternTextureFactory
    {
        public static Texture2D CreateBiomeTexture(BiomeVisualType type, int size)
        {
            var safeSize = Mathf.Clamp(size, 8, 512);
            var texture = new Texture2D(safeSize, safeSize, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Bilinear,
                name = type + "Pattern"
            };

            var pixels = new Color[safeSize * safeSize];
            for (var y = 0; y < safeSize; y++)
            {
                for (var x = 0; x < safeSize; x++)
                {
                    pixels[y * safeSize + x] = GetPixel(type, x, y, safeSize);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply(false, false);
            return texture;
        }

        private static Color GetPixel(BiomeVisualType type, int x, int y, int size)
        {
            switch (type)
            {
                case BiomeVisualType.GlowCaves:
                    return GlowCavesPixel(x, y, size);
                case BiomeVisualType.VerticalRuins:
                    return VerticalRuinsPixel(x, y);
                case BiomeVisualType.CrustFault:
                    return CrustFaultPixel(x, y, size);
                default:
                    return HazardPixel(x, y);
            }
        }

        private static Color GlowCavesPixel(int x, int y, int size)
        {
            var nx = (float)x / size;
            var ny = (float)y / size;
            var wave = Mathf.Sin(nx * 10f) * Mathf.Cos(ny * 9f);
            var glow = Mathf.Clamp01((wave + 1f) * 0.5f);
            var baseColor = new Color(0.08f, 0.18f, 0.16f, 1f);
            var lichenColor = new Color(0.24f, 0.62f, 0.47f, 1f);
            return Color.Lerp(baseColor, lichenColor, glow * 0.55f);
        }

        private static Color VerticalRuinsPixel(int x, int y)
        {
            var stone = new Color(0.39f, 0.37f, 0.33f, 1f);
            var mortar = new Color(0.2f, 0.2f, 0.18f, 1f);
            var block = ((x / 8) + (y / 8)) % 2 == 0;
            var line = (x % 8 == 0) || (y % 8 == 0);
            if (line)
            {
                return mortar;
            }

            return block ? stone : Color.Lerp(stone, new Color(0.48f, 0.45f, 0.4f, 1f), 0.35f);
        }

        private static Color CrustFaultPixel(int x, int y, int size)
        {
            var ny = (float)y / size;
            var cold = new Color(0.16f, 0.28f, 0.36f, 1f);
            var hot = new Color(0.65f, 0.28f, 0.15f, 1f);
            var blend = Mathf.SmoothStep(0f, 1f, ny);
            var baseColor = Color.Lerp(cold, hot, blend);
            var fissure = ((x + y) % 13 == 0) || ((x - y + size * 4) % 17 == 0);
            if (fissure)
            {
                return Color.Lerp(baseColor, new Color(1f, 0.78f, 0.62f, 1f), 0.75f);
            }

            return baseColor;
        }

        private static Color HazardPixel(int x, int y)
        {
            var stripe = ((x + y) / 6) % 2 == 0;
            return stripe ? new Color(0.98f, 0.6f, 0.17f, 1f) : new Color(0.16f, 0.17f, 0.18f, 1f);
        }
    }
}
