using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using Microsoft.Xna.Framework;
using PaddleBallBlitz.Components;

namespace PaddleBallBlitz
{
    public class Collidable : Aspect
    {
        public readonly Spatial Spatial;
        public readonly Collider Collider;

        public Collidable(uint entity, Spatial spatial, Collider collider) : base(entity)
        {
            Spatial = spatial;
            Collider = collider;
        }
    }
}
