using System;
using System.Threading.Tasks;
using System.IO;
using Sockets.Plugin.Abstractions;
using P2PNET.EventArgs;

namespace P2PNET
{
    public class Peer
    {
        public event EventHandler<MsgReceivedEventArgs> MsgReceived;

        public string IpAddress
        {
            get
            {
                return socketClient.RemoteAddress;
            }
        }

        private ITcpSocketClient socketClient;

        //constructor
        public Peer(ITcpSocketClient socketClient)
        {
            this.socketClient = socketClient;

            StartListening();
        }

        //deconstructor
        ~Peer()
        {
            this.socketClient.DisconnectAsync().Wait();
        }

        public async Task SendMsgTCPAsync(byte[] msg)
        {
            Stream outputStream = socketClient.WriteStream;

            if(!outputStream.CanWrite)
            {
                throw (new StreamCannotWrite("Cannot send message to peer because stream is not writable"));
            }

            outputStream.Write(msg, 0, msg.Length);
            await outputStream.FlushAsync();
        }

        private async void StartListening()
        {
            string peerIp = socketClient.RemoteAddress;
            Stream inputStream = socketClient.ReadStream;
            while (true)
            {
                Byte[] buffer = new Byte[5];
                await inputStream.ReadAsync(buffer, 0, 1);
                MsgReceived?.Invoke(this, new MsgReceivedEventArgs(peerIp, buffer, TransportType.TCP));
            }
            
        }
        
    }
}
