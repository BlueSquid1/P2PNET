using P2PNET.FileLayer;
using P2PNET.ObjectLayer;
using P2PNET.ObjectLayer.EventArgs;
using P2PNET.Test;
using P2PNET.TransportLayer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkSpace
{
    public partial class Form1 : Form
    {
        private TransportManager transManager;

        private List<Peer> peers;

        public Form1()
        {
            peers = new List<Peer>();
            transManager = new TransportManager(8080, true);

            InitializeComponent();
        }

        private void TransManager_MsgReceived(object sender, P2PNET.TransportLayer.EventArgs.MsgReceivedEventArgs e)
        {
            byte[] msg = e.Message;

            for(int i = 0; i < msg.Length; i++)
            {
                Console.Write(msg[i]);
            }
            Console.WriteLine();
        }

        private void FileManager_PeerChange(object sender, P2PNET.TransportLayer.EventArgs.PeerChangeEventArgs e)
        {
            peers = e.Peers;
            Console.WriteLine("----peer change----");
            foreach(Peer peer in e.Peers)
            {
                Console.WriteLine(peer.IpAddress);
            }
            Console.WriteLine("====peer change=====");
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            transManager.MsgReceived += TransManager_MsgReceived;
            transManager.PeerChange += FileManager_PeerChange;
            await transManager.StartAsync();
            //await objMgr.StartAsync();
            //await fileManager.StartAsync();
            //await transManager.StartAsync();
        }
        private async void SendObj_Click(object sender, EventArgs e)
        {
            Person x = new Person("Harry", "Potter", 25);
            //await objMgr.SendBroadcastAsyncUDP(x);
        }

        private async void SendFile_Click(object sender, EventArgs e)
        {
            //string targetIp = "192.168.1.114";
            //string targetIp = "192.168.1.112"; //me
            string targetIp = txtIpAddress.Text;

            //string filePath = "test_file.txt";
            //string filePath = "06-train-cat-shake-hands.jpg";
            string filePath = txtFilePath.Text;
            List<string> filePaths = new List<string>();
            filePaths.Add("06-train-cat-shake-hands.jpg");
            filePaths.Add("Debug.7z");
            //await fileManager.SendFileAsync(targetIp, filePaths);
        }

        private async void SendMsg_Click(object sender, EventArgs e)
        {
            string targetIp = txtIpAddress.Text;
            byte[] msg = new byte[] { 255, 0, 15, 70 };

            await transManager.SendAsyncTCP(targetIp, msg);
        }

        private async void Stream_Click(object sender, EventArgs e)
        {
            byte[] msg = new byte[] { 0, 25, 10 };

            //send number indicating message size
            int lenMsg = (int)msg.Length;
            byte[] lenBin = IntToBinary(lenMsg);
            await peers[0].WriteStream.WriteAsync(lenBin, 0, lenBin.Length);

            //send the msg
            await peers[0].WriteStream.WriteAsync(msg, 0, lenMsg);
            await peers[0].WriteStream.FlushAsync();
        }

        private byte[] IntToBinary(int value)
        {
            byte[] valueBin = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBin);
            }
            return valueBin;
        }
    }
}
