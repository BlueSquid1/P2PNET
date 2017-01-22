using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer.SendableObjects
{
    public class FileAckMsg
    {
        //set true to let sender know to keep sending the
        //file parts
        public bool AcceptedFile { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public FileAckMsg()
        {

        }

        public FileAckMsg(FilePartObj filePart, bool acceptFutureParts = true)
        {
            this.AcceptedFile = acceptFutureParts;
            this.FileName = filePart.FileName;
            this.FilePath = filePart.FilePath;
        }
    }
}
