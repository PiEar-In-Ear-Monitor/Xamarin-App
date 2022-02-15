using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PiEar.Models;

namespace PiEar.Views
{
    public partial class Settings
    {
        
        public Settings()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var testList = new ObservableCollection<Stream>();
            testList.Add(new Stream("Hello"));
            testList.Add(new Stream("World"));
            Debug.WriteLine(testList.GetType());
            SettingsTest.ItemsSource = testList;
            
            testList.Add(new Stream("This"));
            testList.Add(new Stream("Is"));
            testList.Add(new Stream("Text"));
        }

    }
}