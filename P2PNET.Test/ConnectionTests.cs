using System;
using NUnit.Framework;
using P2PNET.TransportLayer;
using System.Net;
using System.Threading.Tasks;
using P2PNET.TransportLayer.EventArgs;

namespace P2PNET.Test
{
    [TestFixture]
    public class ConnectionTests
    {
        private TransportManager transManager;

        public ConnectionTests()
        {
            transManager = new TransportManager(8080, true);
            transManager.StartAsync().Wait();
        }

        ~ConnectionTests()
        {
            transManager.CloseConnection();
        }

        [SetUp]
        public void Init()
        {
            
        }

        /*
        [TearDown]
        public void Stop()
        {
            transManager.CloseConnection();
        }
        */

        [Test]
        public async Task EstablishConnection()
        {
            string ipAddress = IPAddress.Loopback.ToString();

            await transManager.DirrectConnectAsyncTCP(ipAddress);

            int peerCount = transManager.KnownPeers.Count;

            Assert.IsTrue(peerCount > 0);
        }

        [Test]
        public async Task SendTCPMsg()
        {
            byte[] reciMsg = null;
            transManager.MsgReceived += (object obj, MsgReceivedEventArgs e) =>
            {
                reciMsg = e.Message;
            };

            string ipAddress = IPAddress.Loopback.ToString();

            byte[] SendMsg = new byte[] { 255, 0, 153, 00 };
            await transManager.SendAsyncTCP(ipAddress, SendMsg);

            Assert.IsTrue(reciMsg != null);
        }
    }
}