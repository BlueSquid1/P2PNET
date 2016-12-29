using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Sockets.Plugin.Abstractions;
using P2PNET.EventArgs;

namespace P2PNET
{
    public class Peer
    {
        public event EventHandler<MsgReceivedEventArgs> MsgReceived;

        public ITcpSocketClient SocketClient { get; set; }

        //constructor
        public Peer(ITcpSocketClient socketClient)
        {
            this.SocketClient = socketClient;

            StartListening();
        }

        public async Task SendMsgTCPAsync(byte[] msg)
        {
            Stream outputStream = SocketClient.WriteStream;
            if(outputStream.CanWrite)
            {
                throw (new StreamCannotWrite("Cannot send message to peer because stream is not writable"));
            }
            await outputStream.FlushAsync();
        }

        private async void StartListening()
        {
            string peerIp = SocketClient.RemoteAddress;
            Stream inputStream = SocketClient.ReadStream;
            while (true)
            {
                Byte[] buffer = new Byte[5];
                await inputStream.ReadAsync(buffer, 0, 1);
                MsgReceived?.Invoke(this, new MsgReceivedEventArgs(peerIp, buffer, TransportType.TCP));
            }
            
        }
        
    }
}
