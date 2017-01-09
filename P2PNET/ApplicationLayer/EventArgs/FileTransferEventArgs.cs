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
        //constructor
        public FileTransferEventArgs(float percentage)
        {
            this.Percent = percentage;
        }
    }
}
