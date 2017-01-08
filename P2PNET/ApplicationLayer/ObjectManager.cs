using System;
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
        private MessageManager peerManager;

        //constructor
        public ObjectManager(int portNum = 8080)
        {
            peerManager = new MessageManager(portNum, true);
            serializer = new Serializer();

            peerManager.msgReceived += PeerManager_msgReceived;
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

        private async Task<byte[]> PackObjectIntoMsg<T>(T obj)
        {
            //generate metadata
            Metadata metadata = await CreateMetadataObj(obj);

            //seralize object
            byte[] objMsg = serializer.SerializeObjectBSON(obj);

            //seralize metadata
            metadata.TotalMsgSizeBytes = objMsg.Length;
            byte[] metadataMsg = serializer.SerializeObjectBSON(metadata);

            //seralize size of metadata section by boxing the length
            byte[] metadataSizeMsg = serializer.WriteInt32(metadataMsg.Length);

            //msg = metadataSizeMsg + metadataMsg + objMsg
            byte[] msg = new byte[metadataSizeMsg.Length + metadataMsg.Length + objMsg.Length];

            Array.Copy(metadataSizeMsg, msg, metadataSizeMsg.Length);
            Array.Copy(metadataMsg, 0, msg, metadataSizeMsg.Length, metadataMsg.Length);
            Array.Copy(objMsg, 0, msg, metadataSizeMsg.Length + metadataMsg.Length, objMsg.Length);
            return msg;
        }

        private async Task<Metadata> CreateMetadataObj<T>(T obj, bool forceTwoWayHandShake = false)
        {
            string sourceIp = await peerManager.GetIpAddress();
            MessageType msgType = MessageType.Object;
            bool isTwoWay = false;

            string objType = obj.GetType().Name;

            if (objType == "File")
            {
                msgType = MessageType.File;
                isTwoWay = true;
            }

            if(forceTwoWayHandShake)
            {
                isTwoWay = true;
            }

            Metadata metaData = new Metadata();
            metaData.MsgType = msgType;
            metaData.SourceIp = sourceIp;
            metaData.IsTwoWay = isTwoWay;
            metaData.ObjType = objType;

            return metaData;
        }

        private void PeerManager_PeerChange(object sender, TransportLayer.EventArgs.PeerChangeEventArgs e)
        {
            PeerChange?.Invoke(this, e);
        }

        private void PeerManager_msgReceived(object sender, TransportLayer.EventArgs.MsgReceivedEventArgs e)
        {
            byte[] msg = e.Message;

            //get metadata size
            byte[] metadataSizeMsg = new byte[sizeof(int)];
            Array.Copy(msg, metadataSizeMsg, sizeof(int));
            int metadataSize = serializer.ReadInt32(metadataSizeMsg);

            //get metadata
            byte[] metadataMsg = new byte[metadataSize];
            Array.Copy(msg, sizeof(int), metadataMsg, 0, metadataSize);
            Metadata metadata = serializer.DeserializeObjectBSON<Metadata>(metadataMsg);

            //get message
            byte[] objectMsg = new byte[metadata.TotalMsgSizeBytes];
            if (metadata.IsTwoWay == false)
            {
                //message attached to this request
                Array.Copy(msg, sizeof(int) + metadataSize, objectMsg, 0, metadata.TotalMsgSizeBytes);
                BObject bObject = new BObject(objectMsg, serializer);
                objReceived?.Invoke(this, new ObjReceivedEventArgs(bObject, metadata));
            }
            else
            {
                //message sent seperately
                //TODO
            }
        }
    }
}
