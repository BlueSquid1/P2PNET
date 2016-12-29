using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.EventArgs
{
    public class PeerChangeEventArgs : System.EventArgs
    {
        public byte[] message { get; }
        public TransportType bindingType { get; }

        public PeerChangeEventArgs(byte[] msg, TransportType bindType)
        {
            this.message = msg;
            this.bindingType = bindType;
        }
    }
}
