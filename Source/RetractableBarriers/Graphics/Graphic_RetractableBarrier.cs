using FrontierDevelopments.RetractableBarriers.Buildings;
using UnityEngine;
using Verse;

namespace FrontierDevelopments.RetractableBarriers.Graphics
{
    public class Graphic_RetractableBarrier : Graphic
    {
        // TODO add shadow
        
        private Material _barrierHorizExtended;
        private Material _barrierVertExtended;
        private Material _barrierHorizRetracted;
        private Material _barrierVertRetracted;
        
        private Material GetMaterial(bool extended, bool horizontal)
        {
            if (extended)
                return horizontal ? _barrierHorizExtended : _barrierVertExtended;
            return horizontal ? _barrierHorizRetracted : _barrierVertRetracted;
        }

        private void Init(Shader newShader, Color newColor)
        {
            _barrierHorizExtended = MaterialPool.MatFrom(Textures.BarrierHorizExtended, newShader, newColor);
            _barrierVertExtended = MaterialPool.MatFrom(Textures.BarrierVertExtended, newShader, newColor);
            _barrierHorizRetracted = MaterialPool.MatFrom(Textures.BarrierHorizRetracted, newShader, newColor);
            _barrierVertRetracted = MaterialPool.MatFrom(Textures.BarrierVertRetracted, newShader, newColor);
        }
        
        public override void Init(GraphicRequest req)
        {
            Init(req.shader, req.color);
        }
        
        public override Material MatAt(Rot4 rot, Thing thing = null)
        {
            if(thing == null || thing.GetType() != typeof(Building_RetractableBarrier))
                return rot.IsHorizontal ? _barrierHorizExtended : _barrierVertExtended;
            
            var barrier = (Building_RetractableBarrier)thing;
            return GetMaterial(barrier.Extended, barrier.Rotation.IsHorizontal);
        }

        public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            var copy = new Graphic_RetractableBarrier();
            copy.Init(newShader, newColor);
            return copy;
        }
    }
    
    [StaticConstructorOnStartup]
    public static class Textures
    {
        public static readonly Texture2D BarrierHorizExtended = ContentFinder<Texture2D>.Get("Things/Buildings/RetractableBarrier/barrier_horizontal_extended");
        public static readonly Texture2D BarrierHorizRetracted = ContentFinder<Texture2D>.Get("Things/Buildings/RetractableBarrier/barrier_horizontal_retracted");
        public static readonly Texture2D BarrierVertExtended = ContentFinder<Texture2D>.Get("Things/Buildings/RetractableBarrier/barrier_vertical_extended");
        public static readonly Texture2D BarrierVertRetracted = ContentFinder<Texture2D>.Get("Things/Buildings/RetractableBarrier/barrier_vertical_retracted");
    }
}