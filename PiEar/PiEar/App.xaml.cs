using System.Collections.ObjectModel;
using PiEar.Helpers;
using PiEar.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace PiEar
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            this.MainPage = new NavigationPage(new MainPage());
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