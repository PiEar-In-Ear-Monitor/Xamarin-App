using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PiEar.Helpers;

namespace PiEar.Models
{
    public class Click : Stream
    {
        private bool _enabled = false;
        private int _stepCount = 10;
        public int Bpm
        {
            get
            {   
                var resp = Task.Run(async () => await Networking.GetRequest($"/bpm"));
                resp.Wait();
                Debug.WriteLine(resp.Result);
                var channel = JsonConvert.DeserializeObject<JsonData>(resp.Result);
                if (channel != null && channel.Error == null)
                {
                    Debug.WriteLine(channel.Bpm);
                    return channel.Bpm;
                }
                return 0;
            } 
            set
            {
                if (value is int)
                {
                    var resp = Task.Run(async () => await Networking.PutRequest($"/bpm/{value}"));
                    resp.Wait();
                }
                OnPropertyChanged();
            }
        }
        public bool Enabled
        {
            get => _enabled; // TODO Get from stream
            set
            {
                _enabled = !_enabled; // TODO Get from stream
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
        public void ChangeBpm()
        {
            OnPropertyChanged(nameof(Bpm));
        }
    }
}