using System;

namespace P2P_Advance_Sample.Messages
{
    [Serializable]
    public enum MessageType
    {
        fileMessage = 0,
        KeyStroke = 1
    }

    public interface IMessage
    {
        MessageType MsgType { get; set; }
    }
}