using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P2PNET.EventArgs;
using System.Threading.Tasks;

namespace P2PNET
{
    public class PeerManager
    {
        private Listener listener;
        private BaseStation baseStation;

        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<MsgReceivedEventArgs> msgReceived;

        public List<Peer> KnownPeers
        {
            get
            {
                return this.baseStation.KnownPeers;
            }
        }

        public int PortNum { get; }

        //constructor
        public PeerManager(int mPortNum = 8080)
        {
            this.PortNum = mPortNum;
            this.listener = new Listener(this.PortNum);
            this.baseStation = new BaseStation(this.PortNum);

            this.baseStation.PeerChange += BaseStation_PeerChange;
            this.baseStation.MsgReceived += Listener_IncomingMsg;
            this.listener.IncomingMsg += Listener_IncomingMsg;

            //baseStation looks up incoming messages to see if there is a new peer talk to us
            this.listener.IncomingMsg += baseStation.IncomingMsg;
            this.listener.PeerConnectTCPRequest += baseStation.NewTCPConnection;
        }

        public void Start()
        {
            listener.Start();
        }

        public async void SendMsgAsyncTCP(string ipAddress, byte[] msg)
        {
            await baseStation.SendTCPMsgAsync(ipAddress, msg);
        }

        public async void SendMsgAsyncUDP(string ipAddress, byte[] msg)
        {
            await baseStation.SendUDPMsgAsync(ipAddress, msg);
        }

        public async void SendBroadcastAsyncUDP(byte[] msg)
        {
            await baseStation.SendUDPBroadcastAsync(msg);
        }

        //This is here for existing Peer to Peer systems that use asynchronous Connections.
        //This method is not needed otherwise because this class automatically keeps track
        //of peer connections
        public void DirrectConnectTCP(string ipAddress)
        {
            baseStation.DirectConnectTCP(ipAddress);
        }

        private void Listener_IncomingMsg(object sender, MsgReceivedEventArgs e)
        {
            //send message out
            msgReceived?.Invoke(this, e);
        }

        private void BaseStation_PeerChange(object sender, PeerChangeEventArgs e)
        {
            PeerChange?.Invoke(this, e);
        }
    }
}
