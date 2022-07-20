using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PiEar.Annotations;
using PiEar.Models;
using Plugin.Settings;
using Xamarin.Forms;

namespace PiEar.ViewModels
{
    public class SettingsViewModel: INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            ResetButtonOnClicked = new Command(_resetButtonOnClicked);
        }

        private static int SelectedItemIndex
        {
            get => CrossSettings.Current.GetValueOrDefault($"clickAudioIndex", 0, Settings.ChannelFile);
            set => CrossSettings.Current.AddOrUpdateValue($"clickAudioIndex", value, Settings.ChannelFile);
        }
        public string SelectedSound
        {
            get => Settings.ClickOptions[SelectedItemIndex].ToString();
            set
            {
                if (Settings.ClickOptions.Contains(value))
                {
                    SelectedItemIndex = Settings.ClickOptions.IndexOf(value);
                    Settings.ClickFilename = Settings.ClickOptions[SelectedItemIndex].ToString();
                }
                ClickFileChanged?.Invoke(this, null);
                OnPropertyChanged();
            }
        }
        public IList ClickOptions => Settings.ClickOptions;
        public ICommand ResetButtonOnClicked { get; }
        private static async void _resetButtonOnClicked()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to reset the Channels?", "Yes", "No");
            if (!answer) return;
            Reset();
        }
        private  static void Reset()
        {
            CrossSettings.Current.Clear(Settings.ChannelFile);
        }
        public static event EventHandler ClickFileChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}