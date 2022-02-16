using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainPage
    {
        public readonly ObservableCollection<Stream> Streams = new ObservableCollection<Stream>();
        public MainPage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            for (int i = 0; i < 20; i++)
            {
                Streams.Add(new Stream($"Channel {i + 1}"));
            }
            ListOfChannels.ItemsSource = Streams;
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            // try
            // {
            //     string newName = await DisplayPromptAsync("What would you like to name this channel?", "");
            //     Debug.Write($"{newName}\n");
            //     if (newName.Length > 0)
            //     {
            //         sender.Text = ((newName.Length > 26 ) ? newName.Substring(0, 26) : newName );
            //     }
            //
            // }
            // catch (Exception ex)
            // {
            //     Debug.Write($"Error: {ex}\n");
            // }
        }

        private void OnChangeBPM(object sender, ValueChangedEventArgs e)
        {
            CountBpm.Text = $"BPM {e.NewValue:F0}";
        }
        
        // private void OnChangeBPMStep(object sender, EventArgs e)
        // {
        //     switch (StepperStepCount.Text)
        //     {
        //         case "+1 &#10132; +10":
        //             BpmStepper.Increment = 10;
        //             StepperStepCount.Text = "+10 &#10132; +5";
        //             break;
        //         case "+10 &#10132; +5":
        //             BpmStepper.Increment = 5;
        //             StepperStepCount.Text = "+5 &#10132; +1";
        //             break;
        //         default:
        //             BpmStepper.Increment = 1;
        //             StepperStepCount.Text = "+1 &#10132; +10";
        //             break;
        //     }
        // }

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