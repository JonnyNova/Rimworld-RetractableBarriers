using FrontierDevelopments.RetractableBarriers.Comps;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.CompProperties
{
    public class CompProperties_RetractableBarrier : Verse.CompProperties
    {
        public bool upBlockLight;
        public float upFillPercentage;
        public float upHideAtSnowDepth;
        public Traversability upTraversability;
        public int upPathCost;
        
        public bool downBlockLight;
        public float downFillPercentage;
        public float downHideAtSnowDepth;
        public Traversability downTraversability;
        public int downPathCost;

        public float toggleToilDuration;

        public CompProperties_RetractableBarrier()
        {
            compClass = typeof(Comp_RetractableBarrier);
        }
    }
}