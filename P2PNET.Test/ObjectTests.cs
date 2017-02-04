using NUnit.Framework;
using P2PNET.ObjectLayer;
using P2PNET.ObjectLayer.EventArgs;
using P2PNET.TransportLayer.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PNET.Test
{
    [TestFixture]
    class ObjectTests
    {
        private ObjectManager objMgr;

        public ObjectTests()
        {
            objMgr = new ObjectManager(8081, true);
            objMgr.StartAsync().Wait();
        }

        [Test]
        public async Task EstablishObjConnection()
        {
            bool peerChange = false;
            objMgr.PeerChange += (object obj, PeerChangeEventArgs e) =>
            {
                peerChange = true;
            };

            string ipAddress = IPAddress.Loopback.ToString();

            await objMgr.DirrectConnectAsyncTCP(ipAddress);

            int peerCount = objMgr.KnownPeers.Count;

            Assert.IsTrue(peerCount > 0);
            Assert.IsTrue(peerChange == true);
        }

        [Test]
        public async Task SendNullTCPObj()
        {
            bool reciObj = false;
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                reciObj = true;
            };

            string ipAddress = IPAddress.Loopback.ToString();


            object sendObj = null;
            await objMgr.SendAsyncTCP(ipAddress, sendObj);
            System.Threading.Thread.Sleep(100);

            Assert.IsTrue(reciObj == false);
        }

        [Test]
        public async Task SendPerson()
        {
            Person RecievedPerson = null;
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                switch(e.Obj.GetType())
                {
                    case "Person":
                        RecievedPerson = e.Obj.GetObject<Person>();
                        break;
                }
            };

            string ipAddress = IPAddress.Loopback.ToString();


            Person sendPerson = new Person("Phillip", "King", 25);
            sendPerson.AddPet(new Dog("Calvin"));
            sendPerson.AddPet(new Cat("Charlie"));
            await objMgr.SendAsyncTCP(ipAddress, sendPerson);
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine(RecievedPerson?.Age);
            //Assert.AreEqual(sendPerson, RecievedPerson);
            Assert.IsTrue(sendPerson.Equals(RecievedPerson));
        }


        /*
        [Test]
        public async Task KnownPeerTest()
        {
            byte[] reciMsg = null;
            transMgr.MsgReceived += (object obj, MsgReceivedEventArgs e) =>
            {
                reciMsg = e.Message;
            };

            string ipAddress = IPAddress.Loopback.ToString();

            //make sure the local peer is known
            await transMgr.DirrectConnectAsyncTCP(ipAddress);

            byte[] SendMsg = new byte[] { 255, 0, 153, 00 };
            await transMgr.SendToAllPeersAsyncUDP(SendMsg);
            System.Threading.Thread.Sleep(100);
            Console.WriteLine(BinToString(reciMsg));

            Assert.IsTrue(reciMsg != null);
        }

        [Test]
        public async Task GetIpAddress()
        {
            string localIp = await transMgr.GetIpAddress();
            Console.WriteLine("local ip address = " + localIp);
            Assert.IsTrue(localIp != null);
        }


        [Test]
        public async Task SendBlankTCPMsg()
        {
            bool msgReceived = false;
            transMgr.MsgReceived += (object obj, MsgReceivedEventArgs e) =>
            {
                msgReceived = true;
            };

            string ipAddress = IPAddress.Loopback.ToString();

            byte[] SendMsg = null;
            try
            {
                await transMgr.SendAsyncTCP(ipAddress, SendMsg);
            }
            catch
            {

            }

            Assert.IsTrue(msgReceived == false);
        }

        public string BinToString(byte[] msg)
        {
            if (msg == null)
            {
                return "no message";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < msg.Length; i++)
            {
                sb.Append(msg[i]);
            }
            return sb.ToString();
        }
        */
    }
}