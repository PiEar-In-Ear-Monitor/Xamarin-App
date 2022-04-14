using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    }
}