using P2PNET;
using Test.messages;
using System;

namespace Test
{
    class Test
    {
        private int portNum = 8080;
        private PeerManager peerManager;

        //constructor
        public Test()
        {
            peerManager = new PeerManager(portNum);
        }

        public void Start()
        {
            peerManager.StartAsync().Wait();

            Person person = new Person("Phillip", "King", 20);

            peerManager.SendObjToAllPeersAsyncUDP(person);
        }

    }
}
