namespace PiEar
{
    public class Stream
    {
        private double _volume;
        private bool _mute;
        private double _pan;
        private string _label;
        
        public double Volume { get { return _volume; } set { _volume = value; } }
        public bool Mute { get { return _mute; } set { _mute = value; } }
        public double Pan { get { return _pan; } set { _pan = value; } }
        public string Label { get { return _label; } set { _label = value; } }

        public Stream(string label)
        {
            _volume = 0.0;
            _mute = false;
            _pan = 0.0;
            _label = label;
        }
    }
}