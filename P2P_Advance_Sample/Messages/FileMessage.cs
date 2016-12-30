using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2P_Advance_Sample.Messages
{
    public class FileMessage : IMessage
    {
        public MessageType MsgType{ get; set; }
        public byte[] FileData { get; set; }

        //constructor
        public FileMessage(byte[] fileData)
        {
            MsgType = MessageType.fileMessage;
            FileData = fileData;
        }
    }
}
