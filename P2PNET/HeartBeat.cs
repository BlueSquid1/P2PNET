using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PNET
{
    class HeartBeat
    {
        private Timer brdcstTmr;
        private int refreshRateMilliSec;
        private BaseStation baseStation;

        //constructor
        public HeartBeat(int mRefreshRateMilliSec, BaseStation mBaseStation)
        {
            this.refreshRateMilliSec = mRefreshRateMilliSec;
            this.baseStation = mBaseStation;
        }

        public void StartBroadcasting()
        {
            brdcstTmr = new Timer(TimeLapseEvent, null, refreshRateMilliSec, Timeout.Infinite);
        }

        private async void TimeLapseEvent(object state)
        {
            //send a blank message
            byte[] heartBeat = new byte[0];
            await baseStation.SendUDPBroadcastAsync(heartBeat);
        }
    }
}
