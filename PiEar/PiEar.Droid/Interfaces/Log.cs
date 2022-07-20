[assembly: Xamarin.Forms.Dependency(typeof(PiEar.Droid.Interfaces.Log))]
namespace PiEar.Droid.Interfaces
{
    public class Log : PiEar.Interfaces.ILog
    {
        private const string Tag = "PiEar";
        public void DebugWrite(string message)
        {
            Android.Util.Log.Debug(Tag, message);
        }
        public void ErrorWrite(string message)
        {
            Android.Util.Log.Error(Tag, message);
        }
        public void WarnWrite(string message)
        {
            Android.Util.Log.Warn(Tag, message);
        }
        public void InfoWrite(string message)
        {
            Android.Util.Log.Info(Tag, message);
        }
        public void VerboseWrite(string message)
        {
            Android.Util.Log.Verbose(Tag, message);
        }
    }
}