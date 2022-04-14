using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Annotations;
using PiEar.Helpers;
using Plugin.Settings;

namespace PiEar.Models
{
    public class Stream : INotifyPropertyChanged
    {
        private static int _count;
        private string Id { get; } = _count++.ToString();
        public string Label
        {
            get
            {
                var resp = Task.Run(async () => await Networking.GetRequest($"/channel-name?id={this.Id}"));
                resp.Wait();
                Debug.WriteLine(resp.Result);
                var channel = JsonConvert.DeserializeObject<JsonData>(resp.Result);
                if (channel != null && channel.Error == null)
                {
                    Debug.WriteLine(channel.ChannelName);
                    return channel.ChannelName;
                }
                return "";
            }
            set
            {
                var resp = Task.Run(async () => await Networking.PutRequest($"/channel-name?id={this.Id}&name={value}"));
                resp.Wait();
                OnPropertyChanged();
            }
        }
        public bool Mute
        {
            get => CrossSettings.Current.GetValueOrDefault($"channelMute{Id}", false, Settings.File);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelMute{Id}", value, Settings.File);
                OnPropertyChanged();
            }
        }
        public double Pan
        {
            get => CrossSettings.Current.GetValueOrDefault($"channelPan{Id}", 0.0, Settings.File);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelPan{Id}", value, Settings.File);
                OnPropertyChanged();
            }
        }
        public double Volume
        {
            get => Convert.ToDouble(CrossSettings.Current.GetValueOrDefault($"channelVolume{Id}", 0.0, Settings.File));
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelVolume{Id}", value, Settings.File);
                OnPropertyChanged();
            }
        }
        public Stream(string label) { Label = label; }
        public Stream() {}
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}