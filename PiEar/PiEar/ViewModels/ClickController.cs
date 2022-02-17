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
    public class ClickController: INotifyPropertyChanged
    {
        public ClickController(){}
        public Click Click { get; } = new Click();
        public string ImageSource => (Click.Mute) ? "mute" : "unmute";
        public bool NotMute => !Click.Mute;
        public double VolumeDouble => Math.Pow(2, (3 * Click.VolumeMultiplier)) + 1;

        // Commands to bind to
        public ICommand ImageTap { get; } = new Command(_imageTap);
        public ICommand StepperTap { get; } = new Command(_stepperTap);
        private static void _imageTap (object click)
        {
            ((Click) click).Mute = !((Click) click).Mute;
            Debug.WriteLine($"ImageTap: Mute now {((Click) click).Mute}");
        }
        private static void _stepperTap (object click)
        {
            switch (((Click) click).StepCount)
            {
                case 10:
                    ((Click) click).StepCount = 5;
                    break;
                case 5:
                    ((Click) click).StepCount = 1;
                    break;
                case 1:
                    ((Click) click).StepCount = 10;
                    break;
            }
            Debug.WriteLine($"StepperTap: Step now {((Click) click).StepCount}");
        }
        // REQUIRED STUFF
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}