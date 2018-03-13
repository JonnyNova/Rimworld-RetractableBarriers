using System.Collections.Generic;
using FrontierDevelopments.RetractableBarriers.CompProperties;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.Comps
{
    public class Comp_RetractableBarrier : ThingComp
    {
        public struct BarrierDefs
        {
            public ThingDef Retracted;
            public ThingDef Extended;
        }

        public static readonly Dictionary<string, BarrierDefs> barrierDefsMap = new Dictionary<string, BarrierDefs>();
        
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