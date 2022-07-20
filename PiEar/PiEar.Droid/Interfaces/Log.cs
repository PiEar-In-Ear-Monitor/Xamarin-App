[assembly: Xamarin.Forms.Dependency(typeof(PiEar.Droid.Interfaces.Log))]
namespace PiEar.Droid.Interfaces
{
    public class Log : PiEar.Interfaces.ILog
    {
        public void DebugWrite(string message)
        {
            Android.Util.Log.Debug("org.OConnor.PiEar", message);
        }
        public void ErrorWrite(string message)
        {
            Android.Util.Log.Error("org.OConnor.PiEar", message);
        }
        public void WarnWrite(string message)
        {
            Android.Util.Log.Warn("org.OConnor.PiEar", message);
        }
        public void InfoWrite(string message)
        {
            Android.Util.Log.Info("org.OConnor.PiEar", message);
        }
        public void VerboseWrite(string message)
        {
            Android.Util.Log.Verbose("org.OConnor.PiEar", message);
        }
    }
}