namespace BeneathSurface.Core
{
    public static class StageBiomeService
    {
        public enum BiomeKind
        {
            GlowCaves,
            VerticalRuins,
            CrustFault,
            SurfaceBreak
        }

        public static BiomeKind GetBiomeKind(int checkpoint, int finishCheckpoint)
        {
            var safeFinish = finishCheckpoint < 1 ? 1 : finishCheckpoint;
            if (checkpoint >= safeFinish)
            {
                return BiomeKind.SurfaceBreak;
            }

            if (checkpoint <= 3)
            {
                return BiomeKind.GlowCaves;
            }

            if (checkpoint <= 7)
            {
                return BiomeKind.VerticalRuins;
            }

            return BiomeKind.CrustFault;
        }

        public static string GetBiomeLabel(int checkpoint, int finishCheckpoint)
        {
            switch (GetBiomeKind(checkpoint, finishCheckpoint))
            {
                case BiomeKind.GlowCaves:
                    return "Glow Caves";
                case BiomeKind.VerticalRuins:
                    return "Vertical Ruins";
                case BiomeKind.CrustFault:
                    return "Crust Fault";
                default:
                    return "Surface Break";
            }
        }

        public static UnityEngine.Color GetBiomeTint(BiomeKind biome)
        {
            switch (biome)
            {
                case BiomeKind.GlowCaves:
                    return new UnityEngine.Color(0.2f, 0.58f, 0.43f, 1f);
                case BiomeKind.VerticalRuins:
                    return new UnityEngine.Color(0.53f, 0.51f, 0.46f, 1f);
                case BiomeKind.CrustFault:
                    return new UnityEngine.Color(0.76f, 0.39f, 0.19f, 1f);
                default:
                    return new UnityEngine.Color(0.83f, 0.86f, 0.9f, 1f);
            }
        }

        public static UnityEngine.Color GetBiomeAccent(BiomeKind biome)
        {
            switch (biome)
            {
                case BiomeKind.GlowCaves:
                    return new UnityEngine.Color(0.72f, 0.98f, 0.83f, 1f);
                case BiomeKind.VerticalRuins:
                    return new UnityEngine.Color(0.87f, 0.84f, 0.77f, 1f);
                case BiomeKind.CrustFault:
                    return new UnityEngine.Color(0.99f, 0.78f, 0.62f, 1f);
                default:
                    return new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
