using System;
using System.Diagnostics;
using System.Net;
using PiEar.Helpers;
using PiEar.Models;

namespace PiEar.ViewModels
{
    public static class Globals
    {
        static Globals()
        {
            IpMulticast.UdpMessageReceived += (sender, args) =>
            {
                if (args.Buffer.Length == 4)
                {
                    // For each item in buffer, check if is zero
                    foreach (var item in args.Buffer)
                    {
                        if (item == 0) continue;
                        ClickEventReceived?.Invoke(null, new ClickEvent());
                    }
                }
                else
                {
                    // Add to sound buffer
                    // Debug.WriteLine($"Received {args.Buffer.Length} bytes for channel {args.Buffer[0]}");
                }
            };
        }
        private static readonly Multicast IpMulticast = new Multicast(IPAddress.Parse(Networking.MulticastIp), Networking.MulticastPort);
        public static event EventHandler<ClickEvent> ClickEventReceived;
        public class ClickEvent: EventArgs
        {
        }
    }
}