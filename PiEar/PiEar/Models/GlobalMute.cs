using System.ComponentModel;
using System.Runtime.CompilerServices;
using PiEar.Annotations;
using Plugin.Settings;

namespace PiEar.Models
{
    public class GlobalMute : INotifyPropertyChanged
    {
        public string GlobalMuteImage => Mute ? "mute" : "unmute";
        public bool Mute
        {
            get => CrossSettings.Current.GetValueOrDefault("globalMute", false, Settings.ClickFile);
            set
            {
                CrossSettings.Current.AddOrUpdateValue("globalMute", value, Settings.ClickFile);
                OnPropertyChanged();
                OnPropertyChanged(nameof(GlobalMuteImage));
            } 
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}