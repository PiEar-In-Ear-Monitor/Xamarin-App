using PiEar.Helpers;
using PiEar.Interfaces;
using PiEar.Views;
using PiEar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PiEar.Models;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PiEar
{
    public partial class App
    {
        public static ILog Logger = DependencyService.Get<ILog>();
        public static bool GlobalMuteStatusValid { get; set; }
        private static bool _globalMuteStatus;
        public static bool GlobalMuteStatus { get => _globalMuteStatus; }
        public App()
        {
            InitializeComponent();
            GlobalMuteName.PropertyChanged += (sender, e) => { if (e.PropertyName == "Mute") { _globalMuteStatus = GlobalMuteName.Mute; } };
            GlobalMuteStatusValid = true;
            MainPage = new NavigationPage(new MainView());

            Task.Run(
                () => {
                    while (true)
                    {
                        while (GlobalMuteStatusValid) ;
                        GlobalMuteName.Mute = !GlobalMuteStatus;
                        GlobalMuteStatusValid = true;
                        Logger.InfoWrite($"GlobalMuteStatus is now {GlobalMuteStatus}");
                    }
                }
            );
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