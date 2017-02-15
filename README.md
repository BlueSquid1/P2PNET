# P2PNET

[![Build status](https://ci.appveyor.com/api/projects/status/5y5reox45a5fjm5u?svg=true)](https://ci.appveyor.com/project/CrazySquid1/p2pnet)


P2PNET is a low level portable peer to peer library written in C# with an emphasises on being simple to use. 
With this library you can send and receive messages, files and objects between peers on a local network. 
P2PNET is fast. Messages are serialized with JSON.NET and then send directly using a transport layer protocol.
This make P2PNET ideal for soft real time systems (i.e. video games).

Establishing a low level peer-to-peer network connection can be hard as you have to deal with the asymmetric nature of establishing a TCP connection and
the add complexities of mutli-threaded programs. This library is designed to simplify this. In fact you don't have to worry about multiple-threads or 
establishing a connection at all. You can just use one of the supplied send methods to start sending messages on the same thread. 
P2PNET will handles establishing a TCP or UDP connection for you.

you can send all kinds of messages. For low level applications use: P2PNET.TransportLayer.TransportManager to send binary arrays. 
Alternatively you can use P2PNET.ObjectLayer.ObjectManager to send objects between peers. 
And in future I plan to implement the ability to send files.

Supported protocols:
* UDP and TCP


##Supported Platforms:
* Mono / .Net Framework 4.5
* ASP.NET Core 1.0
* Windows 8
* Windows Phone 8.1
* Windows Phone Silverlight 8
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)
* UWP

##How To Use
```c#
int portNum = 8080;
TransportManager transMgr = new TransportManager(portNum);
transManager.MsgReceived += (object obj, MsgReceivedEventArgs e) =>
    {
        byte[] message = e.Message;
		//insert code here
    };

//tell P2PNET to start listerning
await transMgr.StartAsync();


//when you want to send a message
string ipAddress = "255.255.255.255";
byte[] message = Encoding.ASCII.GetBytes("Hello world");
transMgr.SendAsyncTCP(ipAddress, message);
```

Refer to wiki for more details.