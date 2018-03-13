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

        private ThingDef retractedDef;
        private ThingDef extendedDef;
        
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

            retractedDef = def;

            // hack cause two different ThingDefs are needed and it isn't cloneable
            extendedDef = Copy(def);
            extendedDef.blockLight = _barrier.Props.blockLight;
            extendedDef.blockWind = _barrier.Props.blockWind;
            extendedDef.fillPercent = _barrier.Props.fillPercent;
            extendedDef.hideAtSnowDepth = _barrier.Props.hideAtSnowDepth;
            extendedDef.passability = _barrier.Props.passability;
            extendedDef.pathCost = _barrier.Props.pathCost;
            
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
            def = extendedDef;
            _barrier.Extended = true;
        }

        private void Retract()
        {
            def = retractedDef;
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

        public static ThingDef Copy(ThingDef def)
        {
            return new ThingDef
            {
                // ThingDefs
                stackLimit = def.stackLimit,
                size = def.size,
                destroyable =  def.destroyable,
                rotatable =  def.rotatable,
                useHitPoints = def.useHitPoints,
                comps = def.comps,
                interactionCellOffset = def.interactionCellOffset,
                scatterableOnMapGen = def.scatterableOnMapGen,
                deepCountPerCell = def.deepCountPerCell,
                generateCommonality = def.generateCommonality,
                generateAllowChance = def.generateAllowChance,
                startingHpRange = def.startingHpRange,
                drawerType = def.drawerType,
                hideAtSnowDepth = def.hideAtSnowDepth,
                drawDamagedOverlay = def.drawDamagedOverlay,
                stealable = def.stealable,
                isSaveable = def.isSaveable,
                tradeability = def.tradeability,
                blueprintClass = def.blueprintClass,
                canBeSpawningInventory = def.canBeSpawningInventory,
                thingClass = def.thingClass,
                category = def.category,
                tickerType = def.tickerType,
                smallVolume = def.smallVolume,
                receivesSignals = def.receivesSignals,
                killedLeavings = def.killedLeavings,
                butcherProducts = def.butcherProducts,
                smeltProducts = def.smeltProducts,
                smeltable = def.smeltable,
                randomizeRotationOnSpawn = def.randomizeRotationOnSpawn,
                damageMultipliers = def.damageMultipliers,
                isBodyPartOrImplant = def.isBodyPartOrImplant,
                recipeMaker = def.recipeMaker,
                minifiedDef = def.minifiedDef,
                leaveResourcesWhenKilled = def.leaveResourcesWhenKilled,
                slagDef = def.slagDef,
                isUnfinishedThing = def.isUnfinishedThing,
                isFrame = def.isFrame,
                hasInteractionCell = def.hasInteractionCell,
                filthLeaving = def.filthLeaving,
                forceDebugSpawnable = def.forceDebugSpawnable,
                intricate = def.intricate,
                deepCommonality = def.deepCommonality,
                itemGeneratorTags = def.itemGeneratorTags,
                alwaysFlee = def.alwaysFlee,
                tools = def.tools,
                graphicData = def.graphicData,
                drawOffscreen = def.drawOffscreen,
                colorGenerator = def.colorGenerator,
                castEdgeShadows = def.castEdgeShadows,
                staticSunShadowHeight = def.staticSunShadowHeight,
                selectable = def.selectable,
                neverMultiSelect = def.neverMultiSelect,
                isAutoAttackableMapObject = def.isAutoAttackableMapObject,
                hasTooltip = def.hasTooltip,
                inspectorTabs = def.inspectorTabs,
                inspectorTabsResolved = def.inspectorTabsResolved,
                seeThroughFog = def.seeThroughFog,
                drawGUIOverlay = def.drawGUIOverlay,
                resourceReadoutPriority = def.resourceReadoutPriority,
                resourceReadoutAlwaysShow = def.resourceReadoutAlwaysShow,
                drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected,
                storedConceptLearnOpportunity = def.storedConceptLearnOpportunity,
                alwaysHaulable = def.alwaysHaulable,
                designateHaulable = def.designateHaulable,
                thingCategories = def.thingCategories,
                mineable = def.mineable,
                socialPropernessMatters = def.socialPropernessMatters,
                soundDrop = def.soundDrop,
                soundPickup = def.soundPickup,
                soundInteract = def.soundInteract,
                soundImpactDefault = def.soundImpactDefault,
                saveCompressible = def.saveCompressible,
                holdsRoof = def.holdsRoof,
                fillPercent = def.fillPercent,
                coversFloor = def.coversFloor,
                neverOverlapFloors = def.neverOverlapFloors,
                surfaceType = def.surfaceType,
                blockPlants = def.blockPlants,
                blockLight = def.blockLight,
                blockWind = def.blockWind,
                affectsRegions = def.affectsRegions,
                tradeTags = def.tradeTags,
                tradeNeverStack = def.tradeNeverStack,
                colorGeneratorInTraderStock = def.colorGeneratorInTraderStock,
                blueprintGraphicData = def.blueprintGraphicData,
                naturalTerrain = def.naturalTerrain,
                leaveTerrain = def.leaveTerrain,
                recipes = def.recipes,
                equippedAngleOffset = def.equippedAngleOffset,
                equipmentType = def.equipmentType,
                techLevel = def.techLevel,
                weaponTags = def.weaponTags,
                techHediffsTags = def.techHediffsTags,
                destroyOnDrop = def.destroyOnDrop,
                equippedStatOffsets = def.equippedStatOffsets,
                entityDefToBuild = def.entityDefToBuild,
                projectileWhenLoaded = def.projectileWhenLoaded,
                ingestible = def.ingestible,
                filth = def.filth,
                gas = def.gas,
                building = def.building,
                race = def.race,
                apparel = def.apparel,
                mote = def.mote,
                plant = def.plant,
                projectile = def.projectile,
                stuffProps = def.stuffProps,
                skyfaller = def.skyfaller,
                
                // BuildableDefs
                pathCostIgnoreRepeat = def.pathCostIgnoreRepeat,
                fertility = def.fertility,
                costStuffCount = def.costStuffCount,
                terrainAffordanceNeeded = def.terrainAffordanceNeeded,
                clearBuildingArea = def.clearBuildingArea,
                defaultPlacingRot = def.defaultPlacingRot,
                resourcesFractionWhenDeconstructed = def.resourcesFractionWhenDeconstructed,
                altitudeLayer = def.altitudeLayer,
                uiIcon = def.uiIcon,
                graphic = def.graphic,
                passability = def.passability,
                pathCost = def.pathCost,
                statBases = def.statBases,
                costList = def.costList,
                stuffCategories = def.stuffCategories,
                buildingPrerequisites = def.buildingPrerequisites,
                researchPrerequisites = def.researchPrerequisites,
                constructionSkillPrerequisite = def.constructionSkillPrerequisite,
                placingDraggableDimensions = def.placingDraggableDimensions,
                repairEffect = def.repairEffect,
                constructEffect = def.constructEffect,
                blueprintDef = def.blueprintDef,
                installBlueprintDef = def.installBlueprintDef,
                frameDef = def.frameDef,
                uiIconPath = def.uiIconPath,
                uiIconAngle = def.uiIconAngle,
                menuHidden = def.menuHidden,
                specialDisplayRadius = def.specialDisplayRadius,
                placeWorkers = def.placeWorkers,
                designationCategory = def.designationCategory,
                designationHotKey = def.designationHotKey,
                minTechLevelToBuild = def.minTechLevelToBuild,
                maxTechLevelToBuild = def.maxTechLevelToBuild,
                
                // Defs
                // TODO what do about you?
                defName = def.defName,
                // TODO what do about you?
                index = def.index,
                label = def.label,
                description = def.description,
                modExtensions = def.modExtensions,
                // TODO what do about you?
                shortHash = def.shortHash
            };
        }
    }
}