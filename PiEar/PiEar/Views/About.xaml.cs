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
using Xamarin.Forms.Internals;

namespace PiEar.Views
{
    public partial class About
    {
        private readonly ObservableCollection<string> _receivedMessages = new ObservableCollection<string>();
        private Multicast _multicast;
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
            Task.Run(() =>
            {
                _multicast = new Multicast(IPAddress.Parse("224.0.0.69"), 6666);
                _multicast.UdpMessageReceived += (sender, args) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _receivedMessages.Insert(0, Encoding.UTF8.GetString(args.Buffer));
                    });
                };
            });
        }
    }
}