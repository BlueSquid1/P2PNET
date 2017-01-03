using Sockets.Plugin;
using System;
using System.Collections.Generic;
using P2PNET.EventArgs;
using System.Threading.Tasks;
using System.Threading;

namespace P2PNET
{
    public class PeerManager
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<MsgReceivedEventArgs> msgReceived;
        public event EventHandler<ObjReceivedEventArgs> objReceived;

        private Listener listener;
        private BaseStation baseStation;
        private Serializer serializer;

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
        public PeerManager(int mPortNum = 8080)
        {

            this.PortNum = mPortNum;
            this.listener = new Listener(this.PortNum);
            this.baseStation = new BaseStation(this.PortNum);
            this.serializer = new Serializer();

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

        public async Task<bool> SendMsgAsyncTCP(string ipAddress, byte[] msg)
        {
            return await baseStation.SendTCPMsgAsync(ipAddress, msg);
        }

        public async Task<bool> SendMsgAsyncUDP(string ipAddress, byte[] msg)
        {
            return await baseStation.SendUDPMsgAsync(ipAddress, msg);
        }

        public async Task SendBroadcastAsyncUDP(byte[] msg)
        {
            await baseStation.SendUDPBroadcastAsync(msg);
        }

        public async Task SendMsgToAllPeersAsyncUDP(byte[] msg)
        {
            await baseStation.SendTCPMsgToAllUDPAsync(msg);
        }

        //sends the message to all known peers
        public async Task SendMsgToAllPeersAsyncTCP(byte[] msg)
        {
            await baseStation.SendTCPMsgToAllTCPAsync(msg);
        }

        //TODO: not finished yet....
        public void SendObjToAllPeersAsyncUDP<T>(T obj)
        {
            ObjectMessage objMsg = new ObjectMessage(obj, obj.GetType());
            var temp = obj.GetType();
            string msg = serializer.SerializeObjectJSON(obj);

            ObjectMessage tempObjMsg = serializer.DeserializeObjectJSON<ObjectMessage>(msg);

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
            msgReceived?.Invoke(this, e);
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