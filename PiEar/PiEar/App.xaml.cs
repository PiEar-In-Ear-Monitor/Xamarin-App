﻿using PiEar.Helpers;
using PiEar.Interfaces;
using PiEar.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PiEar
{
    public partial class App
    {
        public static ILog Logger = DependencyService.Get<ILog>();
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainView());
            
            // Get instance of IMulticastService
            var service = DependencyService.Get<IMulticastLock>();
            service.Acquire();
            Networking.FindServerIp();
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