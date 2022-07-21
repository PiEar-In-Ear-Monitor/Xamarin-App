using System;
using System.Threading.Tasks;
using PiEar.ViewModels;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainView
    {
        public MainView() => InitializeComponent();
        protected override void OnAppearing()
        {
            while (!ViewModel.SetupComplete)
            {
                Task.Yield();
            }
            ListOfChannels.ItemsSource = null;
            ListOfChannels.ItemsSource = ViewModel.Streams;
        }
        private async void OpenSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsView());
        }
        private async void OpenAbout(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutView());
        }
        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            ViewModel.Click.PanVolume(e);
        }
    }
}