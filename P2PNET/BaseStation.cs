using P2PNET.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET
{
    public class BaseStation
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<MsgReceivedEventArgs> MsgReceived;

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

        public async Task SendUDPMsgAsync(string ipAddress, byte[] msg)
        {
            await senderUDP.SendToAsync(msg, ipAddress, this.portNum);
        }

        public async Task SendUDPBroadcastAsync(byte[] msg)
        {
            string brdcstAddress = "255.255.255.255";
            await senderUDP.SendToAsync(msg, brdcstAddress, this.portNum);
        }

        public async Task SendTCPMsgAsync(string ipAddress, byte[] msg)
        {
            int indexNum;
            bool peerKnown = IsPeerKnown(ipAddress, out indexNum);
            if (!peerKnown)
            {
                //maybe I should attempt to connect to it instead?
                throw (new PeerNotKnown("The ipAddress your have entered does not correspond to a known Peer. Check the IP address"));

            }

            await this.KnownPeers[indexNum].SendMsgTCPAsync(msg);

        }

        public void IncomingMsg(object sender, MsgReceivedEventArgs e)
        {
            //check if its from a new peer
            if(e.BindingType == TransportType.UDP)
            {
                string remotePeeripAddress = e.RemoteIp;
                int indexNum;
                bool peerKnown = IsPeerKnown(remotePeeripAddress, out indexNum);
                if (peerKnown)
                {
                    //not a new peer
                    return;
                }
            }

            string ipAddress = e.RemoteIp;

            //new peer establish a TCP connection with this peer
            DirectConnectTCP(ipAddress);
        }

        public void NewTCPConnection(object sender, TcpSocketListenerConnectEventArgs e)
        {
            StoreConnectedPeerTCP(e.SocketClient);
        }

        public async void DirectConnectTCP(string ipAddress)
        {
            //send connection request
            TcpSocketClient senderTCP = new TcpSocketClient();

            //if you get an error on the line below then the person you trying to connect to
            //hasn't accepted in the incoming connection
            await senderTCP.ConnectAsync(ipAddress, this.portNum);
            ITcpSocketClient socketClient = senderTCP;
            StoreConnectedPeerTCP(socketClient);
        }

        private void StoreConnectedPeerTCP( ITcpSocketClient socketClient )
        {
            Peer newPeer = new Peer(socketClient);
            newPeer.MsgReceived += NewPeer_MsgReceived;
            knownPeers.Add(newPeer);
            PeerChange?.Invoke(this, new PeerChangeEventArgs(knownPeers));
        }

        private void NewPeer_MsgReceived(object sender, MsgReceivedEventArgs e)
        {
            MsgReceived?.Invoke(this, e);
        }

        private bool IsPeerKnown(string ipAddress, out int indexNum)
        {
            for(indexNum = 0; indexNum < this.knownPeers.Count; ++indexNum)
            {
                if(this.knownPeers[indexNum].SocketClient.RemoteAddress == ipAddress)
                {
                    return true;
                }
            }

            indexNum = -1;
            return false;
        }
    }
}
