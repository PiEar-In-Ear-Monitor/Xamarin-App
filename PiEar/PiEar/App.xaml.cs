using System.Collections.Generic;
using System.Collections.ObjectModel;
using PiEar.Models;
using PiEar.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace PiEar
{
    public static class GlobalVariables
    {
        public static ObservableCollection<Stream> Streams = new ObservableCollection<Stream>();
        public static int NumberOfStreams = 20;
    }
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            
            var mainPage = new NavigationPage(new MainPage());
            this.MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}