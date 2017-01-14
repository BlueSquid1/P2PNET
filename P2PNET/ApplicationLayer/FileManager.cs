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

        private ObjectManager objManager;

        //constructor
        FileManager()
        {
            this.objManager = new ObjectManager();
        }

        //bufferSize = 32Kb chunks
        public async Task SendFileToAllPeersTCP(string ipAddress, string filePath, long bufferSize = 32 * 1024)
        {
            //get file details
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
            }

            //send remain
            int remaining = (int)(fileLength - totalWritten);
            await fileStream.ReadAsync(buffer, 0, remaining);
            await objManager.SendAsyncTCP(ipAddress, filePart);
            totalWritten += remaining;
            fileStream.Dispose();
        }
    }
}
