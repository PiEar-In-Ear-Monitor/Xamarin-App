using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PiEar.Annotations;
using Xamarin.Forms;

namespace PiEar.Controllers
{
    public class StreamController : INotifyPropertyChanged
    {
        // Commands to bind to
        public static ICommand LabelTap => new Command (OnTapped);

        private static void OnTapped (object s)  {
            Debug.WriteLine ("parameter: " + s);
        }
        
        // Property Changed Default
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}