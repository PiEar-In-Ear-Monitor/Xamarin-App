using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PiEar.Helpers;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public sealed class ClickViewModel
    {
        private const int Minimum = 0;
        private const int Maximum = 999;
        public ClickViewModel()
        {
            BackgroundTasks.ClickEventReceived += HandleClickReceived;
            StepperTapCommand = new Command(StepperTap);
            ChangeBpmCommand = new Command(ChangeBpm);
            MinusStepperCommand = new Command(MinusStepper);
            PlusStepperCommand = new Command(PlusStepper);
        }
        public Click Click { get; } = new Click();
        public ICommand StepperTapCommand { get; }
        public ICommand ChangeBpmCommand { get; }
        public ICommand MinusStepperCommand { get; }
        public ICommand PlusStepperCommand { get; }
        
        public void PanVolume(PanUpdatedEventArgs e)
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
        private void StepperTap ()
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
        private async void ChangeBpm()
        {
            string newBpm = await Application.Current.MainPage.DisplayPromptAsync("Change BPM", "Please enter the new BPM", keyboard:Keyboard.Numeric);
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
        private void MinusStepper()
        {
            Click.Bpm -= Click.StepCount;
            if (Click.Bpm < Minimum)
            {
                Click.Bpm = Minimum;
            }
        }
        private void  PlusStepper()
        {
            Click.Bpm += Click.StepCount;
            if (Click.Bpm > Maximum)
            {
                Click.Bpm = Maximum;
            }
        }
        private void HandleClickReceived(object sender, EventArgs args) {
            if (!App.GlobalMuteStatusValid)
            {
                return;
            }
            Task.Run(() => {
                if (App.GlobalMuteStatus|| !Click.Toggled)
                {
                    return;
                }
                Click.Player.Play();
            });
        }
    }
}