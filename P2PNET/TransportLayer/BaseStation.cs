using P2PNET.TransportLayer.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace P2PNET.TransportLayer
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
        private bool forwardAll;

        private UdpSocketClient senderUDP;

        //constructor
        public BaseStation(int mPortNum, bool mForwardAll = false)
        {
            this.knownPeers = new List<Peer>();
            this.senderUDP = new UdpSocketClient();

            this.forwardAll = mForwardAll;
            this.portNum = mPortNum;
        }

        public async Task<bool> SendUDPMsgAsync(string ipAddress, byte[] msg)
        {
            bool isPeerKnown = DoesPeerExistByIp(ipAddress);
            if(isPeerKnown && this.LocalIpAddress != ipAddress)
            {
                int peerIndex = FindPeerByIp(ipAddress);
                bool isPeerActive = knownPeers[peerIndex].IsPeerActive;
                if(!isPeerActive)
                {
                    //peer not active. therefore can't received messages
                    return false;
                }
            }

            await senderUDP.SendToAsync(msg, ipAddress, this.portNum);
            return true;
        }

        public async Task SendUDPBroadcastAsync(byte[] msg)
        {
            string brdcstAddress = "255.255.255.255";
            await senderUDP.SendToAsync(msg, brdcstAddress, this.portNum);
        }

        public async Task SendTCPMsgToAllUDPAsync(byte[] msg)
        {
            foreach(Peer peer in knownPeers)
            {
                string curIpAddress = peer.IpAddress;
                bool isActive = peer.IsPeerActive;
                if (isActive)
                {
                    await SendUDPMsgAsync(curIpAddress, msg);
                }
            }
        }

        //returns false if was unsuccessful
        //thinking about throwing an exception instead
        public async Task<bool> SendTCPMsgAsync(string ipAddress, byte[] msg)
        {
            //check if ipAddress is from this peer
            if(this.LocalIpAddress == ipAddress)
            {
                throw (new PeerNotKnown("The ipAddress your have entered does not correspond to a valid Peer. Check the IP address"));
            }

            //check if from unknown peer
            bool peerKnown = DoesPeerExistByIp(ipAddress);
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

            int indexNum = FindPeerByIp(ipAddress);
            //make sure peer is active
            if (!this.KnownPeers[indexNum].IsPeerActive)
            {
                //peer not active
                return false;
            }
            return await this.KnownPeers[indexNum].SendMsgTCPAsync(msg);

        }

        public async Task SendTCPMsgToAllTCPAsync(byte[] msg)
        {
            foreach(Peer peer in knownPeers)
            {
                string curIpAddress = peer.IpAddress;
                bool isActive = peer.IsPeerActive;
                if(isActive)
                {
                    await SendTCPMsgAsync(curIpAddress, msg);
                }
            }
        }

        public async void IncomingMsgAsync(object sender, MsgReceivedEventArgs e)
        {
            //check if message is from this peer
            if(e.RemoteIp == this.LocalIpAddress)
            {
                //from this peer.
                //no futher proccessing needed
                if(forwardAll == false)
                {
                    return;
                }
            }

            //check if its from a new peer
            if(e.BindingType == TransportType.UDP)
            {
                string remotePeeripAddress = e.RemoteIp;
                bool peerKnown = DoesPeerExistByIp(remotePeeripAddress);
                if (!peerKnown)
                {
                    //not a known peer
                    await DirectConnectTCPAsync(remotePeeripAddress);
                }
            }

            //check if its a blank UDP packet
            //These are used as heart beats
            if(e.Message.Length <= 0)
            {
                return;
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
            newPeer.peerStatusChange += NewPeer_peerStatusChange;
            knownPeers.Add(newPeer);
            PeerChange?.Invoke(this, new PeerChangeEventArgs(knownPeers));
        }

        private void NewPeer_peerStatusChange(object sender, System.EventArgs e)
        {
            Peer changedPeer = (Peer)sender;
            int indexNum = FindPeerByIp(changedPeer.IpAddress);
            if(indexNum < 0)
            {
                throw (new PeerNotKnown("This error message is imposible to reach. Changed peer is not known"));
            }

            //delete inactive peers
            bool isPeerInactive = knownPeers[indexNum].IsPeerActive;
            if(isPeerInactive)
            {
                //delete from list
                knownPeers.Remove(changedPeer);
            }
        }

        private void NewPeer_MsgReceived(object sender, MsgReceivedEventArgs e)
        {
            MsgReceived?.Invoke(this, e);
        }

        //returns true if the ip address corresponds to known peer. If the ip address is equal to this peer's
        //local ip address then also returns true
        private bool DoesPeerExistByIp(string ipAddress)
        {
            if(this.LocalIpAddress == ipAddress)
            {
                //From local peer
                return true;
            }
            for(int i = 0; i < this.knownPeers.Count; ++i)
            {
                if(this.knownPeers[i].IpAddress == ipAddress)
                {
                    return true;
                }
            }
            return false;
        }

        //returns -1 if can't find peer
        private int FindPeerByIp(string ipAddress)
        {
            for (int i = 0; i < this.knownPeers.Count; ++i)
            {
                if (this.knownPeers[i].IpAddress == ipAddress)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
