using System;
using System.Diagnostics;
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
        private async void _labelTap () 
        {
            try
            {
                Stream.Label = await Application.Current.MainPage.DisplayPromptAsync("Change Channel", "Please enter the new channel name", maxLength:26, initialValue:Stream.Label);
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