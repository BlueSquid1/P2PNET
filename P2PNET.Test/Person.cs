using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.Test
{
    public class Person : Object
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }

        public List<Pet> OwnedPets { get; set; }

        public Person(string firstName, string lastName, int age)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;

            OwnedPets = new List<Pet>();
        }

        public void AddPet(Pet newPet)
        {
            OwnedPets.Add(newPet);
        }

        public bool Equals(Person secPerson)
        {
            if(secPerson.OwnedPets.Count != this.OwnedPets.Count)
            {
                return false;
            }

            
            for(int i = 0; i < OwnedPets.Count; i++)
            {
                if(!this.OwnedPets[i].Equals(secPerson.OwnedPets[i]))
                {
                    return false;
                }
            }

            if(secPerson.Age == this.Age && secPerson.FirstName == this.FirstName && secPerson.LastName == this.LastName)
            {
                return true;
            }
            return false;
        }
    }
}
