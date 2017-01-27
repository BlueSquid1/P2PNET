using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2PNET.TransportLayer;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using P2PNET.TransportLayer.EventArgs;

namespace UnitTestProject
{
    [TestClass]
    public class ConnectionTests
    {
        private TransportManager TransMang = new TransportManager(8080, true);
        private string localIP= IPAddress.Loopback.ToString();

        private int portNum = 8080;
        private int PortNum
        {
            get
            {
                portNum++;
                return portNum;
            }
       }

        
        [TestInitialize]
        public void Init()
        {
            TransMang = new TransportManager(PortNum, true);
        }
        

        [TestCleanup]
        public async void CleanUp()
        {
            await TransMang.StopAsync();
        }


        [TestMethod]
        public async Task EstablishAConnection()
        {
            bool eventRaise = false;
            //should trigger peer change event when new peer is detected
            TransMang.PeerChange += (object obj, PeerChangeEventArgs e) =>
            {
                eventRaise = true;
            };
            await TransMang.StartAsync();

            //should be zero peers by default
            List<Peer> peers = TransMang.KnownPeers;
            Assert.AreEqual(peers.Count, 0);

            await TransMang.SendAsyncTCP(localIP, new byte[] { 1 });
            System.Threading.Thread.Sleep(500);

            peers = TransMang.KnownPeers;
            Assert.AreEqual(peers.Count, 2);
            Assert.AreEqual(eventRaise, true);
        }

        [TestMethod]
        public async Task CanRecreateConnections()
        {
            byte[] messageReceived = new byte[0];
            TransMang.MsgReceived += (object obj, MsgReceivedEventArgs e) =>
            {
                messageReceived = e.Message;
            };

            await TransMang.StartAsync();

            await TransMang.SendAsyncTCP(localIP, new byte[] { 1 });
            System.Threading.Thread.Sleep(500);

            await TransMang.StopAsync();

            await TransMang.StartAsync();

            byte[] msg = new byte[] { 2 };
            await TransMang.SendAsyncTCP(localIP, msg);
            System.Threading.Thread.Sleep(500);

            Console.WriteLine("msg = " + messageReceived[0]);
            Assert.IsTrue(IsEqual(msg, messageReceived));
        }


        [TestMethod]
        public async Task CanReceiveTCPMessage()
        {
            byte[] messageReceived = new byte[0];
            TransMang.MsgReceived += (object obj, MsgReceivedEventArgs e) =>
            {
                messageReceived = e.Message;
            };
            await TransMang.StartAsync();

            byte[] msg = new byte[] { 255, 100, 10, 0, 0 };

            await TransMang.SendAsyncTCP(localIP, msg);
            System.Threading.Thread.Sleep(500);

            Assert.IsTrue(IsEqual(msg, messageReceived));
        }


        private bool IsEqual(Byte[] list1, Byte[] list2)
        {
            //check lengths
            if (list1.Length != list2.Length)
            {
                return false;
            }

            //check values
            for (int i = 0; i < list1.Length; i++)
            {
                if(list1[i] != list2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
