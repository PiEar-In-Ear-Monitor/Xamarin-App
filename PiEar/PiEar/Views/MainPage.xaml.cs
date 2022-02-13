using System;
using System.Diagnostics;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            for (int i = 0; i < GlobalVariables.NumberOfStreams; i++)
            {
                GlobalVariables.Streams.Add(new Stream($"Channel {i + 1}"));
                // SlidersBody.Children.Add(newLayout);
            }

            ListOfChannels.ItemsSource = GlobalVariables.Streams;
        }

        private async void TapGestureRecognizer_OnTapped(Label sender)
        {
            try
            {
                string newName = await DisplayPromptAsync("What would you like to name this channel?", "");
                Debug.Write($"{newName}\n");
                if (newName.Length > 0)
                {
                    sender.Text = ((newName.Length > 26 ) ? newName.Substring(0, 26) : newName );
                }

            }
            catch (Exception e)
            {
                Debug.Write($"Error: {e}\n");
            }
        }

        private void OnChangeBPM(object sender, ValueChangedEventArgs e)
        {
            CountBpm.Text = $"BPM {e.NewValue:F0}";
        }
        
        private void OnChangeBPMStep(object sender, EventArgs e)
        {
            switch (StepperStepCount.Text)
            {
                case "+1 &#10132; +10":
                    BpmStepper.Increment = 10;
                    StepperStepCount.Text = "+10 &#10132; +5";
                    break;
                case "+10 &#10132; +5":
                    BpmStepper.Increment = 5;
                    StepperStepCount.Text = "+5 &#10132; +1";
                    break;
                default:
                    BpmStepper.Increment = 1;
                    StepperStepCount.Text = "+1 &#10132; +10";
                    break;
            }
        }

        private async void OpenSettings(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Settings());
        }

        private async void OpenAbout(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new About());
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void TapGestureRecognizer_MuteButton(Label sender)
        {
            string index = sender.Parent.LogicalChildren[0].Text;
            int id = int.Parse(index);
            Stream toChange = GlobalVariables.Streams[id];
            toChange.Mute = !toChange.Mute;
            // if (sender.IsEnabled)
            // {
            //     toChange.Mute = false;
            //     "mute"
            //     channelSlider.IsEnabled = false;
            //     channelVolume.TextDecorations = TextDecorations.Strikethrough;
            // }
            // else
            // {
            //     channelMute.Source = "unmute";
            //     channelSlider.IsEnabled = true;
            //     channelVolume.TextDecorations = TextDecorations.None;
            // }
        }

        private void PanGestureRecognizer_OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            
            if (e.StatusType == GestureStatus.Running)
            {
                sender.Rotation += e.TotalX / 2.0;
            }

            if (sender.Rotation > 130)
            {
                sender.Rotation = 130;
            } else if (sender.Rotation < -130)
            {
                sender.Rotation = -130;
            }
            Debug.WriteLine($"({e.TotalX}, {e.TotalY})");
        }
    }
}