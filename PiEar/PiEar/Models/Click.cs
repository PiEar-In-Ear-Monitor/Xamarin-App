namespace PiEar.Models
{
    public class Click : Stream
    {
        private int _bpm = 100;
        private bool _enabled = false;
        private int _stepCount = 10;
        public int Bpm
        {
            get => _bpm; // TODO make API  call
            set 
            { 
                _bpm = value; // TODO make API call
                OnPropertyChanged();
            }
        }
        public bool Enabled
        {
            get => _enabled; // TODO make API call
            set
            {
                _enabled = !_enabled; // TODO make API call
                OnPropertyChanged();
            }
        }
        public int StepCount
        {
            get => _stepCount;
            set
            {
                _stepCount = value;
                OnPropertyChanged();
            }
        }
        public Click() { }
    }
}