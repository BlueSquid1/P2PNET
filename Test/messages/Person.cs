using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.messages
{
    public class Person
    {
        public string firstName;
        public string lastName;
        public int age;

        public Person(string mFirst = "Phillip", string mLast = "King", int mAge = 20)
        {
            this.firstName = mFirst;
            this.lastName = mLast;
            this.age = mAge;
        }
    }
}
