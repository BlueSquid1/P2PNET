using System;
using P2PNET.TransportLayer.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System.Threading.Tasks;

namespace P2PNET.TransportLayer
{
    public class Listener
    {
        //triggered when a peer send a connect request to this peer
        public event EventHandler<TcpSocketListenerConnectEventArgs> PeerConnectTCPRequest;
        public event EventHandler<MsgReceivedEventArgs> IncomingMsg;

        private UdpSocketReceiver listenerUDP;
        private TcpSocketListener listenerTCP;

        private int portNum;

        //constructor
        public Listener(int mPortNum)
        {
            this.listenerUDP = new UdpSocketReceiver();
            this.listenerTCP = new TcpSocketListener();

            this.portNum = mPortNum;
        }

        public async Task StartAsync()
        {
            await StartListeningAsyncTCP(this.portNum);
            await StartListeningAsyncUDP(this.portNum);
        }

        private async Task StartListeningAsyncTCP(int portNum)
        {
            listenerTCP.ConnectionReceived += ListenerTCP_ConnectionReceived;
            await listenerTCP.StartListeningAsync(portNum);
        }

        private async Task StartListeningAsyncUDP(int portNum)
        {
            listenerUDP.MessageReceived += ListenerUDP_MessageReceived;
            await listenerUDP.StartListeningAsync(portNum);
        }

        private void ListenerUDP_MessageReceived(object sender, UdpSocketMessageReceivedEventArgs e)
        {
            IncomingMsg?.Invoke(this, new MsgReceivedEventArgs(e.RemoteAddress, e.ByteData, TransportType.UDP));
        }

        private void ListenerTCP_ConnectionReceived(object sender, TcpSocketListenerConnectEventArgs e)
        {
            PeerConnectTCPRequest?.Invoke(this, e);
        }
    }
}
