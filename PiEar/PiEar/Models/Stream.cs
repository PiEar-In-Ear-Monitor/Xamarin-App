using System.ComponentModel;
using System.Runtime.CompilerServices;
using PiEar.Annotations;

namespace PiEar.Models
{
    public class Stream : INotifyPropertyChanged
    {
        // TESTING
        private double _volumeMultiplier;
        
        public static int Count = 0;
        public string Id { get; } = Count++.ToString();
        public string Label { get; set; } = $"Channel {Count + 1}";
        public bool Mute { get; set; } = false;
        public double Pan { get; set; } = 0;

        public double VolumeMultiplier
        {
            get
            {
                return _volumeMultiplier;
            }
            set
            {
                _volumeMultiplier = value;
                OnPropertyChanged();
            }
        }
        public Stream(string label) { Label = label; }
        public Stream() {}
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}