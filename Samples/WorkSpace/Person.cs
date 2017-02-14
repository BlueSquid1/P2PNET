using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.Test
{
    
    public class FavNum
    {
        public int number { get; set; }
    }
    
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }

        //public List<FavNum> favNums { get; set; }
        public List<Pet> OwnedPets { get; set; }

        public Person(string firstName, string lastName, int age)
        {
            //this.favNums = new List<FavNum>();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;

            OwnedPets = new List<Pet>();
        }

        /*
        public void AddNum(FavNum favNum)
        {
            favNums.Add(favNum); 
        }
        */

        public void AddPet(Pet newPet)
        {
            OwnedPets.Add(newPet);
        }
        
    }
}
