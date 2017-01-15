using P2PNET.ApplicationLayer;
using P2PNET.ApplicationLayer.EventArgs;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    public class FileManager
    {
        public event EventHandler<FileTransferEventArgs> FileProgUpdate;
        public event EventHandler<ObjReceivedEventArgs> ObjReceived;

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
            Metadata metadata = e.Metadata;
            switch (metadata.objectType)
            {
                case "FilePartObj":
                    FilePartObj filePart = e.Obj.GetObject<FilePartObj>();
                    await ProcessFilePart(filePart);
                    await SendAckBack(filePart, metadata);
                    break;
                case "AckMessage":
                    AckMessage ackMsg = e.Obj.GetObject<AckMessage>();
                    await ProcessAckMessage(ackMsg, metadata);
                    break;
                default:
                    ObjReceived?.Invoke(this, e);
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

            FileSent fileSend = new FileSent(file, fileStream, bufferSize, ipAddress);
            sentFiles.Add(fileSend);

            //send first file part
            await SendNextFilePart(fileSend);
        }

        private async Task SendNextFilePart(FileSent fileSend)
        {
            int remainingParts = fileSend.RemainingFileParts();
            if (remainingParts <= 0 )
            {
                //no parts left to send
                return;
            }
            FilePartObj filePart = await fileSend.GetNextFilePart();
            string ipAddress = fileSend.TargetIpAddress;
            await objManager.SendAsyncTCP(ipAddress, filePart);

            //TODO: update logging information
        }

        //called when a new file part is received
        private async Task ProcessFilePart(FilePartObj filePart)
        {
            
            if( filePart.FilePartNum == 1)
            {
                //new file being received
                FileReceived newFileReceived = await NewFileInit(filePart);
                receivedFiles.Add(newFileReceived);
            }

            //find correct file to write to
            FileReceived file = GetCorrectFile(filePart.FileName);
            if(file == null)
            {
                //can't find file
                throw new FileNotFound("received message mentions a file not known to this peer.");
            }
            Stream fileStream = file.FileStream;

            byte[] buffer = filePart.FileData;
            await fileStream.WriteAsync(buffer, 0, buffer.Length);

            //if last file part then close stream
            if(filePart.FilePartNum == filePart.TotalPartNum)
            {
                await file.CloseStream();
            }
        }

        private async Task ProcessAckMessage(AckMessage ackMsg, Metadata metadata)
        {
            FileSent fileSent = GetFileSentFromIpAndAck(ackMsg, metadata.SourceIp);
            await SendNextFilePart(fileSent);
        }

        //find a match based on remote ip, file name and file path
        private FileSent GetFileSentFromIpAndAck(AckMessage ackMsg, string remoteIp)
        {
            //find corresponding sentFiles
            foreach (FileSent fileSent in sentFiles)
            {
                if(fileSent.TargetIpAddress == remoteIp && fileSent.FileInfo.Name == ackMsg.FileName && fileSent.FileInfo.Path == ackMsg.FilePath)
                {
                    return fileSent;
                }   
            }
            //can't find coresponding file
            throw new FileNotFound("Received an Ack message for an unknown file");
            return null;
        }

        private async Task SendAckBack(FilePartObj filePart, Metadata metadata)
        {
            //send message back to sender
            string targetIp = metadata.SourceIp;
            AckMessage ackMsg = new AckMessage(filePart);
            await objManager.SendAsyncTCP(targetIp, ackMsg);
        }

        
        private async Task<FileReceived> NewFileInit(FilePartObj filePart)
        {
            IFolder root = await fileSystem.GetFolderFromPathAsync("./");
            if (await root.CheckExistsAsync("./temp/") == ExistenceCheckResult.NotFound)
            {
                //create folder
                await root.CreateFolderAsync("temp", CreationCollisionOption.FailIfExists);
            }
            IFolder tempFolder = await fileSystem.GetFolderFromPathAsync("./temp");
            IFile newFile = await tempFolder.CreateFileAsync(filePart.FileName, CreationCollisionOption.ReplaceExisting);
            FileReceived fileReceived = new FileReceived(newFile);
            await fileReceived.OpenStream();
            return fileReceived;
        }

        private FileReceived GetCorrectFile(string fileName)
        {
            foreach (FileReceived receivedFile in this.receivedFiles)
            {
                if (receivedFile.FileObj.Name == fileName)
                {
                    return receivedFile;
                }
            }
            return null;
        }
    }
}
