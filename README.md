# P2PNET

[![Build status](https://ci.appveyor.com/api/projects/status/5y5reox45a5fjm5u?svg=true)](https://ci.appveyor.com/project/CrazySquid1/p2pnet)


P2PNET is a low level portable peer to peer library written in C# with an emphasises on being simple to use and high performance ideal for soft real time systems. 
With this library you can send and recieve messages, files and objects between peers on a local network.

Establishing a low level peer-to-peer network connection can be hard as you have to deal with the asymetric nature of TCP and data seralization.
This library is designed to simplify this. Infact you don't have to worry about establishing a connection at all. You can just use one of the supplied
send methods to start sending messages. P2PNET will handles establishing a TCP or UDP connection for you.

you can send all kinds of messages. For low level applications use: P2PNET.TransportLayer.TransportManager to send binary arrays. 
Alternatively you can use P2PNET.ObjectLayer.ObjectManager to send objects between peers. 
And Finally for Files you can P2PNET.FileLayer.FileManager to send and retrieve files between peers.

Supported protocals:
* UDP and TCP


##Supported Plateforms:
* Windows 8.1
* Windows Phone 8.1
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)
* UWP
* Mono / .Net 4.5

##How To Use
TODO!
```Peer x = new Peer()```