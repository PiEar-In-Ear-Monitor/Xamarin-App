using System;
using System.Windows.Input;
using PiEar.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiEar.ViewModels
{
    public class GlobalMuteViewModel
    {
        public ICommand OnClickedCommand { get; } = new Command(() =>
        {
            App.Logger.InfoWrite($"Toggling GlobalMute.");
            App.GlobalMuteStatusValid = false;
        });
    }
}