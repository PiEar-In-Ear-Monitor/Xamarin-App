using System.Diagnostics;
using System.Threading.Tasks;
using PiEar.Helpers;

namespace PiEar.Models
{
    public class Click : Stream
    {
        private bool _toggled;
        private int _stepCount = 10;
        private int _bpm = -1;
        public int Bpm
        {
            get => _bpm;
            set
            {
                if (value != -1)
                {
                    var resp = Task.Run(async () => await Networking.PutRequest($"/bpm?bpm={value}"));
                    resp.Wait();
                }
                OnPropertyChanged();
            }
        }
        public bool Toggled
        {
            get => _toggled;
            set
            {
                Debug.WriteLine($"Enabled: {value}");
                var resp = Task.Run(async () => await Networking.PutRequest($"/bpm?bpmEnabled={(value)}"));
                resp.Wait();
                _toggled = value;
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
        public void ChangeBpm(int bpm)
        {
            _bpm = bpm;
            Bpm = -1;
        }
        public void SseToggleEnabled(bool enable)
        {
            Debug.WriteLine($"SSE Enabled: {enable}");
            _toggled = enable;
            OnPropertyChanged(nameof(Toggled));
        }
    }
}