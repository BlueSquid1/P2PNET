using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    public class FileSent
    {
        private FilePartObj filePart;
        private Stream fileDataStream;
        private int curFilePartNum;
        private int totalPartNum;
        private int bufferSize;

        //for identification
        public IFile FileInfo { get; set; }
        public string TargetIpAddress { get; set; }

        //constructor
        public FileSent(IFile file, Stream mFileStream, int mBufferSize, string targtIp)
        {
            this.fileDataStream = mFileStream;
            this.FileInfo = file;
            this.bufferSize = mBufferSize;
            this.TargetIpAddress = targtIp;

            long fileLength = mFileStream.Length;
            this.totalPartNum = (int)Math.Ceiling((float)fileLength / mBufferSize);
            filePart = new FilePartObj(file.Name, file.Path, fileLength, totalPartNum);
            curFilePartNum = 0;
        }

        public int RemainingFileParts()
        {
            int partsRemainding = totalPartNum - curFilePartNum;
            if(partsRemainding < 0 )
            {
                throw new FileBoundaryException("there are a negative number of file parts remaining.");
            }
            return partsRemainding;
        }

        public async Task<FilePartObj> GetNextFilePart()
        {
            int bufferLen = (int)Math.Min(fileDataStream.Length, bufferSize);
            if(bufferLen <= 0)
            {
                //nothing more to be sent
                return null;
            }

            byte[] buffer = new byte[bufferLen];
            await fileDataStream.ReadAsync(buffer, 0, buffer.Length);

            curFilePartNum++;
            bool isFilePartReady = filePart.AppendFileData(buffer, curFilePartNum);
            if (!isFilePartReady)
            {
                throw new FileTransitionError("failed to send the file. Make sure the file is in a valid format");
            }
            return filePart;
        }
    }
}
