using System;
using System.Threading.Tasks;
using System.IO;
using Sockets.Plugin.Abstractions;
using P2PNET.TransportLayer.EventArgs;
using System.Text;
using System.Linq;

namespace P2PNET.TransportLayer
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
            //set timeout for reading
            while (true)
            {
                //read the first 4 bytes = sizeof(int)
                const int intSize = sizeof(int);
                Byte[] lengthBin = await ReadBytesAsync(intSize);
                int msgSize = BinaryToInt(lengthBin);

                //read message
                byte[] messageBin = await ReadBytesAsync(msgSize);

                MsgReceived?.Invoke(this, new MsgReceivedEventArgs(socketClient.RemoteAddress, messageBin, TransportType.TCP));
            }
        }

        private async Task<byte[]> ReadBytesAsync(int bytesToRead)
        {
            Byte[] msgBin = new Byte[bytesToRead];
            int totalBytesRd = 0;
            while (totalBytesRd < bytesToRead)
            {
                //ReadAsync() can return less then intSize therefore keep on looping until intSize is reached
                byte[] tempMsgBin = new Byte[bytesToRead];
                int bytesRead = await socketClient.ReadStream.ReadAsync(tempMsgBin, 0, bytesToRead - totalBytesRd);
                Array.Copy(tempMsgBin, 0, msgBin, totalBytesRd, bytesRead);
                totalBytesRd += bytesRead;
            }
            return msgBin;
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