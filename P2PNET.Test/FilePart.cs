using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.Test
{
    public class FilePart
    {
        public byte[] BinaryData { get; set; }

        public int Length;

        public FilePart(byte[] mBinaryData)
        {
            this.BinaryData = mBinaryData;
            this.Length = mBinaryData.Length;
        }
    }
}
