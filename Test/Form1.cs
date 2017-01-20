using P2PNET.FileLayer;
using P2PNET.ObjectLayer;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private ObjectManager objectManager;
        private FileManager fileManager;
        public Form1()
        {
            /*
            objectManager = new ObjectManager(8080, true);
            objectManager.ObjReceived += ObjectManager_objReceived;
            */

            
            fileManager = new FileManager(8080, true);
            fileManager.ObjReceived += FileManager_ObjReceived;
            fileManager.DebugInfo += FileManager_DebugInfo;
            
            InitializeComponent();
        }

        private void FileManager_DebugInfo(object sender, P2PNET.FileLayer.EventArgs.DebugInfoEventArgs e)
        {
            Console.WriteLine(e.Msg);
        }

        private void FileManager_ObjReceived(object sender, P2PNET.ObjectLayer.EventArgs.ObjReceivedEventArgs e)
        {
            //Console.WriteLine("Got here");
        }

        private void ObjectManager_objReceived(object sender, P2PNET.ObjectLayer.EventArgs.ObjReceivedEventArgs e)
        {
            BObject bObj = e.Obj;
            switch (bObj.GetType())
            {
                case "Person":
                    Person person = bObj.GetObject<Person>();
                    break;
                case "HardClass":
                    HardClass test = bObj.GetObject<HardClass>();
                    break;
                default:
                    //unknown type
                    Console.WriteLine("unknown object type");
                    break;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //await objectManager.StartAsync();
            await fileManager.StartAsync();
        }

        private async void SendObj_Click(object sender, EventArgs e)
        {
            Person x = new Person("Harry", "Potter", 25);
            await objectManager.SendBroadcastAsyncUDP(x);
        }

        private async void SendFile_Click(object sender, EventArgs e)
        {
            //string targetIp = "192.168.1.114";
            string targetIp = "192.168.1.112"; //me
            //string filePath = "test_file.txt";
            string filePath = "06-train-cat-shake-hands.jpg";
            await fileManager.SendFileAsyncTCP(targetIp, filePath);
        }
    }
}
