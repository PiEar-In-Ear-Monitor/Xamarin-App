using System;
using Xamarin.Forms;

namespace PiEar
{
    public partial class MainPage : ContentPage
    {
        int _clickTotal;
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
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalTextAlignment = TextAlignment.Center
                };
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
                    WidthRequest = 200,
                    HeightRequest = 30,
                    Maximum = 1.20
                };
                channelSlider.ValueChanged += (q, qe) =>
                {
                    channelVolume.Text = $"{qe.NewValue:F02}%";
                };
                lSolo = new ImageButton
                // {
                //     Source="solo",
                //     WidthRequest =30,
                //     HorizontalOptions=LayoutOptions.End,
                //     VerticalOptions=LayoutOptions.Center,
                //     BackgroundColor=Color.Transparent
                // };
                // Mute Button
                ImageButton channelMute = new ImageButton
                {
                    Source="unmute",
                    WidthRequest =30,
                    HorizontalOptions=LayoutOptions.End,
                    VerticalOptions=LayoutOptions.Center,
                    BackgroundColor=Color.Transparent
                };
                channelMute.Command = new Command(() =>
                    {
                        OnMuteClick(channelMute, channelSlider, channelVolume);
                    }   
                );
                
                // Solo Button
                // ImageButton channelSolo = new ImageButton
                // {
                //     Source="solo",
                //     WidthRequest =30,
                //     HorizontalOptions=LayoutOptions.End,
                //     VerticalOptions=LayoutOptions.Center,
                //     BackgroundColor=Color.Transparent
                // };
                // channelMute.Command = new Command(() =>
                //     {
                //         OnSoloClick(i);
                //     }   
                // );
                
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
                        // channelSolo
                    }
                };

                
                SlidersBody.Children.Add(newLayout);
                /*
                    Clicked="ImageButton_Mute"
                    Clicked="ImageButton_Solo"
                 */
            }
        }
        
        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            _clickTotal += 1;
            Label.Text = $"{_clickTotal} ImageButton click{(_clickTotal == 1 ? "" : "s")}";
        }

        private async void TapGestureRecognizer_OnTapped(Label sender)
        {
            try
            {
                string newName = await DisplayPromptAsync("What would you like to name this channel?", "");
                if (newName.Length > 0)
                {
                    sender.Text = newName;
                }

            }
            catch (Exception e)
            {
            }
        }

        private void OnMuteClick(ImageButton button, Slider slider, Label volume)
        {
            if (slider.IsEnabled)
            {
                button.Source = "mute";
                slider.IsEnabled = false;
                volume.Text = $"{0.0:F02}%";
            }
            else
            {
                button.Source = "unmute";
                slider.IsEnabled = true;
                volume.Text = $"{slider.Value:F02}%";
            }
        }

        // private void OnSoloClick(int i)
        // {
        //     for (int j = 0; j < SlidersBody.Children.Count; j++)
        //     {
        //         if (j == i)
        //         {
        //             continue;
        //         }
        //
        //         View row = SlidersBody.Children[i];
        //         OnMuteClick(row[3], row[1], row[2]);   
        //     }
        // }
    }
}