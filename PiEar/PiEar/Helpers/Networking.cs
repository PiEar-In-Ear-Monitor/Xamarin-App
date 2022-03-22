using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PiEar.Interfaces;
using Xamarin.Forms;

namespace PiEar.Helpers
{
    public class Networking
    {
        public static async Task<string> GetRequest(string Url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Url);
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<string> FindServerIp()
        {
            byte[] address = DependencyService.Get<IAddress>().ByteIpAddress();
            byte[] gateway = DependencyService.Get<IAddress>().ByteIpGateway();
            byte[] toCheck = new byte[4];
            int diff = 0;
            bool[] maxSet = new bool[] {false, false, false};
            for (int i = 0; i < 4; i++)
            {
                toCheck[i] = gateway[i];
                if (address[i] != gateway[i])
                {
                    diff = i;
                    break;
                }
                else
                {
                    maxSet[i] = true;
                }
            }
            bool foundIp = false;
            
            // var request = new RestRequest("abcdefghijklmnopqrstuvwxyz");
            while ((!foundIp))
            {
                if (toCheck[3] == 0)
                {
                    // Make Request
                    Debug.WriteLine($"Making request to {new IPAddress(toCheck)}");
                }
                toCheck[3]++;
                // Make Request
                Debug.WriteLine($"Making request to {new IPAddress(toCheck)}");
                if (toCheck[3] == 255)
                {
                    if (maxSet[0] && maxSet[1] && maxSet[2] && toCheck[3] == 255)
                    {
                        foundIp = true;
                    }
                    else
                    {
                        toCheck[3] = 0;
                        if (++toCheck[diff] == 255)
                        {
                            maxSet[diff++] = true;
                        }
                    }
                }
            }
            return (toCheck[3] == 255) ? "0.0.0.0" : (new IPAddress(toCheck).ToString());
        }
    }
}