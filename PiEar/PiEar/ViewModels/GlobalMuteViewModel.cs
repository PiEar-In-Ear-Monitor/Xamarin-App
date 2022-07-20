using System.Windows.Input;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public class GlobalMuteViewModel
    {
        public ICommand OnClickedCommand { get; } = new Command(() => App.GlobalMuteStatusValid = false);
    }
}