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
    public class StreamController: INotifyPropertyChanged
    {
        public StreamController(string label)
        {
            Stream.Label = label;
        }
        public Stream Stream { get; set; } = new Stream();
        public string ImageSource => (Stream.Mute) ? "mute" : "unmute";
        public bool NotMute => !Stream.Mute;
        public double VolumeDouble => Math.Pow(2, (3 * Stream.VolumeMultiplier)) + 1;

        // Commands to bind to
        public ICommand LabelTap { get; } = new Command(_labelTap);
        public ICommand ImageTap { get; } = new Command(_imageTap);
        
        private static async void _labelTap (object stream) 
        {
            
            try
            {
                string newLabel = await Application.Current.MainPage.DisplayPromptAsync("What would you like to name this channel?", "");
                if (newLabel.Length <= 0) return;
                ((Stream)stream).Label = ((newLabel.Length > 26 ) ? newLabel.Substring(0, 26) : newLabel );
                Debug.WriteLine ($"LabelTap: Label now {((Stream)stream).Label} at ID {((Stream)stream).Id}");
            }
            catch (Exception ex)
            {
                Debug.Write($"Error: {ex}\n");
            }
        }
        private static void _imageTap (object stream)
        {
            ((Stream) stream).Mute = !((Stream) stream).Mute;
            Debug.WriteLine($"ImageTap: Mute now {((Stream) stream).Mute} at ID {((Stream) stream).Id}");
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