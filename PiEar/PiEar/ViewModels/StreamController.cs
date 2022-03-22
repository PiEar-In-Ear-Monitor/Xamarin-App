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
    public sealed class StreamController: INotifyPropertyChanged
    {
        public StreamController(string label)
        {
            LabelTap = new Command(_labelTap);
            ImageTap = new Command(_imageTap);
            Stream.Label = label;
        }
        public StreamController()
        {
            LabelTap = new Command(_labelTap);
            ImageTap = new Command(_imageTap);
        }
        public Stream Stream { get; } = new Stream();
        public ICommand LabelTap { get; }
        public ICommand ImageTap { get; }
        private async void _labelTap () 
        {
            
            try
            {
                string newLabel = await Application.Current.MainPage.DisplayPromptAsync("What would you like to name this channel?", "");
                if (newLabel.Length <= 0) return;
                Stream.Label = ((newLabel.Length > 26 ) ? newLabel.Substring(0, 26) : newLabel );
            }
            catch (Exception ex)
            {
                Debug.Write($"Error: {ex}\n");
            }
        }
        private void _imageTap ()
        {
            Stream.Mute = !Stream.Mute;
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