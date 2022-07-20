using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using PiEar.Annotations;
using PiEar.Helpers;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public sealed class ClickViewModel
    {
        private const int Minimum = 0;
        private const int Maximum = 999;
        public static Click Click { get; } = new Click();
        public ICommand StepperTap { get; }
        public ICommand ChangeBpm { get; }
        public ICommand MinusStepper { get; }
        public ICommand PlusStepper { get; }
        public ClickViewModel()
        {
            BackgroundTasks.ClickEventReceived += HandleClickReceived;
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
        private void HandleClickReceived(object sender, EventArgs args) {
            Task.Run(() => {
                if (GlobalMuteViewModel.GlobalMuteStatus.Mute || !Click.Toggled)
                {
                    return;
                }
                Click.Player.Play();
            });
        }
        public static void PanVolume(PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                Click.Rotation += e.TotalX / 2.0;
            }
            if (Click.Rotation > 130)
            {
                Click.Rotation = 130;
            }
            else if (Click.Rotation < -130)
            {
                Click.Rotation = -130;
            }
        }
    }
}