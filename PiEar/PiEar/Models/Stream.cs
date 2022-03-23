using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using PiEar.Annotations;
using Plugin.Settings;

namespace PiEar.Models
{
    public class Stream : INotifyPropertyChanged
    {
        protected static int Count;
        private string Id { get; } = Count++.ToString();
        public string Label 
        {
            get => CrossSettings.Current.GetValueOrDefault($"channelLabel{Id}", $"Channel {Id}", Settings.File);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"channelLabel{Id}", value, Settings.File);
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
        public static void Reset()
        {
            Count = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}