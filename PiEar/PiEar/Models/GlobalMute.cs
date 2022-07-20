using System.ComponentModel;
using System.Runtime.CompilerServices;
using PiEar.Annotations;

namespace PiEar.Models
{
    public class GlobalMute : INotifyPropertyChanged
    {
        private bool _mute;
        public bool Mute
        {
            get => _mute;
            set
            {
                _mute = value;
                OnPropertyChanged();
            } 
        } // TODO: Make this a saved setting
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}