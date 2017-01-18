using P2PNET.ObjectLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workspace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ObjectManager objectManager = new ObjectManager();

            Person person = new Person("Harry", "Potter", 16);

            byte[] msg = await objectManager.PackObjectIntoMsg(person);
            BObject temp = objectManager.ProcessReceivedMsg(msg);
            string objType = temp.GetType();
            switch(objType)
            {
                case "Person":
                    Person tempPerson = temp.GetObject<Person>();
                    break;
            }
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Person(string firstName, string lastName, int mAge)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = mAge;

        }
    }
}
