using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using PaddleBallBlitz.Components;

namespace PaddleBallBlitz.Aspects
{
    public class Controlable : Aspect
    {
        public readonly Physics Physics;
        public readonly Input Input;

        public Controlable(uint entity, Input input, Physics physics) : base(entity)
        {
            Input = input;
            Physics = physics;
        }
    }
}
