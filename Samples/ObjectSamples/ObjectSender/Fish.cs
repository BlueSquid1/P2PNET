using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSender
{
    class Fish : Pet
    {
        public bool FreshWater { get; set; }
        public Fish(string petName) : base(petName)
        {
            this.Type = AnimalType.Fish;
        }
    }
}
