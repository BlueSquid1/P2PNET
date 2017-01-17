using P2PNET.FileLayer.EventArgs;
using P2PNET.ObjectLayer;
using P2PNET.ObjectLayer.EventArgs;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer
{
    public class FileManager
    {
        public event EventHandler<FileTransferEventArgs> FileProgUpdate;
        public event EventHandler<ObjReceivedEventArgs> ObjReceived;
        public event EventHandler<DebugInfoEventArgs> DebugInfo;

        private ObjectManager objManager;
        private IFileSystem fileSystem;
        private List<FileReceived> receivedFiles;
        private List<FileSent> sentFiles;
        private TaskCompletionSource<bool> stillProcPrevMsg;

        //constructor
        public FileManager()
        {
            this.receivedFiles = new List<FileReceived>();
            this.sentFiles = new List<FileSent>();
            this.stillProcPrevMsg = new TaskCompletionSource<bool>();
            this.objManager = new ObjectManager();
            this.fileSystem = FileSystem.Current;
            this.objManager.ObjReceived += ObjManager_objReceived;
        }

        public async Task StartAsync()
        {
            await objManager.StartAsync();
        }

        private async void ObjManager_objReceived(object sender, ObjReceivedEventArgs e)
        {
            ObjReceived?.Invoke(this, e);
            Metadata metadata = e.Metadata;
            switch (metadata.objectType)
            {
                case "FilePartObj":
                    FilePartObj filePart = e.Obj.GetObject<FilePartObj>();
                    await ReceivedFilePart(filePart, metadata);
                    await SendAckBack(filePart, metadata);
                    break;
                case "AckMessage":
                    AckMessage ackMsg = e.Obj.GetObject<AckMessage>();
                    await ProcessAckMessage(ackMsg, metadata);
                    break;
                default:
                    break;
            }
        }

        //bufferSize = 32Kb chunks
        public async Task SendFileAsyncTCP(string ipAddress, string filePath, int bufferSize = 32 * 1024)
        {
            //get file details
            IFile file = await fileSystem.GetFileFromPathAsync(filePath);

            //TODO: check if file is already open
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
            //store away file details and the stream
            FilePartObj filePart = new FilePartObj(file, fileStream.Length, bufferSize);
            FileSent fileSend = new FileSent(filePart, fileStream, ipAddress);
            sentFiles.Add(fileSend);

            //send first file part
            await SendNextFilePart(fileSend);
        }

        private void Print(string msg)
        {
            DebugInfo?.Invoke(this, new DebugInfoEventArgs(msg));
        }

        private async Task SendNextFilePart(FileSent fileSend)
        {
            int remainingParts = fileSend.RemainingFileParts();
            if (remainingParts <= 0 )
            {
                //no parts left to send
                return;
            }
            //send only the file part
            FilePartObj filePart = await fileSend.GetNextFilePart();
            string ipAddress = fileSend.TargetIpAddress;
            await objManager.SendAsyncTCP(ipAddress, filePart);

            //update logging information
            Print("Sending file part: " + filePart.FilePartNum);
            FileProgUpdate?.Invoke(this, new FileTransferEventArgs(fileSend));
        }

        //called when a file part is received
        private async Task ReceivedFilePart(FilePartObj filePart, Metadata metadata)
        {
            Print("recieved file part: " + filePart.FilePartNum);
            //check if file part is valid
            if (filePart == null)
            {
                throw new Exception("filePart has not been set.");
            }

            //check if is for a new file
            if( filePart.FilePartNum == 1)
            {
                //new file being received
                FileReceived newFileReceived = await NewFileInit(filePart, metadata);
                receivedFiles.Add(newFileReceived);
            }

            //find correct file to write to
            FileReceived fileReceived = GetFileReceivedFromFilePart(filePart, metadata);

            await fileReceived.WriteFilePartToFile(filePart);

            //log incoming file
            FileProgUpdate?.Invoke(this, new FileTransferEventArgs(fileReceived));

            //if last file part then close stream
            if (filePart.FilePartNum == filePart.TotalPartNum)
            {
                await fileReceived.CloseStream();
            }
        }

        private async Task ProcessAckMessage(AckMessage ackMsg, Metadata metadata)
        {
            Print("recieved Ack: " + ackMsg.FilePartNum);
            FileSent fileSent = GetSendFileFromAck(ackMsg, metadata);
            await SendNextFilePart(fileSent);
        }

        //find a match based on remote ip, file name and file path
        private FileSent GetSendFileFromAck(AckMessage ackMsg, Metadata metadata)
        {
            //find corresponding sentFiles
            foreach (FileSent fileSent in sentFiles)
            {
                if (fileSent.TargetIpAddress == metadata.SourceIp && fileSent.FilePart.FileName == ackMsg.FileName && fileSent.FilePart.FilePath == ackMsg.FilePath)
                {
                    return fileSent;
                }   
            }
            //can't find coresponding file
            throw new FileNotFound("Recieved an Ack but can't find file in sent storage.");
            return null;
        }

        private async Task SendAckBack(FilePartObj filePart, Metadata metadata)
        {
            //send message back to sender
            string targetIp = metadata.SourceIp;
            AckMessage ackMsg = new AckMessage(filePart);
            Print("sending ACK: " + ackMsg.FilePartNum);
            await objManager.SendAsyncTCP(targetIp, ackMsg);
        }

        
        private async Task<FileReceived> NewFileInit(FilePartObj filePart, Metadata metadata)
        {
            //create a folder to store the file
            IFolder root = await fileSystem.GetFolderFromPathAsync("./");
            if (await root.CheckExistsAsync("./temp/") == ExistenceCheckResult.NotFound)
            {
                //create folder
                await root.CreateFolderAsync("temp", CreationCollisionOption.FailIfExists);
            }
            IFolder tempFolder = await fileSystem.GetFolderFromPathAsync("./temp");

            //create the file
            IFile newFile = await tempFolder.CreateFileAsync(filePart.FileName, CreationCollisionOption.ReplaceExisting);
            Stream fileStream = await newFile.OpenAsync(FileAccess.ReadAndWrite);

            //store as a received file
            FileReceived fileReceived = new FileReceived(filePart, fileStream, metadata.SourceIp);
            return fileReceived;
        }

        private FileReceived GetFileReceivedFromFilePart(FilePartObj filePart, Metadata metadata)
        {
            foreach (FileReceived receivedFile in this.receivedFiles)
            {
                if (receivedFile.TargetIpAddress == metadata.SourceIp && receivedFile.FilePart.FileName == filePart.FileName && receivedFile.FilePart.FilePath == filePart.FilePath)
                {
                    return receivedFile;
                }
            }

            //can't find coresponding file
            throw new FileNotFound("Recieved an file part but can't find file in received storage.");
            return null;
        }
    }
}
