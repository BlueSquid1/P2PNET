using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    public class FilePartObj
    {
        public byte[] FileData { get; set; }
        public int FilePartNum { get; set; }
        public int TotalPartNum { get; set; }

        //the file size of the file being transmitted
        public long TotalFileSizeBytes { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public FilePartObj()
        {

        }

        //constructor
        public FilePartObj(string fileName, string filePath, long fileSizeBytes, int totalPartNum)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
            this.TotalFileSizeBytes = fileSizeBytes;
            this.TotalPartNum = totalPartNum;
        }

        public bool AppendFileData(byte[] fileData, int filePart)
        {
            if(filePart > this.TotalPartNum)
            {
                return false;
            }
            if( fileData.Length > TotalFileSizeBytes )
            {
                return false;
            }

            this.FileData = fileData;
            this.FilePartNum = filePart;
            return true;
        }
    }
}
