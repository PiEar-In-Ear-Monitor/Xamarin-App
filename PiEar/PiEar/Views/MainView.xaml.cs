using System;
using PiEar.ViewModels;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            //var tmp = ListOfChannels.ItemsSource;
            //ListOfChannels.ItemsSource = null;
            //ListOfChannels.ItemsSource = tmp;
            // ListOfChannels.ItemsSource = ListOfChannels.ItemsSource;
            ClickViewModel.Click.Rotation = ClickViewModel.Click.Rotation;
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
            ClickViewModel.PanVolume(e);
        }
    }
}