using System.Collections.Generic;
using System.Text;
using FrontierDevelopments.RetractableBarriers.Graphics;
using RimWorld;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.Buildings
{
    public class Building_RetractableBarrier : Building
    {
        private const float FillPercent = 0.8f;
        
        private CompPowerTrader _powerTrader;

        private bool _extended;
        private bool _wantExtended;
        private bool _retractOnPowerFailure;
        
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            _powerTrader = GetComp<CompPowerTrader>();

            _powerTrader.powerStartedAction = () => 
                _extended = _wantExtended;
            _powerTrader.powerStoppedAction = () =>
            {
                if (_retractOnPowerFailure) _extended = false;
            };
        }

        public bool Extended => _extended;

        private void Toggle()
        {
            _wantExtended = !_wantExtended;
            if (_powerTrader?.PowerOn != true) return;
            if (_extended)
                Retract();
            else
                Extend();
            _extended = !_extended;
        }

        private void Extend()
        {
            def.graphicData.shadowData.volume = new Vector3(FillPercent, FillPercent, FillPercent);
            def.blockLight = true;
            def.fillPercent = 1.0f;
            def.hideAtSnowDepth = 0.0f;
            def.passability = Traversability.PassThroughOnly;
            def.pathCost = 100;
            def.fillPercent = FillPercent;
        }

        private void Retract()
        {
            def.graphicData.shadowData.volume = new Vector3(0, 0, 0);
            def.blockLight = false;
            def.fillPercent = 0.0f;
            def.hideAtSnowDepth = 0.25f;
            def.passability = Traversability.Standable;
            def.pathCost = 0;
            def.fillPercent = 1.0f;
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
                    isActive = () => _wantExtended,
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
            Scribe_Values.Look(ref _extended, "extended");
            Scribe_Values.Look(ref _wantExtended, "wantExtended");
            Scribe_Values.Look(ref _retractOnPowerFailure, "retractOnPowerFailure");
            base.ExposeData();
        }
    }
}