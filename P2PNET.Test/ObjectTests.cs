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
    class ObjectTests: IDisposable
    {
        private ObjectManager objMgr;

        private byte[] predefineData = Encoding.ASCII.GetBytes("abc");

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

            await objMgr.DirectConnectAsyncTCP(ipAddress);

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
        public async Task SendDog()
        {
            Dog RecievedDog = null;
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                switch (e.Obj.GetType())
                {
                    case "Dog":
                        RecievedDog = e.Obj.GetObject<Dog>();
                        break;
                }
            };

            string ipAddress = IPAddress.Loopback.ToString();


            Dog sendDog = new Dog("Harry");
            await objMgr.SendAsyncTCP(ipAddress, sendDog);
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(sendDog.Equals(RecievedDog));
        }


        [Test]
        public async Task SendPerson()
        {
            Person RecievedPerson = null;
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                Console.WriteLine("got here");
                switch (e.Obj.GetType())
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
            Assert.IsTrue(sendPerson.Equals(RecievedPerson));
        }

        /*
        [Test]
        public async Task SendFilePart()
        {
            FilePart RecieveFilePart = null;
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                switch (e.Obj.GetType())
                {
                    case "Dog":
                        RecieveFilePart = e.Obj.GetObject<FilePart>();
                        break;
                }
            };

            string ipAddress = IPAddress.Loopback.ToString();


            FilePart sendFilePart = new FilePart(predefineData);
            await objMgr.SendAsyncTCP(ipAddress, sendFilePart);
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(sendFilePart.Equals(RecieveFilePart));
        }
        */

        
        //send multiple objects
        [Test]
        public async Task SendMultiplePeople()
        {
            List<Person> RecievedPeople = new List<Person>();
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                switch (e.Obj.GetType())
                {
                    case "Person":
                        RecievedPeople.Add(e.Obj.GetObject<Person>());
                        break;
                }
            };

            string ipAddress = IPAddress.Loopback.ToString();

            List<Person> SendPeople = new List<Person>();
            Person phillip = new Person("Phillip", "King", 25);
            phillip.AddPet(new Dog("Calvin"));
            phillip.AddPet(new Cat("Charlie"));
            SendPeople.Add(phillip);

            Person ben = new Person("Ben", "Green", 55);
            ben.AddPet(new Fish("Henry"));
            SendPeople.Add(ben);

            Person Ashley = new Person("Ashley", "Blue", 55);
            SendPeople.Add(Ashley);


            //send the one by one
            foreach ( Person person in SendPeople)
            {
                await objMgr.SendAsyncTCP(ipAddress, person);
                System.Threading.Thread.Sleep(1000);
            }
            
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(PeopleListsEqual(RecievedPeople, SendPeople));
        }
        

        /*
        //send multiple objects of different types
        [Test]
        public async Task SendMultiDifferentObjects()
        {
            List<object> RecievedObjects = new List<object>();
            objMgr.ObjReceived += (object obj, ObjReceivedEventArgs e) =>
            {
                switch (e.Obj.GetType())
                {
                    case "Person":
                        RecievedObjects.Add(e.Obj.GetObject<Person>());
                        break;
                    case "FilePart":
                        RecievedObjects.Add(e.Obj.GetObject<FilePart>());
                        break;
                }
            };

            string ipAddress = IPAddress.Loopback.ToString();

            List<object> SendObjects = new List<object>();
            Person phillip = new Person("Phillip", "King", 25);
            phillip.AddPet(new Dog("Calvin"));
            phillip.AddPet(new Cat("Charlie"));
            SendObjects.Add(phillip);

            FilePart imageData = new FilePart(predefineData);
            SendObjects.Add(imageData);

            Person ben = new Person("Ben", "Green", 55);
            ben.AddPet(new Fish("Henry"));
            SendObjects.Add(ben);

            Person Ashley = new Person("Ashley", "Blue", 55);
            SendObjects.Add(Ashley);

            
            for(int i = 0; i < SendObjects.Count; i++)
            {
                switch(SendObjects[i].GetType().Name)
                {
                    case "Person":
                        Person person = (Person)SendObjects[i];
                        await objMgr.SendAsyncTCP(ipAddress, person);
                        break;
                    case "FilePart":
                        FilePart filePart = (FilePart)SendObjects[i];
                        await objMgr.SendAsyncTCP(ipAddress, filePart);
                        break;
                    default:
                        //TODO: test failed
                        break;
                }
            }
            

            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(RecievedObjects.Count == SendObjects.Count);
        }


        */

        private bool PeopleListsEqual(List<Person> list1, List<Person> list2)
        {

            if (list1.Count != list2.Count)
            {
                Console.WriteLine("list size differnt");
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    Console.WriteLine("item different");
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            ((IDisposable)objMgr).Dispose();
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