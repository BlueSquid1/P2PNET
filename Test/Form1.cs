using P2PNET.ApplicationLayer;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        //private ObjectManager objectManager;
        private FileManager fileManager;
        public Form1()
        {
            //objectManager = new ObjectManager();
            //objectManager.ObjReceived += ObjectManager_objReceived;

            fileManager = new FileManager();
            fileManager.ObjReceived += FileManager_ObjReceived;
            InitializeComponent();
        }

        private void FileManager_ObjReceived(object sender, P2PNET.ApplicationLayer.EventArgs.ObjReceivedEventArgs e)
        {
            Console.WriteLine("Got here");
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
            await fileManager.StartAsync();
        }

        private async void SendObj_Click(object sender, EventArgs e)
        {
            Person x = new Person("Harry", "Potter", 25);
            //await objectManager.SendBroadcastAsyncUDP(x);
        }

        private async void SendFile_Click(object sender, EventArgs e)
        {
            string targetIp = "192.168.1.114";
            //string targetIp = "192.168.1.113"; //me
            //string filePath = "test_file.txt";
            string filePath = "06-train-cat-shake-hands.jpg";
            await fileManager.SendFileAsyncTCP(targetIp, filePath);
        }
    }
}
