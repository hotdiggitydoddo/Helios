using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PaddleBallBlitz.Components
{
    public abstract class Collider
    {
        public List<Collision> Collisions { get; }
        public List<Intersection> Intersections { get; }
        public float Scale { get; set; }
        public ColliderType Type { get; }

        protected Collider(ColliderType type)
        {
            Collisions = new List<Collision>();
            Intersections = new List<Intersection>();
            Type = type;
        }
    }
}
