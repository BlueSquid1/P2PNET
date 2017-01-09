using System;
using System.Threading.Tasks;
using P2PNET.TransportLayer;
using P2PNET.TransportLayer.EventArgs;
using P2PNET.ApplicationLayer.EventArgs;
using System.IO;
using PCLStorage;

namespace P2PNET.ApplicationLayer
{
    public class ObjectManager
    {
        public event EventHandler<PeerChangeEventArgs> PeerChange;
        public event EventHandler<ObjReceivedEventArgs> objReceived;
        public event EventHandler<FileTransferEventArgs> fileTransferProgress;

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

        public async Task SendFileToAllPeersTCP(string ipAddress, string filePath)
        {
            //get file content
            IFile file = await FileSystem.Current.GetFileFromPathAsync(filePath);
            Stream fileStream;
            try
            {
                fileStream = await file.OpenAsync(FileAccess.Read);
            }
            catch
            {
                //can't find file
                throw new FileNotFound("Can't access the file: " + filePath);
            }

            //get file details
            long fileLength = fileStream.Length;

            //send metadata to peers
            Metadata metadata = await CreateMetadataFile(file.Name);


            //send file chunks at a time
            int chuckSize = 32 * 1024; // 32k chunks
            long totalWritten = 0;
            byte[] buffer = new byte[chuckSize];
            
            //iterate through all chuncks but the last
            while(totalWritten < fileLength - chuckSize)
            {
                //fill up the buffer
                await fileStream.ReadAsync(buffer, 0, buffer.Length);

                //send buffer
                await peerManager.SendAsyncTCP(ipAddress, buffer);

                totalWritten += chuckSize;
                fileTransferProgress?.Invoke(this, new FileTransferEventArgs(totalWritten / fileLength));
            }

            //send remain
            int remaining = (int)(fileLength - totalWritten);
            await fileStream.ReadAsync(buffer, 0, remaining);
            await peerManager.SendAsyncTCP(ipAddress, buffer);
            fileStream.Dispose();
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
            metaData.Name = objType;

            return metaData;
        }

        private async Task<Metadata> CreateMetadataFile(string fileName)
        {
            string sourceIp = await peerManager.GetIpAddress();
            MessageType msgType = MessageType.Object;
            bool isTwoWay = true;

            Metadata metaData = new Metadata();
            metaData.MsgType = msgType;
            metaData.SourceIp = sourceIp;
            metaData.IsTwoWay = isTwoWay;
            metaData.Name = fileName;

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
