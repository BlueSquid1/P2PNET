using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace P2PNET.FileLayer
{
    //file objects are sent between peers
    //for that reason they must be seralizable
    //contains enough information so that both sender and
    //receiver's are stateless
    public class FilePartObj
    {
        public byte[] FileData { get; set; }
        public int FilePartNum { get; set; }
        public int TotalPartNum { get; set; }

        //the file size of the file being transmitted
        public long TotalFileSizeBytes { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int MaxBufferSize { get; set; }

        //JSON.NET needs a blank constructor
        public FilePartObj()
        {

        }

        //constructor
        public FilePartObj(IFile file, long totalFileSize , int maxBufferSize)
        {
            this.FileName = file.Name;
            this.FilePath = file.Path;
            this.MaxBufferSize = maxBufferSize;
            this.TotalFileSizeBytes = totalFileSize;
            this.TotalPartNum = (int)Math.Ceiling((float)totalFileSize / maxBufferSize);
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
