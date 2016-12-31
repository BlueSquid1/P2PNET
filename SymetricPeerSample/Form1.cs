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

namespace SymetricPeerSample
{
    public partial class Form1 : Form
    {
        private PeerManager peerManager;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            int portNum = 8080;
            peerManager = new PeerManager(portNum);
            peerManager.PeerChange += PeerManager_PeerChange;
            peerManager.msgReceived += PeerManager_msgReceived;
            await peerManager.StartAsync();
        }

        private void PeerManager_msgReceived(object sender, P2PNET.EventArgs.MsgReceivedEventArgs e)
        {
            
        }

        private void PeerManager_PeerChange(object sender, P2PNET.EventArgs.PeerChangeEventArgs e)
        {
            //update view list
            PeerListView.Items.Clear();
            foreach (Peer peer in e.Peers)
            {
                string[] peerDtl = {
                    peer.IpAddress
                };
                ListViewItem item = new ListViewItem(peerDtl);
                PeerListView.Items.Add(item);
            }
        }
    }
}
