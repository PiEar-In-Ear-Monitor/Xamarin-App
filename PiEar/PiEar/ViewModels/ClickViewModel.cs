using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using PiEar.Annotations;
using PiEar.Helpers;
using PiEar.Models;
using PiEar.Views;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public sealed class ClickViewModel
    {
        private const int Minimum = 0;
        private const int Maximum = 999;
        public static Click Click { get; } = new Click();
        public ICommand StepperTapCommand { get; } = new Command(StepperTap);
        public ICommand ChangeBpmCommand { get; } = new Command(ChangeBpm);
        public ICommand MinusStepperCommand { get; } = new Command(MinusStepper);
        public ICommand PlusStepperCommand { get; } = new Command(PlusStepper);
        private static void StepperTap ()
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
        private async static void ChangeBpm()
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
        private static void MinusStepper()
        {
            Click.Bpm -= Click.StepCount;
            if (Click.Bpm < Minimum)
            {
                Click.Bpm = Minimum;
            }
        }
        private static void  PlusStepper()
        {
            Click.Bpm += Click.StepCount;
            if (Click.Bpm > Maximum)
            {
                Click.Bpm = Maximum;
            }
        }
        public static void HandleClickReceived(object sender, EventArgs args) {
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