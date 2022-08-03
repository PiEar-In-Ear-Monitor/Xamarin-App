using System;
using System.Net;
using PiEar.Models;

namespace PiEar.Helpers
{
    public static class BackgroundTasks
    {
        private static readonly Multicast IpMulticast =
            new Multicast(IPAddress.Parse(Networking.MulticastIp), Networking.MulticastPort);
        public static event EventHandler<EventArgs> ClickEventReceived;
        public static event EventHandler<StreamEvent> StreamEventReceived;
        static BackgroundTasks()
        {
            IpMulticast.UdpMessageReceived += HandleNewData;
        }
        public class StreamEvent : EventArgs
        {
            public int Channel { get; }
            public short[] Data { get; }

            public StreamEvent(int channel, short[] data)
            {
                Channel = channel;
                Data = data;
            }
        }
        private static void HandleNewData(object sender, Multicast.UdpMessageReceivedEventArgs args)
        {
            if (args.Buffer.Length == 4)
            {
                // For each item in buffer, check if is zero
                foreach (var item in args.Buffer)
                {
                    if (item == 0) continue;
                    ClickEventReceived?.Invoke(null, null);
                }
            }
            else
            {
                var channel = BitConverter.ToInt16(args.Buffer, 0);
                var data = new short[(args.Buffer.Length / 2) - 1];
                Buffer.BlockCopy(args.Buffer, 2, data, 0, args.Buffer.Length - 2);
                StreamEventReceived?.Invoke(null, new StreamEvent(channel, data));
            }
        }
    }
}