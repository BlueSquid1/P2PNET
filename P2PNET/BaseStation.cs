using P2PNET.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System;
using System.Collections.Generic;
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

        public string LocalIpAddress { get; set; }

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
            //check if ipAddress is from this peer
            if(this.LocalIpAddress == ipAddress)
            {
                throw (new PeerNotKnown("The ipAddress your have entered does not correspond to a valid Peer. Check the IP address"));
            }

            //check if from unknown peer
            int indexNum;
            bool peerKnown = IsPeerKnown(ipAddress, out indexNum);
            if (!peerKnown)
            {
                //ipaddress is unknown
                //try to establish an connection with this ipAddress
                try
                {
                    await DirectConnectTCPAsync(ipAddress);
                }
                catch( Exception )
                {
                    throw (new PeerNotKnown("The ip address your have entered does not correspond to a valid Peer. Check the IP address."));
                }
            }

            await this.KnownPeers[indexNum].SendMsgTCPAsync(msg);

        }

        public async void IncomingMsgAsync(object sender, MsgReceivedEventArgs e)
        {
            //check if message is from this peer
            if(e.RemoteIp == this.LocalIpAddress)
            {
                //from this peer.
                //no futher proccessing needed
                return;
            }

            //check if its from a new peer
            if(e.BindingType == TransportType.UDP)
            {
                string remotePeeripAddress = e.RemoteIp;
                int indexNum;
                bool peerKnown = IsPeerKnown(remotePeeripAddress, out indexNum);
                if (!peerKnown)
                {
                    //not a known peer
                    await DirectConnectTCPAsync(remotePeeripAddress);
                }
            }

            //trigger sent message
            MsgReceived?.Invoke(this, e);
        }

        public void NewTCPConnection(object sender, TcpSocketListenerConnectEventArgs e)
        {
            StoreConnectedPeerTCP(e.SocketClient);
        }

        public async Task DirectConnectTCPAsync(string ipAddress)
        {
            //send connection request
            TcpSocketClient senderTCP = new TcpSocketClient();

            //if you get an error on the line below then the person you trying to connect to
            //hasn't accepted in the incoming connection
            try
            {
                await senderTCP.ConnectAsync(ipAddress, this.portNum);
            }
            catch( Exception e1)
            {
                throw e1;
            }
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

        //returns true if the ip address corresponds to known peer. If the ip address is equal to this peer's
        //local ip address then returns true and indexNum = -1
        private bool IsPeerKnown(string ipAddress, out int indexNum)
        {
            if(this.LocalIpAddress == ipAddress)
            {
                //msg from this peer
                indexNum = -1;
                return true;
            }

            for(indexNum = 0; indexNum < this.knownPeers.Count; ++indexNum)
            {
                if(this.knownPeers[indexNum].IpAddress == ipAddress)
                {
                    return true;
                }
            }

            indexNum = -1;
            return false;
        }
    }
}
