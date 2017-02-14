using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2PNET.ObjectLayer;

namespace ObjectSender
{
    public partial class Form1 : Form
    {
        private ObjectManager ObjMgr;
        private int portNum = 8080;

        public Form1()
        {
            ObjMgr = new ObjectManager(portNum, true);
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ObjMgr.ObjReceived += ObjMgr_ObjReceived;
            await ObjMgr.StartAsync();
        }

        private void ObjMgr_ObjReceived(object sender, P2PNET.ObjectLayer.EventArgs.ObjReceivedEventArgs e)
        {
            Metadata metadata = e.Meta;
            PrintMetadata(metadata);
            switch (metadata.ObjectType)
            {
                case "Dog":
                    Dog receivedDog = e.Obj.GetObject<Dog>();
                    PrintDog(receivedDog);
                    break;
                case "Cat":
                    Cat receivedCat = e.Obj.GetObject<Cat>();
                    PrintCat(receivedCat);
                    break;
                case "Fish":
                    Fish receivedFish = e.Obj.GetObject<Fish>();
                    PrintFish(receivedFish);
                    break;
                case "Person":
                    Person receivedPerson = e.Obj.GetObject<Person>();
                    PrintPerson(receivedPerson);
                    break;
                default:
                    Console.WriteLine("unknown object type");
                    break;
            }
        }

        private void PrintMetadata(Metadata meta)
        {
            Console.WriteLine("source = " + meta.SourceIp + ", " + meta.BindType);
        }

        private void PrintDog(Dog dog)
        {
            Console.WriteLine("dog name = "+ dog.Name);
        }

        private void PrintPet(Pet pet)
        {
            Console.WriteLine("Pet type = " + pet.Type + ", name = " + pet.Name);
        }

        private void PrintCat(Cat cat)
        {
            Console.WriteLine("cat name = " + cat.Name);
        }

        private void PrintFish(Fish fish)
        {
            Console.WriteLine("fish name = " + fish.Name + ", is fresh water = " + fish.FreshWater);
        }

        private void PrintPerson(Person person)
        {
            Console.WriteLine("person name = " + person.FirstName + " " + person.LastName + ", Age = " + person.Age);
            foreach(Pet pet in person.OwnedPets)
            {
                PrintPet(pet);
            }
        }


        private async void btnSendDog_Click(object sender, EventArgs e)
        {
            Dog petDog = new Dog(txtDogName.Text);
            string ipAddress = GetIpAddress();
            await ObjMgr.SendAsyncTCP(ipAddress, petDog);
        }

        private async void btnSendCat_Click(object sender, EventArgs e)
        {
            Cat petCat = new Cat(txtCatName.Text);
            string ipAddress = GetIpAddress();
            await ObjMgr.SendAsyncTCP(ipAddress, petCat);
        }

        private async void btnSendFish_Click(object sender, EventArgs e)
        {
            Fish petFish = new Fish(txtFishName.Text);
            string ipAddress = GetIpAddress();
            await ObjMgr.SendAsyncTCP(ipAddress, petFish);
        }

        private async void btnSendPerson_Click(object sender, EventArgs e)
        {
            Person person = new Person(txtFirstName.Text, txtLastName.Text, 20);

            if (chkIncludeDog.Checked )
            {
                person.AddPet(new Dog(txtDogName.Text));
            }

            if (chkIncludeCat.Checked)
            {
                person.AddPet(new Cat(txtCatName.Text));
            }

            if (chkIncludeFish.Checked)
            {
                person.AddPet(new Fish(txtFishName.Text));
            }

            string ipAddress = GetIpAddress();
            await ObjMgr.SendAsyncTCP(ipAddress, person);
        }

        private string GetIpAddress()
        {
            string tempIpAddress = txtIpAddress.Text;
            if(tempIpAddress == "")
            {
                Console.WriteLine("warning. No ip address entered.");
            }
            return tempIpAddress;
        }
    }
}
