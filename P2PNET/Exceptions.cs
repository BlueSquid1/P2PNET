using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET
{
    public class PeerNotKnown : Exception
    {
        public PeerNotKnown(string message) : base(message)
        {
        }
    }
    public class StreamCannotWrite : Exception
    {
        public StreamCannotWrite(string message) : base(message)
        {

        }
    }

    public class NoNetworkInterface : Exception
    {
        public NoNetworkInterface(string message) : base(message)
        {

        }
    }
}
