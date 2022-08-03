using PiEar.Helpers;
using PiEar.Interfaces;
using PiEar.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PiEar
{
    public partial class App
    {
        public static readonly ILog Logger = DependencyService.Get<ILog>();
        public static bool GlobalMuteStatusValid { get; set; }
        private static bool _globalMuteStatus;
        public static bool GlobalMuteStatus => _globalMuteStatus;
        private readonly IMulticastLock _service = DependencyService.Get<IMulticastLock>();
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
            _service.Acquire();
            Networking.FindServerIp();
        }
    }
}