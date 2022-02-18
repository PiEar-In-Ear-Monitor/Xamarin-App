using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PiEar.Annotations;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public sealed class ClickController: INotifyPropertyChanged
    {
        private double _rotation = -130;
        public Click Click { get; } = new Click();
        public double VolumeDouble => Math.Pow(2, (3 * Click.VolumeMultiplier)) + 1;
        public ICommand StepperTap { get; }
        public ICommand ChangeBpm { get; }
        public double Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                Click.VolumeMultiplier = ((_rotation / 260) * 1.2) + 0.6;
                OnPropertyChanged();
            }
        }
        public ClickController()
        {
            ChangeBpm = new Command(_changeBpm);
            StepperTap = new Command(_stepperTap);
        }
        private void _stepperTap ()
        {
            switch (Click.StepCount)
            {
                case 10:
                    Click.StepCount = 5;
                    break;
                case 5:
                    Click.StepCount = 1;
                    break;
                case 1:
                    Click.StepCount = 10;
                    break;
            }
        }
        private async void _changeBpm()
        {
            string newBpm = await Application.Current.MainPage.DisplayPromptAsync("What is the BPM?", "");
            try
            {
                int intBpm = int.Parse(newBpm);
                Click.Bpm = intBpm;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        // REQUIRED STUFF
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}