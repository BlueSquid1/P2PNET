using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.TransportLayer
{
    class WriteStreamUtil : AbstractStreamUtil
    {
        //constructor
        public WriteStreamUtil(Stream mWriteStream ) : base(mWriteStream)
        {

        }

        public async Task WriteBytesAsync(byte[] msg)
        {
            //send number indicating message size
            int lenMsg = (int)msg.Length;
            byte[] lenBin = IntToBinary(lenMsg);
            await base.ActiveStream.WriteAsync(lenBin, 0, lenBin.Length);

            //send the msg
            await base.ActiveStream.WriteAsync(msg, 0, lenMsg);
            await base.ActiveStream.FlushAsync();

        }
    }
}
