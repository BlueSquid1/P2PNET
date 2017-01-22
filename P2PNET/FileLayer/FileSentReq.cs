using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer
{
    public class FileSentReq : FileTransReq
    {

        //constructor
        public FileSentReq(FilePartObj mFilePart, Stream mFileStream, string targetIp) : base (mFilePart, mFileStream, targetIp)
        {

        }

        public async Task<FilePartObj> GetNextFilePart()
        {
            int bufferLen = (int)Math.Min(fileDataStream.Length - base.BytesProcessed, base.FilePart.MaxBufferSize);
            if(bufferLen <= 0)
            {
                //nothing more to be sent
                return null;
            }

            byte[] buffer = new byte[bufferLen];
            await base.fileDataStream.ReadAsync(buffer, 0, buffer.Length);

            base.BytesProcessed += bufferLen;
            base.curFilePartNum++;
            bool isFilePartReady = base.FilePart.AppendFileData(buffer, curFilePartNum);
            if (!isFilePartReady)
            {
                throw new FileTransitionError("failed to send the file. Make sure the file is in a valid format");
            }
            return FilePart;
        }
    }
}
