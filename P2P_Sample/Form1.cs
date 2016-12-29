using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2PNET;


namespace P2P_Sample
{
    public partial class Form1 : Form
    {
        private PeerManager peerManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int portNum = 8080;
            peerManager = new PeerManager(portNum);
            peerManager.msgReceived += PeerManager_msgReceived;
        }

        private void PeerManager_msgReceived(object sender, P2PNET.EventArgs.MsgReceivedEventArgs e)
        {
            Console.WriteLine("message = " + e.Message.ToString());
        }
    }
}
