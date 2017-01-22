using P2PNET.FileLayer.SendableObjects;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer
{
    public class FileReceiveReq
    {
        //for identification FileSentReq
        public string SenderIpAddress { get; }

        private List<FileTransReq> fileTransReqs;


        //constructor
        public FileReceiveReq(List<FileTransReq> mFileTransReqs, string mIpAddress)
        {
            this.fileTransReqs = mFileTransReqs;
            this.SenderIpAddress = mIpAddress;
        }

        public async Task WriteFilePartToFile(FilePartObj receivedFilePart)
        {
            byte[] buffer = receivedFilePart.FileData;
            await base.fileDataStream.WriteAsync(buffer, 0, buffer.Length);

            base.BytesProcessed += receivedFilePart.FileData.Length;
            base.curFilePartNum++;
        }

        public async Task CloseStream()
        {
            await base.fileDataStream.FlushAsync();
            base.fileDataStream.Dispose();
        }
    }
}
