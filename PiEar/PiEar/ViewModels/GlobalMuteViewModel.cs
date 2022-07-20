using System;
using System.Windows.Input;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public class GlobalMuteViewModel
    {
        public static GlobalMute GlobalMuteStatus { get; } = new GlobalMute();
        public ICommand OnClickedCommand { get; } = new Command(() =>
        {
            GlobalMuteStatus.Mute = !GlobalMuteStatus.Mute;
            App.Logger.InfoWrite($"Status of GlobalMuteStatus.Mute is {GlobalMuteStatus.Mute.ToString()}");
        });
    }
}