using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PiEar.Models;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class MainPage
    {
        private readonly ObservableCollection<Stream> _streams = new ObservableCollection<Stream>();
        public MainPage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            for (int i = 0; i < 20; i++)
            {
                _streams.Add(new Stream($"Channel {i + 1}"));
            }
            ListOfChannels.ItemsSource = _streams;
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
            // throw new NotImplementedException();
        }

        private void TapGestureRecognizer_MuteButton(object sender, EventArgs e)
        {
            var imageButton = (ImageButton) sender;
            Debug.WriteLine(imageButton.BindingContext);
            // string index = sender.Parent.LogicalChildren[0].Text;
            // int id = int.Parse(index);
            // Stream toChange = GlobalVariables.Streams[id];
            // toChange.Mute = !toChange.Mute;
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