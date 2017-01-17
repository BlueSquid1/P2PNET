using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer
{
    public class FileTrans
    {
        public long BytesProcessed { get; set; }
        protected Stream fileDataStream;
        protected int curFilePartNum;

        //for identification
        public FilePartObj FilePart { get; set; }
        public string TargetIpAddress { get; set; }

        //constructor
        public FileTrans(FilePartObj mFilePart, Stream mFileStream, string targetIp)
        {
            this.TargetIpAddress = targetIp;
            this.FilePart = mFilePart;
            this.fileDataStream = mFileStream;
            this.BytesProcessed = 0;
            this.curFilePartNum = 0;
        }

        public int RemainingFileParts()
        {
            int partsRemainding = this.FilePart.TotalPartNum - curFilePartNum;
            if (partsRemainding < 0)
            {
                throw new FileBoundaryException("there are a negative number of file parts remaining.");
            }
            return partsRemainding;
        }
    }
}
