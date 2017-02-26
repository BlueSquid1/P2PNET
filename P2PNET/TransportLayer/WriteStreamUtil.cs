using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PNET.TransportLayer
{
    class WriteStreamUtil : AbstractStreamUtil
    {
        private SemaphoreSlim messageSem = new SemaphoreSlim(1);

        private Queue<byte[]> msgBuffer;

        //constructor
        public WriteStreamUtil(Stream mWriteStream ) : base(mWriteStream)
        {
            msgBuffer = new Queue<byte[]>();
        }

        public async Task WriteBytesAsync(byte[] msg)
        {
            //add message to buffer
            msgBuffer.Enqueue(msg);

            //make sure only one message is sent at a time
            await messageSem.WaitAsync();
            try
            {
                //read next message from buffer
                if (msgBuffer.Count <= 0)
                {
                    throw new LowLevelTransitionError("Expected more messages in the write buffer.");
                }
                byte[] nextMsg = msgBuffer.Dequeue();

                //send number indicating message size
                int lenMsg = (int)nextMsg.Length;
                byte[] lenBin = IntToBinary(lenMsg);
                await base.ActiveStream.WriteAsync(lenBin, 0, lenBin.Length);

                //send the msg
                if (lenMsg > 0)
                {
                    await base.ActiveStream.WriteAsync(nextMsg, 0, lenMsg);
                }
                await base.ActiveStream.FlushAsync();
            }
            finally
            {
                messageSem.Release();
            }
        }
    }
}
