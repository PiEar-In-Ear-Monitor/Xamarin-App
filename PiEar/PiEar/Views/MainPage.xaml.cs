using System;
using System.Collections.ObjectModel;
using System.Reflection;
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
            for (int i = 0; i < 20; i++)
            {
                _streams.Add(new StreamController());
            }
        }
        protected override void OnAppearing()
        {
            _clickController.Rotation = _clickController.Rotation;
            ListOfChannels.ItemsSource = null;
            ListOfChannels.ItemsSource = _streams;
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(_clickFilename);
            _player.Load(stream);
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
            Image image = (Image)sender; 
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
    }
}