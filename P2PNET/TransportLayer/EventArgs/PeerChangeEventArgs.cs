using System.Collections.Generic;


namespace P2PNET.TransportLayer.EventArgs
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
