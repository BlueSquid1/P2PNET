using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSender
{
    public enum AnimalType
    {
        Dog = 0,
        Cat = 1,
        Fish = 2
    }
    public class Pet
    {
        public AnimalType Type { get; set; }
        public string Name { get; set; }

        public Pet(string petName)
        {
            this.Name = petName;
        }

        public bool Equals(Pet pet)
        {
            if(pet.Name == this.Name && pet.Type == this.Type)
            {
                return true;
            }
            return false;
        }
    }
}
