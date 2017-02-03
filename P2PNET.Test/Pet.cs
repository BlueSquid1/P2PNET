using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.Test
{
    public enum AnimalType
    {
        DOG = 0,
        CAT = 1,
        FISH = 2
    }
    abstract class Pet
    {
        public AnimalType Type { get; set; }
        public string Name { get; set; }

        public Pet(string petName)
        {
            this.Name = petName;
        }
    }
}
