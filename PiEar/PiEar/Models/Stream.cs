using System.ComponentModel;
using System.Runtime.CompilerServices;
using PiEar.Annotations;

namespace PiEar.Models
{
    public class Stream : INotifyPropertyChanged
    {
        private static int _count = 0;
        public string Id { get; } = _count++.ToString();
        private string _label;
        private bool _mute;
        private double _pan;
        private double _volumeMultiplier;
        public string Label 
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                OnPropertyChanged();
            }
            
        }
        public bool Mute
        {
            get
            {
                return _mute;
            }
            set
            {
                _mute = value;
                OnPropertyChanged();
            }
        }
        public double Pan
        {
            get
            {
                return _pan;
            }
            set
            {
                _pan = value;
                OnPropertyChanged();
            }
        }
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
        public Stream(string label)
        {
            _label = label;
            _mute =false;
            _pan = 0.0;
            _volumeMultiplier = 0.0;
        }
        public Stream()
        {
            _label = $"Channel {_count + 1}";
            _mute =false;
            _pan = 0.0;
            _volumeMultiplier = 0.0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}