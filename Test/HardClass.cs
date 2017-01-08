using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class HardClass
    {
        public Person person;

        //constructor
        public HardClass()
        {
            this.person = new Test.Person("Harry", "Potter", 16);
        }
    }
}
