using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using Microsoft.Xna.Framework.Input;
using PaddleBallBlitz.Aspects;
using PaddleBallBlitz.Components;

namespace PaddleBallBlitz.Subsystems
{
    public class InputSubsystem : Subsystem
    {
        private readonly List<Controlable> _controlables;


        public InputSubsystem(EntityManager em) : base(em)
        {
            _controlables = new List<Controlable>();
            _bits.SetBit((int)ComponentTypes.Input);
            _bits.SetBit((int)ComponentTypes.Physics);
        }

        public override void CreateAspect(uint entity, List<IComponent> components)
        {
            var physics = (Physics)components.Single(x => x.GetType() == typeof(Physics));
            var input = (Input)components.Single(x => x.GetType() == typeof(Input));
            _controlables.Add(new Controlable(entity, input, physics));
        }

        public override bool HasAspect(uint entity)
        {
            return _controlables.Exists(x => x.Owner == entity);
        }

        public override void Update(float dt)
        {
            foreach (var c in _controlables)
            {
                if (!c.Input.IsCPU)
                {
                    var kbState = Keyboard.GetState();

                    if (kbState.IsKeyDown(Keys.Up))
                        c.Physics.VelY = -1;
                    else if (kbState.IsKeyDown(Keys.Down))
                        c.Physics.VelY = 1;
                    else
                        c.Physics.VelY = 0f;
                }
            }

            base.Update(dt);
        }
    }
}
