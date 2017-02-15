using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2PNET.TransportLayer;

namespace AutoDiscovery
{
    public partial class Form1 : Form
    {
        private TransportManager transMgr;
        private int portNum = 8080;
        private HeartBeatManager hrtBtMgr;

        public Form1()
        {
            transMgr = new TransportManager(portNum);
            hrtBtMgr = new HeartBeatManager("heartbeat", transMgr);

            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            transMgr.PeerChange += TransMgr_PeerChange;
            await transMgr.StartAsync();
            hrtBtMgr.StartBroadcasting();
        }

        private void TransMgr_PeerChange(object sender, P2PNET.TransportLayer.EventArgs.PeerChangeEventArgs e)
        {
            peerListView.Items.Clear();
            foreach(Peer peer in e.Peers)
            {
                string[] peerDtl = {
                    peer.IpAddress
                };
                ListViewItem item = new ListViewItem(peerDtl);
                peerListView.Items.Add(item);
            }
        }
    }
}
