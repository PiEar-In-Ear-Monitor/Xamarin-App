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
        private const int Minimum = 0;
        private const int Maximum = 999;
        public Click Click { get; } = new Click();
        public ICommand StepperTap { get; }
        public ICommand ChangeBpm { get; }
        public ICommand MinusStepper { get; }
        public ICommand PlusStepper { get; }
        public double Rotation
        {
            get => (Click.Volume * 260) - 130;
            set
            {
                Click.Volume = (value + 130) / 260;
                OnPropertyChanged();
            }
        }
        public ClickController()
        {
            ChangeBpm = new Command(_changeBpm);
            StepperTap = new Command(_stepperTap);
            MinusStepper = new Command(_minusStepper);
            PlusStepper = new Command(_plusStepper);
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
                if (intBpm <= Maximum && intBpm >= Minimum)
                {
                    Click.Bpm = intBpm;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        private void _minusStepper()
        {
            Click.Bpm -= Click.StepCount;
            if (Click.Bpm < Minimum)
            {
                Click.Bpm = Minimum;
            }
        }
        private void _plusStepper()
        {
            Click.Bpm += Click.StepCount;
            if (Click.Bpm > Maximum)
            {
                Click.Bpm = Maximum;
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