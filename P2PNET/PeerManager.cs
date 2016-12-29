using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P2PNET
{
    public class PeerManager
    {
        public event EventHandler<PeerUpdateEventArgs> PeerChange;
        public event EventHandler<KeyStrokeEventArgs> msgReceived;

        public int PortNum { get; }

        public void SendMsgAsyncTCP(string ipAddress, byte[] msg)
        {

        }

        public void SendMsgAsyncUDP(string ipAddress, byte[] msg)
        {

        }

        public void SendBroadcast(byte[] msg)
        {

        }


    }
}
