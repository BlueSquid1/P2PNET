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
            //TODO
            throw new NotImplementedException();
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
            Fish petFish = new Fish(txtFishName.Text, chkFreshWater.Checked);
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

            if (chkIncludeDog.Checked)
            {
                person.AddPet(new Cat(txtCatName.Text));
            }

            if (chkIncludeDog.Checked)
            {
                person.AddPet(new Fish(txtFishName.Text, chkFreshWater.Checked));
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
