using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer.EventArgs
{
    public class FileTransferEventArgs : System.EventArgs
    {
        public float Percent { get; }
        public long FileLength { get; }
        public long BytesProcessed { get; }

        //constructor
        public FileTransferEventArgs( long fileLength, long bytesProcessed)
        {
            this.FileLength = fileLength;
            this.BytesProcessed = bytesProcessed;
            this.Percent = bytesProcessed / fileLength;
        }
    }
}
