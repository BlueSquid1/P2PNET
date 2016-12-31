using System;
using System.Threading.Tasks;
using System.IO;
using Sockets.Plugin.Abstractions;
using P2PNET.EventArgs;
using System.Text;

namespace P2PNET
{
    public class Peer
    {
        public event EventHandler<MsgReceivedEventArgs> MsgReceived;
        public event EventHandler peerStatusChange;

        public string IpAddress
        {
            get
            {
                return socketClient.RemoteAddress;
            }
        }

        public bool IsPeerActive { get; set; }

        private ITcpSocketClient socketClient;

        //constructor
        public Peer(ITcpSocketClient mSocketClient)
        {
            this.IsPeerActive = true;
            this.socketClient = mSocketClient;

            StartListening();
        }

        //deconstructor
        ~Peer()
        {
            this.socketClient.DisconnectAsync().Wait();
        }

        public async Task<bool> SendMsgTCPAsync(byte[] msg)
        {
            if(this.IsPeerActive == false)
            {
                //peer has disconnected
                return false;
            }
            //send number indicating message size
            int lenMsg = (int)msg.Length;
            byte[] lenBin = IntToBinary(lenMsg);
            await socketClient.WriteStream.WriteAsync(lenBin, 0, lenBin.Length);

            //send the msg
            await socketClient.WriteStream.WriteAsync(msg, 0, lenMsg);
            await socketClient.WriteStream.FlushAsync();
            return true;
        }

        private async void StartListening()
        {
            while (true)
            {
                const int intSize = sizeof(int);
                Byte[] lengthBin = new Byte[intSize];
                //socketClient.
                try
                {
                    await socketClient.ReadStream.ReadAsync(lengthBin, 0, intSize);
                }
                catch(System.IO.IOException e1)
                {
                    //peer has disconnected
                    this.IsPeerActive = false;
                    peerStatusChange?.Invoke(this, new System.EventArgs());
                    return;
                }
                int msgSize = BinaryToInt(lengthBin);

                byte[] messageBin = new byte[msgSize];
                await socketClient.ReadStream.ReadAsync(messageBin, 0, msgSize);

                MsgReceived?.Invoke(this, new MsgReceivedEventArgs(socketClient.RemoteAddress, messageBin, TransportType.TCP));
            }
        }

        private byte[] IntToBinary(int value)
        {
            byte[] valueBin = BitConverter.GetBytes(value);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(valueBin);
            }
            return valueBin;
        }

        private int BinaryToInt(byte[] binArray)
        {
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(binArray);
            }
            return BitConverter.ToInt32(binArray, 0);
        }
        
    }
}