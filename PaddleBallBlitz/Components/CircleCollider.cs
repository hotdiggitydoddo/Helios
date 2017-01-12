using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using Microsoft.Xna.Framework;
using PaddleBallBlitz.Helpers;

namespace PaddleBallBlitz.Components
{
    public class CircleCollider : Collider, IComponent
    {
        private float _radius;
        
        public Circle Bounds { get; set; }

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
            }
        }

        public Vector2 Center {  get { return Bounds.Center; } }
        public int TypeId()
        {
            return (int)ComponentTypes.Collider;
        }

        public CircleCollider(Vector2 position, float radius) : base(ColliderType.Circle)
        {
            Bounds = new Circle(new Vector2(position.X + radius, position.Y + radius), radius);
            Radius = radius;
        }
    }
}
