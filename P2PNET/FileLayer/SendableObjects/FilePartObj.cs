using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace P2PNET.FileLayer.SendableObjects
{
    //file objects are sent between peers
    //for that reason they must be seralizable
    //contains enough information so that both sender and
    //receiver's are stateless
    public class FilePartObj
    {
        //identification
        public FileMetadata FileMetadata { get; }

        //data
        public byte[] FileData { get; }

        
        public int FilePartNum { get; }
        public int TotalPartNum { get; }


        public FilePartObj(FileMetadata mMetadata, byte[] mFileData, int mFilePartNum, int TotalPartNum)
        {
            this.FileMetadata = mMetadata;
            this.FileData = mFileData;
            this.FilePartNum = mFilePartNum;
            this.TotalPartNum = TotalPartNum;
        }
    }
}
