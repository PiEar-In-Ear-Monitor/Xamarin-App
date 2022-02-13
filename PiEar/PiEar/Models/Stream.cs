using System;

namespace PiEar.Models
{
    public class Stream
    {
        private double _volumeMultiplier;
        private bool _mute;
        private double _pan;
        private string _label;
        
        public double VolumeDouble { get { return Math.Pow(2, (3 * _volumeMultiplier)) + 1; } }
        public string VolumeString { get {return $"{_volumeMultiplier:F02}%";}}
        public double VolumeMultiplier { set {_volumeMultiplier = value; } }
        public bool Mute { get { return _mute; } set { _mute = value; } }
        public bool NotMute  { get { return !_mute; } }
        public double Pan { get { return _pan; } set { _pan = value; } }
        public string Label { get { return _label; } set { _label = value; } }
        public string ImageSource { get { return (_mute)? "mute" : "unmute" ; }}

        public Stream(string label)
        {
            _volumeMultiplier = 0.0;
            _mute = false;
            _pan = 0.0;
            _label = label;
        }
    }
}