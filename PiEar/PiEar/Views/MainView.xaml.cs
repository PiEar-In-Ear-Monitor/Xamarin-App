using System;
using PiEar.ViewModels;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainView
    {
        // private readonly MainViewViewModel pageViewModel = new MainViewViewModel();
        public MainView()
        {
            InitializeComponent();
            // BindingContext = _clickViewModel;
        }
        protected override void OnAppearing()
        {
            var tmp = ListOfChannels.ItemsSource;
            ListOfChannels.ItemsSource = null;
            ListOfChannels.ItemsSource = tmp;
            // ListOfChannels.ItemsSource = ListOfChannels.ItemsSource;
            ClickViewModel.Click.Rotation = ClickViewModel.Click.Rotation;
        }
        private async void _openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsView());
        }
        private async void _openAbout(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutView());
        }
        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            ClickViewModel.PanVolume(e);
        }
    }
}