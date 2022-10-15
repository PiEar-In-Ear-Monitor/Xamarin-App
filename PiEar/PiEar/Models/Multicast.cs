using System;
using System.Net;
using System.Net.Sockets;

namespace PiEar.Models
{
    public class Multicast
    {
     UdpClient _udpclient;
     IPEndPoint _remoteEndPoint;

     public Multicast(IPAddress multicastIPaddress, int port, IPAddress localIPaddress = null)
     {
         // Store params
         var localIPaddress1 = localIPaddress;
         if (localIPaddress == null)
             localIPaddress1 = IPAddress.Any;

         // Create endpoints
         _remoteEndPoint = new IPEndPoint(multicastIPaddress, port);
         var localEndPoint = new IPEndPoint(localIPaddress1, port);

         // Create and configure UdpClient
         _udpclient = new UdpClient();
         // The following three lines allow multiple clients on the same PC
         _udpclient.ExclusiveAddressUse = false;
         _udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
         _udpclient.ExclusiveAddressUse = false;
         // Bind, Join
         _udpclient.Client.Bind(localEndPoint);
         // _udpclient.JoinMulticastGroup(_multicastIPaddress, _localIPaddress);

         // Start listening for incoming data
         byte[] fooData = new byte[1];
         SendMulticast(fooData);
         _udpclient.BeginReceive(ReceivedCallback, null);
     }

     /// <summary>
     /// Send the buffer by UDP to multicast address
     /// </summary>
     /// <param name="bufferToSend"></param>
     public void SendMulticast(byte[] bufferToSend)
     {
         _udpclient.Send(bufferToSend, bufferToSend.Length, _remoteEndPoint);
     }

     /// <summary>
     /// Callback which is called when UDP packet is received
     /// </summary>
     /// <param name="ar"></param>
     private void ReceivedCallback(IAsyncResult ar)
     {
         // Get received data
         IPEndPoint sender = new IPEndPoint(0, 0);
         Byte[] receivedBytes = _udpclient.EndReceive(ar, ref sender);

         // fire event if defined
         if (UdpMessageReceived != null)
             UdpMessageReceived(this, new UdpMessageReceivedEventArgs() { Buffer = receivedBytes });

         // Restart listening for udp data packages
         _udpclient.BeginReceive(ReceivedCallback, null);
     }

     /// <summary>
     /// Event handler which will be invoked when UDP message is received
     /// </summary>
     public event EventHandler<UdpMessageReceivedEventArgs> UdpMessageReceived;

     /// <summary>
     /// Arguments for UdpMessageReceived event handler
     /// </summary>
     public class UdpMessageReceivedEventArgs: EventArgs
     {
         public byte[] Buffer {get;set;}
     }

    }
}