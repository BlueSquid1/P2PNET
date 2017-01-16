using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    public class FileReceived : FileTrans
    {
        //constructor
        public FileReceived(FilePartObj filePart, Stream mFileStream, string senderIp) : base(filePart, mFileStream, senderIp)
        {

        }

        public async Task WriteFilePartToFile(FilePartObj receivedFilePart)
        {
            byte[] buffer = receivedFilePart.FileData;
            await base.fileDataStream.WriteAsync(buffer, 0, buffer.Length);

        }

        public async Task CloseStream()
        {
            await base.fileDataStream.FlushAsync();
        }
    }
}
