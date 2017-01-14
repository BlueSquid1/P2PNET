using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer
{
    public enum MessageType
    {
        Object = 0,
        File = 1
    }
    public class Metadata
    {
        //the type of the object
        public string Name { get; set; }

        //whether the message is an object or a file
        public MessageType MsgType { get; set; }
        //the number of bytes to be send across the network
        //used so the receive knowns when the object ends and
        //can also be used in two way handshake to reject the
        //incoming message based on its size
        public int TotalMsgSizeBytes { get; set; }
        //for big objects that are send over multiple smaller messages
        //the first message is sent with a IsTwoWay = true. This gives
        //the recieve to reject the remaining message
        public bool IsTwoWay { get; set; }

        public string SourceIp { get; set; }
    }
}
