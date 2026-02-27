namespace BeneathSurface.Core
{
    public static class StorylineService
    {
        public static string GetObjectiveLine(int checkpoint, int finishCheckpoint, bool hasFinished)
        {
            if (hasFinished)
            {
                return "You break the final ice shell and return to the surface alive.";
            }

            if (checkpoint <= 0)
            {
                return "After the fall, you wake inside a hollow mountain. Climb before it collapses.";
            }

            if (checkpoint <= 3)
            {
                return "Glow Caves: swing across roots and fungi while the abyss keeps opening below.";
            }

            if (checkpoint <= 7)
            {
                return "Vertical Ruins: collapsing towers force fast route choices through broken stone lines.";
            }

            if (checkpoint < finishCheckpoint)
            {
                return "Crust Fault: ice and geothermal vents collide. Time wall jumps between eruptions.";
            }

            return "Final ascent: break through the frozen crust.";
        }
    }
}
