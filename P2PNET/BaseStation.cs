using P2PNET.EventArgs;
using Sockets.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET
{
    public class BaseStation
    {
        private UdpSocketClient senderUDP;

        private int portNum;

        //constructor
        public BaseStation(int mPortNum)
        {
            this.senderUDP = new UdpSocketClient();

            this.portNum = mPortNum;
        }

        public void SendUDPMsg(string ipAddress, byte[] msg)
        {
            senderUDP.SendToAsync(msg, ipAddress, this.portNum);
        }


        public void IncomingMsg(object sender, MsgReceivedEventArgs e)
        {
            if(e.BindingType == TransportType.UDP)
            {
                string remotePeeripAddress = e.RemoteIp;
                if(!isNewPeer(remotePeeripAddress))
                {
                    //not a new peer
                    return;
                }
            }

            //new peer establish a TCP connection with this peer
            ConnectionWithNewPeer();
        }

        public void NewTCPConnection(object sender, PeerConnectReqEventArgs e)
        {
            ConnectionWithNewPeer();
        }

        private void ConnectionWithNewPeer()
        {
            //TODO

        }

        private bool isNewPeer(string ipAddress)
        {
            //TODO
            return true;
        }
    }
}
