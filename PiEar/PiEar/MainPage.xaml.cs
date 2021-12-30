using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PiEar
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            for (int i = 0; i < GlobalVariables.numberOfStreams; i++)
            {
                // Channel Label
                Label channelLabel = new Label
                {
                    Text = $"Channel {i + 1}",
                    TextColor = Color.Black,
                    WidthRequest = 100,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                };
                
                // Volume Indicator
                Label channelVolume = new Label
                {
                  Text = "0.00%",
                  HorizontalOptions = LayoutOptions.End,
                  VerticalOptions = LayoutOptions.Center
                };
                
                // Channel Slider
                Slider channelSlider = new Slider
                {
                    ThumbColor = Color.Black,
                    BackgroundColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 170,
                    HeightRequest = 30,
                    Maximum = 1.20
                };

                // Mute Button
                ImageButton channelMute = new ImageButton
                {
                    Source="unmute",
                    WidthRequest =30,
                    HorizontalOptions=LayoutOptions.End,
                    VerticalOptions=LayoutOptions.Center,
                    BackgroundColor=Color.Transparent
                };

                // Pan Button
                Image channelPan = new Image
                {
                    Source="rotate",
                    WidthRequest  = 30,
                    HeightRequest = 30,
                    HorizontalOptions=LayoutOptions.End,
                    VerticalOptions=LayoutOptions.Center,
                    BackgroundColor=Color.Transparent
                };
                
                // New Row
                var newLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal, 
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    Children =
                    {
                        channelLabel,
                        channelSlider,
                        channelVolume,
                        channelMute,
                        channelPan
                    }
                };
                
                // Button Actions
                channelLabel.GestureRecognizers.Add(
                    new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired    = 2,
                        Command = new Command(() =>
                        {
                            TapGestureRecognizer_OnTapped(channelLabel);
                        })
                    }
                );
                
                channelSlider.ValueChanged += (q, qe) =>
                {
                    channelVolume.Text = $"{qe.NewValue:F02}%";
                };
                
                channelMute.Command = new Command(() =>
                    {
                        if (channelSlider.IsEnabled)
                        {
                            channelMute.Source = "mute";
                            channelSlider.IsEnabled = false;
                            channelVolume.TextDecorations = TextDecorations.Strikethrough;
                        }
                        else
                        {
                            channelMute.Source = "unmute";
                            channelSlider.IsEnabled = true;
                            channelVolume.TextDecorations = TextDecorations.None;
                        }
                    }   
                );
                
                PanGestureRecognizer panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += (s, e) => {
                    switch (e.StatusType)
                    {
                        case GestureStatus.Started:
                        case GestureStatus.Running:
                            if (e.TotalY > 0)
                            {
                                channelPan.Rotation = channelPan.Rotation + 7;
                            }
                            if (e.TotalY < 0)
                            {
                                channelPan.Rotation = channelPan.Rotation - 7;
                            }
                            break;
                        case GestureStatus.Completed:
                        case GestureStatus.Canceled:
                        default:
                            break;
                    }

                    if (channelPan.Rotation > 130)
                    {
                        channelPan.Rotation = 130;
                    } else if (channelPan.Rotation < -130)
                    {
                        channelPan.Rotation = -130;
                    }
                };
                channelPan.GestureRecognizers.Add(panGesture);
                
                SlidersBody.Children.Add(newLayout);
                
            }
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
            if (StepperStepCount.Text == "+1 &#10132; +10")
            {
                BpmStepper.Increment = 10;
                StepperStepCount.Text = "+10 &#10132; +5";
            }
            else if (StepperStepCount.Text == "+10 &#10132; +5")
            {
                BpmStepper.Increment = 5;
                StepperStepCount.Text = "+5 &#10132; +1";
            } 
            else
            {
                BpmStepper.Increment = 1;
                StepperStepCount.Text = "+1 &#10132; +10";
            }
        }
    }
}