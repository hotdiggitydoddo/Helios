using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using PaddleBallBlitz.Components;

namespace PaddleBallBlitz.Aspects
{
    public class BallAI : Aspect
    {
        public readonly Spatial Spatial;
        public readonly Physics Physics;
        public readonly CircleCollider Collider;
        public readonly Sprite Sprite;

        public BallAI(uint entity, Spatial spatial, Physics physics, CircleCollider collider, Sprite sprite) : base(entity)
		{
            Spatial = spatial;
            Physics = physics;
		    Collider = collider;
		    Sprite = sprite;
		}
    }
}
