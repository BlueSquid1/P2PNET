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

namespace AutoPeerDiscovery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            int portNum = 8080;
            PeerManager peerManager = new PeerManager(portNum);
            peerManager.PeerChange += PeerManager_PeerChange;
            await peerManager.StartAsync();
        }

        private void PeerManager_PeerChange(object sender, P2PNET.EventArgs.PeerChangeEventArgs e)
        {
            ListViewPeer.Items.Clear();

            foreach(Peer peer in e.Peers)
            {
                ListViewPeer.Items.Add(peer.IpAddress);
            }
        }
    }
}
