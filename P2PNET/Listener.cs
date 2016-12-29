using System;
using P2PNET.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace P2PNET
{
    public class Listener
    {
        //triggered when a peer send a connect request to this peer
        public event EventHandler<PeerConnectReqEventArgs> PeerConnectTCPRequest;
        public event EventHandler<MsgReceivedEventArgs> IncomingMsg;

        private UdpSocketReceiver listenerUDP;
        private TcpSocketListener listenerTCP;

        //constructor
        public Listener(int mPortNum)
        {
            this.listenerUDP = new UdpSocketReceiver();
            this.listenerTCP = new TcpSocketListener();

            StartListeningTCP(mPortNum);
            StartListeningUDP(mPortNum);
        }

        private void StartListeningTCP(int portNum)
        {
            listenerTCP.ConnectionReceived += ListenerTCP_ConnectionReceived;
            listenerTCP.StartListeningAsync(portNum);
        }

        private void StartListeningUDP(int portNum)
        {
            listenerUDP.MessageReceived += ListenerUDP_MessageReceived;
            listenerUDP.StartListeningAsync(portNum);
        }

        private void ListenerUDP_MessageReceived(object sender, UdpSocketMessageReceivedEventArgs e)
        {
            IncomingMsg?.Invoke(this, new MsgReceivedEventArgs(e.RemoteAddress, e.ByteData, TransportType.UDP));
        }

        private void ListenerTCP_ConnectionReceived(object sender, TcpSocketListenerConnectEventArgs e)
        {
            Peer newPeer = new Peer();
            PeerConnectTCPRequest?.Invoke(this, new PeerConnectReqEventArgs(newPeer));
        }
    }
}
