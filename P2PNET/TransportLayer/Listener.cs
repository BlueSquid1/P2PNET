using System;
using P2PNET.TransportLayer.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System.Threading.Tasks;
using System.IO;

namespace P2PNET.TransportLayer
{
    public class Listener : IDisposable
    {
        //triggered when a peer send a connect request to this peer
        public event EventHandler<TcpSocketListenerConnectEventArgs> PeerConnectTCPRequest;
        public event EventHandler<MsgReceivedEventArgs> IncomingMsg;

        private UdpSocketReceiver listenerUDP;
        private TcpSocketListener listenerTCP;

        //private WriteStreamUtil writeUtil;
        //private ReadStreamUtil readUtil;

        private int portNum;

        //constructor
        public Listener(int mPortNum)
        {
            this.listenerUDP = new UdpSocketReceiver();
            this.listenerTCP = new TcpSocketListener();

            Stream listenStream = new MemoryStream();
            //writeUtil = new WriteStreamUtil(listenStream);
            //readUtil = new ReadStreamUtil(listenStream);

            this.portNum = mPortNum;
        }

        //destory connection
        public void Dispose()
        {
            listenerUDP.Dispose();
            listenerTCP.Dispose();
        }

        public async Task StartAsync()
        {
            await StartListeningAsyncTCP(this.portNum);
            await StartListeningAsyncUDP(this.portNum);
        }

        /*
        public async Task StopAsync()
        {
            await listenerTCP.StopListeningAsync();
            await listenerUDP.StopListeningAsync();
        }
        */

        private async Task StartListeningAsyncTCP(int portNum)
        {
            listenerTCP.ConnectionReceived += ListenerTCP_ConnectionReceived;
            await listenerTCP.StartListeningAsync(portNum);
        }

        private async Task StartListeningAsyncUDP(int portNum)
        {
            listenerUDP.MessageReceived += ListenerUDP_MessageReceived;
            await listenerUDP.StartListeningAsync(portNum);

            /*
            bool udpListenerActive = true;
            while(udpListenerActive)
            {
                byte[] binMsg = await readUtil.ReadBytesAsync();
                IncomingMsg?.Invoke(this, new MsgReceivedEventArgs("255.255.255.255", binMsg, TransportType.UDP));
            }
            */
        }

        private async void ListenerUDP_MessageReceived(object sender, UdpSocketMessageReceivedEventArgs e)
        {
            //await writeUtil.WriteBytesAsync(e.ByteData);
            IncomingMsg?.Invoke(this, new MsgReceivedEventArgs(e.RemoteAddress, e.ByteData, TransportType.UDP));
        }

        private void ListenerTCP_ConnectionReceived(object sender, TcpSocketListenerConnectEventArgs e)
        {
            PeerConnectTCPRequest?.Invoke(this, e);
        }
    }
}
