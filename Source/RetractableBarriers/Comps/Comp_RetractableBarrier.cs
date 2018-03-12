using FrontierDevelopments.RetractableBarriers.CompProperties;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.Comps
{
    public class Comp_RetractableBarrier : ThingComp
    {
        private bool _extended;
        private bool _wantExtended;

        public bool Extended
        {
            get { return _extended; }
            set { _extended = value; }
        }
        
        public bool WantExtended
        {
            get { return _wantExtended; }
            set { _wantExtended = value; }
        }

        public CompProperties_RetractableBarrier Props => (CompProperties_RetractableBarrier) props;

        public void Toggle()
        {
            _extended = !_extended;
        }

        public void ToggleWant()
        {
            _wantExtended = !_wantExtended;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref _extended, "extended");
            Scribe_Values.Look(ref _wantExtended, "wantExtended");
        }
    }
}