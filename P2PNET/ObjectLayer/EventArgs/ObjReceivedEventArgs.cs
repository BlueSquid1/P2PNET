using P2PNET.ObjectLayer;
using P2PNET.TransportLayer;

namespace P2PNET.ObjectLayer.EventArgs
{
    public class ObjReceivedEventArgs : System.EventArgs
    {
        public BObject Obj { get; }
        public Metadata Metadata { get; }

        //constructor
        public ObjReceivedEventArgs(BObject mObj, Metadata metadata)
        {
            this.Obj = mObj;
            this.Metadata = metadata;
        }
    }
}
