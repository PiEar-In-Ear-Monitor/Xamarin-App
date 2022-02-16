using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PiEar.Annotations;
using PiEar.Models;
using PiEar.Views;

using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public class StreamController: INotifyPropertyChanged
    {
        public Stream Stream { get; set; } = new Stream();
        public string ImageSource => (Stream.Mute) ? "mute" : "unmute";
        public bool NotMute => !Stream.Mute;
        public double VolumeDouble => Math.Pow(2, (3 * Stream.VolumeMultiplier)) + 1;
        public string Id { get; } = (Stream.Count++).ToString();

        // Commands to bind to
        public ICommand LabelTap { get; } = new Command(_labelTap);
        public ICommand ImageTap { get; } = new Command(_imageTap);
        
        private static void _labelTap ()  {
            Debug.WriteLine ("LabelTap");
        }
        private static void _imageTap (object stream)
        {
            Stream toChange = (Stream) stream;
            toChange.Mute = !toChange.Mute;
            Debug.WriteLine($"{stream} in ImageTap");
            // if (sender.IsEnabled)
            // {
            //     toChange.Mute = false;
            //     "mute"
            //     channelSlider.IsEnabled = false;
            //     channelVolume.TextDecorations = TextDecorations.Strikethrough;
            // }
            // else
            // {
            //     channelMute.Source = "unmute";
            //     channelSlider.IsEnabled = true;
            //     channelVolume.TextDecorations = TextDecorations.None;
            // }
        }
        // REQUIRED STUFFS
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}