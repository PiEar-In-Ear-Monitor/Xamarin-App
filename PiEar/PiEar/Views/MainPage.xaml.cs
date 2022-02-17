using System;
using System.Collections.ObjectModel;
using PiEar.ViewModels;

namespace PiEar.Views
{
    public partial class MainPage
    {
        private readonly ObservableCollection<StreamController> _streams = new ObservableCollection<StreamController>();
        private readonly ClickController _clickController = new ClickController();
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            for (int i = 0; i < 20; i++)
            {
                _streams.Add(new StreamController($"Channel {i + 1}"));
            }
            ListOfChannels.ItemsSource = _streams;
            BindingContext = _clickController;
        }
        private async void OpenSettings(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Settings());
        }
        private async void OpenAbout(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new About());
        }
        // private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        // {
        //     Image image = (Image)sender; 
        //     if (e.StatusType == GestureStatus.Running)
        //     {
        //         image.Rotation += e.TotalX / 2.0;
        //     }
        //     
        //     if (image.Rotation > 130)
        //     {
        //         image.Rotation = 130;
        //     } else if (image.Rotation < -130)
        //     {
        //         image.Rotation = -130;
        //     }
        //     Debug.WriteLine($"({e.TotalX}, {e.TotalY})");
        // }
    }
}