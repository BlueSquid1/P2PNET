using P2PNET.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET
{
    public class BaseStation
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;

        public List<Peer> KnownPeers {
            get
            {
                return knownPeers;
            }
        }

        private List<Peer> knownPeers;
        private int portNum;

        private UdpSocketClient senderUDP;

        //constructor
        public BaseStation(int mPortNum)
        {
            this.knownPeers = new List<Peer>();
            this.senderUDP = new UdpSocketClient();

            this.portNum = mPortNum;
        }

        public void SendUDPMsg(string ipAddress, byte[] msg)
        {
            senderUDP.SendToAsync(msg, ipAddress, this.portNum);
        }

        public void SendUDPBroadcast(byte[] msg)
        {
            string brdcstAddress = "255.255.255.255";
            senderUDP.SendToAsync(msg, brdcstAddress, this.portNum);
        }


        public void IncomingMsg(object sender, MsgReceivedEventArgs e)
        {
            //check if its from a new peer
            if(e.BindingType == TransportType.UDP)
            {
                string remotePeeripAddress = e.RemoteIp;
                if(!isNewPeer(remotePeeripAddress))
                {
                    //not a new peer
                    return;
                }
            }

            //new peer establish a TCP connection with this peer
            ConnectionWithNewPeer();
        }

        public void NewTCPConnection(object sender, TcpSocketListenerConnectEventArgs e)
        {
            ConnectionWithNewPeer();
        }

        private void ConnectionWithNewPeer()
        {
            //TODO
            //...
            
            PeerChange?.Invoke(this, new PeerChangeEventArgs(knownPeers));
        }

        private bool isNewPeer(string ipAddress)
        {
            //TODO
            return true;
        }
    }
}
