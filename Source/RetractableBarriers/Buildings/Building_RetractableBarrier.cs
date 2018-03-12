using System.Collections.Generic;
using System.Text;
using FrontierDevelopments.RetractableBarriers.Comps;
using FrontierDevelopments.RetractableBarriers.Graphics;
using RimWorld;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.Buildings
{
    public class Building_RetractableBarrier : Building
    {
        private CompPowerTrader _powerTrader;

        private bool _retractOnPowerFailure;

        private Comp_RetractableBarrier _barrier;
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _powerTrader = GetComp<CompPowerTrader>();
            _barrier = GetComp<Comp_RetractableBarrier>();

            _powerTrader.powerStartedAction = () =>
                _barrier.Extended = _barrier.WantExtended;
            _powerTrader.powerStoppedAction = () =>
            {
                if (_retractOnPowerFailure) _barrier.Extended = false;
            };
        }

        public bool Extended => _barrier.Extended;

        private void Toggle()
        {
            _barrier.ToggleWant();
            if (_powerTrader?.PowerOn != true) return;
            if (_barrier.Extended)
                Retract();
            else
                Extend();
            _barrier.Toggle();
        }

        private void Extend()
        {
            def.blockLight = _barrier.Props.upBlockLight;
            def.fillPercent = _barrier.Props.upFillPercentage;
            def.hideAtSnowDepth = _barrier.Props.upHideAtSnowDepth;
            def.passability = _barrier.Props.upTraversability;
            def.pathCost = _barrier.Props.upPathCost;
        }

        private void Retract()
        {
            def.blockLight = _barrier.Props.downBlockLight;
            def.fillPercent = _barrier.Props.downFillPercentage;
            def.hideAtSnowDepth = _barrier.Props.downHideAtSnowDepth;
            def.passability = _barrier.Props.downTraversability;
            def.pathCost = _barrier.Props.downPathCost;
        }

        public override string GetInspectString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "fd.barriers.ui.inspect.state".Translate() + ": " +
                (Extended ? "fd.barriers.ui.inspect.extended".Translate() : "fd.barriers.ui.inspect.retracted".Translate()) + "\n");
            if (_powerTrader?.PowerOn == false)
            {
                stringBuilder.Append("fd.barriers.ui.inspect.offline".Translate() + "\n");
            }
            stringBuilder.Append(base.GetInspectString());
            return stringBuilder.ToString();
        }
        
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }

            if (Faction == Faction.OfPlayer)
            {
                yield return new Command_Toggle
                {
                    icon = Textures.BarrierHorizExtended,
                    defaultLabel = "fd.barriers.ui.extend.label".Translate(),
                    defaultDesc = "fd.barriers.ui.extend.desc".Translate(),
                    isActive = () => _barrier.WantExtended,
                    toggleAction = () => Toggle()
                };
                
                yield return new Command_Toggle
                {
                    icon = Textures.BarrierHorizRetracted,
                    defaultLabel = "fd.barriers.ui.retract.label".Translate(),
                    defaultDesc = "fd.barriers.ui.retract.desc".Translate(),
                    isActive = () => _retractOnPowerFailure,
                    toggleAction = () => _retractOnPowerFailure = !_retractOnPowerFailure
                };
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref _retractOnPowerFailure, "retractOnPowerFailure");
            base.ExposeData();
        }
    }
}