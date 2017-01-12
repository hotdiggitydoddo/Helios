using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;

namespace PaddleBallBlitz.Components
{
    public class Input : IComponent
    {
        public bool IsCPU { get; set; }

        public Input(bool isCpu = false)
        {
            IsCPU = isCpu;
        }
        public int TypeId()
        {
            return (int)ComponentTypes.Input;
        }
    }
}
