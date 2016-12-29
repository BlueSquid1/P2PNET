using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.EventArgs
{
    public class PeerChangeEventArgs
    {
        public List<Peer> Peers { get; }

        //constructor
        public PeerChangeEventArgs( List<Peer> peers )
        {
            this.Peers = peers;
        }
    }
}
