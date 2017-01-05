using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2PNET.TransportLayer;
using P2PNET.TransportLayer.EventArgs;
using P2PNET.ApplicationLayer.EventArgs;

namespace P2PNET.ApplicationLayer
{
    public class ObjectManager
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<ObjReceivedEventArgs> objReceived;

        private Serializer serializer;
        private PeerManager peerManager;

        //constructor
        public ObjectManager(int portNum = 8080)
        {
            peerManager = new PeerManager(portNum);
            serializer = new Serializer();

            peerManager.msgReceived += PeerManager_msgReceived;
            peerManager.PeerChange += PeerManager_PeerChange;
        }

        public async void SendObjBroadcastUDP<T>(T obj)
        {
            byte[] objBin = serializer.SerializeObjectBSON(obj);
            await peerManager.SendBroadcastAsyncUDP(objBin);
        }

        private void PeerManager_PeerChange(object sender, TransportLayer.EventArgs.PeerChangeEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PeerManager_msgReceived(object sender, TransportLayer.EventArgs.MsgReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
