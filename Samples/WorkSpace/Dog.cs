using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.Test
{
    class Dog : Pet
    {

        public Dog(string petName) : base(petName)
        {
            this.Type = AnimalType.Dog;
        }
    }
}
