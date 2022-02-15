using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PiEar.Annotations;
using Xamarin.Forms;

namespace PiEar.Models
{
    public class Stream : INotifyPropertyChanged
    {
        // Internal ID, Useful???
        private static int _count = 0;
        
        // Public values
        public string Label { get; set; }
        public bool Mute { get; set; } = false;
        public double Pan { get; set; } = 0;
        public double VolumeMultiplier { get; set; } = 0;

        // Alternate Expressions of Public values
        public string ImageSource => (Mute) ? "mute" : "unmute";
        public bool NotMute => !Mute;
        public double VolumeDouble => Math.Pow(2, (3 * VolumeMultiplier)) + 1;
        public string Id { get; } = (_count++).ToString();

        // Initializer
        public Stream(string label)
        {
            Label = label;
        }

        protected Stream()
        {
            Label = $"Channel {_count + 1}";
        }
        // Commands to bind to
        public static ICommand LabelTap => new Command(_labelTap);
        public static ICommand ImageTap => new Command(_imageTap);
        private static void _labelTap ()  {
            Debug.WriteLine ("LabelTap");
        }
        private static void _imageTap ()  {
            Debug.WriteLine ("ImageTap");
        }
        
        // Property Changed Default
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}