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
        private TaskCompletionSource<bool> stillProcPrevMsg;

        //constructor
        public FileManager()
        {
            this.receivedFiles = new List<FileReceived>();
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
            switch (e.Metadata.objectType)
            {
                case "FilePartObj":
                    FilePartObj filePart = e.Obj.GetObject<FilePartObj>();
                    await ProcessFilePart(filePart);
                    break;
                default:
                    ObjReceived?.Invoke(this, e);
                    break;
            }
        }

        //bufferSize = 32Kb chunks
        public async Task SendFileAsyncTCP(string ipAddress, string filePath, long bufferSize = 32 * 1024)
        {
            //get file details
            IFile file = await fileSystem.GetFileFromPathAsync(filePath);
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
            long fileLength = fileStream.Length;


            int totalPartNum = (int)Math.Ceiling((float)fileLength / bufferSize);
            FilePartObj filePart = new FilePartObj(file.Name, file.Path, fileLength, totalPartNum);

            //for logging purposes
            long totalWritten = 0;

            //for each file part
            for (int i = 1; i < totalPartNum; i++)
            {
                byte[] buffer = new byte[bufferSize];
                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                bool isFilePartReady = filePart.AppendFileData(buffer, i);
                if(!isFilePartReady)
                {
                    throw new FileTransitionError("failed to send the file. Make sure the file is in a valid format");
                }
                await objManager.SendAsyncTCP(ipAddress, filePart);
                totalWritten += bufferSize;
                FileProgUpdate?.Invoke(this, new FileTransferEventArgs(fileLength, totalWritten));
            }

            //send remain
            int remaining = (int)(fileLength - totalWritten);
            byte[] remainBuffer = new byte[remaining];
            await fileStream.ReadAsync(remainBuffer, 0, remaining);
            bool isFinFilePartReady = filePart.AppendFileData(remainBuffer, totalPartNum);
            if (!isFinFilePartReady)
            {
                throw new FileTransitionError("failed to send the file. Make sure the file is in a valid format");
            }
            await objManager.SendAsyncTCP(ipAddress, filePart);
            totalWritten += remaining;
            FileProgUpdate?.Invoke(this, new FileTransferEventArgs(totalWritten, totalWritten));
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
