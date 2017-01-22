using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer.SendableObjects
{
    public class FileMetadata
    {
        public string FileName { get; }
        public string FilePath { get; }
        public long FileSize { get; }

        //constructor
        public FileMetadata(string mFileName, string mFilePath, long mFileSize)
        {
            this.FileName = mFileName;
            this.FilePath = mFilePath;
            this.FileSize = mFileSize;
        }
    }
}
