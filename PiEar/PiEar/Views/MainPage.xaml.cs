using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Helpers;
using PiEar.ViewModels;
using Plugin.Settings;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainPage
    {
        private readonly ObservableCollection<StreamController> _streams = new ObservableCollection<StreamController>();
        private readonly ClickController _clickController = new ClickController();
        private bool _globalMute = false;
        private string _clickFilename => $"PiEar.Click.{CrossSettings.Current.GetValueOrDefault("click", PiEar.Settings.Click, PiEar.Settings.File)}.ogg";
        private readonly ISimpleAudioPlayer _player = CrossSimpleAudioPlayer.Current;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _clickController;
            _setupChannels();
        }
        protected override void OnAppearing()
        {
            ListOfChannels.ItemsSource = null;
            ListOfChannels.ItemsSource = _streams;
            _clickController.Rotation = _clickController.Rotation;
            _player.Load(typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream(_clickFilename));
        }
        private async void _openSettings(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Settings());
        }
        private async void _openAbout(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new About());
        }
        private void _panVolume(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                _clickController.Rotation += e.TotalX / 2.0;
            }
            if (_clickController.Rotation > 130)
            {
                _clickController.Rotation = 130;
            } else if (_clickController.Rotation < -130)
            {
                _clickController.Rotation = -130;
            }
        }
        private void _muteAudio(object sender, EventArgs e)
        {
            _globalMute = !_globalMute;
            GlobalMuteIcon.IconImageSource = (_globalMute) ? "mute" : "unmute";
        }

        private void _pressForSound(object sender, EventArgs e)
        {
            _player.Volume = _clickController.Click.Volume;
            _player.Play();
        }
        private async void _setupChannels()
        {
            Networking.FindServerIp();
            while(Networking.ServerIp == "IP Not Found")
            {
                await Task.Delay(500);
            }
            var channelCount = await Networking.GetRequest("/");
            channelCount = channelCount.Replace("{\"channel_count\":", "");
            channelCount = channelCount.Replace("}", "");
            for (int i = 0; i < int.Parse(channelCount) - 1; i++)
            {
                _streams.Add(new StreamController());
            }
            ListOfChannels.ItemsSource = null;
            ListOfChannels.ItemsSource = _streams;
        }
        
        // // Server-Sent Events Client
        // private void _serverSentEventsClient()
        // {
        //     var client = new SSEClient(Networking.ServerIp, 9090);
        //     client.OnMessage += (sender, e) =>
        //     {
        //         var message = JsonConvert.DeserializeObject<StreamMessage>(e.Data);
        //         if (message.Channel == "click")
        //         {
        //             _clickController.Click = message;
        //         }
        //         else
        //         {
        //             var stream = _streams.Find(x => x.Channel == message.Channel);
        //             stream.Stream = message;
        //         }
        //     };
        //     client.Start();
        // }
        private async void _sse()
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            string url = $"http://localhost:9090/channel-name/listen";
            while (true)
            {
                try
                {
                    Debug.WriteLine("Establishing connection");
                    using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var message = await streamReader.ReadLineAsync();
                            Debug.WriteLine($"Received: {message}");
                        }
                    }
                }
                catch(Exception ex)
                {
                    //Here you can check for 
                    //specific types of errors before continuing
                    //Since this is a simple example, i'm always going to retry
                    Debug.WriteLine($"Error: {ex.Message}");
                    Debug.WriteLine("Retrying in 5 seconds");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}