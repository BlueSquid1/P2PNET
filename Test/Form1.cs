using P2PNET.FileLayer;
using P2PNET.ObjectLayer;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private FileManager fileManager;
        public Form1()
        {   
            fileManager = new FileManager(8080, true);
            fileManager.ObjReceived += FileManager_ObjReceived;
            fileManager.FileReceived += FileManager_FileReceived;
            fileManager.FileProgUpdate += FileManager_FileProgUpdate;

            InitializeComponent();
        }

        private void FileManager_FileProgUpdate(object sender, P2PNET.FileLayer.EventArgs.FileTransferEventArgs e)
        {
            Console.WriteLine("got here");
        }

        private void FileManager_FileReceived(object sender, P2PNET.FileLayer.EventArgs.FileReceivedEventArgs e)
        {
            Console.WriteLine("received file");
        }

        private void FileManager_ObjReceived(object sender, P2PNET.ObjectLayer.EventArgs.ObjReceivedEventArgs e)
        {
            //Console.WriteLine("Got here");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //await objectManager.StartAsync();
            await fileManager.StartAsync();
        }

        private async void SendFile_Click(object sender, EventArgs e)
        {
            //string targetIp = "192.168.1.114";
            //string targetIp = "192.168.1.112"; //me
            string targetIp = txtIpAddress.Text;
            //string filePath = "test_file.txt";
            string filePath = "06-train-cat-shake-hands.jpg";
            await fileManager.SendFileAsyncTCP(targetIp, filePath);
        }
    }
}
