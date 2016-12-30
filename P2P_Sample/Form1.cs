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
        private List<Peer> knownPeers;
        private PeerManager peerManager;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            int portNum = 8080;
            peerManager = new PeerManager(portNum);
            peerManager.msgReceived += PeerManager_msgReceived;
            peerManager.PeerChange += PeerManager_PeerChange;
            await peerManager.StartAsync();
            Console.WriteLine("started listening");
        }

        private void PeerManager_PeerChange(object sender, P2PNET.EventArgs.PeerChangeEventArgs e)
        {
            foreach(Peer peer in e.Peers)
            {
                string ipAddress = peer.SocketClient.RemoteAddress;
                Console.WriteLine("new peer. IP address = " + ipAddress);
            }
            this.knownPeers = e.Peers;
        }

        private void PeerManager_msgReceived(object sender, P2PNET.EventArgs.MsgReceivedEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine("message = " + msg + ". From = " + e.BindingType.ToString());
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            byte[] x = Encoding.UTF8.GetBytes("Hello World!");
            await peerManager.SendBroadcastAsyncUDP(x);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            Peer firstPeer = knownPeers[0];
            string ipAddress = firstPeer.SocketClient.RemoteAddress;

            byte[] x = Encoding.UTF8.GetBytes("TCP Hello World!");
            await peerManager.SendMsgAsyncTCP(ipAddress, x);
        }
    }
}
