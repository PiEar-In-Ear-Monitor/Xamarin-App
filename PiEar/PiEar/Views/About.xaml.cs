using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PiEar.Helpers;
using PiEar.Interfaces;
using PiEar.Models;

namespace PiEar.Views
{
    public partial class About
    {
        private readonly ObservableCollection<string> _receivedMessages = new ObservableCollection<string>();
        public About()
        {
            InitializeComponent();
            License.Text = @"MIT License

Copyright (c) 2021 Alexander O'Connor

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";
        }
        protected override void OnAppearing()
        {
            MulticastReceived.ItemsSource = _receivedMessages;
            // Get instance of IMulticastService
            var service = DependencyService.Get<IMulticastLock>();
            service.Acquire();
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        while (Networking.ServerIp == "IP not found")
                        {
                            Debug.WriteLine("Waiting for IP...");
                            Task.Delay(1000).Wait();
                        }
                        var endpoint = new IPEndPoint(IPAddress.Parse(Multicast.MulticastAddress), Multicast.Port);
                        var multicast = new UdpClient(endpoint);
                        while (true)
                        {
                            // var multicastBytes = multicast.Receive(ref endpoint);
                            var multicastBytes = await multicast.ReceiveAsync();
                            Debug.WriteLine(multicastBytes.RemoteEndPoint.ToString());
                            // var message = Encoding.UTF8.GetString(multicastBytes);
                            var message = Encoding.UTF8.GetString(Multicast.Uncompress(multicastBytes.Buffer));
                            Debug.WriteLine($"Received: {message}");
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _receivedMessages.Add(message);
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            });
        }
    }
}