using System;
using System.Threading.Tasks;
using P2PNET.TransportLayer;
using P2PNET.TransportLayer.EventArgs;
using P2PNET.ObjectLayer.EventArgs;
using System.IO;
using PCLStorage;

namespace P2PNET.ObjectLayer
{
    public class ObjectManager
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<ObjReceivedEventArgs> ObjReceived;

        private Serializer serializer;
        private MessageManager peerManager;

        //constructor
        public ObjectManager(int portNum = 8080, bool mForwardAll = false)
        {
            peerManager = new MessageManager(portNum, mForwardAll);
            serializer = new Serializer();

            peerManager.MsgReceived += PeerManager_msgReceived;
            peerManager.PeerChange += PeerManager_PeerChange;
        }

        public async Task StartAsync()
        {
            await peerManager.StartAsync();
        }
        
        public async Task SendBroadcastAsyncUDP<T>(T obj)
        {
            byte[] msg = await PackObjectIntoMsg(obj);

            await peerManager.SendBroadcastAsyncUDP(msg);
        }

        public async Task<bool> SendAsyncTCP<T>(string ipAddress, T obj)
        {
            byte[] msg = await PackObjectIntoMsg(obj);

            return await peerManager.SendAsyncTCP(ipAddress, msg);
        }

        public async Task<bool> SendAsyncUDP<T>(string ipAddress, T obj)
        {
            byte[] msg = await PackObjectIntoMsg(obj);

            return await peerManager.SendAsyncUDP(ipAddress, msg);
        }

        public async Task SendToAllPeersAsyncUDP<T>(T obj)
        {
            byte[] msg = await PackObjectIntoMsg(obj);

            await peerManager.SendToAllPeersAsyncUDP(msg);
        }

        public async Task SendToAllPeersAsyncTCP<T>(T obj)
        {
            byte[] msg = await PackObjectIntoMsg(obj);

            await peerManager.SendToAllPeersAsyncTCP(msg);
        }

        //TODO: make this private
        public async Task<byte[]> PackObjectIntoMsg<T>(T obj)
        {
            //generate metadata
            Metadata metadata = await CreateMetadataObj(obj);

            //place object into a package
            ObjPackage<T> objPackage = new ObjPackage<T>( obj, metadata);

            //seralize package
            byte[] objMsg = serializer.SerializeObject(objPackage);
            ObjPackage<T> temp = serializer.DeserializeObject<ObjPackage<T>>(objMsg);

            return objMsg;
        }

        private async Task<Metadata> CreateMetadataObj<T>(T obj)
        {
            string sourceIp = await peerManager.GetIpAddress();
            string objType = obj.GetType().Name;
            Metadata metaData = new Metadata();
            metaData.SourceIp = sourceIp;
            metaData.ObjectType = objType;
            return metaData;
        }

        private void PeerManager_PeerChange(object sender, TransportLayer.EventArgs.PeerChangeEventArgs e)
        {
            PeerChange?.Invoke(this, e);
        }

        private void PeerManager_msgReceived(object sender, TransportLayer.EventArgs.MsgReceivedEventArgs e)
        {
            byte[] msg = e.Message;
            BObject obj = ProcessReceivedMsg(msg);
            ObjReceived?.Invoke(this, new ObjReceivedEventArgs(obj));
        }

        //TODO: make private
        public BObject ProcessReceivedMsg(byte[] msg )
        {
            BObject bObject = new BObject(msg, serializer);
            return bObject;
        }
    }
}
