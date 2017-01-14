using P2PNET.ApplicationLayer;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private ObjectManager objectManager;
        public Form1()
        {
            objectManager = new ObjectManager();
            objectManager.ObjReceived += ObjectManager_objReceived;
            InitializeComponent();
        }

        private void ObjectManager_objReceived(object sender, P2PNET.ApplicationLayer.EventArgs.ObjReceivedEventArgs e)
        {
            switch(e.Metadata.objectType)
            {
                case "Person":
                    Person person = e.Obj.GetObject<Person>();
                    break;
                case "HardClass":
                    HardClass test = e.Obj.GetObject<HardClass>();
                    break;
                default:
                    //unknown type
                    Console.WriteLine("unknown object type");
                    break;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await objectManager.StartAsync();
        }

        private async void SendObj_Click(object sender, EventArgs e)
        {
            Person x = new Person("Harry", "Potter", 25);
            await objectManager.SendBroadcastAsyncUDP(x);
        }
    }
}
