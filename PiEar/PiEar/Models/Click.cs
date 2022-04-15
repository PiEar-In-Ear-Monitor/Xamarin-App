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
        private int _bpm = -1;
        public int Bpm
        {
            get => _bpm;
            set
            {
                if (value != -1)
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
        public void ChangeBpm(int bpm)
        {
            _bpm = bpm;
            Bpm = -1;
        }
    }
}