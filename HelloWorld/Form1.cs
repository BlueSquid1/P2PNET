using P2PNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        private PeerManager peerManager;
        private int portNum;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            portNum = 8080;
            peerManager = new PeerManager(portNum);
            peerManager.msgReceived += PeerManager_msgReceived;
            await peerManager.StartAsync();
        }

        private void PeerManager_msgReceived(object sender, P2PNET.EventArgs.MsgReceivedEventArgs e)
        {
            byte[] msgBytes = e.Message;
            string recievedMsg = Encoding.Unicode.GetString(msgBytes);
            TxtReceived.Text = recievedMsg;
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            //check IP address
            string ipAddress = TxtIpAddress.Text;
            string sendMsg = TxtMsgBox.Text;
            byte[] msgBytes = Encoding.Unicode.GetBytes(sendMsg);
            await peerManager.SendMsgAsyncTCP(ipAddress, msgBytes);
        }
    }
}
