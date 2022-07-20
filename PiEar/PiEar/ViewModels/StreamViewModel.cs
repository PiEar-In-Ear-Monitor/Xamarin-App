using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Input;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public sealed class StreamViewModel
    {
        public StreamViewModel()
        {
            LabelTap = new Command(_labelTap);
            ImageTap = new Command(_imageTap);
        }
        public Stream Stream { get; } = new Stream();
        public ICommand LabelTap { get; }
        public ICommand ImageTap { get; }
        public static EventHandler GlobalMuteCommand { get; } = (sender, args) =>
        {
            Stream.GlobalMute = !Stream.GlobalMute;
        };
        private async void _labelTap () 
        {
            try
            {
                string newLabel = await Application.Current.MainPage.DisplayPromptAsync("What would you like to name this channel?", "");
                if (newLabel.Length <= 0)
                {
                    return;
                }
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
    }
}