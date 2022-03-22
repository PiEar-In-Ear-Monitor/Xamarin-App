using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PiEar.Interfaces;
using PiEar.Models;
using Plugin.Settings;
using Xamarin.Forms;

namespace PiEar.Views
{
    public partial class Settings
    {
        private string _clickFilename
        {
            get => CrossSettings.Current.GetValueOrDefault("click", PiEar.Settings.Click, PiEar.Settings.File);
            set => CrossSettings.Current.AddOrUpdateValue("click", value, PiEar.Settings.File);
        }

        private readonly ObservableCollection<string> _clickOptions = new ObservableCollection<string>()
        { "Beep", "Click", "Clink", "Clonk", "Cluck", "Metal", "Plastic", "Sticks", "Tap", "Wood" };
        public Settings()
        {
            InitializeComponent();
            ClickDropdown.ItemsSource = _clickOptions;
            ClickDropdown.SelectedItem = _clickFilename;
            _clickFilename = _clickOptions[ClickDropdown.SelectedIndex];
        }
        private void ClickDropdown_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _clickFilename = _clickOptions[ClickDropdown.SelectedIndex];
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert ("Confirmation", "Are you sure you want to reset?", "Yes", "No");
            if (answer)
            {
                Stream.Count = 0;
                string clickBackup = _clickFilename;
                CrossSettings.Current.Clear(PiEar.Settings.File);
                _clickFilename = clickBackup;
            }
        }
    }
}