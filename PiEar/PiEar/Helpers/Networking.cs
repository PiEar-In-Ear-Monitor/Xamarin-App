using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PiEar.Helpers
{
    public static class Networking
    {
        private static UdpClient _udpClient;
        public static string ServerIp { get; private set; } = null;
        public const int Port = 9090;
        public const int MulticastPort = 6666;
        public const string MulticastIp = "10.14.2.20";
        public static async Task<string> GetRequest(string endpoint)
        {
            if (ServerIp == null) return null;
            try
            {
                WebRequest request = WebRequest.Create ($"http://{ServerIp}:{Port}{endpoint}");
                request.Timeout = 5000;
                return await _getResp(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                App.Logger.DebugWrite(e.Message);
                return null;
            }
        }
        public static async Task<string> PutRequest(string endpoint)
        {
            if (ServerIp == null) return null;
            try
            {
                WebRequest request = WebRequest.Create($"http://{ServerIp}:{Port}{endpoint}");
                request.Method = "PUT";
                request.Timeout = 5000;
                return await _getResp(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                App.Logger.DebugWrite(e.Message);
                return null;
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
            catch
            {
                return "";
            }
        }
        public static Task FindServerIp()
        {
            return Task.Run( () =>
            {
                ServerIp = "10.14.2.20";
            });
            // return Task.Run(() =>
            // {
            //     _udpClient.BeginReceive((result) =>
            //     {
            //         var udpClient = result.AsyncState as UdpClient;
            //         if (udpClient == null)
            //             return;
            //         IPEndPoint remoteAddr = null;
            //         udpClient.EndReceive(result, ref remoteAddr);
            //         ServerIp = remoteAddr.Address.ToString();
            //     }, _udpClient);
            // });
        }
    }
}