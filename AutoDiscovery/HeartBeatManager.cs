using P2PNET.TransportLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDiscovery
{
    class HeartBeatManager
    {
        private string heartBeatMsg;
        private TransportManager transMgr;
        private Timer hrtBtTimer;

        //constructor
        public HeartBeatManager(string mHeartBeatMsg, TransportManager mTransMgr)
        {
            this.heartBeatMsg = mHeartBeatMsg;
            this.transMgr = mTransMgr;
            this.hrtBtTimer = new Timer();
        }

        public void StartBroadcasting()
        {
            byte[] msgBin = Encoding.ASCII.GetBytes(heartBeatMsg);

            hrtBtTimer.Tick += async (object sender, EventArgs e) =>
            {
                await this.transMgr.SendBroadcastAsyncUDP(msgBin);
                Console.WriteLine("sent heartbeat");
            };
            hrtBtTimer.Interval = 1000;
            hrtBtTimer.Start();
        }
    }
}
