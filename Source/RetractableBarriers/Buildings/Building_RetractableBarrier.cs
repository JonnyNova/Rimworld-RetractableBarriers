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

            Comp_RetractableBarrier.BarrierDefs barrierDefs;

            if (!Comp_RetractableBarrier.barrierDefsMap.TryGetValue(_barrier.Props.id, out barrierDefs))
            {
                barrierDefs = new Comp_RetractableBarrier.BarrierDefs
                {
                    Retracted = def,
                    Extended = Comp_RetractableBarrier.Copy(def)
                };

                barrierDefs.Extended.blockLight = _barrier.Props.blockLight;
                barrierDefs.Extended.blockWind = _barrier.Props.blockWind;
                barrierDefs.Extended.fillPercent = _barrier.Props.fillPercent;
                barrierDefs.Extended.hideAtSnowDepth = _barrier.Props.hideAtSnowDepth;
                barrierDefs.Extended.passability = _barrier.Props.passability;
                barrierDefs.Extended.pathCost = _barrier.Props.pathCost;

                Comp_RetractableBarrier.barrierDefsMap[_barrier.Props.id] = barrierDefs;
            }
            
            // Apply right def states on load
            if (_barrier.Extended)
                Extend();
            else
                Retract();
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
        }

        private void Extend()
        {
            def = Comp_RetractableBarrier.barrierDefsMap[_barrier.Props.id].Extended;
            _barrier.Extended = true;
        }

        private void Retract()
        {
            def = Comp_RetractableBarrier.barrierDefsMap[_barrier.Props.id].Retracted;
            _barrier.Extended = false;
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