using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
                var resp = Task.Run(async () => await Networking.PutRequest($"/bpm?bpm={value}"));
                resp.Wait();
                // Parse resp into JSON
                var json = JsonConvert.DeserializeObject<JsonData>(resp.Result);
                if (json == null || json.Error != null)
                {
                    Debug.WriteLine(json?.Error);
                }
                else
                {
                    _bpm = json.Bpm;
                    OnPropertyChanged();
                }
            }
        }
        public bool Toggled
        {
            get => _toggled;
            set
            {
                var resp = Task.Run(async () => await Networking.PutRequest($"/bpm?bpmEnabled={(value)}"));
                resp.Wait();
                var json = JsonConvert.DeserializeObject<JsonData>(resp.Result);
                if (json == null || json.Error != null)
                {
                    Debug.WriteLine(json?.Error);
                }
                else
                {
                    _toggled = json.BpmEnabled;
                    OnPropertyChanged();
                }
            }
        }        
        public void ChangeToggle(bool value)
        {
            _toggled = value;
            OnPropertyChanged(nameof(Toggled));
        }
        public void ChangeBpm(int value)
        {
            _bpm = value;
            OnPropertyChanged(nameof(Bpm));
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
        public void SseToggleEnabled(bool enable)
        {
            Debug.WriteLine($"SSE Enabled: {enable}");
            _toggled = enable;
            OnPropertyChanged(nameof(Toggled));
        }
    }
}