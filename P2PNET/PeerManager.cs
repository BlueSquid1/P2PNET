using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P2PNET.EventArgs;

namespace P2PNET
{
    public class PeerManager
    {
        private Listener listener;
        private HeartBeat heartBeat;
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
            this.heartBeat = new HeartBeat();

            this.baseStation.PeerChange += BaseStation_PeerChange;

            this.listener.IncomingMsg += Listener_IncomingMsg;
            //baseStation looks up incoming messages to see if there is a new peer talk to us
            this.listener.IncomingMsg += baseStation.IncomingMsg;
            this.listener.PeerConnectTCPRequest += baseStation.NewTCPConnection;
        }

        private void BaseStation_PeerChange(object sender, PeerChangeEventArgs e)
        {
            PeerChange?.Invoke(this, e);
        }

        public void Start()
        {
            listener.Start();
        }

        public void SendMsgAsyncTCP(string ipAddress, byte[] msg)
        {
            
        }

        public void SendMsgAsyncUDP(string ipAddress, byte[] msg)
        {
            baseStation.SendUDPMsg(ipAddress, msg);
        }

        public void SendBroadcastUDP(byte[] msg)
        {
            baseStation.SendUDPBroadcast(msg);
        }


        private void Listener_IncomingMsg(object sender, MsgReceivedEventArgs e)
        {
            //send message out
            msgReceived?.Invoke(this, e);
        }
    }
}
