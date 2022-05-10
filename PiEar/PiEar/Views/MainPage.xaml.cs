using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Helpers;
using PiEar.Models;
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
        private bool _globalMute;
        private string _clickFilename => $"PiEar.Click.{CrossSettings.Current.GetValueOrDefault("click", PiEar.Settings.Click, PiEar.Settings.File)}.ogg";
        private readonly ISimpleAudioPlayer _player = CrossSimpleAudioPlayer.Current;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _clickController;
            _setupChannels();
            Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            using (var client = new HttpClient())
                            {
                                while (Networking.ServerIp == "IP Not Found")
                                {
                                    await Task.Delay(500);
                                }
                                using (var stream = await client.GetStreamAsync($"http://{Networking.ServerIp}:{Networking.Port}/channel-name/listen"))
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        while (true)
                                        {
                                            string line = await reader.ReadLineAsync();
                                            if (line.Length < 6)
                                            {
                                                continue;
                                            }
                                            line = line.Substring(6);
                                            if (line.Contains("channel_name"))
                                            {
                                                _processChannelName(line);
                                            } else if (line.Contains("bpm"))
                                            {
                                                _processBpm(line);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                    }
                }
            );
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
            while(Networking.ServerIp == "IP Not Found")
            {
                await Task.Delay(500);
            }
            var channelCount = await Networking.GetRequest("/channel-name");
            channelCount = channelCount.Replace("{\"channel_count\":", "");
            channelCount = channelCount.Replace("}", "");
            for (int i = 0; i < int.Parse(channelCount) - 1; i++)
            {
                _streams.Add(new StreamController());
            }
            var bpm = await Networking.GetRequest("/bpm");
            _processBpm(bpm);
            ListOfChannels.ItemsSource = null;
            ListOfChannels.ItemsSource = _streams;
        }
        private void _processChannelName(string line)
        {
            // Parse line into JSON
            var channelName = JsonConvert.DeserializeObject<JsonData>(line);
            if (channelName == null) return;
            _streams[channelName.Id - 1].Stream.ChangeStreamName(channelName.ChannelName);
        }
        private void _processBpm(string line)
        {
            JsonData bpm = JsonConvert.DeserializeObject<JsonData>(line);
            if (bpm == null) return;
            if (line.Contains("bpm_enabled"))
            {
                _clickController.Click.SseToggleEnabled(bpm.BpmEnabled);
            }
            if (bpm.Bpm != -1)
            {
                _clickController.Click.ChangeBpm(bpm.Bpm);
            }
        }
    }
}