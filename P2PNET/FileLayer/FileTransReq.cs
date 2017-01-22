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
    public class FileTransReq
    {
        //file details
        public FileMetadata FileDetails { get; }

        public int curFilePartNum
        {
            get
            {
                return (int)Math.Ceiling((float)BytesProcessed / bufferSize);
            }
        }
        public int TotalPartNum
        {
            get
            {
                return (int)Math.Ceiling((float)FileDetails.FileSize / bufferSize);
            }
        }

        //transmittion details
        public long BytesProcessed
        {
            get
            {
                return bytesProccessed;
            }
        }
        private long bytesProccessed;

        //buffer size is only for file parts calculations 
        private int bufferSize;

        private Stream fileDataStream;

        //constructor
        public FileTransReq(IFile mFileDetails, Stream mFileStream, int mBufferSize)
        {
            this.FileDetails = new FileMetadata(mFileDetails.Name, mFileDetails.Path, mFileStream.Length);
            this.bytesProccessed = 0;
            this.fileDataStream = mFileStream;
        }

        public async Task<byte[]> ReadBytes(int numOfBytes)
        {
            byte[] fileData = new byte[numOfBytes];

            await this.fileDataStream.ReadAsync(fileData, 0, numOfBytes);

            this.bytesProccessed += numOfBytes;

            return fileData;
        }
    }
}
