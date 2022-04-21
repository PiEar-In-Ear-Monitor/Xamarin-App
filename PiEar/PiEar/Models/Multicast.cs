using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PiEar.Helpers;
using PiEar.ViewModels;

namespace PiEar.Models
{
    public class Multicast
    {
        public const int Port = 6666;
        public const string MulticastAddress = "224.0.0.69";
        public List<byte[]> Streams { get; set; }
        public bool Click { get; set; }
        public Multicast()
        {
        }
        public async void MainLoop()
        {
            while (true)
            {
                try
                {
                    while (Networking.ServerIp == "IP not found")
                    {
                        Task.Delay(1000).Wait();
                    }
                    var endpoint = new IPEndPoint(IPAddress.Parse(MulticastAddress), Port);
                    var multicast = new UdpClient(endpoint);
                    while (true)
                    {
                        var multicastBytes = multicast.Receive(ref endpoint);
                        // var message = Encoding.UTF8.GetString(multicastBytes);
                        var message = Encoding.UTF8.GetString(Uncompress(multicastBytes));
                        Debug.WriteLine($"Received: {message}");
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        public static byte[] Uncompress(byte[] compressed)
        {
            using (var compressedStream = new MemoryStream(compressed))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}