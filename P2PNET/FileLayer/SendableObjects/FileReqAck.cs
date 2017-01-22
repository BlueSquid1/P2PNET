using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.FileLayer.SendableObjects
{
    class FileReqAck
    {
        public bool AcceptedFile { get; }

        //constructor
        public FileReqAck(bool mAcceptedFile)
        {
            this.AcceptedFile = mAcceptedFile;
        }
    }
}
