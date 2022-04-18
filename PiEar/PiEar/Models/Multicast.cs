using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PiEar.Helpers;

namespace PiEar.Models
{
    public class Multicast
    {

        public void MainLoop()
        {
            try
            {
                while (Networking.ServerIp == "IP not found")
                {
                    Console.WriteLine("Waiting for IP...");
                    Task.Delay(1000).Wait();
                }
                var udpClient = new UdpClient(Networking.ServerIp, Networking.Port);
                var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    var data = udpClient.Receive(ref ipEndPoint);
                    var message = Encoding.UTF8.GetString(data);
                    Debug.WriteLine(message);
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}