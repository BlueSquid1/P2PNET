using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSender
{
    class Cat : Pet
    {
        public Cat(string petName) : base(petName)
        {
            this.Type = AnimalType.Cat;
        }
    }
}
