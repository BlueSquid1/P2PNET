using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.EventArgs
{
    public class PeerConnectReqEventArgs
    {
        public Peer peer { get; }

        //constructor
        public PeerConnectReqEventArgs(Peer mPeer)
        {
            this.peer = mPeer;
        }
    }
}
