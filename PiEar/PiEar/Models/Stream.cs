using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Annotations;
using PiEar.Helpers;
using PiEar.Interfaces;
using Plugin.Settings;
using Xamarin.Forms;

namespace PiEar.Models
{
    public sealed class Stream : INotifyPropertyChanged
    {
        private static int _count;
        private string Id { get; } = _count++.ToString();
        private string _label;
        // public System.IO.Stream Buffer { get; } = new System.IO.MemoryStream( );
        public IPiearAudio Player { get; }
        // public ISimpleAudioPlayer Player { get; } = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        public string Label
        {
            get => _label;
            set
            {
                var resp = Task.Run(async () => await Networking.PutRequest($"/channel-name?id={Id}&name={_toBase64(value)}"));
                resp.Wait();
                _label = value;
                OnPropertyChanged();
            }
        }
        public bool Mute
        {
            get => CrossSettings.Current.GetValueOrDefault($"channelMute{Id}", false, Settings.ChannelFile);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelMute{Id}", value, Settings.ChannelFile);
                OnPropertyChanged();
            }
        }
        public double Pan
        {
            get => CrossSettings.Current.GetValueOrDefault($"channelPan{Id}", 0.0, Settings.ChannelFile);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelPan{Id}", value, Settings.ChannelFile);
                OnPropertyChanged();
            }
        }
        public double Volume
        {
            get => CrossSettings.Current.GetValueOrDefault($"channelVolume{Id}", 0.0, Settings.ChannelFile);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelVolume{Id}", value, Settings.ChannelFile);
                Player?.SetVolume((float) value);
                OnPropertyChanged();
            }
        }
        public Stream()
        {
            var resp = Task.Run(async () => await Networking.GetRequest($"/channel-name?id={Id}"));
            resp.Wait();
            if (resp.Result == null)
            {
                App.Logger.ErrorWrite($"Failed to get channel name on channel {Id}");
                return;
            }
            var channel = JsonConvert.DeserializeObject<JsonData>(resp.Result);
            if (channel != null && channel.Error != "")
            {
                channel.ChannelName = _fromBase64(channel.ChannelName);
                _label = channel.ChannelName;
            }
            else
            {
                _label = "";
                App.Logger.ErrorWrite($"Failed to get channel name on channel {Id}");
            }

            try
            {
                Player = DependencyService.Get<IPiearAudio>();
                Player.Init();
                Player.Play();
            }
            catch (Exception e)
            {
                App.Logger.ErrorWrite($"Failed to get player on channel {Id}");
                App.Logger.ErrorWrite(e.Message);
            }
        }
        public void ChangeLabel(string value)
        {
            string converted = null;
            try
            {
                converted = _fromBase64(value);
            } catch (Exception e)
            {
                App.Logger.DebugWrite($"Trouble converting {value} to base64: {e.Message}");
            }
            _label = converted;
            OnPropertyChanged(nameof(Label));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _toBase64(string data) => Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        private string _fromBase64(string data) => Encoding.UTF8.GetString(Convert.FromBase64String(data));
    }
}