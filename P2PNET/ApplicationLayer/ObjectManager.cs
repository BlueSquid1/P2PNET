using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2PNET.TransportLayer;
using P2PNET.TransportLayer.EventArgs;
using P2PNET.ApplicationLayer.EventArgs;
using P2PNET.ApplicationLayer.MsgMetadata;
using System.IO;

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
            peerManager = new PeerManager(portNum, true);
            serializer = new Serializer();

            peerManager.msgReceived += PeerManager_msgReceived;
            peerManager.PeerChange += PeerManager_PeerChange;
        }

        public async Task StartAsync()
        {
            await peerManager.StartAsync();
        }
        
        public async void SendObjBroadcastUDP<T>(T obj)
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
            
            //send msg
            await peerManager.SendBroadcastAsyncUDP(msg);
        }

        private async Task<Metadata> CreateMetadataObj<T>(T obj, bool forceTwoWayHandShake = false)
        {
            string sourceIp = await peerManager.GetIpAddress();
            //TODO: check for if type is a file
            MessageType msgType = MessageType.Object;
            bool isTwoWay = false;
            if (obj.GetType().Name == "File")
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
            
            return metaData;
        }

        private void PeerManager_PeerChange(object sender, TransportLayer.EventArgs.PeerChangeEventArgs e)
        {
            PeerChange?.Invoke(this, e);
        }

        private void PeerManager_msgReceived(object sender, TransportLayer.EventArgs.MsgReceivedEventArgs e)
        {
            //e.Message
        }
    }
}
