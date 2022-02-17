namespace PiEar.Models
{
    public class Click : Stream
    {
        private int _bpm = 100;
        private bool _enabled = false;
        private int _stepCount = 10;
        public string AudioFile { get; set; }
        public int Bpm
        {
            get
            {
                return _bpm;
            }
            set 
            { 
                _bpm = value;
                OnPropertyChanged();
            }
        }
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = !_enabled;
                OnPropertyChanged();
            }
        }
        public int StepCount
        {
            get
            {
                return _stepCount;
            }
            set
            {
                _stepCount = value;
                OnPropertyChanged();
            }
        }
        public Click()
        {
            Label = "Click";
        }
    }
}