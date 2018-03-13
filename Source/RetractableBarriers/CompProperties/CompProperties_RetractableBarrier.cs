using FrontierDevelopments.RetractableBarriers.Comps;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.CompProperties
{
    public class CompProperties_RetractableBarrier : Verse.CompProperties
    {
        public float toggleToilDuration;
        
        public bool blockLight;
        public bool blockWind;
        public float fillPercent;
        public float hideAtSnowDepth;
        public Traversability passability;
        public int pathCost;

        public CompProperties_RetractableBarrier()
        {
            compClass = typeof(Comp_RetractableBarrier);
        }
    }
}