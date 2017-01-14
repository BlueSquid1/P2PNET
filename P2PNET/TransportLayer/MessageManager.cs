using Sockets.Plugin;
using System;
using System.Collections.Generic;
using P2PNET.TransportLayer.EventArgs;
using System.Threading.Tasks;

namespace P2PNET.TransportLayer
{
    public class MessageManager
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<MsgReceivedEventArgs> MsgReceived;

        private Listener listener;
        private BaseStation baseStation;

        public List<Peer> KnownPeers
        {
            get
            {
                return this.baseStation.KnownPeers;
            }
        }

        private string ipAddress = null;

        public async Task<string> GetIpAddress()
        {
            if(ipAddress == null)
            {
                ipAddress = await GetLocalIPAddress();
            }
            return ipAddress;
        }
        public int PortNum { get; }

        //constructor
        //mForwardAll = true will listener for duplicate messages and messages broadcasted from
        //this peer
        public MessageManager(int mPortNum = 8080, bool mForwardAll = false)
        {

            this.PortNum = mPortNum;
            this.listener = new Listener(this.PortNum);
            this.baseStation = new BaseStation(this.PortNum, mForwardAll);

            //this.serializer

            this.baseStation.PeerChange += BaseStation_PeerChange;
            this.baseStation.MsgReceived += IncomingMsg;

            //baseStation looks up incoming messages to see if there is a new peer talk to us
            this.listener.IncomingMsg += baseStation.IncomingMsgAsync;
            this.listener.PeerConnectTCPRequest += baseStation.NewTCPConnection;
        }

        public async Task StartAsync()
        {
            baseStation.LocalIpAddress = await GetLocalIPAddress();
            await listener.StartAsync();
        }

        public async Task<bool> SendAsyncTCP(string ipAddress, byte[] msg)
        {
            return await baseStation.SendTCPMsgAsync(ipAddress, msg);
        }

        public async Task<bool> SendAsyncUDP(string ipAddress, byte[] msg)
        {
            return await baseStation.SendUDPMsgAsync(ipAddress, msg);
        }

        public async Task SendBroadcastAsyncUDP(byte[] msg)
        {
            await baseStation.SendUDPBroadcastAsync(msg);
        }

        public async Task SendToAllPeersAsyncUDP(byte[] msg)
        {
            await baseStation.SendTCPMsgToAllUDPAsync(msg);
        }

        //sends the message to all known peers
        public async Task SendToAllPeersAsyncTCP(byte[] msg)
        {
            await baseStation.SendTCPMsgToAllTCPAsync(msg);
        }

        //This is here for existing Peer to Peer systems that use asynchronous Connections.
        //This method is not needed otherwise because this class automatically keeps track
        //of peer connections
        public async Task DirrectConnectAsyncTCP(string ipAddress)
        {
            await baseStation.DirectConnectTCPAsync(ipAddress);
        }

        private void IncomingMsg(object sender, MsgReceivedEventArgs e)
        {
            //send message out
            MsgReceived?.Invoke(this, e);
        }

        private void BaseStation_PeerChange(object sender, PeerChangeEventArgs e)
        {
            PeerChange?.Invoke(this, e);
        }

        private async Task<string> GetLocalIPAddress()
        {
            List<CommsInterface> interfaces = await CommsInterface.GetAllInterfacesAsync();
            foreach(CommsInterface comms in interfaces)
            {
                if(comms.ConnectionStatus == Sockets.Plugin.Abstractions.CommsInterfaceStatus.Connected)
                {
                    return comms.IpAddress;
                }
            }

            //raise exception
            throw (new NoNetworkInterface("Unable to find an active network interface connection. Is this device connected to wifi?"));
        }
    }
}