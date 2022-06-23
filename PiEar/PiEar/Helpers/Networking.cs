using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PiEar.Helpers
{
    public static class Networking
    {
        private static bool _foundIp;
        private static string _serverIp;
        public static string ServerIp => (_foundIp) ? _serverIp : "IP Not Found";
        public const int Port = 9090;
        public const int MulticastPort = 6666;
        public const string MulticastIp = "224.0.0.69";
        public static async Task<string> GetRequest(string endpoint, bool forDiscovery = false)
        {
            if (!_foundIp && !forDiscovery) return "";
            try
            {
                WebRequest request = WebRequest.Create ($"http://{_serverIp}:{Port}{endpoint}");
                request.Timeout = 500;
                return await _getResp(request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "";
            }
        }
        public static async Task<string> PutRequest(string endpoint)
        {
            if (!_foundIp) return "";
            try
            {
                WebRequest request = WebRequest.Create($"http://{ServerIp}:{Port}{endpoint}");
                request.Method = "PUT";
                request.Timeout = 500;
                return await _getResp(request);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "";
            }
        }
        private static async Task<string> _getResp(WebRequest req)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)await req.GetResponseAsync();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream ?? throw new InvalidOperationException());
                string responseFromServer = await reader.ReadToEndAsync();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "";
            }
        }
        
        
        private static void OnUdpDataReceived(IAsyncResult result)
        {
            Debug.WriteLine($">>> in receive");

            var udpClient = result.AsyncState as UdpClient;
            if (udpClient == null)
                return;

            IPEndPoint remoteAddr = null;
            var recvBuffer = udpClient.EndReceive(result, ref remoteAddr);

            Debug.WriteLine($"MESSAGE FROM: {remoteAddr.Address}:{remoteAddr.Port}, MESSAGE SIZE: {recvBuffer?.Length ?? 0}");

            udpClient.BeginReceive(OnUdpDataReceived, udpClient);
        }

        private static UdpClient _udpClient;
        
        public static async void FindServerIp()
        {
            _udpClient = new UdpClient()
            {
                ExclusiveAddressUse = false,
                EnableBroadcast = true
            };
            _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, MulticastPort));
            _udpClient.BeginReceive((result) =>
            {
                var udpClient = result.AsyncState as UdpClient;
                if (udpClient == null)
                    return;
                IPEndPoint remoteAddr = null;
                udpClient.EndReceive(result, ref remoteAddr);
                _serverIp = remoteAddr.Address.ToString();
                _foundIp = true;
            }, _udpClient);
            // This was brute force, but it works. Not needed since we can get IP from the multicast packets.
            // byte[] address = DependencyService.Get<IAddress>().ByteIpAddress();
            // byte[] gateway = DependencyService.Get<IAddress>().ByteIpGateway();
            // byte[] toCheck = new byte[4];
            // int diff = 0;
            // bool[] maxSet = {false, false, false};
            // for (int i = 0; i < 4; i++)
            // {
            //     toCheck[i] = gateway[i];
            //     if (address[i] != gateway[i])
            //     {
            //         diff = i;
            //         break;
            //     }
            //     else
            //     {
            //         maxSet[i] = true;
            //     }
            // }
            // // toCheck[2] = 154; // Save a lot of time!
            // while (!_foundIp)
            // {
            //     for (int i = 0; i < 256; i++)
            //     {
            //         byte[] intBytes = BitConverter.GetBytes(i);
            //         Array.Reverse(intBytes);
            //         toCheck[3] = intBytes[3];
            //         _serverIp = new IPAddress(toCheck).ToString();
            //         if (await GetRequest("/abcdefghijklmnopqrstuvwxyz", true) == "zyxwvutsrqponmlkjihgfedcba")
            //         {
            //             _foundIp = true;
            //             break;
            //         }
            //     }
            //     if (maxSet[0] && maxSet[1] && maxSet[2] && toCheck[3] == 255)
            //     {
            //         break;
            //     }
            //     if (++toCheck[diff] == 255)
            //     {
            //         maxSet[diff++] = true;
            //     }
            // }
        }
    }
}