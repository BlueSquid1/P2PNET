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
    class FileManager
    {
        public event EventHandler<FileTransferEventArgs> FileProgUpdate;
        public event EventHandler<ObjReceivedEventArgs> ObjReceived;

        private ObjectManager objManager;
        private IFileSystem fileSystem;
        private List<FileReceived> receivedFiles;

        //constructor
        FileManager()
        {
            this.objManager = new ObjectManager();
            this.fileSystem = FileSystem.Current;
            this.objManager.ObjReceived += ObjManager_objReceived;
        }

        private void ObjManager_objReceived(object sender, ObjReceivedEventArgs e)
        {
            switch (e.Metadata.objectType)
            {
                case "FilePartObj":
                    FilePartObj filePart = e.Obj.GetObject<FilePartObj>();
                    ProcessFilePart(filePart);
                    break;
                default:
                    ObjReceived?.Invoke(this, e);
                    break;
            }
        }

        //bufferSize = 32Kb chunks
        public async Task SendFileToAllPeersTCP(string ipAddress, string filePath, long bufferSize = 32 * 1024)
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

            byte[] buffer = new byte[bufferSize];

            //for each file part
            for (int i = 1; i < totalPartNum; i++)
            {
                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                filePart.AppendFileData(buffer, i);
                await objManager.SendAsyncTCP(ipAddress, filePart);
                totalWritten += bufferSize;
                FileProgUpdate?.Invoke(this, new FileTransferEventArgs(fileLength, totalWritten));
            }

            //send remain
            int remaining = (int)(fileLength - totalWritten);
            await fileStream.ReadAsync(buffer, 0, remaining);
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
