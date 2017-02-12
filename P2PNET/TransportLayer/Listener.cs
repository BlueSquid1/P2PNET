using System;
using P2PNET.TransportLayer.EventArgs;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace P2PNET.TransportLayer
{
    public class Listener : IDisposable
    {
        //triggered when a peer send a connect request to this peer
        public event EventHandler<TcpSocketListenerConnectEventArgs> PeerConnectTCPRequest;
        public event EventHandler<MsgReceivedEventArgs> IncomingMsg;

        private UdpSocketReceiver listenerUDP;
        private TcpSocketListener listenerTCP;

        private SemaphoreSlim messageReceive = new SemaphoreSlim(0);
        private SemaphoreSlim messageProccessed = new SemaphoreSlim(1);

        private UdpSocketMessageReceivedEventArgs curUDPMessage;

        private bool isListening;
        public bool IsListening
        {
            get
            {
                return isListening;
            }
        }

        private int portNum;

        //constructor
        public Listener(int mPortNum)
        {
            isListening = false;

            this.listenerUDP = new UdpSocketReceiver();
            this.listenerTCP = new TcpSocketListener();

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
            isListening = true;
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

            ListenLoop();
        }

        private async void ListenLoop()
        {
            bool udpListenerActive = true;
            while (udpListenerActive)
            {
                //wait until signal is recieved
                UdpSocketMessageReceivedEventArgs udpMsg = await MessageReceived();
                IncomingMsg?.Invoke(this, new MsgReceivedEventArgs(udpMsg.RemoteAddress, udpMsg.ByteData, TransportType.UDP));
            }
        }

        private async Task<UdpSocketMessageReceivedEventArgs> MessageReceived()
        {
            //wait until recieved signal
            UdpSocketMessageReceivedEventArgs tempMsgHandler;
            try
            {
                await messageReceive.WaitAsync();
                tempMsgHandler = curUDPMessage;
            }
            finally
            {
                //ready to recieve next message
                messageProccessed.Release();
            }
            return tempMsgHandler;
        }

        //This method runs in a seperate thread.
        //This is undesirable because windows form elements will complain about shared resources not being avaliable
        //solution is to use a semaphore that is picked up in the other thread
        private async void ListenerUDP_MessageReceived(object sender, UdpSocketMessageReceivedEventArgs e)
        {
            //wait until previous message has been handled
            await messageProccessed.WaitAsync();

            //update the shared memory
            curUDPMessage = e;

            //tell main thread of new message (using signal)
            messageReceive.Release();
        }

        private void ListenerTCP_ConnectionReceived(object sender, TcpSocketListenerConnectEventArgs e)
        {
            PeerConnectTCPRequest?.Invoke(this, e);
        }
    }
}
