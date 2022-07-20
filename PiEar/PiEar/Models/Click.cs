using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Annotations;
using PiEar.Helpers;
using Plugin.Settings;
using Plugin.SimpleAudioPlayer;

namespace PiEar.Models
{
    public sealed class Click : INotifyPropertyChanged
    {
        private int _stepCount = 10;
        public ISimpleAudioPlayer Player { get; } = CrossSimpleAudioPlayer.Current;
        private double Volume
        {
            get => CrossSettings.Current.GetValueOrDefault($"clickVolume", 0.0, Settings.ClickFile);
            set
            {
                CrossSettings.Current.AddOrUpdateValue($"clickVolume", value, Settings.ClickFile);
                Player.Volume = value;
                OnPropertyChanged();
            }
        }
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
        private bool _toggled;
        public bool Toggled
        {
            get => _toggled;
            set
            {
                var resp = Task.Run(async () => await Networking.PutRequest($"/bpm?bpmEnabled={(value)}"));
                resp.Wait();
                var json = JsonConvert.DeserializeObject<JsonData>(resp.Result);
                if (json == null || json.Error != "")
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
        public double Rotation
        {
            get => (Volume * 260) - 130;
            set
            {
                Volume = (float) ((value + 130) / 260);
                OnPropertyChanged();
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