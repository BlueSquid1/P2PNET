using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.ApplicationLayer.MsgMetadata
{
    public class Metadata
    {
        string MsgType { get; }
        int MsgSizeBytes { get; }
        //if true then the meta data is sent seperately to the message
        //this is needed to give the receiver a change to reject the
        //incoming message
        bool IsTwoWay { get; }

        string SourceIp { get; }
        string TargetIp { get; }

        //constructor
        public Metadata(string msgType, int msgSizeBytes, bool isTwoWay, string sourceIp, string TargetIp)
        {
            this.MsgType = msgType;
            this.MsgSizeBytes = msgSizeBytes;
            this.IsTwoWay = isTwoWay;
            this.SourceIp = sourceIp;
            this.TargetIp = TargetIp;
        }
    }
}
